import AdminDataTable from "../../components/AdminDataTable";
import { columns } from "./columns";

const data = [
    { id: 1, name: "Zielone Tarasy", city: "Warszawa", status: "Aktywna" },
    { id: 2, name: "Nowe Zacisze", city: "Kraków", status: "Aktywna" },
    { id: 3, name: "Parkowe", city: "Gdańsk", status: "Zakończona" },
];

export default function InvestmentsAdmin() {
    return (
        <AdminDataTable
            title="Zarządzaj inwestycjami"
            columns={columns}
            data={data}
            onAdd={() => alert("Dodaj nową inwestycję")}
        />
    );
}
