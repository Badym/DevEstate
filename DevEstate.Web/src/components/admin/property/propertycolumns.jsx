import { useState } from "react";
import PropertyEditModal from "@/components/admin/property/PropertyEditModal";
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

const formatPln = (v) =>
    typeof v === "number" || typeof v === "bigint"
        ? v.toLocaleString("pl-PL", { style: "currency", currency: "PLN", maximumFractionDigits: 0 })
        : v;

export const columns = [
    {
        accessorKey: "apartmentNumber",
        header: "Nr lokalu / domu",
    },
    {
        accessorKey: "type",
        header: "Typ",
        cell: ({ row }) => {
            const t = row.getValue("type");
            const label = t === "apartment" ? "Mieszkanie" : "Dom";
            return (
                <span className="px-2 py-0.5 rounded-md bg-slate-100 text-slate-700 text-xs">
          {label}
        </span>
            );
        },
    },
    {
        accessorKey: "buildingNumber",
        header: "Budynek",
    },
    {
        accessorKey: "investmentName",
        header: "Inwestycja",
    },
    {
        accessorKey: "area",
        header: "Pow. (m²)",
    },
    {
        accessorKey: "price",
        header: "Cena",
        cell: ({ row }) => <span>{formatPln(row.getValue("price"))}</span>,
    },
    {
        accessorKey: "pricePerMeter",
        header: "Cena/m²",
        cell: ({ row }) => {
            const v = row.getValue("pricePerMeter");
            return <span>{typeof v === "number" ? `${v.toLocaleString("pl-PL")} PLN` : "-"}</span>;
        },
    },
    {
        accessorKey: "status",
        header: "Status",
        cell: ({ row }) => {
            const status = row.getValue("status");
            const color =
                status === "Aktualne"
                    ? "bg-green-100 text-green-700"
                    : status === "Zarezerwowane"
                        ? "bg-amber-100 text-amber-700"
                        : "bg-gray-100 text-gray-600";
            return (
                <span className={`px-3 py-1 rounded-full text-xs font-medium ${color}`}>
          {status}
        </span>
            );
        },
    },
    {
        id: "actions",
        header: "",
        cell: ({ row }) => {
            const property = row.original;
            const [open, setOpen] = useState(false);

            const handleDelete = async () => {
                if (!confirm(`Usunąć "${property.apartmentNumber}"?`)) return;

                const token = localStorage.getItem("token");
                const res = await fetch(`/api/property/${property.id}`, {
                    method: "DELETE",
                    headers: { Authorization: `Bearer ${token}` },
                });

                if (res.ok) {
                    alert("✅ Usunięto.");
                    window.location.reload();
                } else if (res.status === 401) {
                    alert("❌ Brak autoryzacji. Zaloguj się ponownie jako administrator.");
                } else if (res.status === 403) {
                    alert("🚫 Brak uprawnień (wymagana rola Admin).");
                } else {
                    const text = await res.text();
                    alert(`❌ Błąd usuwania (${res.status}) ${text}`);
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

                    <PropertyEditModal
                        open={open}
                        onClose={() => setOpen(false)}
                        property={property}
                        onSave={() => window.location.reload()}
                    />
                </>
            );
        },
    },
];
