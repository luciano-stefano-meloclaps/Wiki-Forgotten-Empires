import { Civilization } from "@/types/civilization";
import { fetchApi } from "./api";

export const civilizationService = {
  getAll: async (): Promise<Civilization[]> => {
    try {
      return await fetchApi<Civilization[]>("Civilization");
    } catch (error) {
      console.error("Failed to fetch civilizations:", error);
      return [];
    }
  },
  getById: async (id: number): Promise<Civilization | null> => {
    try {
      return await fetchApi<Civilization>(`Civilization/${id}`);
    } catch (error) {
      console.error(`Failed to fetch civilization with id ${id}:`, error);
      return null;
    }
  },
};
