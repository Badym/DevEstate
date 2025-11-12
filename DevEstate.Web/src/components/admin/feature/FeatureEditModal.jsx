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

export default function FeatureEditModal({ open, onClose, feature, onSave }) {
    const [form, setForm] = useState({
        price: "",
        description: "",
        isAvailable: true,
    });

    useEffect(() => {
        if (feature) {
            setForm({
                price: feature.price ?? "",
                description: feature.description ?? "",
                isAvailable: feature.isAvailable ?? true,
            });
        }
    }, [feature]);

    const handleChange = (e) => {
        const { name, value, type, checked } = e.target;
        setForm((prev) => ({
            ...prev,
            [name]: type === "checkbox" ? checked : value,
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const res = await fetch(`/api/Feature/${feature.id}`, {
                method: "PATCH",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({
                    price: form.price ? Number(form.price) : null,
                    description: form.description || null,
                    isAvailable: form.isAvailable,
                }),
            });
            if (!res.ok) throw new Error("Nie udało się zaktualizować cechy.");
            alert("✅ Zaktualizowano cechę!");
            onSave();
            onClose();
        } catch (err) {
            alert(`❌ ${err.message}`);
        }
    };

    return (
        <Dialog open={open} onOpenChange={onClose}>
            <DialogContent className="sm:max-w-md">
                <DialogHeader>
                    <DialogTitle>Edytuj cechę</DialogTitle>
                </DialogHeader>

                <form onSubmit={handleSubmit} className="space-y-4 mt-2">
                    <div>
                        <label className="block text-xs text-gray-600 mb-1">Cena (PLN)</label>
                        <Input
                            name="price"
                            type="number"
                            step="0.01"
                            value={form.price}
                            onChange={handleChange}
                        />
                    </div>

                    <div>
                        <label className="block text-xs text-gray-600 mb-1">Opis</label>
                        <Input
                            name="description"
                            value={form.description}
                            onChange={handleChange}
                            placeholder="Opis cechy"
                        />
                    </div>

                    <div className="flex items-center space-x-2">
                        <input
                            id="isAvailable"
                            name="isAvailable"
                            type="checkbox"
                            checked={form.isAvailable}
                            onChange={handleChange}
                            className="w-4 h-4 accent-green-600"
                        />
                        <label htmlFor="isAvailable" className="text-sm text-gray-700">
                            Dostępna
                        </label>
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
