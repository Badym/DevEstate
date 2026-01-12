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
import { useNotification } from "@/components/NotificationProvider";

export default function BuildingAddModal({ open, onClose, onSave }) {
    const { notifySuccess, notifyError } = useNotification();

    const [form, setForm] = useState({
        investmentId: "",
        buildingNumber: "",
        description: "",
        status: "Aktualne",
    });

    const [investments, setInvestments] = useState([]);

    useEffect(() => {
        if (!open) return;

        fetch("/api/investment/all")
            .then((res) => res.json())
            .then(setInvestments)
            .catch(() => notifyError("Błąd pobierania inwestycji."));
    }, [open]);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setForm((prev) => ({ ...prev, [name]: value }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const res = await fetch("/api/building", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${localStorage.getItem("token")}`,
                },
                body: JSON.stringify(form),
            });

            if (!res.ok) throw new Error("Nie udało się dodać budynku.");

            notifySuccess("Budynek został dodany!");

            // 🔥 najpierw toast, potem zamknięcie, potem odświeżenie danych
            setTimeout(() => {
                onClose();
                onSave();
            }, 400);

        } catch (err) {
            notifyError(err.message);
        }
    };

    return (
        <Dialog open={open} onOpenChange={onClose}>
            <DialogContent className="sm:max-w-xl">
                <DialogHeader>
                    <DialogTitle>Dodaj nowy budynek</DialogTitle>
                </DialogHeader>

                <form onSubmit={handleSubmit} className="space-y-4 mt-4">
                    <div>
                        <label className="block text-sm font-medium">Inwestycja</label>
                        <select
                            name="investmentId"
                            value={form.investmentId}
                            onChange={handleChange}
                            required
                            className="w-full border rounded-md p-2"
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
                        <label className="block text-sm font-medium">Numer budynku</label>
                        <Input
                            name="buildingNumber"
                            value={form.buildingNumber}
                            onChange={handleChange}
                            required
                        />
                    </div>

                    <div>
                        <label className="block text-sm font-medium">Opis</label>
                        <textarea
                            name="description"
                            value={form.description}
                            onChange={handleChange}
                            rows={3}
                            className="w-full border rounded-md p-2"
                        />
                    </div>

                    <div>
                        <label className="block text-sm font-medium">Status</label>
                        <select
                            name="status"
                            value={form.status}
                            onChange={handleChange}
                            className="w-full border rounded-md p-2"
                        >
                            <option value="Aktualne">Aktualne</option>
                            <option value="Zakończony">Zakończony</option>
                        </select>
                    </div>

                    <DialogFooter>
                        <Button type="button" variant="outline" onClick={onClose}>
                            Anuluj
                        </Button>
                        <Button type="submit" className="bg-green-600 text-white">
                            Zapisz
                        </Button>
                    </DialogFooter>
                </form>
            </DialogContent>
        </Dialog>
    );
}
