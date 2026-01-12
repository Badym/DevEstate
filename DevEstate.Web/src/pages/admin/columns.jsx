import { useState } from "react";
import InvestmentEditModal from "@/components/modals/InvestmentEditModal";
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
        header: "Nazwa inwestycji",
    },
    {
        accessorKey: "city",
        header: "Miasto",
    },
    {
        accessorKey: "status",
        header: "Status",
        cell: ({ row }) => {
            const status = row.getValue("status");
            const color =
                status === "Aktywna"
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
            const investment = row.original;
            const [open, setOpen] = useState(false);

            // 🗑️ Funkcja usuwania inwestycji
            const handleDelete = async () => {
                if (!confirm(`Czy na pewno chcesz usunąć inwestycję "${investment.name}"?`)) return;

                try {
                    const res = await fetch(`/api/investment/${investment.id}`, {
                        method: "DELETE",
                        headers: {
                            Authorization: `Bearer ${localStorage.getItem("token")}`,
                        },
                    });

                    if (!res.ok) throw new Error("Nie udało się usunąć inwestycji.");
                    alert(`✅ Inwestycja "${investment.name}" została usunięta.`);
                    window.location.reload();
                } catch (err) {
                    alert(`❌ Błąd: ${err.message}`);
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

                    {/* 💡 Modal edycji */}
                    <InvestmentEditModal
                        open={open}
                        onClose={() => setOpen(false)}
                        investment={investment}
                        onSave={() => window.location.reload()}
                    />
                </>
            );
        },
    },
];
