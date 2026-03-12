  "use client";
import { useState, useEffect } from "react"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { Skeleton } from "@/components/ui/skeleton"
import KintsugiDivider from "./KintsugiDivider"
import Link from "next/link" // Importamos Link

// 1. Importaciones clave
import { ageService } from "@/services/ageService"
import { mapAgesToDataItems } from "@/mappers/ageMapper"

// 2. El tipo de dato que el componente renderiza
interface DataItem {
  id: string
  name: string
  description: string
  attributes: Record<string, string>
}

export default function DataSection() {
  // 3. Simplificamos el estado, ya no necesitamos 'selectedEntity'
  const [data, setData] = useState<DataItem[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    const fetchData = async () => {
      setLoading(true)
      setError(null)
      try {
        // 4. Llamamos al servicio real y luego al mapper
        const rawAges = await ageService.getAll()
        const mappedData = mapAgesToDataItems(rawAges)
        setData(mappedData)
      } catch (err) {
        setError("Error al cargar los datos. Por favor, inténtalo de nuevo.")
      } finally {
        setLoading(false)
      }
    }

    fetchData()
  }, []) // Se ejecuta solo una vez al cargar el componente

  return (
    <section id="data" className="py-20 relative">
      {/* ... (Tu JSX de título y divisor permanece igual) ... */}
      <div className="max-w-7xl mx-auto px-4">
        <div className="text-center mb-16">
          <h2 className="font-serif font-bold text-4xl md:text-5xl text-foreground mb-6">
            Explora las <span className="golden-shine">Eras</span>
          </h2>
          <p className="font-display text-lg md:text-xl text-muted-foreground max-w-3xl mx-auto">
            Estos son los períodos históricos disponibles en nuestra base de datos.
          </p>
        </div>

        {/* ... (Tu JSX para loading, error y no data permanece igual) ... */}
        
        {/* Modificamos el renderizado de las tarjetas */}
        {!loading && !error && data.length > 0 && (
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            {data.map((item) => (
              // 5. Envolvemos la tarjeta en un Link para ir a la página de detalle
              <Link href={`/ages/${item.id}`} key={item.id}>
                <Card
                  className="bg-white dark:bg-slate-800 rounded-2xl border-2 hover:shadow-xl transition-all duration-300 h-full"
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
                            className="font-sans font-semibold capitalize px-3 py-1.5 text-xs border"
                            style={{
                              backgroundColor: "#f8f5ed", color: "#bb9e47",
                              borderColor: "#ebe3cb", borderWidth: "1px", borderRadius: "8px",
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
              </Link>
            ))}
          </div>
        )}
      </div>
    </section>
  )
}