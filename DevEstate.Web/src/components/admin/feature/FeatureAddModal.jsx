import { useState, useMemo } from "react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import {
    Dialog,
    DialogContent,
    DialogHeader,
    DialogTitle,
    DialogFooter,
} from "@/components/ui/dialog";

export default function FeatureAddModal({
                                            open,
                                            onClose,
                                            onSave,
                                            featureTypes,
                                            buildings,
                                            investments,
                                        }) {
    const [form, setForm] = useState({
        featureTypeId: "",
        price: "",
        description: "",
        scope: "building", // "building" | "investment"
        buildingId: "",
        investmentId: "",
        isAvailable: true,
    });

    // ⚙️ Filtrowanie budynków po inwestycji
    const filteredBuildings = useMemo(() => {
        if (!form.investmentId) return buildings ?? [];
        return (buildings ?? []).filter((b) => b.investmentId === form.investmentId);
    }, [buildings, form.investmentId]);

    const handleChange = (e) => {
        const { name, value, type, checked } = e.target;
        setForm((prev) => ({
            ...prev,
            [name]: type === "checkbox" ? checked : value,
        }));
    };

    const canSubmit = () => {
        if (!form.featureTypeId) return false;
        if (form.scope === "building" && !form.buildingId) return false;
        if (form.scope === "investment" && !form.investmentId) return false;
        return true;
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        const body = {
            featureTypeId: form.featureTypeId,
            description: form.description || null,
            price: form.price ? Number(form.price) : 0,
            isAvailable: form.isAvailable,
            buildingId: form.scope === "building" ? form.buildingId : null,
            investmentId: form.scope === "investment" ? form.investmentId : null,
        };

        try {
            const res = await fetch("/api/Feature", {
                method: "POST",
                headers: { "Content-Type": "application/json",
                    Authorization: `Bearer ${localStorage.getItem("token")}`,},
                body: JSON.stringify(body),
            });
            if (!res.ok) throw new Error("Nie udało się dodać cechy.");
            alert("✅ Dodano cechę!");
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
                    <DialogTitle>Dodaj nową cechę (Feature)</DialogTitle>
                </DialogHeader>

                <form onSubmit={handleSubmit} className="space-y-4 mt-4">
                    {/* Typ cechy */}
                    <div>
                        <label className="block text-xs text-gray-600 mb-1">Typ cechy</label>
                        <select
                            name="featureTypeId"
                            value={form.featureTypeId}
                            onChange={handleChange}
                            required
                            className="w-full border rounded-md p-2 text-sm"
                        >
                            <option value="">-- wybierz typ cechy --</option>
                            {(featureTypes ?? []).map((ft) => (
                                <option key={ft.id} value={ft.id}>
                                    {ft.name}
                                </option>
                            ))}
                        </select>
                    </div>

                    {/* Zakres */}
                    <div>
                        <label className="block text-xs text-gray-600 mb-1">Zakres</label>
                        <select
                            name="scope"
                            value={form.scope}
                            onChange={handleChange}
                            className="w-full border rounded-md p-2 text-sm"
                        >
                            <option value="building">Dla budynku</option>
                            <option value="investment">Dla inwestycji</option>
                        </select>
                    </div>

                    {/* Inwestycja — obowiązkowa dla obu */}
                    <div>
                        <label className="block text-xs text-gray-600 mb-1">Inwestycja</label>
                        <select
                            name="investmentId"
                            value={form.investmentId}
                            onChange={handleChange}
                            required={form.scope === "investment"}
                            className="w-full border rounded-md p-2 text-sm"
                        >
                            <option value="">-- wybierz inwestycję --</option>
                            {(investments ?? []).map((inv) => (
                                <option key={inv.id} value={inv.id}>
                                    {inv.name}
                                </option>
                            ))}
                        </select>
                    </div>

                    {/* Budynek — tylko przy scope=building */}
                    {form.scope === "building" && (
                        <div>
                            <label className="block text-xs text-gray-600 mb-1">Budynek</label>
                            <select
                                name="buildingId"
                                value={form.buildingId}
                                onChange={handleChange}
                                disabled={!form.investmentId}
                                required={form.scope === "building"}
                                className="w-full border rounded-md p-2 text-sm disabled:bg-gray-100"
                            >
                                <option value="">
                                    {form.investmentId
                                        ? "-- wybierz budynek --"
                                        : "Najpierw wybierz inwestycję"}
                                </option>
                                {filteredBuildings.map((b) => (
                                    <option key={b.id} value={b.id}>
                                        {b.buildingNumber}
                                    </option>
                                ))}
                            </select>
                        </div>
                    )}

                    {/* Cena */}
                    <div>
                        <label className="block text-xs text-gray-600 mb-1">Cena (PLN)</label>
                        <Input
                            name="price"
                            type="number"
                            step="0.01"
                            placeholder="Cena (PLN)"
                            value={form.price}
                            onChange={handleChange}
                        />
                    </div>

                    {/* Opis */}
                    <div>
                        <label className="block text-xs text-gray-600 mb-1">Opis</label>
                        <Input
                            name="description"
                            placeholder="Opis cechy"
                            value={form.description}
                            onChange={handleChange}
                        />
                    </div>

                    {/* Dostępność */}
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
                            Cechę można aktualnie przypisać
                        </label>
                    </div>

                    <DialogFooter>
                        <Button type="button" variant="outline" onClick={onClose}>
                            Anuluj
                        </Button>
                        <Button
                            type="submit"
                            disabled={!canSubmit()}
                            className="bg-green-600 text-white hover:bg-green-700 disabled:opacity-50"
                        >
                            Zapisz
                        </Button>
                    </DialogFooter>
                </form>
            </DialogContent>
        </Dialog>
    );
}
