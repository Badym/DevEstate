import { useState } from "react";
import useFetch from "../hooks/useFetch";
import PropertyCard from "./PropertyCard";
import PropertyModal from "./PropertyModal";

export default function PropertyGrid({ investmentId, buildingId }) {
    const { data: properties, loading, error } =
        useFetch(`/api/Investment/${investmentId}/properties`);
    const [selectedProperty, setSelectedProperty] = useState(null);

    if (loading) return <p className="text-center mt-8">Ładowanie nieruchomości…</p>;
    if (error) return <p className="text-center mt-8">Błąd: {error}</p>;
    if (!properties || properties.length === 0) return null;

    const filtered = buildingId
        ? properties.filter((p) => p.buildingId === buildingId)
        : properties;

    return (
        <section className="bg-[#FAF9F6] py-12">
            <div className="max-w-7xl mx-auto px-6">
                <div className="flex items-end justify-between mb-6">
                    <h3 className="text-2xl font-semibold">
                        {buildingId ? "Lokale w wybranym budynku" : "Wszystkie lokale w inwestycji"}
                    </h3>
                    <p className="text-sm text-gray-600">{filtered.length} pozycji</p>
                </div>

                <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-8">
                    {filtered.map((property) => (
                        <div key={property.id} onClick={() => setSelectedProperty(property)}>
                            <PropertyCard property={property} />
                        </div>
                    ))}
                </div>
            </div>

            {selectedProperty && (
                <PropertyModal
                    property={selectedProperty}
                    onClose={() => setSelectedProperty(null)}
                />
            )}
        </section>
    );
}
