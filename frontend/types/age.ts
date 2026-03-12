export interface Age {
  id: number;
  name: string;
  summary: string | null;
  date: string | null;
  overview: string | null;
}
export interface AgeDetail extends Age {
  characters: any[]; // TODO: Reemplazar 'any' con el tipo 'Character' cuando se defina
  battles: any[];    // TODO: Reemplazar 'any' con el tipo 'Battle' cuando se defina
  civilizations: any[]; // TODO: Reemplazar 'any' con el tipo 'CivilizationAge' cuando se defina
}