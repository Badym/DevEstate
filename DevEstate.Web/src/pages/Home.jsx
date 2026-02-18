import TopBar from "../components/TopBar";
import AboutSection from "../components/AboutSection";
import InvestmentCarousel from "../components/InvestmentCarousel";
import ContactSection from "../components/ContactSection";
import useFetch from "../hooks/useFetch";

export default function Home() {
    const {
        data: companyData,
        loading: companyLoading,
        error: companyError
    } = useFetch("/api/Company/details");

    const {
        data: investmentsAktualne,
        loading: investmentsAktualneLoading,
        error: investmentsAktualneError
    } = useFetch("/api/Investment/status/Aktualne");

    const {
        data: investmentsSprzedane,
        loading: investmentsSprzedaneLoading
        // ❌ celowo NIE bierzemy error
    } = useFetch("/api/Investment/status/Sprzedane");

    if (companyLoading || investmentsAktualneLoading || investmentsSprzedaneLoading) {
        return (
            <p className="text-center mt-20 text-gray-600">
                Ładowanie...
            </p>
        );
    }

    // ❗ tylko krytyczne błędy
    if (companyError || investmentsAktualneError) {
        return (
            <p className="text-center mt-20 text-red-600">
                Błąd: {companyError || investmentsAktualneError}
            </p>
        );
    }

    return (
        <div className="min-h-screen text-[#1A1A1A] scroll-smooth">

            {/* 🔝 Pasek nawigacyjny */}
            <TopBar />

            {/* 🏠 Hero */}
            <section className="bg-white pt-40 pb-24 text-center" id="home">
                <h1 className="text-5xl md:text-6xl font-semibold tracking-tight mb-6 animate-fade-in">
                    {companyData?.name || "Witamy w DevEstate"}
                </h1>
                <p className="text-lg text-gray-600 animate-fade-in delay-150">
                    Profesjonalne inwestycje nieruchomości — nowoczesne, przejrzyste, solidne.
                </p>
            </section>

            {/* 📄 O nas */}
            <section className="bg-[#FAF9F6] pt-4 pb-24" id="about">
                <div className="max-w-4xl mx-auto px-6 animate-fade-in">
                    <AboutSection />
                </div>
            </section>

            {/* 🏢 Aktualne inwestycje */}
            <section className="bg-[#D1B28D] py-24" id="sales">
                <div className="text-center text-white">
                    <h2 className="text-3xl font-semibold mb-8 animate-fade-in">
                        Aktualne inwestycje
                    </h2>

                    <div className="animate-fade-in delay-150">
                        <InvestmentCarousel investments={investmentsAktualne || []} />
                    </div>

                    <p className="text-white mt-6 opacity-90 animate-fade-in delay-300">
                        {investmentsAktualne?.length || 0} inwestycji dostępnych
                    </p>
                </div>
            </section>

            {/* 🏘️ Sprzedane inwestycje — tylko jeśli są */}
            {Array.isArray(investmentsSprzedane) && investmentsSprzedane.length > 0 && (
                <section className="bg-[#FAF9F6] py-24" id="history">
                    <div className="text-center">
                        <h2 className="text-3xl font-semibold mb-8 animate-fade-in">
                            Sprzedane inwestycje
                        </h2>

                        <div className="animate-fade-in delay-150">
                            <InvestmentCarousel investments={investmentsSprzedane} />
                        </div>

                        <p className="mt-6 text-gray-700 animate-fade-in delay-300">
                            {investmentsSprzedane.length} inwestycji sprzedanych
                        </p>
                    </div>
                </section>
            )}

            {/* 📬 Kontakt */}
            <section id="contact" className="animate-fade-in py-24">
                <ContactSection companyData={companyData} />
            </section>
        </div>
    );
}
