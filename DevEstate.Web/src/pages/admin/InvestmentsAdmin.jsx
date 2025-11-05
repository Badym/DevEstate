import { useState } from "react";
import AdminDataTable from "../../components/AdminDataTable";
import InvestmentAddModal from "@/components/modals/InvestmentAddModal";
import { columns } from "./columns";
import useFetch from "../../hooks/useFetch";

export default function InvestmentsAdmin() {
    const {
        data: investments,
        loading,
        error,
    } = useFetch("/api/investment/all");

    const [openAdd, setOpenAdd] = useState(false);

    if (loading) return <p className="p-10 text-gray-600">Ładowanie inwestycji...</p>;
    if (error) return <p className="p-10 text-red-600">{error}</p>;

    return (
        <>
            <AdminDataTable
                title="Zarządzaj inwestycjami"
                columns={columns}
                data={investments || []}
                onAdd={() => setOpenAdd(true)} // 👈 teraz otwieramy modal
                filterKey="name"
            />

            {/* 💡 Modal dodawania inwestycji */}
            <InvestmentAddModal
                open={openAdd}
                onClose={() => setOpenAdd(false)}
                onSave={() => window.location.reload()} // po dodaniu odświeża listę
            />
        </>
    );
}
