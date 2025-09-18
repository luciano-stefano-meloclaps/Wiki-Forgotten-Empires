import Header from "@/components/Header"
import Hero from "@/components/Hero"
import EntitiesSection from "@/components/EntitiesSection"
import HeritageSection from "@/components/HeritageSection"
import DataSection from "@/components/DataSection"
import Footer from "@/components/Footer"

export default function Home() {
  return (
    <main className="min-h-screen marble-bg">
      <Header />
      <Hero />
      <EntitiesSection />
      <HeritageSection />
      <DataSection />
      <Footer />
    </main>
  )
}
