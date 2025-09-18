"use client"

import { useState, useEffect } from "react"
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { Skeleton } from "@/components/ui/skeleton"
import { AlertCircle, RefreshCw } from "lucide-react"
import KintsugiDivider from "./KintsugiDivider"

type EntityType = "battles" | "civilizations" | "ages" | "characters"

interface DataItem {
  id: string
  name: string
  description: string
  attributes: Record<string, string>
}

const mockData: Record<EntityType, DataItem[]> = {
  battles: [
    {
      id: "1",
      name: "Batalla de Hastings",
      description: "Batalla decisiva que cambió el curso de la historia inglesa",
      attributes: {
        fecha: "14 de octubre de 1066",
        participantes: "Normandos vs Anglosajones",
        resultado: "Victoria normanda",
        ubicación: "Hastings, Inglaterra",
      },
    },
    {
      id: "2",
      name: "Batalla de Waterloo",
      description: "La batalla final de Napoleón Bonaparte",
      attributes: {
        fecha: "18 de junio de 1815",
        participantes: "Francia vs Coalición",
        resultado: "Derrota francesa",
        ubicación: "Waterloo, Bélgica",
      },
    },
  ],
  civilizations: [
    {
      id: "1",
      name: "Imperio Romano",
      description: "Una de las civilizaciones más influyentes de la historia",
      attributes: {
        región: "Mediterráneo",
        era: "Antigüedad Clásica",
        estado: "Extinto",
        tecnología: "Avanzada para su época",
      },
    },
    {
      id: "2",
      name: "Imperio Bizantino",
      description: "Continuación del Imperio Romano en Oriente",
      attributes: {
        región: "Anatolia y Balcanes",
        era: "Medieval",
        estado: "Extinto",
        tecnología: "Sofisticada",
      },
    },
  ],
  ages: [
    {
      id: "1",
      name: "Edad Media",
      description: "Período histórico entre la Antigüedad y el Renacimiento",
      attributes: {
        período: "476 - 1453 d.C.",
        rasgos: "Feudalismo, Cristiandad",
        eventos: "Cruzadas, Peste Negra",
        tecnología: "Limitada pero innovadora",
      },
    },
    {
      id: "2",
      name: "Renacimiento",
      description: "Época de renovación cultural y artística",
      attributes: {
        período: "1400 - 1600 d.C.",
        rasgos: "Humanismo, Arte",
        eventos: "Descubrimientos, Reforma",
        tecnología: "Imprenta, Navegación",
      },
    },
  ],
  characters: [
    {
      id: "1",
      name: "Julio César",
      description: "General y político romano que cambió la historia",
      attributes: {
        rol: "Dictador de Roma",
        destino: "Asesinado",
        afiliación: "República Romana",
        logros: "Conquista de las Galias",
      },
    },
    {
      id: "2",
      name: "Carlomagno",
      description: "Emperador del Sacro Imperio Romano Germánico",
      attributes: {
        rol: "Emperador",
        destino: "Muerte natural",
        afiliación: "Imperio Carolingio",
        logros: "Unificación de Europa Occidental",
      },
    },
  ],
}

