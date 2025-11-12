import { useState, useMemo } from "react";
import useFetch from "../../hooks/useFetch";
import AdminDataTable from "../../components/AdminDataTable";
import { columns as defaultColumns } from "@/components/admin/feature/featurecolumns";
import FeatureAddModal from "@/components/admin/feature/FeatureAddModal";

export default function FeaturesAdmin() {
    const { data: features, loading, error } = useFetch("/api/Feature/all");
    const { data: featureTypes } = useFetch("/api/FeatureType/all");
    const { data: buildings } = useFetch("/api/building/all");
    const { data: investments } = useFetch("/api/investment/all");

    const [openAdd, setOpenAdd] = useState(false);

    // 🔹 Mapowanie typów cech
    const featureTypeMap = useMemo(() => {
        const map = new Map();
        (featureTypes ?? []).forEach((ft) => map.set(ft.id, ft.name));
        return map;
    }, [featureTypes]);

    // 🔹 Wzbogacone dane (nazwy typów, budynków i inwestycji)
    const enrichedData = useMemo(() => {
        if (!features) return [];
        return features.map((f) => ({
            ...f,
            featureTypeName: featureTypeMap.get(f.featureTypeId) ?? "-",
        }));
    }, [features, featureTypeMap]);

    if (loading) return <p className="p-10 text-gray-600">Ładowanie cech...</p>;
    if (error) return <p className="p-10 text-red-600">{error}</p>;

    return (
        <>
            <AdminDataTable
                title="Zarządzaj udogodnieniami (Features)"
                columns={defaultColumns}
                data={enrichedData}
                onAdd={() => setOpenAdd(true)}
                filterKey="featureTypeName"
            />

            <FeatureAddModal
                open={openAdd}
                onClose={() => setOpenAdd(false)}
                onSave={() => window.location.reload()}
                featureTypes={featureTypes ?? []}
                buildings={buildings ?? []}
                investments={investments ?? []}
            />
        </>
    );
}
