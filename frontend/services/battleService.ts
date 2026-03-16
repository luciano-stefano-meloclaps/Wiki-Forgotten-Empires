import { Battle } from "@/types/battle";
import { fetchApi } from "./api";

export const battleService = {
  getAll: async (): Promise<Battle[]> => {
    try {
      return await fetchApi<Battle[]>("Battle");
    } catch (error) {
      console.error("Failed to fetch battles:", error);
      return [];
    }
  },
  getById: async (id: number): Promise<Battle | null> => {
    try {
      return await fetchApi<Battle>(`Battle/${id}`);
    } catch (error) {
      console.error(`Failed to fetch battle with id ${id}:`, error);
      return null;
    }
  },
};
