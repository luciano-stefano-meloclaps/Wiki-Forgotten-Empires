import Header from "@/components/Header";
import Hero from "@/components/Hero";
import EntityExplorer from "@/components/EntityExplorer";
import HeritageSection from "@/components/HeritageSection";
import Footer from "@/components/Footer";

export default function Home() {
  return (
    <main className="min-h-screen marble-bg">
      <Header />
      <Hero />
      <EntityExplorer />
      <HeritageSection />
      <Footer />
    </main>
  );
}
