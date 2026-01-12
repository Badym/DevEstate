import { useState } from "react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import {
    Dialog,
    DialogContent,
    DialogHeader,
    DialogTitle,
    DialogFooter,
} from "@/components/ui/dialog";

export default function InvestmentAddModal({ open, onClose, onSave }) {
    const [form, setForm] = useState({
        name: "",
        city: "",
        street: "",
        postalCode: "",
        description: "",
        status: "Aktualne",
    });

    const handleChange = (e) => {
        const { name, value } = e.target;
        setForm((prev) => ({ ...prev, [name]: value }));
    };

    // 🟢 Dodawanie nowej inwestycji
    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const body = { ...form }; // 👈 bez companyId!

            const res = await fetch("/api/investment", {
                method: "POST",
                headers: { "Content-Type": "application/json",
                Authorization: `Bearer ${localStorage.getItem("token")}` },
                body: JSON.stringify(body),
            });

            if (!res.ok) {
                const msg = await res.text();
                throw new Error(msg || "Nie udało się dodać inwestycji.");
            }

            alert("✅ Inwestycja została dodana pomyślnie!");
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
                    <DialogTitle>Dodaj nową inwestycję</DialogTitle>
                </DialogHeader>

                <form onSubmit={handleSubmit} className="space-y-4 mt-4">
                    <div className="grid grid-cols-2 gap-4">
                        <div>
                            <label className="block text-sm font-medium text-gray-700">Nazwa</label>
                            <Input name="name" value={form.name} onChange={handleChange} required />
                        </div>

                        <div>
                            <label className="block text-sm font-medium text-gray-700">Miasto</label>
                            <Input name="city" value={form.city} onChange={handleChange} required />
                        </div>

                        <div>
                            <label className="block text-sm font-medium text-gray-700">Ulica</label>
                            <Input name="street" value={form.street} onChange={handleChange} />
                        </div>

                        <div>
                            <label className="block text-sm font-medium text-gray-700">Kod pocztowy</label>
                            <Input name="postalCode" value={form.postalCode} onChange={handleChange} />
                        </div>
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
                            <option value="Sprzedane">Sprzedane</option>
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
