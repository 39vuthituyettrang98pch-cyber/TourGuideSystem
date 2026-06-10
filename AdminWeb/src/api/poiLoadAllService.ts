// Service class + UI-friendly error handling for GET /api/v1/poi/load-all
// Offline-First strategy:
//  - If navigator.onLine is false: return data from IndexedDB (if any)
//  - If online: use ETag via If-None-Match to support delta sync / not-modified
//  - On network errors: fallback to IndexedDB if present
//
// NOTE: This module is written for the Web (AdminWeb React). If you later
// integrate it into UserMobile PWA, the logic remains the same.

export type PoiSnapshot = unknown;

export type LoadAllResult = {
  status: "network" | "offline" | "not-modified";
  etag?: string | null;
  data?: PoiSnapshot;
  cached?: boolean;
};

export type LoadAllOptions = {
  lang?: string;
  radius?: number;
  deltaFromEtag?: string | null;
};

const DB_NAME = "tourguide_poi";
const DB_VERSION = 1;
const STORE_SNAPSHOTS = "poi_snapshots";
const CACHE_KEY = "snapshot::load-all";

function isOnline(): boolean {
  return typeof navigator !== "undefined" ? navigator.onLine : true;
}

async function openDb(): Promise<IDBDatabase> {
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

async function getCachedSnapshot(): Promise<{ etag?: string | null; payload: any } | null> {
  const db = await openDb();
  return new Promise((resolve) => {
    const tx = db.transaction(STORE_SNAPSHOTS, "readonly");
    const store = tx.objectStore(STORE_SNAPSHOTS);
    const req = store.get(CACHE_KEY);
    req.onsuccess = () => resolve(req.result ?? null);
    req.onerror = () => resolve(null);
  });
}

async function putCachedSnapshot(value: { etag?: string | null; payload: any }): Promise<void> {
  const db = await openDb();
  return new Promise((resolve, reject) => {
    const tx = db.transaction(STORE_SNAPSHOTS, "readwrite");
    const store = tx.objectStore(STORE_SNAPSHOTS);
    const req = store.put(value, CACHE_KEY);
    req.onsuccess = () => resolve();
    req.onerror = () => reject(req.error);
  });
}

export type FriendlyApiError = {
  title: string;
  message: string;
  code?: number;
};

function toFriendlyError(err: any): FriendlyApiError {
  const status: number | undefined = err?.status ?? err?.code;
  if (status === 401) {
    return {
      title: "Phiên đăng nhập đã hết hạn",
      message: "Vui lòng đăng nhập lại để tiếp tục tải dữ liệu.",
      code: status,
    };
  }
  if (status === 429) {
    return {
      title: "Tải quá nhanh",
      message: "Hệ thống đang bận. Thử lại sau vài giây.",
      code: status,
    };
  }
  return {
    title: "Không thể tải dữ liệu",
    message: "Vui lòng kiểm tra kết nối mạng và thử lại.",
    code: status,
  };
}

export class PoiLoadAllService {
  constructor(private baseUrl: string = "/") {}

  /**
   * Gọi GET /api/v1/poi/load-all với chiến lược Offline-First + ETag.
   */
  async loadAll(opts: LoadAllOptions = {}): Promise<LoadAllResult> {
    // 1) luôn thử đọc cache để UI có dữ liệu nhanh
    const cached = await getCachedSnapshot();

    // 2) Offline: trả cache
    if (!isOnline()) {
      if (cached?.payload) {
        return {
          status: "offline",
          etag: cached.etag ?? null,
          data: cached.payload,
          cached: true,
        };
      }
      return { status: "offline", etag: cached?.etag ?? null, data: undefined, cached: true };
    }

    try {
      // 3) Online: conditional request with ETag
      const headers: Record<string, string> = {
        Accept: "application/json",
      };

      const etagToUse = opts.deltaFromEtag ?? cached?.etag;
      if (etagToUse) headers["If-None-Match"] = String(etagToUse);

      const url = new URL(this.baseUrl.replace(/\/$/, "") + "/api/v1/poi/load-all");
      if (opts.lang) url.searchParams.set("lang", opts.lang);
      if (typeof opts.radius === "number") url.searchParams.set("radius", String(opts.radius));

      const resp = await fetch(url.toString(), {
        method: "GET",
        headers,
        credentials: "include", // endpoint public nhưng cookie có thể tồn tại
      });

      if (resp.status === 304) {
        return {
          status: "not-modified",
          etag: cached?.etag ?? null,
          data: cached?.payload,
          cached: true,
        };
      }

      if (!resp.ok) {
        // fallback to cache for offline-like behavior
        if (cached?.payload) {
          return {
            status: "network",
            etag: cached.etag ?? null,
            data: cached.payload,
            cached: true,
          };
        }

        const err: any = new Error(`HTTP ${resp.status}`);
        err.status = resp.status;
        throw err;
      }

      const etag = resp.headers.get("ETag");
      const payload = (await resp.json()) as PoiSnapshot;

      // 4) persist fresh snapshot
      await putCachedSnapshot({ etag: etag ?? null, payload });

      return {
        status: "network",
        etag: etag ?? null,
        data: payload,
        cached: false,
      };
    } catch (err: any) {
      // 5) Lỗi mạng: fallback cache
      const friendly = toFriendlyError(err);
      const cache = await getCachedSnapshot();

      if (cache?.payload) {
        return {
          status: "offline",
          etag: cache.etag ?? null,
          data: cache.payload,
          cached: true,
        };
      }

      // không có cache => ném lỗi thân thiện
      throw friendly;
    }
  }
}

// Convenience singleton (dễ import ở UI)
export const poiLoadAllService = new PoiLoadAllService();

