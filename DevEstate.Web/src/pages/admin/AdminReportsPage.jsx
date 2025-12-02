import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { Button } from "@/components/ui/button";

export default function AdminReportsPage() {
    const navigate = useNavigate();
    const [xmlLinks, setXmlLinks] = useState(null);
    const token = localStorage.getItem("token");

    // --- 1) Pobranie CSV (DOWNLOAD)
    const downloadCsv = async () => {
        try {
            const res = await fetch("http://localhost:5086/api/ProspectReport/download-csv", {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });

            if (!res.ok) {
                alert("Błąd pobierania CSV");
                return;
            }

            // Pobranie jako blob + wymuszenie pobrania
            const blob = await res.blob();
            const url = window.URL.createObjectURL(blob);

            const a = document.createElement("a");
            a.href = url;
            a.download = "cennik.csv";
            a.click();

        } catch (err) {
            console.error(err);
        }
    };

    // --- 2) Generowanie pełnego zestawu (CSV + MD5 + XML)
    const generateFullExport = async () => {
        try {
            const res = await fetch("http://localhost:5086/api/xml/generate", {
                method: "POST",
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });

            const data = await res.json();
            setXmlLinks(data); // tutaj w data: { csv: "...", md5: "...", xml: "..." }

        } catch (err) {
            console.error(err);
        }
    };

    return (
        <div className="min-h-screen bg-[#FAF9F6] flex flex-col">

            {/* Główny nagłówek */}
            <header className="bg-[#1A1A1A] text-white py-4 px-8 flex justify-between items-center shadow-md">
                <h1 className="text-2xl font-semibold">Raporty & Eksport danych</h1>
                <Button
                    variant="outline"
                    onClick={() => navigate("/admin/dashboard")}
                    className="border-gray-400 text-gray-200 hover:bg-gray-200 hover:text-[#1A1A1A]"
                >
                    Powrót
                </Button>
            </header>

            <main className="flex">

                {/* Sidebar */}
                <aside className="w-64 bg-white shadow-lg border-r border-gray-200 p-6 space-y-4">
                    <h2 className="text-lg font-semibold mb-4">Nawigacja</h2>

                    <Button variant="ghost" className="justify-start" onClick={() => navigate("/admin/dashboard")}>
                        📊 Pulpit
                    </Button>
                    <Button variant="ghost" className="justify-start" onClick={() => navigate("/admin/investments")}>
                        🏢 Inwestycje
                    </Button>
                    <Button variant="ghost" className="justify-start" onClick={() => navigate("/admin/buildings")}>
                        🏗️ Budynki
                    </Button>
                    <Button variant="ghost" className="justify-start" onClick={() => navigate("/admin/properties")}>
                        🏠 Nieruchomości
                    </Button>
                    <Button variant="ghost" className="justify-start" onClick={() => navigate("/admin/features")}>
                        ⚙️ Udogodnienia
                    </Button>
                    <Button variant="ghost" className="justify-start" onClick={() => navigate("/admin/feature-types")}>
                        🧩 Typy cech
                    </Button>
                    <Button variant="ghost" className="justify-start" onClick={() => navigate("/admin/users")}>
                        👤 Użytkownicy
                    </Button>
                </aside>

                {/* --- CONTENT --- */}
                <section className="flex-1 p-10">

                    {/* Karty operacji */}
                    <div className="bg-white shadow p-6 rounded-xl mb-10">
                        <h2 className="text-xl font-semibold mb-4">Eksport danych</h2>
                        <p className="text-gray-600 mb-6">
                            Tutaj możesz generować pliki wymagane do publikacji cen zgodnie z wymogami ustawowymi.
                        </p>

                        <div className="flex gap-6">

                            {/* Przyciski */}
                            <Button
                                className="bg-[#C8A27E] text-white hover:bg-[#b18e6b]"
                                onClick={downloadCsv}
                            >
                                📥 Pobierz CSV
                            </Button>

                            <Button
                                className="bg-[#1A1A1A] text-white hover:bg-[#333]"
                                onClick={generateFullExport}
                            >
                                ⚙️ Generuj pełny zestaw (CSV + MD5 + XML)
                            </Button>
                        </div>
                    </div>

                    {xmlLinks && (
                        <div className="bg-white shadow p-6 rounded-xl mt-6">
                            <h3 className="text-lg font-semibold mb-4">📄 Wygenerowane pliki</h3>

                            <ul className="space-y-4">

                                {/* CSV */}
                                <li className="flex items-center justify-between bg-gray-50 p-3 rounded-lg border">
                                    <div className="flex flex-col">
                                        <span className="font-medium text-[#C8A27E]">CSV</span>
                                        <a
                                            href={xmlLinks.csv}
                                            target="_blank"
                                            className="text-blue-600 underline break-all"
                                        >
                                            {xmlLinks.csv}
                                        </a>
                                    </div>

                                    <Button
                                        variant="outline"
                                        onClick={() => navigator.clipboard.writeText(xmlLinks.csv)}
                                        className="ml-4"
                                    >
                                        📋 Kopiuj
                                    </Button>
                                </li>

                                {/* MD5 */}
                                <li className="flex items-center justify-between bg-gray-50 p-3 rounded-lg border">
                                    <div className="flex flex-col">
                                        <span className="font-medium text-[#C8A27E]">MD5</span>
                                        <a
                                            href={xmlLinks.md5}
                                            target="_blank"
                                            className="text-blue-600 underline break-all"
                                        >
                                            {xmlLinks.md5}
                                        </a>
                                    </div>

                                    <Button
                                        variant="outline"
                                        onClick={() => navigator.clipboard.writeText(xmlLinks.md5)}
                                    >
                                        📋 Kopiuj
                                    </Button>
                                </li>

                                {/* XML */}
                                <li className="flex items-center justify-between bg-gray-50 p-3 rounded-lg border">
                                    <div className="flex flex-col">
                                        <span className="font-medium text-[#C8A27E]">XML</span>
                                        <a
                                            href={xmlLinks.xml}
                                            target="_blank"
                                            className="text-blue-600 underline break-all"
                                        >
                                            {xmlLinks.xml}
                                        </a>
                                    </div>

                                    <Button
                                        variant="outline"
                                        onClick={() => navigator.clipboard.writeText(xmlLinks.xml)}
                                    >
                                        📋 Kopiuj
                                    </Button>
                                </li>

                            </ul>
                        </div>
                    )}


                </section>
            </main>
        </div>
    );
}
