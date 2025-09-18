"use client"

import { Github, ExternalLink } from "lucide-react"
import { Button } from "@/components/ui/button"

export default function Footer() {
  const currentYear = new Date().getFullYear()

  return (
    <footer className="bg-primary text-primary-foreground py-16">
      <div className="max-w-7xl mx-auto px-4">
        <div className="grid grid-cols-1 md:grid-cols-3 gap-8 mb-8">
          {/* Brand */}
          <div className="space-y-4">
            <h3 className="font-serif font-bold text-2xl">Forgotten Empires</h3>
            <p className="font-sans text-primary-foreground/80 leading-relaxed">
              Preservando la historia a través de la tecnología moderna. Accede a miles de registros históricos
              verificados.
            </p>
          </div>

          {/* Links */}
          <div className="space-y-4">
            <h4 className="font-display font-semibold text-lg">Recursos</h4>
            <div className="space-y-2">
              <Button
                variant="ghost"
                className="text-primary-foreground/80 hover:text-primary-foreground hover:bg-primary-foreground/10 p-0 h-auto font-sans justify-start"
                onClick={() => window.open("https://github.com/luciano-meloclaps/Wiki-Forgotten-Empires.git", "_blank")}
              >
                <Github className="h-4 w-4 mr-2" />
                Repositorio GitHub
              </Button>
              <Button
                variant="ghost"
                className="text-primary-foreground/80 hover:text-primary-foreground hover:bg-primary-foreground/10 p-0 h-auto font-sans justify-start"
                onClick={() => window.open("https://github.com/luciano-meloclaps/Wiki-Forgotten-Empires.git", "_blank")}
              >
                <ExternalLink className="h-4 w-4 mr-2" />
                Documentación API
              </Button>
            </div>
          </div>

          {/* Contact */}
          <div className="space-y-4">
            <h4 className="font-display font-semibold text-lg">Contacto</h4>
            <div className="space-y-2 font-sans text-primary-foreground/80">
              <p>¿Tienes preguntas sobre la API?</p>
              <p>¿Quieres contribuir al proyecto?</p>
              <Button
                variant="outline"
                className="border-accent text-accent hover:bg-accent hover:text-accent-foreground mt-4 bg-transparent"
                onClick={() =>
                  window.open("https://github.com/luciano-meloclaps/Wiki-Forgotten-Empires.git/issues", "_blank")
                }
              >
                Abrir Issue en GitHub
              </Button>
            </div>
          </div>
        </div>

        {/* Bottom Bar */}
        <div className="border-t border-primary-foreground/20 pt-8 flex flex-col md:flex-row justify-between items-center">
          <p className="font-sans text-primary-foreground/60 text-sm">
            © {currentYear} Forgotten Empires. Todos los derechos reservados.
          </p>
          <div className="flex items-center space-x-4 mt-4 md:mt-0">
            <span className="font-sans text-primary-foreground/60 text-sm">Hecho con ❤️ para la historia</span>
          </div>
        </div>
      </div>
    </footer>
  )
}
