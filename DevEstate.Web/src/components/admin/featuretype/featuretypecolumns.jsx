import { useState } from "react";
import FeatureTypeEditModal from "@/components/admin/featuretype/FeatureTypeEditModal";
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
        accessorKey: "name",
        header: "Nazwa typu cechy",
    },
    {
        accessorKey: "unitName",
        header: "Jednostka",
        cell: ({ row }) => row.getValue("unitName") || "–",
    },
    {
        accessorKey: "isActive",
        header: "Aktywny",
        cell: ({ row }) => {
            const active = row.getValue("isActive");
            const color = active
                ? "bg-green-100 text-green-700"
                : "bg-gray-100 text-gray-600";
            return (
                <span className={`px-3 py-1 rounded-full text-xs font-medium ${color}`}>
                    {active ? "Tak" : "Nie"}
                </span>
            );
        },
    },
    {
        id: "actions",
        header: "",
        cell: ({ row }) => {
            const featureType = row.original;
            const [open, setOpen] = useState(false);

            const handleDelete = async () => {
                if (!confirm(`Usunąć typ cechy "${featureType.name}"?`)) return;

                const token = localStorage.getItem("token");
                const res = await fetch(`/api/FeatureType/${featureType.id}`, {
                    method: "DELETE",
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                });

                if (res.ok) {
                    alert("✅ Usunięto typ cechy.");
                    window.location.reload();
                } else {
                    const text = await res.text();
                    alert(`❌ Błąd (${res.status}): ${text}`);
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

                    <FeatureTypeEditModal
                        open={open}
                        onClose={() => setOpen(false)}
                        featureType={featureType}
                        onSave={() => window.location.reload()}
                    />
                </>
            );
        },
    },
];
