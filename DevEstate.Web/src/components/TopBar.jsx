import { useState } from "react";
import { useNavigate } from "react-router-dom";

export default function TopBar() {
    const [isMobileMenuOpen, setIsMobileMenuOpen] = useState(false);
    const navigate = useNavigate();

    const toggleMenu = () => setIsMobileMenuOpen(!isMobileMenuOpen);

    // 👇 Funkcja: przejście do strony głównej + przewinięcie do sekcji
    const handleNavigateToSection = (sectionId) => {
        navigate("/"); // przejdź na stronę główną
        setTimeout(() => {
            const section = document.getElementById(sectionId);
            if (section) section.scrollIntoView({ behavior: "smooth" });
        }, 100); // małe opóźnienie, żeby React zdążył załadować Home
    };

    return (
        <nav className="fixed top-0 left-0 w-full bg-white/90 backdrop-blur-sm shadow-sm z-50">
            <div className="max-w-7xl mx-auto px-6 py-4 flex justify-between items-center">
                {/* LOGO */}
                <div
                    className="text-2xl font-semibold tracking-tight text-[#1A1A1A] cursor-pointer"
                    onClick={() => handleNavigateToSection("home")}
                >
                    DevEstate
                </div>

                {/* Desktop menu */}
                <div className="hidden md:flex items-center gap-10 text-[#1A1A1A] font-medium">
                    <button
                        onClick={() => handleNavigateToSection("about")}
                        className="hover:text-[#C8A27E] transition-colors"
                    >
                        O nas
                    </button>
                    <button
                        onClick={() => handleNavigateToSection("history")}
                        className="hover:text-[#C8A27E] transition-colors"
                    >
                        Historia inwestycji
                    </button>
                    <button
                        onClick={() => handleNavigateToSection("sales")}
                        className="hover:text-[#C8A27E] transition-colors"
                    >
                        Na sprzedaż
                    </button>
                    <button
                        onClick={() => handleNavigateToSection("contact")}
                        className="hover:text-[#C8A27E] transition-colors"
                    >
                        Kontakt
                    </button>
                </div>

                {/* Hamburger (mobile) */}
                <button
                    onClick={toggleMenu}
                    className="md:hidden text-[#1A1A1A] p-2 rounded-md"
                >
                    {isMobileMenuOpen ? (
                        <span className="text-2xl">×</span>
                    ) : (
                        <span className="text-2xl">≡</span>
                    )}
                </button>

                {/* Mobile menu */}
                {isMobileMenuOpen && (
                    <div className="absolute top-16 left-0 right-0 bg-white shadow-md md:hidden">
                        <div className="flex flex-col items-center py-4">
                            <button
                                onClick={() => handleNavigateToSection("about")}
                                className="text-[#1A1A1A] py-2 px-4 hover:bg-[#C8A27E] transition-colors w-full text-center"
                            >
                                O nas
                            </button>
                            <button
                                onClick={() => handleNavigateToSection("history")}
                                className="text-[#1A1A1A] py-2 px-4 hover:bg-[#C8A27E] transition-colors w-full text-center"
                            >
                                Historia inwestycji
                            </button>
                            <button
                                onClick={() => handleNavigateToSection("sales")}
                                className="text-[#1A1A1A] py-2 px-4 hover:bg-[#C8A27E] transition-colors w-full text-center"
                            >
                                Na sprzedaż
                            </button>
                            <button
                                onClick={() => handleNavigateToSection("contact")}
                                className="text-[#1A1A1A] py-2 px-4 hover:bg-[#C8A27E] transition-colors w-full text-center"
                            >
                                Kontakt
                            </button>
                        </div>
                    </div>
                )}

                {/* Przycisk kontaktowy */}
                <button
                    onClick={() => handleNavigateToSection("contact")}
                    className="bg-[#C8A27E] hover:bg-[#b18e6b] text-white font-medium px-5 py-2 rounded-full transition hidden md:block"
                >
                    Skontaktuj się
                </button>
            </div>
        </nav>
    );
}
