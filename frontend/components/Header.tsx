"use client"

import { useState } from "react"
import { Menu, X, Moon, Sun, Github } from "lucide-react"
import { Button } from "@/components/ui/button"
import { useTheme } from "@/hooks/use-theme"
import KintsugiDivider from "./KintsugiDivider"

export default function Header() {
  const [isMenuOpen, setIsMenuOpen] = useState(false)
  const { theme, toggleTheme } = useTheme()

  const scrollToSection = (sectionId: string) => {
    const element = document.getElementById(sectionId)
    if (element) {
      element.scrollIntoView({ behavior: "smooth" })
    }
    setIsMenuOpen(false)
  }

  const openGitHub = () => {
    window.open("https://github.com/luciano-meloclaps/Wiki-Forgotten-Empires.git", "_blank")
  }

  return (
    <header className="fixed top-0 left-0 right-0 z-50 h-[90px] glass">
      <div className="max-w-7xl mx-auto px-4 h-full flex items-center justify-between">
        {/* Logo */}
        <h1 className="font-serif font-bold text-2xl md:text-3xl text-foreground tracking-wide">
          Forgotten <span className="golden-shine">Empires</span>
        </h1>

        {/* Desktop Navigation */}
        <nav className="hidden md:flex items-center space-x-8">
          <button
            onClick={() => scrollToSection("hero")}
            className="font-sans text-foreground hover:text-accent transition-all duration-300 luxury-hover relative group"
          >
            Inicio
            <span className="absolute bottom-0 left-0 w-0 h-0.5 bg-gradient-to-r from-accent to-accent/60 transition-all duration-300 group-hover:w-full"></span>
          </button>
          <button
            onClick={() => scrollToSection("entities")}
            className="font-sans text-foreground hover:text-accent transition-all duration-300 luxury-hover relative group"
          >
            Entidades
            <span className="absolute bottom-0 left-0 w-0 h-0.5 bg-gradient-to-r from-accent to-accent/60 transition-all duration-300 group-hover:w-full"></span>
          </button>
          <button
            onClick={() => scrollToSection("heritage")}
            className="font-sans text-foreground hover:text-accent transition-all duration-300 luxury-hover relative group"
          >
            Herencia
            <span className="absolute bottom-0 left-0 w-0 h-0.5 bg-gradient-to-r from-accent to-accent/60 transition-all duration-300 group-hover:w-full"></span>
          </button>
          <button
            onClick={() => scrollToSection("data")}
            className="font-sans text-foreground hover:text-accent transition-all duration-300 luxury-hover relative group"
          >
            Datos
            <span className="absolute bottom-0 left-0 w-0 h-0.5 bg-gradient-to-r from-accent to-accent/60 transition-all duration-300 group-hover:w-full"></span>
          </button>

          <Button
            variant="ghost"
            size="icon"
            onClick={openGitHub}
            className="text-foreground hover:text-accent hover:bg-accent/10 transition-all duration-300 luxury-hover w-10 h-10"
            aria-label="Ver repositorio en GitHub"
          >
            <Github className="h-5 w-5" />
          </Button>

          <Button
            variant="ghost"
            size="icon"
            onClick={toggleTheme}
            className="text-foreground hover:text-accent hover:bg-accent/10 transition-all duration-300 luxury-hover w-10 h-10"
            aria-label={theme === "light" ? "Cambiar a tema oscuro" : "Cambiar a tema claro"}
          >
            {theme === "light" ? <Moon className="h-5 w-5" /> : <Sun className="h-5 w-5" />}
          </Button>
        </nav>

        {/* Mobile Menu and Theme Toggle */}
        <div className="md:hidden flex items-center space-x-2">
          <Button
            variant="ghost"
            size="icon"
            onClick={openGitHub}
            className="text-foreground hover:text-accent hover:bg-accent/10 transition-all duration-300 w-10 h-10"
            aria-label="Ver repositorio en GitHub"
          >
            <Github className="h-5 w-5" />
          </Button>
          <Button
            variant="ghost"
            size="icon"
            onClick={toggleTheme}
            className="text-foreground hover:text-accent hover:bg-accent/10 transition-all duration-300 w-10 h-10"
            aria-label={theme === "light" ? "Cambiar a tema oscuro" : "Cambiar a tema claro"}
          >
            {theme === "light" ? <Moon className="h-5 w-5" /> : <Sun className="h-5 w-5" />}
          </Button>
          <Button variant="ghost" size="icon" onClick={() => setIsMenuOpen(!isMenuOpen)} className="w-10 h-10">
            {isMenuOpen ? <X className="h-6 w-6" /> : <Menu className="h-6 w-6" />}
          </Button>
        </div>
      </div>

      {/* Mobile Navigation */}
      {isMenuOpen && (
        <div className="md:hidden glass border-t border-border">
          <nav className="flex flex-col space-y-4 p-4">
            <button
              onClick={() => scrollToSection("hero")}
              className="font-sans text-foreground hover:text-accent transition-colors duration-300 text-left"
            >
              Inicio
            </button>
            <button
              onClick={() => scrollToSection("entities")}
              className="font-sans text-foreground hover:text-accent transition-colors duration-300 text-left"
            >
              Entidades
            </button>
            <button
              onClick={() => scrollToSection("heritage")}
              className="font-sans text-foreground hover:text-accent transition-colors duration-300 text-left"
            >
              Herencia
            </button>
            <button
              onClick={() => scrollToSection("data")}
              className="font-sans text-foreground hover:text-accent transition-colors duration-300 text-left"
            >
              Datos
            </button>
          </nav>
        </div>
      )}

      {/* Kintsugi line under header */}
      <div className="absolute bottom-0 left-0 right-0">
        <KintsugiDivider />
      </div>
    </header>
  )
}
