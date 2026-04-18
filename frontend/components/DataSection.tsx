"use client";
import { useState, useEffect } from "react";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Skeleton } from "@/components/ui/skeleton";
import { Button } from "@/components/ui/button";
import KintsugiDivider from "./KintsugiDivider";
import Link from "next/link";

// Importaciones de servicios
import { ageService } from "@/services/ageService";
import { battleService } from "@/services/battleService";
import { civilizationService } from "@/services/civilizationService";
import { characterService } from "@/services/characterService";

// Importaciones de mappers
import { mapAgesToDataItems } from "@/mappers/ageMapper";
import { mapBattlesToDataItems } from "@/mappers/battleMapper";
import { mapCivilizationsToDataItems } from "@/mappers/civilizationMapper";
import { mapCharactersToDataItems } from "@/mappers/characterMapper";
import type { EntityType } from "@/types/entity";
import { entityLabels } from "@/types/entity";

interface DataItem {
  id: string;
  name: string;
  description: string;
  attributes: Record<string, string>;
}

interface DataSectionProps {
  selectedEntity: EntityType;
  onSelectEntity: (entity: EntityType) => void;
}

const entities = [
  { id: "ages", label: entityLabels.ages },
  { id: "civilizations", label: entityLabels.civilizations },
  { id: "characters", label: entityLabels.characters },
  { id: "battles", label: entityLabels.battles },
] as const;

