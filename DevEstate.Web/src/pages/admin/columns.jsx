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
        cell: ({ row }) => (
            <span className="font-medium text-[#1A1A1A]">
        {row.getValue("name")}
      </span>
        ),
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
                <span
                    className={`px-3 py-1 rounded-full text-xs font-medium ${color}`}
                >
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

            return (
                <DropdownMenu>
                    <DropdownMenuTrigger asChild>
                        <Button variant="ghost" className="h-8 w-8 p-0">
                            <MoreHorizontal />
                        </Button>
                    </DropdownMenuTrigger>
                    <DropdownMenuContent align="end">
                        <DropdownMenuLabel>Akcje</DropdownMenuLabel>
                        <DropdownMenuItem
                            onClick={() =>
                                alert(`Edytuj inwestycję: ${investment.name}`)
                            }
                        >
                            <Edit className="mr-2 h-4 w-4" /> Edytuj
                        </DropdownMenuItem>
                        <DropdownMenuSeparator />
                        <DropdownMenuItem
                            className="text-red-600"
                            onClick={() =>
                                alert(`Usuń inwestycję: ${investment.name}`)
                            }
                        >
                            <Trash2 className="mr-2 h-4 w-4" /> Usuń
                        </DropdownMenuItem>
                    </DropdownMenuContent>
                </DropdownMenu>
            );
        },
    },
];
