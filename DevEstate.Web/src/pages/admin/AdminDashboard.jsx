import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { Button } from "@/components/ui/button";
import AdminTopBar from "@/components/admin/AdminTopBar"; // 🔥 nowy topbar

export default function AdminDashboard() {
    const navigate = useNavigate();

    // 🔒 Autoryzacja
    useEffect(() => {
        const token = localStorage.getItem("token");
        const user = JSON.parse(localStorage.getItem("user") || "{}");

        if (!token || (user.role !== "Admin" && user.role !== "Moderator")) {
            navigate("/admin/login");
        }
    }, [navigate]);


    const user = JSON.parse(localStorage.getItem("user") || "{}");
    const isAdmin = user.role === "Admin";
    const isModerator = user.role === "Moderator";


    return (
        <div className="min-h-screen bg-[#FAF9F6] flex flex-col">

            {/* 🔥 NOWY TOPBAR */}
            <AdminTopBar />

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

                        <Button
                            variant="ghost"
                            className="justify-start text-left hover:bg-[#C8A27E]/20 hover:text-[#C8A27E]"
                            onClick={() => navigate("/admin/features")}
                        >
                            ⚙️ Udogodnienia
                        </Button>

                        <Button
                            variant="ghost"
                            className="justify-start text-left hover:bg-[#C8A27E]/20 hover:text-[#C8A27E]"
                            onClick={() => navigate("/admin/feature-types")}
                        >
                            🧩 Typy udogodnień
                        </Button>
                    </nav>
                </aside>

                {/* Zawartość */}
                <section className="flex-1 p-10">
                    <h2 className="text-3xl font-semibold mb-6">
                        Witaj, {user.fullName || "Administrator"}!
                    </h2>

                    <p className="text-gray-600 text-lg mb-10">
                        W tym panelu możesz zarządzać inwestycjami, budynkami, lokalami oraz udogodnieniami.
                    </p>

                    {/* Wszystkie kafelki 1:1 */}
                    <div className="grid grid-cols-1 md:grid-cols-5 gap-6">
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

                        <div className="bg-white p-6 rounded-lg shadow hover:shadow-md transition">
                            <h3 className="font-semibold text-lg mb-2">Udogodnienia</h3>
                            <p className="text-gray-500">Zarządzaj dodatkowymi funkcjami i usługami.</p>
                            <Button
                                className="mt-4 bg-[#C8A27E] text-white hover:bg-[#b18e6b]"
                                onClick={() => navigate("/admin/features")}
                            >
                                Przejdź
                            </Button>
                        </div>

                        <div className="bg-white p-6 rounded-lg shadow hover:shadow-md transition">
                            <h3 className="font-semibold text-lg mb-2">Typy udogodnień</h3>
                            <p className="text-gray-500">Zarządzaj kategoriami udogodnień (np. garaż, ogród).</p>
                            <Button
                                className="mt-4 bg-[#C8A27E] text-white hover:bg-[#b18e6b]"
                                onClick={() => navigate("/admin/feature-types")}
                            >
                                Przejdź
                            </Button>
                        </div>

                        <div className="bg-white p-6 rounded-lg shadow hover:shadow-md transition">
                            <h3 className="font-semibold text-lg mb-2">Użytkownicy</h3>
                            <p className="text-gray-500">Zarządzaj użytkownikami i ich danymi.</p>
                            <Button
                                className="mt-4 bg-[#C8A27E] text-white hover:bg-[#b18e6b]"
                                onClick={() => navigate("/admin/users")}
                            >
                                Przejdź
                            </Button>
                        </div>

                        {isAdmin && (
                            <div className="bg-white p-6 rounded-lg shadow hover:shadow-md transition">
                                <h3 className="font-semibold text-lg mb-2">Logi administratora</h3>
                                <p className="text-gray-500">Historia operacji wykonywanych w panelu.</p>
                                <Button
                                    className="mt-4 bg-[#C8A27E] text-white hover:bg-[#b18e6b]"
                                    onClick={() => navigate("/admin/logs")}
                                >
                                    Przejdź
                                </Button>
                            </div>
                        )}


                        <div className="bg-white p-6 rounded-lg shadow hover:shadow-md transition">
                            <h3 className="font-semibold text-lg mb-2">Informacje o firmie</h3>
                            <p className="text-gray-500">Dane rejestrowe oraz kontaktowe firmy.</p>
                            <Button
                                className="mt-4 bg-[#C8A27E] text-white hover:bg-[#b18e6b]"
                                onClick={() => navigate("/admin/company-info")}
                            >
                                Przejdź
                            </Button>
                        </div>

                        {isAdmin && (
                            <div className="bg-white p-6 rounded-lg shadow hover:shadow-md transition">
                                <h3 className="font-semibold text-lg mb-2">Raporty & Eksport</h3>
                                <p className="text-gray-500">Generowanie plików cenowych (CSV, MD5, XML).</p>
                                <Button
                                    className="mt-4 bg-[#C8A27E] text-white hover:bg-[#b18e6b]"
                                    onClick={() => navigate("/admin/reports")}
                                >
                                    Przejdź
                                </Button>
                            </div>
                        )}


                        <div className="bg-white p-6 rounded-lg shadow hover:shadow-md transition">
                            <h3 className="font-semibold text-lg mb-2">Porównanie regionów</h3>
                            <p className="text-gray-500">Porównanie średnich cen mieszkań wg regionów.</p>
                            <Button
                                className="mt-4 bg-[#C8A27E] text-white hover:bg-[#b18e6b]"
                                onClick={() => navigate("/admin/compare-prices")}
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
