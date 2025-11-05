import { useState, useEffect } from "react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import {
    Dialog,
    DialogContent,
    DialogHeader,
    DialogTitle,
    DialogFooter,
} from "@/components/ui/dialog";

export default function BuildingAddModal({ open, onClose, onSave }) {
    const [form, setForm] = useState({
        investmentId: "",
        buildingNumber: "",
        description: "",
        status: "Aktualne",
    });

    const [investments, setInvestments] = useState([]);

    // 📡 Pobierz inwestycje przy otwarciu modala
    useEffect(() => {
        if (!open) return;
        fetch("/api/investment/all")
            .then((res) => res.json())
            .then(setInvestments)
            .catch((err) => console.error("Błąd pobierania inwestycji:", err));
    }, [open]);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setForm((prev) => ({ ...prev, [name]: value }));
    };

    // 🟢 Dodawanie budynku
    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const res = await fetch("/api/building", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(form),
            });

            if (!res.ok) throw new Error("Nie udało się dodać budynku.");

            alert("✅ Budynek został dodany pomyślnie!");
            onSave();
            onClose();
        } catch (err) {
            alert(`❌ Błąd: ${err.message}`);
        }
    };

    return (
        <Dialog open={open} onOpenChange={onClose}>
            <DialogContent className="sm:max-w-xl">
                <DialogHeader>
                    <DialogTitle>Dodaj nowy budynek</DialogTitle>
                </DialogHeader>

                <form onSubmit={handleSubmit} className="space-y-4 mt-4">
                    {/* 🔽 Wybór inwestycji */}
                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-1">
                            Inwestycja
                        </label>
                        <select
                            name="investmentId"
                            value={form.investmentId}
                            onChange={handleChange}
                            required
                            className="w-full border border-gray-300 rounded-md p-2 text-sm focus:outline-none focus:ring-2 focus:ring-[#C8A27E]"
                        >
                            <option value="">-- wybierz inwestycję --</option>
                            {investments.map((inv) => (
                                <option key={inv.id} value={inv.id}>
                                    {inv.name}
                                </option>
                            ))}
                        </select>
                    </div>

                    <div>
                        <label className="block text-sm font-medium text-gray-700">Numer budynku</label>
                        <Input
                            name="buildingNumber"
                            value={form.buildingNumber}
                            onChange={handleChange}
                            required
                        />
                    </div>

                    <div>
                        <label className="block text-sm font-medium text-gray-700">Opis</label>
                        <textarea
                            name="description"
                            value={form.description}
                            onChange={handleChange}
                            rows={3}
                            className="w-full border border-gray-300 rounded-md p-2 text-sm focus:outline-none focus:ring-2 focus:ring-[#C8A27E]"
                        />
                    </div>

                    <div>
                        <label className="block text-sm font-medium text-gray-700">Status</label>
                        <select
                            name="status"
                            value={form.status}
                            onChange={handleChange}
                            className="w-full border border-gray-300 rounded-md p-2 text-sm focus:outline-none focus:ring-2 focus:ring-[#C8A27E]"
                        >
                            <option value="Aktualne">Aktualne</option>
                            <option value="Zakończony">Zakończony</option>
                        </select>
                    </div>

                    <DialogFooter>
                        <Button type="button" variant="outline" onClick={onClose}>
                            Anuluj
                        </Button>
                        <Button type="submit" className="bg-green-600 text-white hover:bg-green-700">
                            Zapisz
                        </Button>
                    </DialogFooter>
                </form>
            </DialogContent>
        </Dialog>
    );
}
