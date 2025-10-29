export default function AboutSection() {
    return (
        <section className="py-24 bg-[#FAF9F6]">
            <div className="max-w-7xl mx-auto px-6 text-center">
                <h2 className="text-4xl font-bold text-[#1A1A1A] mb-12">O nas</h2>

                <div className="grid grid-cols-1 md:grid-cols-1 lg:grid-cols-1 gap-12">
                    {/* Karta 1 - Opis firmy */}
                    <div className="bg-white shadow-lg rounded-xl p-6 text-[#1A1A1A]">
                        <h3 className="text-xl font-semibold mb-4">O nas</h3>
                        <p className="description">
                            {`Naszą misją jest tworzenie przestrzeni, które poprawiają jakość życia i wprowadzają innowacje
                            w projektowaniu budynków. Inwestujemy w zrównoważony rozwój, koncentrując się na ekologicznych rozwiązaniach budowlanych, które dbają o przyszłość naszej planety.`}
                        </p>
                    </div>
                </div>

                {/* Przyciski na dole */}
                <div className="mt-12 flex justify-center gap-6">
                    <a href="#investments">
                        <button className="bg-[#C8A27E] hover:bg-[#b18e6b] text-white px-6 py-3 rounded-full transition">
                            Zobacz inwestycje
                        </button>
                    </a>
                    <a href="#contact">
                        <button className="bg-[#C8A27E] hover:bg-[#b18e6b] text-white px-6 py-3 rounded-full transition">
                            Skontaktuj się
                        </button>
                    </a>
                </div>
            </div>
        </section>
    );
}
