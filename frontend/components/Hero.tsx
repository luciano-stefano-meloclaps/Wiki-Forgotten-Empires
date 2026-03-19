"use client"

import { Button } from "@/components/ui/button"
import { ExternalLink, ArrowDown } from "lucide-react"
import KintsugiDivider from "./KintsugiDivider"

export default function Hero() {
  const scrollToSection = (sectionId: string) => {
    const element = document.getElementById(sectionId)
    if (element) {
      element.scrollIntoView({ behavior: "smooth" })
    }
  }

  const openGitHub = () => {
    window.open("https://github.com/luciano-meloclaps/Wiki-Forgotten-Empires.git", "_blank")
  }

  return (
    <section id="hero" className="min-h-screen flex items-center justify-center relative overflow-hidden pt-[90px]">
      {/* Marble background with Kintsugi veins */}
      <div className="absolute inset-0 marble-bg">
        <div className="absolute inset-0 opacity-20">
          <svg width="100%" height="100%" viewBox="0 0 1200 800" className="absolute inset-0">
            <defs>
              <linearGradient id="kintsugiVein" x1="0%" y1="0%" x2="100%" y2="100%">
                <stop offset="0%" stopColor="#BFA14A" stopOpacity="0.6" />
                <stop offset="50%" stopColor="#E6D28A" stopOpacity="0.8" />
                <stop offset="100%" stopColor="#BFA14A" stopOpacity="0.6" />
              </linearGradient>
            </defs>
            <path
              d="M0,200 Q300,100 600,300 T1200,250"
              stroke="url(#kintsugiVein)"
              strokeWidth="3"
              fill="none"
              className="animate-pulse"
            />
            <path
              d="M0,500 Q400,400 800,600 T1200,550"
              stroke="url(#kintsugiVein)"
              strokeWidth="2"
              fill="none"
              className="animate-pulse"
              style={{ animationDelay: "1s" }}
            />
          </svg>
        </div>
      </div>

      <div className="max-w-7xl mx-auto px-4 text-center relative z-10">
        <h1 className="font-serif font-bold text-5xl md:text-7xl lg:text-8xl text-foreground mb-6 tracking-wide">
          Forgotten <span className="golden-shine">Empires</span>
        </h1>

        <p className="font-display text-xl md:text-2xl lg:text-3xl text-muted-foreground mb-12 max-w-4xl mx-auto leading-relaxed">
          Descubre batallas épicas, personajes legendarios, civilizaciones poderosas y eras históricas a través de
          nuestra <span className="golden-shine font-semibold">API completa</span>
        </p>

        <div className="flex flex-col sm:flex-row gap-6 justify-center mb-16">
          <Button
            onClick={() => scrollToSection("entities")}
            className="btn-classical-primary px-12 py-6 text-lg min-w-[280px] h-20"
          >
            <ArrowDown className="mr-2 h-5 w-5" />
            Explorar API
          </Button>

          <Button
            onClick={openGitHub}
            variant="outline"
            className="btn-classical-secondary px-12 py-6 text-lg bg-transparent min-w-[280px] h-20"
          >
            <ExternalLink className="mr-2 h-5 w-5" />
            Documentación
          </Button>
        </div>

        <div className="grid grid-cols-2 md:grid-cols-4 gap-8 max-w-6xl mx-auto">
          <div className="kpi-card-rectangular">
            <div className="sacred-number-rectangular">500+</div>
            <div className="font-sans text-base md:text-lg text-muted-foreground">Batallas</div>
          </div>
          <div className="kpi-card-rectangular">
            <div className="sacred-number-rectangular">200+</div>
            <div className="font-sans text-base md:text-lg text-muted-foreground">Personajes</div>
          </div>
          <div className="kpi-card-rectangular">
            <div className="sacred-number-rectangular">50+</div>
            <div className="font-sans text-base md:text-lg text-muted-foreground">Civilizaciones</div>
          </div>
          <div className="kpi-card-rectangular">
            <div className="sacred-number-rectangular">10+</div>
            <div className="font-sans text-base md:text-lg text-muted-foreground">Eras</div>
          </div>
        </div>
      </div>

      {/* Bottom divider */}
      <div className="absolute bottom-0 left-0 right-0">
        <KintsugiDivider />
      </div>
    </section>
  )
}
