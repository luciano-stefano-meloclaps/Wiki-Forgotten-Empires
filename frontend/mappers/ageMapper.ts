import { Age } from "@/types/age";
// Este es el tipo genérico que tu componente DataSection espera
interface DataItem {
  id: string;
  name: string;
  description: string;
  attributes: Record<string, string>;
}

export const mapAgesToDataItems = (ages: Age[]): DataItem[] => {
  return ages.map(age => ({
    id: age.id.toString(),
    name: age.name,
    description: age.summary || "No summary available.",
    attributes: {
      Período: age.date || "N/A",
      "Visión General": age.overview || "N/A",
    },
  }));
};