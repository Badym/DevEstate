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

export default function FeatureTypeAddModal({ open, onClose, onSave }) {
    const [form, setForm] = useState({
        name: "",
        unitName: "",
        isActive: true,
    });

    const handleChange = (e) => {
        const { name, value } = e.target;
        setForm((prev) => ({ ...prev, [name]: value }));
    };

    const handleToggle = () => {
        setForm((prev) => ({ ...prev, isActive: !prev.isActive }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const token = localStorage.getItem("token");
            const res = await fetch("/api/FeatureType", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${token}`,
                },
                body: JSON.stringify(form),
            });

            if (!res.ok) throw new Error("Nie udało się dodać typu cechy.");
            alert("✅ Typ cechy został dodany!");
            onSave();
            onClose();
        } catch (err) {
            alert(`❌ Błąd: ${err.message}`);
        }
    };

    return (
        <Dialog open={open} onOpenChange={onClose}>
            <DialogContent className="sm:max-w-md">
                <DialogHeader>
                    <DialogTitle>Dodaj nowy typ cechy</DialogTitle>
                </DialogHeader>

                <form onSubmit={handleSubmit} className="space-y-4 mt-4">
                    <div>
                        <label className="block text-xs text-gray-600 mb-1">Nazwa *</label>
                        <Input
                            name="name"
                            value={form.name}
                            onChange={handleChange}
                            required
                        />
                    </div>

                    <div>
                        <label className="block text-xs text-gray-600 mb-1">
                            Jednostka (np. szt., m²)
                        </label>
                        <Input
                            name="unitName"
                            value={form.unitName}
                            onChange={handleChange}
                        />
                    </div>

                    <div className="flex items-center gap-2 mt-2">
                        <input
                            type="checkbox"
                            checked={form.isActive}
                            onChange={handleToggle}
                            id="isActive"
                        />
                        <label htmlFor="isActive" className="text-sm text-gray-700">
                            Aktywny
                        </label>
                    </div>

                    <DialogFooter>
                        <Button type="button" variant="outline" onClick={onClose}>
                            Anuluj
                        </Button>
                        <Button
                            type="submit"
                            className="bg-green-600 text-white hover:bg-green-700"
                        >
                            Dodaj
                        </Button>
                    </DialogFooter>
                </form>
            </DialogContent>
        </Dialog>
    );
}
