import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { Button } from "@/components/ui/button";

export default function AdminDashboard() {
    const navigate = useNavigate();

    // 🔒 Sprawdzamy, czy użytkownik jest zalogowany
    useEffect(() => {
        const token = localStorage.getItem("token");
        const user = JSON.parse(localStorage.getItem("user") || "{}");

        if (!token || user.role !== "Admin") {
            navigate("/admin/login");
        }
    }, [navigate]);

    const user = JSON.parse(localStorage.getItem("user") || "{}");

    return (
        <div className="min-h-screen bg-[#FAF9F6] flex flex-col">
            {/* Górny pasek */}
            <header className="bg-[#1A1A1A] text-white py-4 px-8 flex justify-between items-center shadow-md">
                <h1 className="text-2xl font-semibold">Panel administratora</h1>
                <div className="flex items-center gap-4">
                    <p className="text-sm text-gray-200">
                        Zalogowano jako: <strong>{user.fullName || "Admin"}</strong>
                    </p>

                    <Button
                        variant="outline"
                        className="border-gray-400 text-gray-200 hover:bg-gray-200 hover:text-[#1A1A1A] transition"
                        onClick={() => navigate("/")}
                    >
                        Strona główna
                    </Button>

                    <Button
                        variant="outline"
                        className="border-[#C8A27E] text-[#C8A27E] hover:bg-[#C8A27E] hover:text-white transition"
                        onClick={() => {
                            localStorage.removeItem("token");
                            localStorage.removeItem("user");
                            navigate("/admin/login");
                        }}
                    >
                        Wyloguj
                    </Button>
                </div>
            </header>

            {/* Główna sekcja */}
            <main className="flex flex-1">
                {/* Lewy panel */}
                <aside className="w-64 bg-white shadow-lg border-r border-gray-200 p-6 space-y-4">
                    <h2 className="text-lg font-semibold mb-4">Nawigacja</h2>
                    <nav className="flex flex-col gap-3 text-gray-700">
                        <Button
                            variant="ghost"
                            className="justify-start text-left hover:bg-[#C8A27E]/20 hover:text-[#C8A27E]"
                            onClick={() => navigate("/admin/dashboard")}
                        >
                            📊 Pulpit
                        </Button>

                        <Button
                            variant="ghost"
                            className="justify-start text-left hover:bg-[#C8A27E]/20 hover:text-[#C8A27E]"
                            onClick={() => navigate("/admin/investments")}
                        >
                            🏢 Inwestycje
                        </Button>

                        <Button
                            variant="ghost"
                            className="justify-start text-left hover:bg-[#C8A27E]/20 hover:text-[#C8A27E]"
                            onClick={() => navigate("/admin/buildings")}
                        >
                            🏗️ Budynki
                        </Button>

                        <Button
                            variant="ghost"
                            className="justify-start text-left hover:bg-[#C8A27E]/20 hover:text-[#C8A27E]"
                            onClick={() => navigate("/admin/properties")}
                        >
                            🏠 Nieruchomości
                        </Button>
                    </nav>
                </aside>

                {/* Zawartość */}
                <section className="flex-1 p-10">
                    <h2 className="text-3xl font-semibold mb-6">
                        Witaj, {user.fullName || "Administrator"}!
                    </h2>
                    <p className="text-gray-600 text-lg mb-10">
                        W tym panelu możesz zarządzać inwestycjami, budynkami i mieszkaniami.
                    </p>

                    <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                        <div className="bg-white p-6 rounded-lg shadow hover:shadow-md transition">
                            <h3 className="font-semibold text-lg mb-2">Inwestycje</h3>
                            <p className="text-gray-500">Podgląd i zarządzanie inwestycjami.</p>
                            <Button
                                className="mt-4 bg-[#C8A27E] text-white hover:bg-[#b18e6b]"
                                onClick={() => navigate("/admin/investments")}
                            >
                                Przejdź
                            </Button>
                        </div>

                        <div className="bg-white p-6 rounded-lg shadow hover:shadow-md transition">
                            <h3 className="font-semibold text-lg mb-2">Budynki</h3>
                            <p className="text-gray-500">Przegląd budynków w inwestycjach.</p>
                            <Button
                                className="mt-4 bg-[#C8A27E] text-white hover:bg-[#b18e6b]"
                                onClick={() => navigate("/admin/buildings")}
                            >
                                Przejdź
                            </Button>
                        </div>

                        <div className="bg-white p-6 rounded-lg shadow hover:shadow-md transition">
                            <h3 className="font-semibold text-lg mb-2">Mieszkania</h3>
                            <p className="text-gray-500">Lista lokali i ich status sprzedaży.</p>
                            <Button
                                className="mt-4 bg-[#C8A27E] text-white hover:bg-[#b18e6b]"
                                onClick={() => navigate("/admin/properties")}
                            >
                                Przejdź
                            </Button>
                        </div>
                    </div>
                </section>
            </main>
        </div>
    );
}
