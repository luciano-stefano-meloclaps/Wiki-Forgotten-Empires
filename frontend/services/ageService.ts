import { Age, AgeDetail } from "@/types/age";
import { fetchApi } from "./api";

export const ageService = {
  getAll: async (): Promise<Age[]> => {
    try {
      return await fetchApi<Age[]>("Age");
    } catch (error) {
      console.error("Failed to fetch ages:", error);
      return [];
    }
  },
  getById: async (id: number): Promise<AgeDetail | null> => {
    try {
      return await fetchApi<AgeDetail>(`Age/${id}`);
    } catch (error) {
      console.error(`Failed to fetch age with id ${id}:`, error);
      return null;
    }
  },
};