export default function DataSection({
  selectedEntity,
  onSelectEntity,
}: DataSectionProps) {
  const [data, setData] = useState<DataItem[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchData = async () => {
      setLoading(true);
      setError(null);
      try {
        let mappedData: DataItem[] = [];
        switch (selectedEntity) {
          case "ages":
            const rawAges = await ageService.getAll();
            mappedData = mapAgesToDataItems(rawAges);
            break;
          case "battles":
            const rawBattles = await battleService.getAll();
            mappedData = mapBattlesToDataItems(rawBattles);
            break;
          case "civilizations":
            const rawCivs = await civilizationService.getAll();
            mappedData = mapCivilizationsToDataItems(rawCivs);
            break;
          case "characters":
            const rawChars = await characterService.getAll();
            mappedData = mapCharactersToDataItems(rawChars);
            break;
        }
        setData(mappedData);
      } catch (err) {
        setError("Error al cargar los datos. Por favor, inténtalo de nuevo.");
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [selectedEntity]);

  return (
    <section id="data" className="py-20 relative">
      <div className="absolute top-0 left-0 right-0">
        <KintsugiDivider />
      </div>

      <div className="max-w-7xl mx-auto px-4">
        <div className="text-center mb-16">
          <h2 className="font-serif font-bold text-4xl md:text-5xl text-foreground mb-6">
            Explora la <span className="golden-shine">Historia</span>
          </h2>
          <p className="font-display text-lg md:text-xl text-muted-foreground max-w-3xl mx-auto mb-8">
            Selecciona una categoría para sumergirte en los detalles de nuestro
            archivo histórico.
          </p>

          <div className="flex flex-wrap justify-center gap-4 mb-12">
            {entities.map((entity) => (
              <Button
                key={entity.id}
                onClick={() => onSelectEntity(entity.id as EntityType)}
                variant={selectedEntity === entity.id ? "default" : "outline"}
                className={`px-8 py-2 text-lg transition-all duration-300 border-2 ${
                  selectedEntity === entity.id
                    ? "bg-[#b7992a] text-white border-[#b7992a] hover:bg-[#9a8124] shadow-lg shadow-[#b7992a]/20"
                    : "border-[#b7992a] text-[#b7992a] hover:bg-[#b7992a]/10 hover:text-[#b7992a]"
                }`}
                style={{ borderRadius: "4px" }}
              >
                {entity.label}
              </Button>
            ))}
          </div>
        </div>

        {loading ? (
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            {[1, 2, 3, 4].map((i) => (
              <Skeleton key={i} className="h-64 w-full rounded-2xl" />
            ))}
          </div>
        ) : error ? (
          <div className="text-center text-red-500 py-10">{error}</div>
        ) : data.length === 0 ? (
          <div className="text-center py-20 text-slate-600 dark:text-slate-300">
            <p className="text-xl font-semibold">
              No se encontraron {entityLabels[selectedEntity].toLowerCase()}.
            </p>
            <p className="mt-3 max-w-2xl mx-auto text-base">
              Puede que aún no se hayan cargado registros para esta categoría o
              que la sincronización esté en proceso.
            </p>
          </div>
        ) : (
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            {data.map((item) => {
              const hasVisionGeneral = "Visión General" in item.attributes;
              const { "Visión General": visionGeneral, ...otherAttributes } =
                item.attributes;

              let mainText = item.description;
              let tagsArray: string[] = [];

              if (hasVisionGeneral) {
                // Si existe Visión General, toma protagonismo como texto principal
                mainText =
                  visionGeneral && visionGeneral !== "N/A"
                    ? visionGeneral
                    : "Descubre más detalles sobre este elemento histórico accediendo a su archivo completo.";

                // Convertimos el summary (la vieja description) en tags visuales separados
                tagsArray = (item.description || "")
                  .split(/\s*[-–—,;|]\s*/)
                  .map((t) => t.trim())
                  .filter(
                    (t) =>
                      t.length > 2 &&
                      !t.toLowerCase().includes("available") &&
                      !t.toLowerCase().includes("n/a"),
                  );
              }

              return (
                <Link
                  href={`/${selectedEntity}/${item.id}`}
                  key={item.id}
                  className="transition-transform duration-300"
                >
                  <Card className="classical-card group h-full flex flex-col justify-between vt-card-container">
                    <CardHeader className="pb-4 relative z-10 px-0 pt-0">
                      <div className="flex justify-between items-start gap-4 mb-3">
                        <CardTitle className="font-serif text-2xl tracking-wide text-slate-900 dark:text-[#e6d28a] group-hover:text-[#af944d] dark:group-hover:text-[#ffed99] transition-colors duration-300 uppercase leading-none vt-entity-title">
                          {item.name}
                        </CardTitle>

                        {/* El atributo principal (ej. Período, Fecha) se eleva a la esquina superior como insignia elegante */}
                        {Object.entries(otherAttributes)
                          .slice(0, 1)
                          .map(([key, value]) => (
                            <div
                              key={key}
                              className="text-right shrink-0 bg-[#af944d]/5 dark:bg-[#e6d28a]/5 px-2.5 py-1.5 rounded-[2px] border border-[#af944d]/20 transition-colors group-hover:bg-[#af944d]/10"
                            >
                              <span className="block font-serif text-[9px] font-bold tracking-widest uppercase text-[#af944d] dark:text-[#c4a962] leading-tight mb-0.5">
                                {key}
                              </span>
                              <span className="block font-sans text-[11px] font-semibold text-slate-800 dark:text-[#e6d28a] leading-tight">
                                {value}
                              </span>
                            </div>
                          ))}
                      </div>

                      <div className="w-12 h-[1px] bg-[#af944d]/40 mb-3" />

                      <CardDescription className="font-sans text-sm leading-relaxed text-slate-600 dark:text-[#b8b09d] line-clamp-3">
                        {mainText}
                      </CardDescription>
                    </CardHeader>

                    <CardContent className="relative z-10 px-0 pb-0 mt-auto flex flex-col gap-4">
                      {/* Atributos secundarios (si existen más de 1 atributo original) */}
                      {Object.entries(otherAttributes).slice(1).length > 0 && (
                        <div className="space-y-2.5 pt-4 border-t border-[#af944d]/10">
                          {Object.entries(otherAttributes)
                            .slice(1)
                            .map(([key, value]) => (
                              <div
                                key={key}
                                className="flex justify-between items-center gap-4 group/row"
                              >
                                <span className="font-serif text-[11px] font-bold tracking-widest uppercase text-[#af944d] dark:text-[#c4a962]">
                                  {key}
                                </span>
                                <span className="font-sans text-right text-sm text-slate-700 dark:text-[#d1c7b1] font-medium transition-colors group-hover/row:text-slate-900 dark:group-hover/row:text-white line-clamp-1">
                                  {value || "N/A"}
                                </span>
                              </div>
                            ))}
                        </div>
                      )}

                      {/* Sección de Tags (Conceptos Clave, Tipos, etc.) anclada al fondo de la card */}
                      {tagsArray.length > 0 && (
                        <div
                          className={`flex flex-wrap gap-1.5 ${Object.entries(otherAttributes).slice(1).length === 0 ? "pt-4 border-t border-[#af944d]/10" : ""}`}
                        >
                          {tagsArray.map((tag, idx) => (
                            <span
                              key={idx}
                              className="font-sans text-[10px] font-semibold tracking-wide uppercase px-2 py-1 bg-white/50 dark:bg-black/20 text-[#8a7339] dark:text-[#c4a962] border border-[#af944d]/20 rounded-[2px]"
                            >
                              {tag}
                            </span>
                          ))}
                        </div>
                      )}
                    </CardContent>
                  </Card>
                </Link>
              );
            })}
          </div>
        )}
      </div>

      <div className="absolute bottom-0 left-0 right-0">
        <KintsugiDivider />
      </div>
    </section>
  );
}
