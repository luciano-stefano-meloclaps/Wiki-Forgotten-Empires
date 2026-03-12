const API_URL = process.env.NEXT_PUBLIC_API_URL;

if (!API_URL) {
  throw new Error("Missing NEXT_PUBLIC_API_URL environment variable");
}

export async function fetchApi<T>(endpoint: string, options?: RequestInit): Promise<T> {
  const response = await fetch(`${API_URL}/${endpoint}`, options);
  if (!response.ok) {
    throw new Error(`API call to ${endpoint} failed: ${response.statusText}`);
  }
  return response.status === 204 ? (null as T) : (response.json() as Promise<T>);
}