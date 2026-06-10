import { poiLoadAllService, type LoadAllOptions, type LoadAllResult } from "./poiLoadAllService";

/**
 * Hàm gọi nhanh cho UI: GET /api/v1/poi/load-all
 * - Offline-First (IndexedDB fallback)
 * - ETag conditional request
 */
export async function fetchPoiLoadAll(
  opts: LoadAllOptions = {}
): Promise<LoadAllResult> {
  return poiLoadAllService.loadAll(opts);
}

