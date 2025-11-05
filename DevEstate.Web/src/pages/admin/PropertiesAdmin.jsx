import { useEffect, useMemo, useState } from "react";
import useFetch from "../../hooks/useFetch";
import AdminDataTable from "../../components/AdminDataTable";
import { columns as defaultColumns } from "@/components/admin/property/propertycolumns";
import PropertyAddModal from "@/components/admin/property/PropertyAddModal";

export default function PropertiesAdmin() {
    const { data: properties, loading, error } = useFetch("/api/property/all");
    const { data: buildings } = useFetch("/api/building/all");
    const { data: investments } = useFetch("/api/investment/all");

    const [openAdd, setOpenAdd] = useState(false);

    // Słowniki do wzbogacania
    const buildingsById = useMemo(() => {
        const map = new Map();
        (buildings ?? []).forEach(b => map.set(b.id, b));
        return map;
    }, [buildings]);

    const investmentsById = useMemo(() => {
        const map = new Map();
        (investments ?? []).forEach(i => map.set(i.id, i));
        return map;
    }, [investments]);

    // Wzbogacone dane: buildingNumber + investmentName (dla house bezpośrednio, dla apartment przez building)
    const enrichedData = useMemo(() => {
        if (!properties) return [];
        return properties.map(p => {
            let buildingNumber = "-";
            let investmentName = "-";

            if (p.type === "apartment" && p.buildingId) {
                const b = buildingsById.get(p.buildingId);
                if (b) {
                    buildingNumber = b.buildingNumber ?? "-";
                    const inv = investmentsById.get(b.investmentId);
                    if (inv) investmentName = inv.name ?? "-";
                }
            } else if (p.type === "house" && p.investmentId) {
                const inv = investmentsById.get(p.investmentId);
                if (inv) investmentName = inv.name ?? "-";
            }

            return {
                ...p,
                buildingNumber,
                investmentName
            };
        });
    }, [properties, buildingsById, investmentsById]);

    if (loading) return <p className="p-10 text-gray-600">Ładowanie lokali...</p>;
    if (error) return <p className="p-10 text-red-600">{error}</p>;

    return (
        <>
            <AdminDataTable
                title="Zarządzaj lokalami / nieruchomościami"
                columns={defaultColumns}
                data={enrichedData}
                onAdd={() => setOpenAdd(true)}
                // domyślny filtr po inwestycji (sprawdzi się dla obu typów)
                filterKey="investmentName"
            />

            <PropertyAddModal
                open={openAdd}
                onClose={() => setOpenAdd(false)}
                onSave={() => window.location.reload()}
                buildings={buildings ?? []}
                investments={investments ?? []}
            />
        </>
    );
}
