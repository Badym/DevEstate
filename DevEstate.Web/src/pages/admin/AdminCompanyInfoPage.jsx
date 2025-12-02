import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Button } from "@/components/ui/button";

export default function AdminCompanyInfoPage() {
    const navigate = useNavigate();
    const [loading, setLoading] = useState(true);
    const [saving, setSaving] = useState(false);
    const [company, setCompany] = useState(null);

    const token = localStorage.getItem("token");

    const fetchData = async () => {
        const res = await fetch("http://localhost:5086/api/Company/details", {
            headers: { Authorization: `Bearer ${token}` }
        });

        const data = await res.json();
        setCompany(data);
        setLoading(false);
    };

    useEffect(() => {
        const user = JSON.parse(localStorage.getItem("user") || "{}");

        if (!token || (user.role !== "Admin" && user.role !== "Moderator")) {
            navigate("/admin/login");
            return;
        }

        fetchData();
    }, []);

    const handleSave = async () => {
        setSaving(true);

        await fetch("http://localhost:5086/api/Company/details", {
            method: "PATCH",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${token}`
            },
            body: JSON.stringify(company)
        });

        setSaving(false);
        alert("Zapisano zmiany!");
    };

    const update = (field, value) => {
        setCompany(prev => ({ ...prev, [field]: value }));
    };

    return (
        <div className="min-h-screen bg-[#FAF9F6] flex flex-col">

            {/* HEADER */}
            <header className="bg-[#1A1A1A] text-white py-4 px-8 flex justify-between items-center shadow-md">
                <h1 className="text-2xl font-semibold">Informacje o firmie</h1>
                <Button
                    variant="outline"
                    onClick={() => navigate("/admin/dashboard")}
                    className="border-gray-400 text-gray-200 hover:bg-gray-200 hover:text-[#1A1A1A]"
                >
                    Powrót
                </Button>
            </header>

            <main className="flex">

                {/* SIDEBAR */}
                <aside className="w-64 bg-white shadow-lg border-r border-gray-200 p-6 space-y-4">
                    <h2 className="text-lg font-semibold mb-4">Nawigacja</h2>

                    <Button variant="ghost" className="justify-start" onClick={() => navigate("/admin/dashboard")}>📊 Pulpit</Button>
                    <Button variant="ghost" className="justify-start" onClick={() => navigate("/admin/investments")}>🏢 Inwestycje</Button>
                    <Button variant="ghost" className="justify-start" onClick={() => navigate("/admin/buildings")}>🏗️ Budynki</Button>
                    <Button variant="ghost" className="justify-start" onClick={() => navigate("/admin/properties")}>🏠 Nieruchomości</Button>
                    <Button variant="ghost" className="justify-start" onClick={() => navigate("/admin/features")}>⚙️ Udogodnienia</Button>
                    <Button variant="ghost" className="justify-start" onClick={() => navigate("/admin/feature-types")}>🧩 Typy cech</Button>
                    <Button variant="ghost" className="justify-start" onClick={() => navigate("/admin/users")}>👤 Użytkownicy</Button>
                    <Button variant="ghost" className="justify-start" onClick={() => navigate("/admin/logs")}>📝 Logi</Button>
                </aside>

                {/* MAIN CONTENT */}
                <section className="flex-1 p-10">
                    {loading ? (
                        <p className="text-gray-600 text-lg">Ładowanie danych...</p>
                    ) : (
                        <div className="bg-white shadow-md rounded-lg p-8 max-w-4xl">

                            <h2 className="text-2xl font-semibold mb-6">
                                Edycja danych firmy
                            </h2>

                            {/* GRID FORM */}
                            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">

                                {/* NAME */}
                                <div>
                                    <label className="text-sm text-gray-600">Nazwa firmy</label>
                                    <input
                                        type="text"
                                        value={company.name}
                                        onChange={(e) => update("name", e.target.value)}
                                        className="w-full mt-1 p-2 border rounded"
                                    />
                                </div>

                                {/* LEGAL FORM */}
                                <div>
                                    <label className="text-sm text-gray-600">Forma prawna</label>
                                    <input
                                        type="text"
                                        value={company.legalForm}
                                        onChange={(e) => update("legalForm", e.target.value)}
                                        className="w-full mt-1 p-2 border rounded"
                                    />
                                </div>

                                {/* KRS */}
                                <div>
                                    <label className="text-sm text-gray-600">KRS</label>
                                    <input
                                        type="text"
                                        value={company.krs}
                                        onChange={(e) => update("krs", e.target.value)}
                                        className="w-full mt-1 p-2 border rounded"
                                    />
                                </div>

                                {/* CEIDG */}
                                <div>
                                    <label className="text-sm text-gray-600">CEIDG</label>
                                    <input
                                        type="text"
                                        value={company.ceidgNumber || ""}
                                        onChange={(e) => update("ceidgNumber", e.target.value)}
                                        className="w-full mt-1 p-2 border rounded"
                                    />
                                </div>

                                {/* NIP */}
                                <div>
                                    <label className="text-sm text-gray-600">NIP</label>
                                    <input
                                        type="text"
                                        value={company.nip}
                                        onChange={(e) => update("nip", e.target.value)}
                                        className="w-full mt-1 p-2 border rounded"
                                    />
                                </div>

                                {/* REGON */}
                                <div>
                                    <label className="text-sm text-gray-600">REGON</label>
                                    <input
                                        type="text"
                                        value={company.regon}
                                        onChange={(e) => update("regon", e.target.value)}
                                        className="w-full mt-1 p-2 border rounded"
                                    />
                                </div>

                                {/* PHONE */}
                                <div>
                                    <label className="text-sm text-gray-600">Telefon</label>
                                    <input
                                        type="text"
                                        value={company.phone}
                                        onChange={(e) => update("phone", e.target.value)}
                                        className="w-full mt-1 p-2 border rounded"
                                    />
                                </div>

                                {/* FAX */}
                                <div>
                                    <label className="text-sm text-gray-600">Fax</label>
                                    <input
                                        type="text"
                                        value={company.fax || ""}
                                        onChange={(e) => update("fax", e.target.value)}
                                        className="w-full mt-1 p-2 border rounded"
                                    />
                                </div>

                                {/* EMAIL */}
                                <div className="col-span-2">
                                    <label className="text-sm text-gray-600">Email</label>
                                    <input
                                        type="text"
                                        value={company.email}
                                        onChange={(e) => update("email", e.target.value)}
                                        className="w-full mt-1 p-2 border rounded"
                                    />
                                </div>

                                {/* WEBSITE */}
                                <div className="col-span-2">
                                    <label className="text-sm text-gray-600">Strona internetowa</label>
                                    <input
                                        type="text"
                                        value={company.website}
                                        onChange={(e) => update("website", e.target.value)}
                                        className="w-full mt-1 p-2 border rounded"
                                    />
                                </div>

                                {/* ADDRESS */}
                                <h3 className="text-lg font-semibold col-span-2 mt-4">Adres</h3>

                                <div>
                                    <label className="text-sm text-gray-600">Województwo</label>
                                    <input
                                        type="text"
                                        value={company.province}
                                        onChange={(e) => update("province", e.target.value)}
                                        className="w-full mt-1 p-2 border rounded"
                                    />
                                </div>

                                <div>
                                    <label className="text-sm text-gray-600">Powiat</label>
                                    <input
                                        type="text"
                                        value={company.county}
                                        onChange={(e) => update("county", e.target.value)}
                                        className="w-full mt-1 p-2 border rounded"
                                    />
                                </div>

                                <div>
                                    <label className="text-sm text-gray-600">Gmina</label>
                                    <input
                                        type="text"
                                        value={company.municipality}
                                        onChange={(e) => update("municipality", e.target.value)}
                                        className="w-full mt-1 p-2 border rounded"
                                    />
                                </div>

                                <div>
                                    <label className="text-sm text-gray-600">Miasto</label>
                                    <input
                                        type="text"
                                        value={company.city}
                                        onChange={(e) => update("city", e.target.value)}
                                        className="w-full mt-1 p-2 border rounded"
                                    />
                                </div>

                                <div>
                                    <label className="text-sm text-gray-600">Ulica</label>
                                    <input
                                        type="text"
                                        value={company.street}
                                        onChange={(e) => update("street", e.target.value)}
                                        className="w-full mt-1 p-2 border rounded"
                                    />
                                </div>

                                <div>
                                    <label className="text-sm text-gray-600">Numer budynku</label>
                                    <input
                                        type="text"
                                        value={company.buildingNumber}
                                        onChange={(e) => update("buildingNumber", e.target.value)}
                                        className="w-full mt-1 p-2 border rounded"
                                    />
                                </div>

                                <div>
                                    <label className="text-sm text-gray-600">Numer lokalu</label>
                                    <input
                                        type="text"
                                        value={company.apartmentNumber || ""}
                                        onChange={(e) => update("apartmentNumber", e.target.value)}
                                        className="w-full mt-1 p-2 border rounded"
                                    />
                                </div>

                                <div>
                                    <label className="text-sm text-gray-600">Kod pocztowy</label>
                                    <input
                                        type="text"
                                        value={company.postalCode}
                                        onChange={(e) => update("postalCode", e.target.value)}
                                        className="w-full mt-1 p-2 border rounded"
                                    />
                                </div>

                                {/* DESCRIPTION */}
                                <div className="col-span-2">
                                    <label className="text-sm text-gray-600">Opis firmy</label>
                                    <textarea
                                        rows="4"
                                        value={company.description}
                                        onChange={(e) => update("description", e.target.value)}
                                        className="w-full mt-1 p-2 border rounded"
                                    ></textarea>
                                </div>

                                {/* CONTACT METHODS */}
                                <div className="col-span-2">
                                    <label className="text-sm text-gray-600">Metody kontaktu</label>
                                    <input
                                        type="text"
                                        value={company.contactMethond}
                                        onChange={(e) => update("contactMethond", e.target.value)}
                                        className="w-full mt-1 p-2 border rounded"
                                    />
                                </div>

                                {/* LOGO */}
                                <div className="col-span-2">
                                    <label className="text-sm text-gray-600">Logo (URL)</label>
                                    <input
                                        type="text"
                                        value={company.logoImage}
                                        onChange={(e) => update("logoImage", e.target.value)}
                                        className="w-full mt-1 p-2 border rounded"
                                    />
                                    {company.logoImage && (
                                        <img
                                            src={company.logoImage}
                                            alt="Logo"
                                            className="w-32 h-auto mt-3 rounded shadow"
                                        />
                                    )}
                                </div>

                            </div>

                            {/* SAVE BUTTON */}
                            <Button
                                onClick={handleSave}
                                disabled={saving}
                                className="mt-8 bg-[#C8A27E] text-white hover:bg-[#b18e6b]"
                            >
                                {saving ? "Zapisywanie..." : "Zapisz zmiany"}
                            </Button>

                        </div>
                    )}
                </section>
            </main>
        </div>
    );
}
