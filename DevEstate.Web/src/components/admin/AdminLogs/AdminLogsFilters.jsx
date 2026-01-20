import { useState } from "react";
import { Button } from "@/components/ui/button";

export default function AdminLogsFilters({ onFilter }) {
    const [userName, setUserName] = useState("");
    const [action, setAction] = useState("");
    const [entity, setEntity] = useState("");

    const applyFilters = () => {
        onFilter({
            userName: userName || undefined,
            action: action || undefined,
            entity: entity || undefined
        });
    };

    return (
        <div className="bg-white p-6 rounded-lg shadow mb-6 flex gap-6 items-end">
            <div className="flex flex-col">
                <label className="text-sm font-medium">Użytkownik</label>
                <input
                    className="border p-2 rounded"
                    value={userName}
                    onChange={(e) => setUserName(e.target.value)}
                />
            </div>

            <div className="flex flex-col">
                <label className="text-sm font-medium">Akcja</label>
                <select
                    className="border p-2 rounded"
                    value={action}
                    onChange={(e) => setAction(e.target.value)}
                >
                    <option value="">Wszystkie</option>
                    <option value="CREATE">CREATE</option>
                    <option value="UPDATE">UPDATE</option>
                    <option value="DELETE">DELETE</option>
                </select>
            </div>

            <div className="flex flex-col">
                <label className="text-sm font-medium">Encja</label>
                <input
                    className="border p-2 rounded"
                    value={entity}
                    onChange={(e) => setEntity(e.target.value)}
                />
            </div>

            <Button className="bg-[#C8A27E] text-white" onClick={applyFilters}>
                Filtruj
            </Button>
        </div>
    );
}
