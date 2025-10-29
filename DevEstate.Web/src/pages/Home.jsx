import TopBar from "../components/TopBar";
import AboutSection from "../components/AboutSection";
import InvestmentCarousel from "../components/InvestmentCarousel";
import ContactSection from "../components/ContactSection";
import useFetch from "../hooks/useFetch";

export default function Home() {
    const { data: companyData, loading: companyLoading, error: companyError } = useFetch("/api/Company");
    const { data: investmentsAktualne, loading: investmentsAktualneLoading, error: investmentsAktualneError } =
        useFetch("/api/Investment/status/Aktualne");
    const { data: investmentsSprzedane, loading: investmentsSprzedaneLoading, error: investmentsSprzedaneError } =
        useFetch("/api/Investment/status/Sprzedane");

    if (companyLoading || investmentsAktualneLoading || investmentsSprzedaneLoading) return <p>Ładowanie...</p>;
    if (companyError || investmentsAktualneError || investmentsSprzedaneError)
        return <p>Błąd: {companyError || investmentsAktualneError || investmentsSprzedaneError}</p>;

    return (
        <div className="min-h-screen text-[#1A1A1A] scroll-smooth">
            {/* Pasek nawigacyjny */}
            <TopBar />

            {/* Sekcja powitalna */}
            <div className="bg-white pt-32 text-center" id="home">
                <h1 className="text-5xl font-semibold mb-4">
                    {companyData?.name || "Witamy w DevEstate"}
                </h1>
            </div>

            {/* Sekcja O nas */}
            <div className="bg-[#FAF9F6] py-20" id="about">
                <AboutSection />
            </div>

            {/* Sekcja Na sprzedaż */}
            <div className="bg-[#D1B28D] py-20" id="sales">
                <div className="text-center">
                    <h2 className="text-3xl font-semibold text-white mb-6">
                        Aktualne inwestycje
                    </h2>
                    <InvestmentCarousel investments={investmentsAktualne || []} />
                    <p className="text-white mt-4">
                        {investmentsAktualne?.length || 0} inwestycji dostępnych
                    </p>
                </div>
            </div>

            {/* Sekcja Historia inwestycji */}
            <div className="bg-[#FAF9F6] py-20" id="history">
                <div className="text-center">
                    <h2 className="text-3xl font-semibold mb-6">
                        Sprzedane inwestycje
                    </h2>
                    <InvestmentCarousel investments={investmentsSprzedane || []} />
                    <p className="mt-4">
                        {investmentsSprzedane?.length || 0} inwestycji sprzedanych
                    </p>
                </div>
            </div>

            {/* Sekcja Kontakt */}
            <div id="contact">
                <ContactSection companyData={companyData} />
            </div>
        </div>
    );
}
