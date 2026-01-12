export default function AboutSection() {
    return (
        <section className="py-24 bg-[#FAF9F6]">
            <div className="max-w-5xl mx-auto px-6 text-center animate-fade-in">

                {/* Nagłówek */}
                <h2 className="text-3xl font-semibold text-[#1A1A1A] mb-10 tracking-tight">
                    O nas
                </h2>

                {/* Karta opisu */}
                <div className="bg-white shadow-md rounded-2xl p-10 text-[#1A1A1A] mx-auto">
                    <p className="text-lg leading-relaxed text-gray-700">
                        Naszą misją jest tworzenie przestrzeni, które podnoszą komfort życia
                        i wprowadzają innowacje w projektowaniu nowoczesnych budynków.
                        <br /><br />
                        Koncentrujemy się na zrównoważonym rozwoju oraz ekologicznych
                        rozwiązaniach budowlanych, dbając o przyszłość naszych klientów
                        i środowiska naturalnego.
                    </p>
                </div>

                {/* Przyciski CTA */}
                <div className="mt-12 flex justify-center gap-6">
                    <a href="#sales">
                        <button className="bg-[#C8A27E] hover:bg-[#b18e6b] text-white px-8 py-3 rounded-full transition text-sm tracking-wide shadow-md">
                            Zobacz inwestycje
                        </button>
                    </a>

                    <a href="#contact">
                        <button className="border border-[#C8A27E] hover:bg-[#C8A27E] hover:text-white text-[#C8A27E] px-8 py-3 rounded-full transition text-sm tracking-wide shadow-sm">
                            Skontaktuj się
                        </button>
                    </a>
                </div>

            </div>
        </section>
    );
}
