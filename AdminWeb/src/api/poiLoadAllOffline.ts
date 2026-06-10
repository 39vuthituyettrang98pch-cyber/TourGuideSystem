// Offline-First helper for GET /api/v1/poi/load-all (ETag + cache fallback)
// NOTE: This file is written for Web/React usage (IndexedDB + Cache Storage).

type PoiSnapshot = unknown;

export type PoiLoadAllResponse = {
  data: PoiSnapshot;
  etag?: string | null;
};

const DB_NAME = "tourguide_poi";
const DB_VERSION = 1;
const STORE_SNAPSHOTS = "poi_snapshots";

function openDb(): Promise<IDBDatabase> {
  return new Promise((resolve, reject) => {
    const req = indexedDB.open(DB_NAME, DB_VERSION);
    req.onupgradeneeded = () => {
      const db = req.result;
      if (!db.objectStoreNames.contains(STORE_SNAPSHOTS)) {
        db.createObjectStore(STORE_SNAPSHOTS);
      }
    };
    req.onsuccess = () => resolve(req.result);
    req.onerror = () => reject(req.error);
  });
}

async function getCachedSnapshot(key: string): Promise<{ etag?: string; payload: any } | null> {
  const db = await openDb();
  return new Promise((resolve) => {
    const tx = db.transaction(STORE_SNAPSHOTS, "readonly");
    const store = tx.objectStore(STORE_SNAPSHOTS);
    const req = store.get(key);
    req.onsuccess = () => resolve(req.result ?? null);
    req.onerror = () => resolve(null);
  });
}

async function putCachedSnapshot(key: string, value: { etag?: string; payload: any }): Promise<void> {
  const db = await openDb();
  return new Promise((resolve, reject) => {
    const tx = db.transaction(STORE_SNAPSHOTS, "readwrite");
    const store = tx.objectStore(STORE_SNAPSHOTS);
    const req = store.put(value, key);
    req.onsuccess = () => resolve();
    req.onerror = () => reject(req.error);
  });
}

function isOnline(): boolean {
  return typeof navigator !== "undefined" ? navigator.onLine : true;
}

export type LoadAllOptions = {
  lang?: string;
  radius?: number;
  // If contract supports it; kept generic.
  deltaFromEtag?: string | null;
};

export type LoadAllResult = {
  status: "network" | "offline" | "not-modified";
  etag?: string | null;
  data?: PoiSnapshot;
  cached?: boolean;
};

// Low-level fetch wrapper using Fetch API.
export async function loadPoiLoadAllOfflineFirst(
  opts: LoadAllOptions = {}
): Promise<LoadAllResult> {
  const cacheKey = "snapshot::load-all";

  // 1) Read cached snapshot first (always available for UI fast render)
  const cached = await getCachedSnapshot(cacheKey);

  // 2) If offline -> return cached immediately
  if (!isOnline()) {
    if (cached?.payload) {
      return {
        status: "offline",
        etag: cached.etag ?? null,
        data: cached.payload,
        cached: true,
      };
    }
    return { status: "offline", etag: null, data: undefined, cached: true };
  }

  // 3) Online: do conditional request using ETag if we have it
  const headers: Record<string, string> = {
    "Accept": "application/json",
  };

  if (cached?.etag) headers["If-None-Match"] = String(cached.etag);

  // Build query string (only include known params)
  const url = new URL("/api/v1/poi/load-all", (typeof window !== "undefined" ? window.location.origin : ""));
  if (opts.lang) url.searchParams.set("lang", opts.lang);
  // radius/delta can be set depending on backend.
  // We keep them optional and only set if provided.
  if (typeof opts.radius === "number") url.searchParams.set("radius", String(opts.radius));

  // If contract supports delta sync, we can send a custom header or query.
  // Here we only demonstrate If-None-Match.
  const response = await fetch(url.toString(), {
    method: "GET",
    headers,
    credentials: "include", // harmless for public endpoints
  });

  // 4) Handle 304 Not Modified
  if (response.status === 304) {
    return {
      status: "not-modified",
      etag: cached?.etag ?? null,
      data: cached?.payload,
      cached: true,
    };
  }

  if (!response.ok) {
    // 5) On error fallback to cache
    if (cached?.payload) {
      return {
        status: "network",
        etag: cached.etag ?? null,
        data: cached.payload,
        cached: true,
      };
    }
    // let caller handle as exception
    throw new Error(`load-all failed: HTTP ${response.status}`);
  }

  const etag = response.headers.get("ETag");
  const payload = await response.json();

  // 6) Persist fresh snapshot
  await putCachedSnapshot(cacheKey, { etag: etag ?? undefined, payload });

  return {
    status: "network",
    etag: etag ?? null,
    data: payload,
    cached: false,
  };
}

