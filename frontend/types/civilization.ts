export interface Civilization {
  id: number;
  name: string;
  summary?: string;
  overview?: string;
  imageUrl?: string;
  territory: number;
  state: number;
}