export default function DataSection() {
  const [selectedEntity, setSelectedEntity] = useState<EntityType>("battles")
  const [data, setData] = useState<DataItem[]>([])
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)

  const fetchData = async (entityType: EntityType) => {
    setLoading(true)
    setError(null)

    try {
      // Simulate API call delay
      await new Promise((resolve) => setTimeout(resolve, 1000))

      // For demo purposes, use mock data
      // In real implementation, this would be:
      // const response = await fetch(`/api/${entityType}`)
      // const result = await response.json()

      setData(mockData[entityType] || [])
    } catch (err) {
      setError("Error al cargar los datos. Por favor, inténtalo de nuevo.")
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    fetchData(selectedEntity)
  }, [selectedEntity])

  // Listen for entity selection from EntitiesSection
  useEffect(() => {
    const handleEntitySelection = (event: CustomEvent) => {
      const entityMap: Record<string, EntityType> = {
        batallas: "battles",
        civilizaciones: "civilizations",
        eras: "ages",
        personajes: "characters",
      }

      const entityType = entityMap[event.detail]
      if (entityType) {
        setSelectedEntity(entityType)
      }
    }

    window.addEventListener("selectEntity", handleEntitySelection as EventListener)
    return () => window.removeEventListener("selectEntity", handleEntitySelection as EventListener)
  }, [])

  const entityLabels: Record<EntityType, string> = {
    battles: "Batallas",
    civilizations: "Civilizaciones",
    ages: "Eras",
    characters: "Personajes",
  }

  return (
    <section id="data" className="py-20 relative">
      <div className="absolute top-0 left-0 right-0">
        <KintsugiDivider />
      </div>

      <div className="max-w-7xl mx-auto px-4">
        <div className="text-center mb-16">
          <h2 className="font-serif font-bold text-4xl md:text-5xl text-foreground mb-6">
            Explora los <span className="golden-shine">Datos</span>
          </h2>
          <p className="font-display text-lg md:text-xl text-muted-foreground max-w-3xl mx-auto">
            Selecciona una entidad para explorar nuestros datos históricos. Cada registro contiene información
            verificada y detallada.
          </p>
        </div>

        {/* Entity Selector */}
        <div className="flex justify-center mb-12">
          <div
            className="bg-white dark:bg-slate-800 rounded-2xl p-2 shadow-lg border-2"
            style={{ borderColor: "#b7992a" }}
          >
            <select
              value={selectedEntity}
              onChange={(e) => setSelectedEntity(e.target.value as EntityType)}
              className="px-4 py-3 bg-transparent text-slate-900 dark:text-slate-100 font-medium text-lg border-none outline-none cursor-pointer min-w-[200px] rounded-xl"
            >
              {(Object.keys(entityLabels) as EntityType[]).map((entityType) => (
                <option key={entityType} value={entityType}>
                  {entityLabels[entityType]}
                </option>
              ))}
            </select>
          </div>
        </div>

        {/* Data Display */}
        <div className="space-y-6">
          {loading && (
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              {[1, 2, 3, 4].map((i) => (
                <Card key={i} className="glass rounded-xl border-accent/40">
                  <CardHeader>
                    <Skeleton className="h-6 w-3/4 mb-2" />
                    <Skeleton className="h-4 w-full" />
                  </CardHeader>
                  <CardContent>
                    <div className="space-y-2">
                      <Skeleton className="h-4 w-full" />
                      <Skeleton className="h-4 w-2/3" />
                      <Skeleton className="h-4 w-3/4" />
                    </div>
                  </CardContent>
                </Card>
              ))}
            </div>
          )}

          {error && (
            <Card className="glass rounded-xl border-destructive/40 bg-destructive/5">
              <CardContent className="p-6 text-center">
                <AlertCircle className="h-12 w-12 text-destructive mx-auto mb-4" />
                <h3 className="font-display font-semibold text-lg text-destructive mb-2">Error al cargar datos</h3>
                <p className="font-sans text-muted-foreground mb-4">{error}</p>
                <Button
                  onClick={() => fetchData(selectedEntity)}
                  variant="outline"
                  className="border-destructive text-destructive hover:bg-destructive hover:text-destructive-foreground"
                >
                  <RefreshCw className="h-4 w-4 mr-2" />
                  Reintentar
                </Button>
              </CardContent>
            </Card>
          )}

          {!loading && !error && data.length === 0 && (
            <Card className="glass rounded-xl border-accent/40">
              <CardContent className="p-12 text-center">
                <h3 className="font-display font-semibold text-xl text-foreground mb-2">No hay datos disponibles</h3>
                <p className="font-sans text-muted-foreground">
                  No se encontraron registros para {entityLabels[selectedEntity].toLowerCase()}.
                </p>
              </CardContent>
            </Card>
          )}

          {!loading && !error && data.length > 0 && (
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              {data.map((item) => (
                <Card
                  key={item.id}
                  className="bg-white dark:bg-slate-800 rounded-2xl border-2 hover:shadow-xl transition-all duration-300"
                  style={{ borderColor: "#b7992a" }}
                >
                  <CardHeader>
                    <CardTitle className="font-display text-xl text-slate-900 dark:text-slate-100 mb-2">
                      {item.name}
                    </CardTitle>
                    <CardDescription className="font-sans text-slate-600 dark:text-slate-300">
                      {item.description}
                    </CardDescription>
                  </CardHeader>
                  <CardContent>
                    <div className="space-y-3">
                      {Object.entries(item.attributes).map(([key, value]) => (
                        <div key={key} className="flex justify-between items-start">
                          <span
                            className="font-sans font-semibold capitalize px-3 py-1.5 text-xs border" // Smaller tags matching EntitiesSection
                            style={{
                              backgroundColor: "#f8f5ed",
                              color: "#bb9e47",
                              borderColor: "#ebe3cb",
                              borderWidth: "1px",
                              borderRadius: "8px", // More rounded tags
                            }}
                          >
                            {key}:
                          </span>
                          <span className="font-sans text-slate-700 dark:text-slate-300 text-right max-w-[60%]">
                            {value}
                          </span>
                        </div>
                      ))}
                    </div>
                  </CardContent>
                </Card>
              ))}
            </div>
          )}
        </div>
      </div>

      <div className="absolute bottom-0 left-0 right-0">
        <KintsugiDivider />
      </div>
    </section>
  )
}
