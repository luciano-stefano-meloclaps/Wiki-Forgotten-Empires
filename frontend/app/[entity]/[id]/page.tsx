import { ageService } from "@/services/ageService";
import { battleService } from "@/services/battleService";
import { characterService } from "@/services/characterService";
import { civilizationService } from "@/services/civilizationService";
import { getMetadata } from "@/constants/metadata";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { ArrowLeft, Landmark, Sword, Users, Clock } from "lucide-react";
import Link from "next/link";
import KintsugiDivider from "@/components/KintsugiDivider";

interface PageProps {
  params: {
    entity: string;
    id: string;
  };
}

const entityIcons: Record<string, any> = {
  ages: Clock,
  battles: Sword,
  characters: Users,
  civilizations: Landmark,
};

export default async function EntityDetailPage({ params }: PageProps) {
  const { entity, id } = params;
  const numericId = parseInt(id);

  let data: any = null;
  let title = "";
  let description = "";
  let attributes: Record<string, any> = {};

  try {
    switch (entity) {
      case "ages":
        data = await ageService.getById(numericId);
        title = data?.name || "Era Desconocida";
        description = data?.overview || data?.summary || "";
        attributes = {
          Período: data?.date,
        };
        break;
      case "battles":
        data = await battleService.getById(numericId);
        title = data?.name || "Batalla Desconocida";
        description = data?.detailedDescription || data?.summary || "";
        attributes = {
          Fecha: data?.date,
          Resultado: getMetadata("battle", "outcome", data?.outcome || 0),
        };
        break;
      case "civilizations":
        data = await civilizationService.getById(numericId);
        title = data?.name || "Civilización Desconocida";
        description = data?.overview || data?.summary || "";
        attributes = {
          "Estado Histórico": data?.state || "Desconocido",
          Territorios: data?.territories?.length
            ? data.territories.join(", ")
            : "N/A",
          "Eras Asociadas": data?.ages?.length
            ? data.ages.map((age: any) => age.name).join(", ")
            : "N/A",
          "Personajes Asociados": data?.characters?.length
            ? data.characters.map((char: any) => char.name).join(", ")
            : "N/A",
          "Batallas Relacionadas": data?.battles?.length
            ? data.battles.map((battle: any) => battle.name).join(", ")
            : "N/A",
        };
        break;
      case "characters":
        data = await characterService.getById(numericId);
        title = data?.name || "Personaje Desconocido";
        description = data?.description || "";
        attributes = {
          "Título Honorífico": data?.honorificTitle,
          "Período de Vida": data?.lifePeriod,
        };
        break;
    }
  } catch (error) {
    console.error("Error fetching entity detail:", error);
  }

  if (!data) {
    return (
      <main className="min-h-screen marble-bg flex items-center justify-center">
        <div className="text-center">
          <h1 className="font-serif text-3xl text-[#af944d] mb-4">
            Registro Histórico de No Encontrado
          </h1>
          <Link href="/">
            <Button variant="outline" className="btn-classical-secondary">
              Regresar a los Archivos
            </Button>
          </Link>
        </div>
      </main>
    );
  }

  const Icon = entityIcons[entity] || Landmark;

  return (
    <main className="min-h-screen marble-bg py-12 md:py-20">
      <div className="max-w-6xl mx-auto px-4">
        {/* Navigation */}
        <div className="mb-12">
          <Link href="/">
            <Button
              variant="ghost"
              className="text-[#af944d] hover:text-[#e6d28a] hover:bg-transparent -ml-4 flex gap-2 font-serif uppercase tracking-widest text-xs"
            >
              <ArrowLeft className="h-4 w-4" /> Volver a la Cronología
            </Button>
          </Link>
        </div>

        {/* Hero Section */}
        <div className="vt-card-container classical-card p-0 md:p-1 overflow-hidden relative mb-12">
          <div className="bg-[#fdfaf3]/80 dark:bg-[#1a1816]/90 p-8 md:p-16 backdrop-blur-md">
            <div className="flex flex-col md:flex-row gap-8 items-center md:items-start text-center md:text-left">
              <div className="p-6 bg-amber-50 dark:bg-[#2d2a25] border border-[#af944d]/30 rounded-[2px] shrink-0 transform -rotate-3">
                <Icon className="h-12 w-12 text-[#af944d]" />
              </div>
              <div className="flex-1">
                <h1 className="font-serif text-5xl md:text-7xl text-slate-900 dark:text-[#e6d28a] mb-6 uppercase tracking-tight vt-entity-title leading-tight">
                  {title}
                </h1>
                <div className="w-24 h-[1px] bg-[#af944d] mb-8 mx-auto md:mx-0" />
                <p className="font-serif text-xl md:text-2xl italic text-[#af944d] dark:text-[#c4a962]">
                  Archivo Oficial de la{" "}
                  {entity.slice(0, -1).charAt(0).toUpperCase() +
                    entity.slice(0, -1).slice(1)}
                </p>
              </div>
            </div>
          </div>
          <div className="absolute bottom-0 left-0 right-0">
            <KintsugiDivider />
          </div>
        </div>

        {/* Content Grid */}
        <div className="detail-grid items-start">
          {/* Narrative Column */}
          <section className="space-y-8">
            <h2 className="font-serif text-3xl text-slate-900 dark:text-[#e6d28a] flex items-center gap-4">
              Visión General
              <span className="h-[1px] flex-1 bg-[#af944d]/20"></span>
            </h2>
            <div className="prose prose-slate dark:prose-invert max-w-none">
              <p className="font-sans text-lg leading-loose text-slate-700 dark:text-[#b8b09d] first-letter:text-5xl first-letter:font-serif first-letter:mr-3 first-letter:float-left first-letter:text-[#af944d]">
                {description}
              </p>
            </div>

            {/* Si es una era o batalla, podríamos añadir más bloques aquí en el futuro */}
          </section>

          {/* Technical Data Column */}
          <aside className="space-y-8 sticky top-24">
            <Card className="classical-card p-0">
              <CardHeader className="bg-[#af944d]/5 border-b border-[#af944d]/10 py-6">
                <CardTitle className="font-serif text-xl uppercase tracking-widest text-[#af944d]">
                  Ficha Técnica
                </CardTitle>
              </CardHeader>
              <CardContent className="p-8 space-y-8">
                {Object.entries(attributes).map(([key, value]) => {
                  const isMetadata = typeof value === "object" && value?.label;
                  return (
                    <div key={key} className="space-y-2">
                      <span className="font-serif text-[11px] font-bold tracking-widest uppercase text-[#af944d]/60 mb-1 block">
                        {key}
                      </span>
                      <div className="flex flex-col gap-2">
                        <span
                          className={`font-sans text-lg font-medium ${isMetadata ? "text-slate-900 dark:text-[#e6d28a]" : "text-slate-700 dark:text-white"}`}
                        >
                          {isMetadata
                            ? value.label
                            : value || "Registro Pendiente"}
                        </span>
                        {isMetadata && value.desc && (
                          <p className="text-xs text-slate-500 dark:text-muted-foreground italic border-l-2 border-[#af944d]/20 pl-3">
                            {value.desc}
                          </p>
                        )}
                      </div>
                    </div>
                  );
                })}

                <div className="pt-8 border-t border-[#af944d]/10">
                  <p className="text-[10px] uppercase tracking-tighter text-slate-400 text-center font-sans">
                    Autenticado por los registros de Forgotten Empires
                  </p>
                </div>
              </CardContent>
            </Card>
          </aside>
        </div>
      </div>
    </main>
  );
}
