import KintsugiDivider from "./KintsugiDivider"

export default function HeritageSection() {
  return (
    <section id="heritage" className="py-24 relative bg-gradient-to-b from-[#f0ede6] to-[#f9f7f2] dark:from-[#141210] dark:to-[#1a1816]">
      <div className="absolute top-0 left-0 right-0">
        <KintsugiDivider />
      </div>

      <div className="max-w-7xl mx-auto px-4">
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-16 items-center">
          {/* Text Content */}
          <div className="space-y-6">
            <h2 className="font-serif font-bold text-4xl md:text-5xl text-foreground mb-6">
              El <span className="golden-shine">Legado</span> de los Imperios Olvidados
            </h2>

            <div className="space-y-4 font-sans text-base text-muted-foreground leading-relaxed">
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
                <h3 className="font-serif uppercase tracking-widest text-sm text-[#af944d] dark:text-[#e6d28a] mb-4">Características Principales</h3>
                <ul className="space-y-3 font-sans text-muted-foreground">
                  {[
                    "Datos históricos verificados y actualizados",
                    "API RESTful con documentación completa",
                    "Búsqueda avanzada por múltiples criterios",
                    "Integración sencilla para desarrolladores",
                    "Soporte para proyectos educativos y comerciales",
                  ].map((item) => (
                    <li key={item} className="flex items-center gap-3">
                      {/* Sharp gold dash — kintsugi seam motif */}
                      <span className="block w-6 h-px bg-gradient-to-r from-[#af944d] to-[#e6d28a] flex-shrink-0" />
                      {item}
                    </li>
                  ))}
                </ul>
              </div>
            </div>
          </div>

          {/* Visual Content — Classical card */}
          <div className="relative">
            <div className="classical-card p-8 text-center">
              {/* Monumental icon — sculptural gold star */}
              <div className="mb-8">
                <div
                  className="w-20 h-20 mx-auto border border-[#af944d]/40 dark:border-[#e6d28a]/30 flex items-center justify-center"
                  style={{ borderRadius: "2px" }}
                >
                  <span className="font-serif text-4xl text-[#af944d] dark:text-[#e6d28a]">✦</span>
                </div>
              </div>

              <h3 className="font-serif font-semibold text-2xl text-foreground mb-4 tracking-wide uppercase">
                Preservando la Historia
              </h3>

              <p className="font-sans text-muted-foreground mb-8 text-sm leading-relaxed">
                Un archivo digital que conecta el pasado con el presente, ofreciendo acceso democrático al
                conocimiento histórico.
              </p>

              <div className="grid grid-cols-3 gap-4 text-center">
                {[
                  { value: "500+", label: "Eventos" },
                  { value: "50+", label: "Culturas" },
                  { value: "10+", label: "Milenios" },
                ].map(({ value, label }) => (
                  <div key={label} className="kpi-card-rectangular p-4">
                    <div className="sacred-number-rectangular">{value}</div>
                    <div className="font-sans text-xs text-muted-foreground uppercase tracking-wider">{label}</div>
                  </div>
                ))}
              </div>
            </div>

            {/* Kintsugi accent lines — replace round blobs */}
            <div className="absolute -top-3 -right-3 w-12 h-px bg-gradient-to-r from-[#af944d] to-transparent opacity-60" />
            <div className="absolute -top-3 -right-3 w-px h-12 bg-gradient-to-b from-[#af944d] to-transparent opacity-60" />
            <div className="absolute -bottom-3 -left-3 w-12 h-px bg-gradient-to-l from-[#af944d] to-transparent opacity-40" />
            <div className="absolute -bottom-3 -left-3 w-px h-12 bg-gradient-to-t from-[#af944d] to-transparent opacity-40" />
          </div>
        </div>
      </div>

      <div className="absolute bottom-0 left-0 right-0">
        <KintsugiDivider />
      </div>
    </section>
  )
}
