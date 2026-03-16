import { Civilization } from "@/types/civilization";

interface DataItem {
  id: string;
  name: string;
  description: string;
  attributes: Record<string, string>;
}

export const mapCivilizationsToDataItems = (civilizations: Civilization[]): DataItem[] => {
  return civilizations.map(civ => ({
    id: civ.id.toString(),
    name: civ.name,
    description: civ.summary || "No description available.",
    attributes: {
      "Visión General": civ.overview || "N/A",
      Territorio: civ.territory?.toString() || "N/A",
    },
  }));
};
