import { useState } from "react";
import FeatureEditModal from "@/components/admin/feature/FeatureEditModal";
import { Button } from "@/components/ui/button";
import {
    DropdownMenu,
    DropdownMenuContent,
    DropdownMenuItem,
    DropdownMenuLabel,
    DropdownMenuSeparator,
    DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { MoreHorizontal, Edit, Trash2 } from "lucide-react";

// 💰 Formatowanie PLN
const formatPln = (v) =>
    typeof v === "number" || typeof v === "bigint"
        ? v.toLocaleString("pl-PL", { style: "currency", currency: "PLN", maximumFractionDigits: 0 })
        : v;

export const columns = [
    {
        accessorKey: "featureTypeName",
        header: "Typ cechy",
        cell: ({ row }) => (
            <span className="font-medium text-gray-800">{row.getValue("featureTypeName")}</span>
        ),
    },
    {
        accessorKey: "price",
        header: "Cena (PLN)",
        cell: ({ row }) => {
            const val = row.getValue("price");
            return <span>{val ? formatPln(val) : "—"}</span>;
        },
    },
    {
        accessorKey: "description",
        header: "Opis",
        cell: ({ row }) => (
            <span className="text-gray-700 text-sm">
        {row.getValue("description") || "—"}
      </span>
        ),
    },
    {
        accessorKey: "buildingId",
        header: "Budynek",
        cell: ({ row }) => {
            const b = row.getValue("buildingId");
            return b ? (
                <span className="px-2 py-0.5 rounded-md bg-blue-50 text-blue-700 text-xs">TAK</span>
            ) : (
                <span className="text-gray-400 text-xs">–</span>
            );
        },
    },
    {
        accessorKey: "investmentId",
        header: "Inwestycja",
        cell: ({ row }) => {
            const i = row.getValue("investmentId");
            return i ? (
                <span className="px-2 py-0.5 rounded-md bg-emerald-50 text-emerald-700 text-xs">TAK</span>
            ) : (
                <span className="text-gray-400 text-xs">–</span>
            );
        },
    },
    {
        accessorKey: "isAvailable",
        header: "Dostępność",
        cell: ({ row }) => {
            const status = row.getValue("isAvailable");
            const color = status
                ? "bg-green-100 text-green-700"
                : "bg-red-100 text-red-700";
            return (
                <span className={`px-3 py-1 rounded-full text-xs font-medium ${color}`}>
          {status ? "Dostępne" : "Niedostępne"}
        </span>
            );
        },
    },
    {
        id: "actions",
        header: "",
        cell: ({ row }) => {
            const feature = row.original;
            const [open, setOpen] = useState(false);

            const handleDelete = async () => {
                if (!confirm(`Usunąć cechę "${feature.featureTypeName}"?`)) return;

                try {
                    const res = await fetch(`/api/Feature/${feature.id}`, {
                        method: "DELETE",
                        headers: {
                            Authorization: `Bearer ${localStorage.getItem("token")}`
                        },
                        
                    });
                    if (!res.ok) throw new Error("Nie udało się usunąć cechy.");
                    alert("✅ Usunięto cechę!");
                    window.location.reload();
                } catch (err) {
                    alert(`❌ ${err.message}`);
                }
            };

            return (
                <>
                    <DropdownMenu>
                        <DropdownMenuTrigger asChild>
                            <Button variant="ghost" className="h-8 w-8 p-0">
                                <MoreHorizontal />
                            </Button>
                        </DropdownMenuTrigger>
                        <DropdownMenuContent align="end">
                            <DropdownMenuLabel>Akcje</DropdownMenuLabel>
                            <DropdownMenuItem onClick={() => setOpen(true)}>
                                <Edit className="mr-2 h-4 w-4" /> Edytuj
                            </DropdownMenuItem>
                            <DropdownMenuSeparator />
                            <DropdownMenuItem className="text-red-600" onClick={handleDelete}>
                                <Trash2 className="mr-2 h-4 w-4" /> Usuń
                            </DropdownMenuItem>
                        </DropdownMenuContent>
                    </DropdownMenu>

                    <FeatureEditModal
                        open={open}
                        onClose={() => setOpen(false)}
                        feature={feature}
                        onSave={() => window.location.reload()}
                    />
                </>
            );
        },
    },
];
