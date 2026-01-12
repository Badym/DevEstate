import { useState, useEffect } from "react";
import useFetch from "../../hooks/useFetch";
import AdminTopBar from "@/components/admin/AdminTopBar";

import {
    BarChart,
    Bar,
    XAxis,
    YAxis,
    Tooltip,
    CartesianGrid,
    ResponsiveContainer,
} from "recharts";

export default function AdminComparisonPage() {

    const {
        data: wojewodztwa,
        loading,
        error
    } = useFetch("/api/DeveloperPrice/wojewodztwa");

    const [powiaty, setPowiaty] = useState([]);
    const [selectedWoj, setSelectedWoj] = useState("");
    const [selectedPow, setSelectedPow] = useState("");

    const [loadingPow, setLoadingPow] = useState(false);
    const [searchLoading, setSearchLoading] = useState(false);

    const [locations, setLocations] = useState([]);


    useEffect(() => {
        if (!selectedWoj) {
            setPowiaty([]);
            setSelectedPow("");
            return;
        }

        setLoadingPow(true);

        fetch(`/api/DeveloperPrice/powiaty?woj=${selectedWoj}`)
            .then(r => r.json())
            .then(data => setPowiaty(data))
            .finally(() => setLoadingPow(false));

    }, [selectedWoj]);


    const handleAddLocation = async () => {
        if (!selectedWoj) {
            alert("Musisz wybrać województwo!");
            return;
        }
        if (locations.length >= 7) {
            alert("Możesz dodać maksymalnie 7 lokalizacji.");
            return;
        }

        setSearchLoading(true);

        const url = new URL("/api/DeveloperPrice/search", window.location.origin);
        url.searchParams.append("woj", selectedWoj);
        if (selectedPow) url.searchParams.append("pow", selectedPow);

        const res = await fetch(url);
        const data = await res.json();

        const label = selectedPow
            ? `${selectedPow}, ${selectedWoj}`
            : `${selectedWoj} (wszystkie powiaty)`;

        setLocations(prev => [
            ...prev,
            {
                label,
                avgPriceM2: parseFloat(data.avgPriceM2)
            }
        ]);

        setSearchLoading(false);
    };

    const removeLocation = (i) => setLocations(locations.filter((_, idx) => idx !== i));

    if (loading) return <p className="p-10 text-gray-600">Ładowanie województw...</p>;
    if (error) return <p className="p-10 text-red-600">{error}</p>;

    const maxValue = locations.length > 0
        ? Math.max(...locations.map(l => Number(l.avgPriceM2))) + 5000
        : 10000;


    return (
        <div className="min-h-screen bg-[#FAF9F6] flex flex-col">

            {/* 🔥 ADMIN TOP BAR DODANY */}
            <AdminTopBar />

            <div className="py-10 flex justify-center w-full">

                <div className="w-full max-w-5xl px-6">

                    <h1 className="text-4xl font-semibold mb-4 text-[#1A1A1A] text-center">
                        Porównanie średnich cen m²
                    </h1>

                    <p className="text-gray-600 mb-10 text-center text-lg">
                        Dodaj do 7 regionów i porównaj ich średnie ceny za pomocą wykresu słupkowego.
                    </p>

                    {/* FORMULARZ */}
                    <div className="bg-white p-8 rounded-xl shadow-lg border border-gray-200 mx-auto mb-12">

                        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">

                            <div>
                                <label className="block mb-2 font-semibold text-gray-700">
                                    Województwo
                                </label>
                                <select
                                    value={selectedWoj}
                                    onChange={e => setSelectedWoj(e.target.value)}
                                    className="border border-gray-300 p-3 rounded-lg w-full bg-[#FAF9F6]"
                                >
                                    <option value="">-- wybierz województwo --</option>
                                    {wojewodztwa?.map((w) => (
                                        <option key={w} value={w.toLowerCase()}>
                                            {w}
                                        </option>
                                    ))}
                                </select>
                            </div>

                            <div>
                                <label className="block mb-2 font-semibold text-gray-700">
                                    Powiat (opcjonalnie)
                                </label>
                                <select
                                    value={selectedPow}
                                    disabled={!selectedWoj}
                                    onChange={e => setSelectedPow(e.target.value)}
                                    className="border border-gray-300 p-3 rounded-lg w-full disabled:bg-gray-200 bg-[#FAF9F6]"
                                >
                                    <option value="">-- wszystkie powiaty --</option>
                                    {loadingPow ? (
                                        <option>Ładowanie...</option>
                                    ) : (
                                        powiaty.map((p) => (
                                            <option key={p} value={p.toLowerCase()}>
                                                {p}
                                            </option>
                                        ))
                                    )}
                                </select>
                            </div>
                        </div>

                        <button
                            onClick={handleAddLocation}
                            className="w-full mt-6 bg-[#C8A27E] text-white p-4 rounded-lg 
                                    hover:bg-[#b18e6b] transition text-lg shadow"
                        >
                            {searchLoading ? "Dodawanie..." : "Dodaj lokalizację do porównania"}
                        </button>

                    </div>


                    {/* LISTA WYBRANYCH */}
                    {locations.length > 0 && (
                        <div className="mb-12 mx-auto max-w-3xl">

                            <h2 className="text-2xl font-semibold mb-6 text-[#1A1A1A] text-center">
                                Wybrane lokalizacje
                            </h2>

                            <ul className="space-y-3">
                                {locations.map((loc, idx) => (
                                    <li
                                        key={idx}
                                        className="flex justify-between items-center bg-white p-4 shadow-md 
                                                rounded-lg border border-gray-200"
                                    >
                                        <span className="text-gray-800 font-medium">{loc.label}</span>

                                        <button
                                            onClick={() => removeLocation(idx)}
                                            className="text-red-600 font-bold hover:text-red-800 text-xl"
                                        >
                                            ✕
                                        </button>
                                    </li>
                                ))}
                            </ul>
                        </div>
                    )}


                    {/* WYKRES */}
                    {locations.length > 0 && (
                        <div className="bg-white p-10 rounded-xl shadow-xl border border-gray-200">

                            <h2 className="text-3xl font-semibold mb-8 text-center text-[#1A1A1A]">
                                Wykres porównawczy – średnia cena m²
                            </h2>

                            <ResponsiveContainer width="100%" height={450}>
                                <BarChart data={locations}>
                                    <CartesianGrid strokeDasharray="3 3" stroke="#e5e5e5" />
                                    <XAxis
                                        dataKey="label"
                                        tick={{ fontSize: 12, fill: "#666" }}
                                        interval={0}
                                    />
                                    <YAxis
                                        domain={[0, maxValue]}
                                        tick={{ fill: "#666" }}
                                    />

                                    <Tooltip
                                        contentStyle={{
                                            background: "#fff",
                                            border: "1px solid #ddd",
                                            borderRadius: "8px",
                                            padding: "10px"
                                        }}
                                    />

                                    <Bar
                                        dataKey="avgPriceM2"
                                        fill="url(#grad)"
                                        radius={[8, 8, 0, 0]}
                                        animationDuration={900}
                                    />

                                    <defs>
                                        <linearGradient id="grad" x1="0" y1="0" x2="0" y2="1">
                                            <stop offset="0%" stopColor="#C8A27E" stopOpacity={0.9}/>
                                            <stop offset="100%" stopColor="#C8A27E" stopOpacity={0.6}/>
                                        </linearGradient>
                                    </defs>

                                </BarChart>
                            </ResponsiveContainer>

                        </div>
                    )}

                </div>
            </div>
        </div>
    );
}
