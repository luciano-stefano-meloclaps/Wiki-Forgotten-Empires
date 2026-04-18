export interface CivilizationRelation {
  id: number;
  name: string;
  overview?: string;
  state?: string;
  summary?: string;
}

export interface Civilization {
  id: number;
  name: string;
  summary?: string;
  overview?: string;
  imageUrl?: string;
  territories?: string[];
  state?: string;
  agesCount?: number;
  charactersCount?: number;
  battlesCount?: number;
  ages?: CivilizationRelation[];
  characters?: CivilizationRelation[];
  battles?: CivilizationRelation[];
}
