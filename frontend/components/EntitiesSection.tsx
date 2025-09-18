"use client"

import { Button } from "@/components/ui/button"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { Sword, Crown, Clock, Users } from "lucide-react"
import KintsugiDivider from "./KintsugiDivider"

const entities = [
  {
    title: "Batallas",
    icon: Sword,
    description: "Conflictos épicos que definieron el curso de la historia",
    attributes: ["Fecha", "Participantes", "Resultado", "Ubicación", "Estrategias"],
    color: "text-red-500",
  },
  {
    title: "Civilizaciones",
    icon: Crown,
    description: "Imperios y culturas que dominaron diferentes épocas",
    attributes: ["Región", "Era", "Estado", "Tecnologías", "Legado"],
    color: "text-blue-500",
  },
  {
    title: "Eras",
    icon: Clock,
    description: "Períodos históricos con características distintivas",
    attributes: ["Período", "Rasgos", "Innovaciones", "Eventos", "Transiciones"],
    color: "text-green-500",
  },
  {
    title: "Personajes",
    icon: Users,
    description: "Figuras legendarias que marcaron la historia",
    attributes: ["Rol", "Resultado", "Afiliación", "Hazañas", "Legado"],
    color: "text-purple-500",
  },
]

export default function EntitiesSection() {
  const scrollToData = (entityType: string) => {
    const element = document.getElementById("data")
    if (element) {
      element.scrollIntoView({ behavior: "smooth" })
      // Simulate entity selection after scroll
      setTimeout(() => {
        const event = new CustomEvent("selectEntity", { detail: entityType })
        window.dispatchEvent(event)
      }, 1000)
    }
  }

  return (
    <section
      id="entities"
      className="py-20 relative"
      style={{
        background: "linear-gradient(to bottom, #f2ede4, #e8e3da)",
      }}
    >
      <div className="absolute top-0 left-0 right-0">
        <KintsugiDivider />
      </div>

      <div className="max-w-7xl mx-auto px-4">
        <div className="text-center mb-16">
          <h2 className="font-serif font-bold text-4xl md:text-5xl text-foreground mb-6">
            Explora Nuestras <span className="golden-shine">Entidades</span>
          </h2>
          <p className="font-display text-lg md:text-xl text-muted-foreground max-w-3xl mx-auto">
            Sumérgete en un universo de datos históricos cuidadosamente catalogados. Cada entidad contiene información
            detallada y verificada.
          </p>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
          {entities.map((entity, index) => {
            const IconComponent = entity.icon
            return (
              <Card
                key={entity.title}
                className="bg-white dark:bg-slate-700 border-3 transition-all duration-300 hover:shadow-xl group cursor-pointer"
                style={{
                  borderColor: "#b7992a",
                  borderWidth: "2px",
                  borderRadius: "4px", // More square borders for formal look
                }}
              >
                <CardHeader className="text-center pb-4">
                  <div className="flex justify-center mb-4">
                    <div
                      className="p-4 bg-amber-50 dark:bg-amber-950 transition-colors duration-300"
                      style={{ borderRadius: "4px" }}
                    >
                      <IconComponent
                        className={`h-8 w-8 ${entity.color} group-hover:text-[#b7992a] transition-colors duration-300`}
                      />
                    </div>
                  </div>
                  <CardTitle className="font-display text-2xl text-slate-900 dark:text-[#e6d28a] mb-2 group-hover:text-[#b7992a] transition-colors duration-300">
                    {entity.title}
                  </CardTitle>
                  <CardDescription className="font-sans text-slate-600 dark:text-slate-300">
                    {entity.description}
                  </CardDescription>
                </CardHeader>

                <CardContent className="space-y-4">
                  <div className="flex flex-wrap gap-2">
                    {entity.attributes.map((attr) => (
                      <span
                        key={attr}
                        className="px-3 py-1.5 text-xs font-sans font-semibold border" // Smaller tags
                        style={{
                          backgroundColor: "#f8f5ed",
                          color: "#bb9e47",
                          borderColor: "#ebe3cb",
                          borderWidth: "1px",
                          borderRadius: "8px", // More rounded tags
                        }}
                      >
                        {attr}
                      </span>
                    ))}
                  </div>

                  <Button
                    onClick={() => scrollToData(entity.title.toLowerCase())}
                    variant="outline"
                    className="w-full border-2 text-accent hover:bg-accent/10 hover:text-accent px-6 py-3 text-lg font-medium shadow-lg hover:shadow-accent/25 transition-all duration-300 bg-transparent golden-glow"
                    style={{
                      borderColor: "#b7992a",
                      color: "#b7992a",
                      borderRadius: "4px", // More square button borders
                    }}
                  >
                    Ver Datos
                  </Button>
                </CardContent>
              </Card>
            )
          })}
        </div>
      </div>

      <div className="absolute bottom-0 left-0 right-0">
        <KintsugiDivider />
      </div>
    </section>
  )
}
