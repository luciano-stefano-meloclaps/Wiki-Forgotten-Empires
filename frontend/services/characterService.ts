import { Character } from "@/types/character";
import { fetchApi } from "./api";

export const characterService = {
  getAll: async (): Promise<Character[]> => {
    try {
      return await fetchApi<Character[]>("Character");
    } catch (error) {
      console.error("Failed to fetch characters:", error);
      return [];
    }
  },
  getById: async (id: number): Promise<Character | null> => {
    try {
      return await fetchApi<Character>(`Character/${id}`);
    } catch (error) {
      console.error(`Failed to fetch character with id ${id}:`, error);
      return null;
    }
  },
};
