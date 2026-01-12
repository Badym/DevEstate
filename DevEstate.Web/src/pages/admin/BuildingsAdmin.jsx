import { useState, useEffect } from "react";
import useFetch from "../../hooks/useFetch";
import AdminDataTable from "../../components/AdminDataTable";
import { columns as defaultColumns } from "@/components/admin/building/buildingcolumns";
import BuildingAddModal from "@/components/admin/building/BuildingAddModal";

export default function BuildingsAdmin() {
    const { data: buildings, loading, error, refetch } = useFetch("/api/building/all");
    const { data: investments } = useFetch("/api/investment/all");

    const [openAdd, setOpenAdd] = useState(false);
    const [enrichedData, setEnrichedData] = useState([]);

    // 🔁 Połącz budynki z nazwą inwestycji
    useEffect(() => {
        if (buildings && investments) {
            const mapped = buildings.map((b) => {
                const inv = investments.find((i) => i.id === b.investmentId);
                return {
                    ...b,
                    investmentName: inv?.name || "Brak przypisania",
                };
            });
            setEnrichedData(mapped);
        }
    }, [buildings, investments]);

    if (loading) return <p className="p-10 text-gray-600">Ładowanie budynków...</p>;
    if (error) return <p className="p-10 text-red-600">{error}</p>;

    return (
        <>
            <AdminDataTable
                title="Zarządzaj budynkami"
                columns={defaultColumns}
                data={enrichedData}
                onAdd={() => setOpenAdd(true)}
                filterKey="investmentName"
            />

            <BuildingAddModal
                open={openAdd}
                onClose={() => setOpenAdd(false)}
                onSave={() => refetch()}  // ⬅️ ODNAWIA LISTĘ, BEZ PRZEŁADOWANIA STRONY
                investments={investments}
            />
        </>
    );
}
