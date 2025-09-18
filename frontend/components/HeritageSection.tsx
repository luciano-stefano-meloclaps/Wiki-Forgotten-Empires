import KintsugiDivider from "./KintsugiDivider"

export default function HeritageSection() {
  return (
    <section id="heritage" className="py-20 relative">
      <div className="absolute top-0 left-0 right-0">
        <KintsugiDivider />
      </div>

      <div className="max-w-7xl mx-auto px-4">
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-12 items-center">
          {/* Text Content */}
          <div className="space-y-6">
            <h2 className="font-serif font-bold text-4xl md:text-5xl text-foreground mb-6">
              El <span className="golden-shine">Legado</span> de los Imperios Olvidados
            </h2>

            <div className="space-y-4 font-sans text-lg text-muted-foreground leading-relaxed">
              <p>
                Nuestra API representa años de investigación meticulosa, combinando fuentes históricas verificadas con
                tecnología moderna para preservar el conocimiento de civilizaciones que han marcado el curso de la
                humanidad.
              </p>

              <p>
                Desde las batallas más épicas hasta los personajes más influyentes, cada dato ha sido cuidadosamente
                catalogado y estructurado para ofrecer una experiencia de exploración histórica sin precedentes.
              </p>
            </div>

            <div className="pt-6">
              <div className="space-y-4">
                <h3 className="font-display font-semibold text-xl text-foreground mb-3">Características Principales</h3>
                <ul className="space-y-2 font-sans text-muted-foreground">
                  <li className="flex items-center">
                    <span className="w-2 h-2 bg-accent rounded-full mr-3"></span>
                    Datos históricos verificados y actualizados
                  </li>
                  <li className="flex items-center">
                    <span className="w-2 h-2 bg-accent rounded-full mr-3"></span>
                    API RESTful con documentación completa
                  </li>
                  <li className="flex items-center">
                    <span className="w-2 h-2 bg-accent rounded-full mr-3"></span>
                    Búsqueda avanzada por múltiples criterios
                  </li>
                  <li className="flex items-center">
                    <span className="w-2 h-2 bg-accent rounded-full mr-3"></span>
                    Integración sencilla para desarrolladores
                  </li>
                  <li className="flex items-center">
                    <span className="w-2 h-2 bg-accent rounded-full mr-3"></span>
                    Soporte para proyectos educativos y comerciales
                  </li>
                </ul>
              </div>
            </div>
          </div>

          {/* Visual Content */}
          <div className="relative">
            <div className="glass rounded-xl p-8 text-center luxury-hover">
              <div className="mb-6">
                <div className="w-32 h-32 mx-auto rounded-full bg-gradient-to-br from-accent to-accent/60 flex items-center justify-center pulse-glow float-animation">
                  <span className="font-decorative text-4xl text-accent-foreground">★</span>
                </div>
              </div>

              <h3 className="font-display font-semibold text-2xl text-foreground mb-4">Preservando la Historia</h3>

              <p className="font-sans text-muted-foreground mb-6">
                Un archivo digital que conecta el pasado con el presente, ofreciendo acceso democrático al conocimiento
                histórico.
              </p>

              <div className="grid grid-cols-3 gap-4 text-center">
                <div className="p-4 glass rounded-lg luxury-hover">
                  <div className="font-decorative text-3xl golden-shine mb-2">500+</div>
                  <div className="font-sans text-xs text-muted-foreground">Eventos</div>
                </div>
                <div className="p-4 glass rounded-lg luxury-hover">
                  <div className="font-decorative text-3xl golden-shine mb-2">50+</div>
                  <div className="font-sans text-xs text-muted-foreground">Culturas</div>
                </div>
                <div className="p-4 glass rounded-lg luxury-hover">
                  <div className="font-decorative text-3xl golden-shine mb-2">10+</div>
                  <div className="font-sans text-xs text-muted-foreground">Milenios</div>
                </div>
              </div>
            </div>

            {/* Decorative Kintsugi elements */}
            <div className="absolute -top-4 -right-4 w-8 h-8 bg-gradient-to-br from-accent to-accent/60 rounded-full opacity-60 float-animation"></div>
            <div
              className="absolute -bottom-4 -left-4 w-6 h-6 bg-gradient-to-br from-accent to-accent/60 rounded-full opacity-40 float-animation"
              style={{ animationDelay: "2s" }}
            ></div>
          </div>
        </div>
      </div>

      <div className="absolute bottom-0 left-0 right-0">
        <KintsugiDivider />
      </div>
    </section>
  )
}
