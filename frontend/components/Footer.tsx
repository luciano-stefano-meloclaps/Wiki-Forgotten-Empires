"use client"

import { Github, ExternalLink } from "lucide-react"
import { Button } from "@/components/ui/button"

export default function Footer() {
  const currentYear = new Date().getFullYear()

  return (
    <footer className="bg-[#2a2520] dark:bg-[#100f0d] text-[#f0ebe0] py-16 border-t border-[#af944d]/30">
      <div className="max-w-7xl mx-auto px-4">
        <div className="grid grid-cols-1 md:grid-cols-3 gap-8 mb-12">
          {/* Brand */}
          <div className="space-y-4">
            <h3 className="font-serif font-bold text-2xl tracking-wide">
              Forgotten <span className="text-[#e6d28a]">Empires</span>
            </h3>
            <p className="font-sans text-[#b8b09d] leading-relaxed text-sm">
              Preservando la historia a través de la tecnología moderna. Accede a miles de registros históricos
              verificados.
            </p>
          </div>

          {/* Links */}
          <div className="space-y-4">
            <h4 className="font-serif uppercase tracking-widest text-sm text-[#af944d]">Recursos</h4>
            <div className="space-y-3">
              <Button
                variant="ghost"
                className="text-[#b8b09d] hover:text-[#e6d28a] hover:bg-[#af944d]/5 p-0 h-auto font-sans justify-start transition-colors duration-300"
                onClick={() => window.open("https://github.com/luciano-meloclaps/Wiki-Forgotten-Empires.git", "_blank")}
              >
                <Github className="h-4 w-4 mr-2" />
                Repositorio GitHub
              </Button>
              <br />
              <Button
                variant="ghost"
                className="text-[#b8b09d] hover:text-[#e6d28a] hover:bg-[#af944d]/5 p-0 h-auto font-sans justify-start transition-colors duration-300"
                onClick={() => window.open("https://github.com/luciano-meloclaps/Wiki-Forgotten-Empires.git", "_blank")}
              >
                <ExternalLink className="h-4 w-4 mr-2" />
                Documentación API
              </Button>
            </div>
          </div>

          {/* Contact */}
          <div className="space-y-4">
            <h4 className="font-serif uppercase tracking-widest text-sm text-[#af944d]">Contacto</h4>
            <div className="space-y-2 font-sans text-[#b8b09d] text-sm">
              <p>¿Tienes preguntas sobre la API?</p>
              <p>¿Quieres contribuir al proyecto?</p>
              <Button
                variant="outline"
                className="btn-classical-secondary mt-4 bg-transparent"
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
        <div className="border-t border-[#af944d]/20 pt-8 flex flex-col md:flex-row justify-between items-center">
          <p className="font-sans text-[#b8b09d]/60 text-sm">
            © {currentYear} Forgotten Empires. Todos los derechos reservados.
          </p>
          <div className="flex items-center space-x-4 mt-4 md:mt-0">
            <span className="font-serif italic text-[#af944d]/60 text-sm">In memoriam historiae</span>
          </div>
        </div>
      </div>
    </footer>
  )
}
