import { useState } from "react";
import BuildingEditModal from "@/components/admin/building/BuildingEditModal";
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

export const columns = [
    {
        accessorKey: "buildingNumber",
        header: "Numer budynku",
    },
    {
        accessorKey: "investmentName",
        header: "Inwestycja",
    },
    {
        accessorKey: "status",
        header: "Status",
        cell: ({ row }) => {
            const status = row.getValue("status");
            const color =
                status === "Aktualne"
                    ? "bg-green-100 text-green-700"
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
            const building = row.original;
            const [open, setOpen] = useState(false);

            const handleDelete = async () => {
                if (!confirm(`Czy na pewno chcesz usunąć budynek ${building.buildingNumber}?`)) return;

                const token = localStorage.getItem("token");

                const res = await fetch(`/api/building/${building.id}`, {
                    method: "DELETE",
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                });

                if (res.ok) {
                    alert("✅ Budynek został usunięty.");
                    window.location.reload();
                } else if (res.status === 401) {
                    alert("❌ Brak autoryzacji. Zaloguj się ponownie jako administrator.");
                } else if (res.status === 403) {
                    alert("🚫 Brak uprawnień (wymagana rola Admin).");
                } else {
                    const text = await res.text();
                    alert(`❌ Nie udało się usunąć budynku. (${res.status}) ${text}`);
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
                            <DropdownMenuItem
                                className="text-red-600"
                                onClick={handleDelete}
                            >
                                <Trash2 className="mr-2 h-4 w-4" /> Usuń
                            </DropdownMenuItem>
                        </DropdownMenuContent>
                    </DropdownMenu>

                    <BuildingEditModal
                        open={open}
                        onClose={() => setOpen(false)}
                        building={building}
                        onSave={() => window.location.reload()}
                    />
                </>
            );
        },
    },
];
