"use client";
import { useState, useEffect } from "react"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { Skeleton } from "@/components/ui/skeleton"
import { Button } from "@/components/ui/button"
import KintsugiDivider from "./KintsugiDivider"
import Link from "next/link"

// Importaciones de servicios
import { ageService } from "@/services/ageService"
import { battleService } from "@/services/battleService"
import { civilizationService } from "@/services/civilizationService"
import { characterService } from "@/services/characterService"

// Importaciones de mappers
import { mapAgesToDataItems } from "@/mappers/ageMapper"
import { mapBattlesToDataItems } from "@/mappers/battleMapper"
import { mapCivilizationsToDataItems } from "@/mappers/civilizationMapper"
import { mapCharactersToDataItems } from "@/mappers/characterMapper"

interface DataItem {
  id: string
  name: string
  description: string
  attributes: Record<string, string>
}

type EntityType = 'ages' | 'battles' | 'civilizations' | 'characters';

const entities = [
  { id: 'ages', label: 'Eras' },
  { id: 'civilizations', label: 'Civilizaciones' },
  { id: 'characters', label: 'Personajes' },
  { id: 'battles', label: 'Batallas' },
] as const;

export default function DataSection() {
  const [data, setData] = useState<DataItem[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)
  const [selectedEntity, setSelectedEntity] = useState<EntityType>('ages')

  useEffect(() => {
    const fetchData = async () => {
      setLoading(true)
      setError(null)
      try {
        let mappedData: DataItem[] = []
        switch (selectedEntity) {
          case 'ages':
            const rawAges = await ageService.getAll()
            mappedData = mapAgesToDataItems(rawAges)
            break
          case 'battles':
            const rawBattles = await battleService.getAll()
            mappedData = mapBattlesToDataItems(rawBattles)
            break
          case 'civilizations':
            const rawCivs = await civilizationService.getAll()
            mappedData = mapCivilizationsToDataItems(rawCivs)
            break
          case 'characters':
            const rawChars = await characterService.getAll()
            mappedData = mapCharactersToDataItems(rawChars)
            break
        }
        setData(mappedData)
      } catch (err) {
        setError("Error al cargar los datos. Por favor, inténtalo de nuevo.")
      } finally {
        setLoading(false)
      }
    }

    fetchData()
  }, [selectedEntity])

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
            Selecciona una categoría para sumergirte en los detalles de nuestro archivo histórico.
          </p>

          <div className="flex flex-wrap justify-center gap-4 mb-12">
            {entities.map((entity) => (
              <Button
                key={entity.id}
                onClick={() => setSelectedEntity(entity.id as EntityType)}
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
        ) : (
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            {data.map((item) => (
              <Link href={`/${selectedEntity}/${item.id}`} key={item.id}>
                <Card
                  className="bg-white dark:bg-slate-800 rounded-2xl border-2 hover:shadow-xl transition-all duration-300 h-full overflow-hidden"
                  style={{ borderColor: "#b7992a" }}
                >
                  <CardHeader>
                    <CardTitle className="font-display text-xl text-slate-900 dark:text-slate-100 mb-2">
                      {item.name}
                    </CardTitle>
                    <CardDescription className="font-sans text-slate-600 dark:text-slate-300 line-clamp-3">
                      {item.description}
                    </CardDescription>
                  </CardHeader>
                  <CardContent>
                    <div className="space-y-3">
                      {Object.entries(item.attributes).map(([key, value]) => (
                        <div key={key} className="flex justify-between items-start gap-4">
                          <span
                            className="font-sans font-semibold capitalize px-3 py-1 text-xs border whitespace-nowrap"
                            style={{
                              backgroundColor: "#f8f5ed", color: "#bb9e47",
                              borderColor: "#ebe3cb", borderWidth: "1px", borderRadius: "8px",
                            }}
                          >
                            {key}:
                          </span>
                          <span className="font-sans text-slate-700 dark:text-slate-300 text-right text-sm line-clamp-2">
                            {value}
                          </span>
                        </div>
                      ))}
                    </div>
                  </CardContent>
                </Card>
              </Link>
            ))}
          </div>
        )}
      </div>

      <div className="absolute bottom-0 left-0 right-0">
        <KintsugiDivider />
      </div>
    </section>
  )
}