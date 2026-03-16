import { Character } from "@/types/character";

interface DataItem {
  id: string;
  name: string;
  description: string;
  attributes: Record<string, string>;
}

export const mapCharactersToDataItems = (characters: Character[]): DataItem[] => {
  return characters.map(char => ({
    id: char.id.toString(),
    name: char.name,
    description: char.description || "No description available.",
    attributes: {
      Título: char.honorificTitle || "N/A",
      Periodo: char.lifePeriod || "N/A",
    },
  }));
};
