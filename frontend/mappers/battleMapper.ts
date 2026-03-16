import { Battle } from "@/types/battle";

interface DataItem {
  id: string;
  name: string;
  description: string;
  attributes: Record<string, string>;
}

export const mapBattlesToDataItems = (battles: Battle[]): DataItem[] => {
  return battles.map(battle => ({
    id: battle.id.toString(),
    name: battle.name,
    description: battle.summary || "No description available.",
    attributes: {
      Fecha: battle.date || "N/A",
      "Visión General": battle.detailedDescription || "N/A",
    },
  }));
};
