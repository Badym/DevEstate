import { useState } from "react";
import useFetch from "../../hooks/useFetch";
import AdminDataTable from "../../components/AdminDataTable";
import FeatureTypeAddModal from "@/components/admin/featuretype/FeatureTypeAddModal";
import { columns } from "@/components/admin/featuretype/featuretypecolumns";

export default function FeatureTypesAdmin() {
    const { data: featureTypes, loading, error } = useFetch("/api/FeatureType/all");
    const [openAdd, setOpenAdd] = useState(false);

    if (loading) return <p className="p-10 text-gray-600">Ładowanie typów cech...</p>;
    if (error) return <p className="p-10 text-red-600">{error}</p>;

    return (
        <>
            <AdminDataTable
                title="Zarządzaj typami cech (FeatureType)"
                columns={columns}
                data={featureTypes ?? []}
                onAdd={() => setOpenAdd(true)}
                filterKey="name"
            />

            <FeatureTypeAddModal
                open={openAdd}
                onClose={() => setOpenAdd(false)}
                onSave={() => window.location.reload()}
            />
        </>
    );
}
