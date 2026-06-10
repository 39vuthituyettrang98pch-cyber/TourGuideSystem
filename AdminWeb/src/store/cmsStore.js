import { create } from "zustand";
import { mockPois, mockTours } from "../data/mockData.js";

export const useCmsStore = create((set) => ({
  pois: mockPois,
  tours: mockTours,
  upsertPoi: (poi) =>
    set((state) => {
      const exists = state.pois.some((item) => item.id === poi.id);
      return {
        pois: exists
          ? state.pois.map((item) => (item.id === poi.id ? poi : item))
          : [{ ...poi, id: Math.max(0, ...state.pois.map((item) => item.id)) + 1 }, ...state.pois],
      };
    }),
  upsertTour: (tour) =>
    set((state) => {
      const exists = state.tours.some((item) => item.id === tour.id);
      return {
        tours: exists
          ? state.tours.map((item) => (item.id === tour.id ? tour : item))
          : [{ ...tour, id: Math.max(0, ...state.tours.map((item) => item.id)) + 1 }, ...state.tours],
      };
    }),
}));
