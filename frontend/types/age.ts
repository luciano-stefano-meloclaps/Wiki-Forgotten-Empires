export interface Age {
  id: number;
  name: string;
  summary: string | null;
  date: string | null;
  overview: string | null;
}

export interface Character {
  id: number;
  name: string;
  description?: string;
  honorificTitle?: string;
  lifePeriod?: string;
  role?: string;
}

export interface Battle {
  id: number;
  name: string;
  date?: string;
  summary?: string;
  territory?: string;
}

export interface AgeDetail extends Age {
  characters: Character[];
  battles: Battle[];
  civilizations: { id: number; name: string }[];
}
