import { fetchApi } from "./api";

export interface GlobalStats {
  totalBattles: number;
  totalCharacters: number;
  totalCivilizations: number;
  totalAges: number;
}

export const statisticsService = {
  getGlobalStats: async (): Promise<GlobalStats> => {
    return fetchApi<GlobalStats>("Statistics");
  },
};
