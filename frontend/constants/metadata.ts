/**
 * Metadata Engine: Historical Data Dictionary
 * 
 * Maps raw backend ID enumerations to human-readable concepts,
 * descriptions, and aesthetic tokens (colors/icons).
 */

export const METADATA = {
  civilization: {
    state: {
      0: { label: "Soberanía Unificada", desc: "Un poder centralizado que domina un territorio coherente.", color: "text-amber-600" },
      1: { label: "Federación Regional", desc: "Coalición de estados o tribus bajo un mando común.", color: "text-blue-600" },
      2: { label: "Fragmentación Noble", desc: "Poder dividido entre señores feudales con autonomía local.", color: "text-slate-500" },
      3: { label: "Imperio Hegemónico", desc: "Dominio absoluto sobre múltiples culturas y vastas regiones.", color: "text-purple-600" },
    },
    territory: {
      0: { label: "Continental", desc: "Vastas extensiones de tierra sin fronteras marítimas dominantes." },
      1: { label: "Archipiélago", desc: "Nación dispersa en múltiples islas con alto poder naval." },
      2: { label: "Ribereño", desc: "Desarrollo lineal a lo largo de grandes arterias fluviales." },
      3: { label: "Montañoso", desc: "Fortalezas naturales y terrenos de difícil acceso." },
    }
  },
  battle: {
    outcome: {
      0: { label: "Victoria Absoluta", desc: "Aniquilación completa o rendición incondicional del enemigo.", color: "text-emerald-600" },
      1: { label: "Victoria Pírrica", desc: "Triunfo obtenido a un costo devastador para el vencedor.", color: "text-orange-500" },
      2: { label: "Empate Táctico", desc: "Ningún bando logró sus objetivos primordiales sin pérdidas críticas.", color: "text-slate-400" },
      3: { label: "Derrota Honrosa", desc: "Repliegue estratégico que permitió preservar el grueso de las fuerzas.", color: "text-red-600" },
    }
  },
  // Fallback for unknown IDs
  unknown: { label: "Archivo Desconocido", desc: "Dato perdido en el tiempo o aún no catalogado." }
};

export type MetadataCategory = keyof typeof METADATA;

export const getMetadata = (category: string, subCategory: string, id: number | string) => {
  const cat = (METADATA as any)[category];
  if (!cat || !cat[subCategory]) return METADATA.unknown;
  return cat[subCategory][id] || METADATA.unknown;
};
