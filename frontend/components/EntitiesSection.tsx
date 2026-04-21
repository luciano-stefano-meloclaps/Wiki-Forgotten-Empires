"use client";

import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Sword, Crown, Clock, Users } from "lucide-react";
import KintsugiDivider from "./KintsugiDivider";
import type { EntityType } from "@/types/entity";

const entities: Array<{
  id: EntityType;
  title: string;
  icon: any;
  description: string;
  attributes: string[];
  color: string;
}> = [
  {
    id: "battles",
    title: "Batallas",
    icon: Sword,
    description: "Conflictos épicos que definieron el curso de la historia",
    attributes: [
      "Fecha",
      "Participantes",
      "Resultado",
      "Ubicación",
      "Estrategias",
    ],
    color: "text-red-500",
  },
  {
    id: "civilizations",
    title: "Civilizaciones",
    icon: Crown,
    description: "Imperios y culturas que dominaron diferentes épocas",
    attributes: ["Región", "Era", "Estado", "Tecnologías", "Legado"],
    color: "text-blue-500",
  },
  {
    id: "ages",
    title: "Eras",
    icon: Clock,
    description: "Períodos históricos con características distintivas",
    attributes: [
      "Período",
      "Rasgos",
      "Innovaciones",
      "Eventos",
      "Transiciones",
    ],
    color: "text-green-500",
  },
  {
    id: "characters",
    title: "Personajes",
    icon: Users,
    description: "Figuras legendarias que marcaron la historia",
    attributes: ["Rol", "Resultado", "Afiliación", "Hazañas", "Legado"],
    color: "text-purple-500",
  },
];

interface EntitiesSectionProps {
  selectedEntity: EntityType;
  onSelectEntity: (entity: EntityType) => void;
}

export default function EntitiesSection({
  selectedEntity,
  onSelectEntity,
}: EntitiesSectionProps) {
  const handleSelect = (entityType: EntityType) => {
    onSelectEntity(entityType);
    const element = document.getElementById("data");
    if (element) {
      element.scrollIntoView({ behavior: "smooth" });
    }
  };

  return (
    <section
      id="entities"
      className="py-20 relative bg-gradient-to-b from-[#f2ede4] to-[#e8e3da] dark:from-[#121212] dark:to-[#1a1917]"
    >
      <div className="absolute top-0 left-0 right-0">
        <KintsugiDivider />
      </div>

      <div className="max-w-7xl mx-auto px-4">
        <div className="text-center mb-16">
          <h2 className="font-serif font-bold text-4xl md:text-5xl text-foreground dark:text-white mb-6">
            Explora Nuestras <span className="golden-shine">Entidades</span>
          </h2>
          <p className="font-display text-lg md:text-xl text-muted-foreground dark:text-[#d1c7b1] max-w-3xl mx-auto">
            Sumérgete en un universo de datos históricos cuidadosamente
            catalogados. Cada entidad contiene información detallada y
            verificada.
          </p>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
          {entities.map((entity, index) => {
            const IconComponent = entity.icon;
            return (
              <Card
                key={entity.title}
                className={`classical-card group cursor-pointer transition-shadow duration-300 ${
                  selectedEntity === entity.id
                    ? "ring-2 ring-[#b7992a] shadow-lg shadow-[#b7992a]/10"
                    : "hover:shadow-lg hover:shadow-[#b7992a]/10"
                }`}
              >
                <CardHeader className="text-center pb-4">
                  <div className="flex justify-center mb-4">
                    <div className="p-4 bg-amber-50 dark:bg-[#2d2a25] border border-[#af944d]/30 dark:border-[#af944d]/30 transition-colors duration-300 rounded-[2px]">
                      <IconComponent
                        className={`h-8 w-8 ${entity.color} group-hover:text-[#af944d] dark:group-hover:text-[#e6d28a] transition-colors duration-300`}
                      />
                    </div>
                  </div>
                  <CardTitle className="font-serif text-2xl text-slate-900 dark:text-[#e6d28a] mb-2 group-hover:text-[#af944d] dark:group-hover:text-[#ffed99] transition-colors duration-300 uppercase tracking-wide">
                    {entity.title}
                  </CardTitle>
                  <CardDescription className="font-sans text-slate-600 dark:text-[#b8b09d]">
                    {entity.description}
                  </CardDescription>
                </CardHeader>

                <CardContent className="space-y-4">
                  <div className="flex flex-wrap gap-2">
                    {entity.attributes.map((attr) => (
                      <span
                        key={attr}
                        className="px-3 py-1.5 text-xs font-serif font-semibold border bg-white/50 text-[#af944d] border-[#af944d]/20 dark:bg-black/20 dark:text-[#e6d28a] dark:border-[#af944d]/20 rounded-[2px]"
                      >
                        {attr}
                      </span>
                    ))}
                  </div>

                  <Button
                    onClick={() => handleSelect(entity.id as EntityType)}
                    variant={
                      selectedEntity === entity.id ? "default" : "outline"
                    }
                    className="btn-classical-secondary w-full px-6 py-3 text-lg h-14"
                  >
                    Ver Datos
                  </Button>
                </CardContent>
              </Card>
            );
          })}
        </div>
      </div>

      <div className="absolute bottom-0 left-0 right-0">
        <KintsugiDivider />
      </div>
    </section>
  );
}
