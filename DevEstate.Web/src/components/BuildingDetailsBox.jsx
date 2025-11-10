import { useState, useEffect } from "react";
import { FaCar, FaVideo, FaTree, FaChild, FaBuilding, FaWarehouse } from "react-icons/fa";
import useFetch from "../hooks/useFetch";

export default function BuildingDetailsBox({ buildingId, investment }) {
    if (!buildingId) return null;

    const { data: building, loading, error } = useFetch(`/api/Building/${buildingId}`);
    const { data: features } = useFetch(`/api/Building/${buildingId}/features`);
    const { data: documents } = useFetch(`/api/Document/building/${buildingId}`);  // Pobranie dokumentów

    if (loading) return <p className="text-center mt-6 text-gray-500">Ładowanie danych budynku...</p>;
    if (error || !building) return null;

    // 🧩 Ikony dopasowane do typu cechy
    const getFeatureIcon = (name) => {
        const lower = name?.toLowerCase() || "";
        if (lower.includes("gara")) return <FaCar className="text-[#C8A27E] w-6 h-6" />;
        if (lower.includes("wind")) return <FaBuilding className="text-[#C8A27E] w-6 h-6" />;
        if (lower.includes("monitor")) return <FaVideo className="text-[#C8A27E] w-6 h-6" />;
        if (lower.includes("ogród")) return <FaTree className="text-[#C8A27E] w-6 h-6" />;
        if (lower.includes("plac")) return <FaChild className="text-[#C8A27E] w-6 h-6" />;
        if (lower.includes("komór") || lower.includes("piwnic") || lower.includes("rower"))
            return <FaWarehouse className="text-[#C8A27E] w-6 h-6" />;
        return <FaTree className="text-[#C8A27E] w-6 h-6" />;
    };

    return (
        <section className="max-w-6xl mx-auto mt-16 mb-16 bg-white shadow-xl rounded-2xl p-10">
            {/* Główny układ */}
            <div className="flex flex-col md:flex-row gap-10 items-center">
                {/* Zdjęcie */}
                {building.images && building.images.length > 0 && (
                    <div className="flex-1">
                        <img
                            src={building.images[0]}
                            alt={`Budynek ${building.buildingNumber}`}
                            className="w-full h-[380px] object-cover rounded-xl shadow-md"
                        />
                    </div>
                )}

                {/* Dane */}
                <div className="flex-1 text-left">
                    <h3 className="text-4xl font-bold text-[#1A1A1A] mb-4">
                        Budynek {building.buildingNumber}
                    </h3>

                    {building.description && (
                        <p className="text-gray-700 mb-6 leading-relaxed text-lg">
                            {building.description}
                        </p>
                    )}

                    <div className="grid grid-cols-1 sm:grid-cols-2 gap-x-8 gap-y-3 text-base text-gray-800">
                        <p>
                            <strong>Adres:</strong> {investment?.street}, {investment?.city}
                        </p>
                        <p>
                            <strong>Kod pocztowy:</strong> {investment?.postalCode}
                        </p>
                        <p>
                            <strong>Status:</strong> {building.status}
                        </p>
                        <p>
                            <strong>Data utworzenia:</strong>{" "}
                            {new Date(building.createdAt).toLocaleDateString("pl-PL")}
                        </p>
                    </div>
                </div>
            </div>

            {/* Udogodnienia */}
            {features && features.length > 0 && (
                <div className="mt-12 border-t border-gray-200 pt-10">
                    <h4 className="text-3xl font-semibold mb-8 text-center text-[#1A1A1A]">
                        Udogodnienia budynku
                    </h4>

                    <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-8 px-4">
                        {features.map((f) => (
                            <div
                                key={f.id}
                                className="flex flex-col items-center justify-center bg-[#FAF9F6] hover:bg-[#f4ede3] transition rounded-xl shadow-md py-6 px-4"
                            >
                                {getFeatureIcon(f.name)}
                                <p className="mt-3 text-lg font-semibold text-[#1A1A1A]">{f.name}</p>
                                {f.description && (
                                    <p className="text-sm text-gray-600 mt-2 text-center">
                                        {f.description}
                                    </p>
                                )}
                                {f.price && (
                                    <p className="text-[#C8A27E] font-medium mt-2">
                                        {f.price.toLocaleString("pl-PL")} zł
                                    </p>
                                )}
                            </div>
                        ))}
                    </div>
                </div>
            )}

            {/* Dokumenty (tylko, jeśli dostępne) */}
            {documents && documents.length > 0 && (
                <div className="mt-12 border-t border-gray-200 pt-10">
                    <h4 className="text-3xl font-semibold mb-8 text-center text-[#1A1A1A]">
                        Dokumenty budynku
                    </h4>

                    <div className="space-y-4">
                        {documents.map((doc) => (
                            <a
                                key={doc.id}
                                href={doc.fileUrl}
                                target="_blank"
                                rel="noopener noreferrer"
                                className="block text-[#C8A27E] hover:text-[#b18e6b] font-semibold text-lg"
                            >
                                {doc.fileName}
                            </a>
                        ))}
                    </div>
                </div>
            )}
        </section>
    );
}
