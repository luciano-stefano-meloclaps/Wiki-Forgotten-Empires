import { Civilization } from "@/types/civilization";

interface DataItem {
  id: string;
  name: string;
  description: string;
  attributes: Record<string, string>;
}

export const mapCivilizationsToDataItems = (
  civilizations: Civilization[],
): DataItem[] => {
  return civilizations.map((civ) => ({
    id: civ.id.toString(),
    name: civ.name,
    description: civ.summary || civ.overview || civ.name,
    attributes: {
      Estado: civ.state || "Desconocido",
      Territorios:
        civ.territories && civ.territories.length > 0
          ? civ.territories.join(", ")
          : "N/A",
      "Eras Asociadas": civ.agesCount?.toString() ?? "0",
      Personajes: civ.charactersCount?.toString() ?? "0",
      Batallas: civ.battlesCount?.toString() ?? "0",
    },
  }));
};
