import { useEffect, useMemo, useState } from "react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import {
    Dialog,
    DialogContent,
    DialogHeader,
    DialogTitle,
    DialogFooter,
} from "@/components/ui/dialog";

export default function PropertyAddModal({ open, onClose, onSave, buildings, investments }) {
    const [form, setForm] = useState({
        type: "apartment",            // apartment / house
        investmentId: "",             // zawsze wybieramy inwestycję najpierw
        buildingId: "",               // tylko dla apartment
        apartmentNumber: "",
        area: "",
        terraceArea: "",
        price: "",
        pricePerMeter: 0,
        status: "Aktualne",
    });

    // ⚙️ Filtrowanie budynków po wybranej inwestycji
    const filteredBuildings = useMemo(() => {
        if (!form.investmentId) return [];
        return (buildings ?? []).filter(b => b.investmentId === form.investmentId);
    }, [buildings, form.investmentId]);

    // 🧮 Auto-przeliczanie ceny za m² z dwoma miejscami po przecinku
    useEffect(() => {
        const area = parseFloat(form.area);
        const price = parseFloat(form.price);
        if (!isNaN(area) && area > 0 && !isNaN(price) && price >= 0) {
            const pricePerMeter = (price / area).toFixed(2); // Oblicz cenę za m², zaokrąglij do 2 miejsc
            setForm(f => ({ ...f, pricePerMeter: parseFloat(pricePerMeter) }));
        } else {
            setForm(f => ({ ...f, pricePerMeter: 0 }));
        }
    }, [form.area, form.price]);

    // 🔄 Zmiana typu: czyścimy pole nieużywane
    useEffect(() => {
        setForm(f => ({
            ...f,
            buildingId: f.type === "apartment" ? f.buildingId : "", // dla house budynek niepotrzebny
        }));
    }, [form.type]);

    // 🔄 Zmiana inwestycji: czyścimy budynek (bo filtr się zmienia)
    useEffect(() => {
        setForm(f => ({ ...f, buildingId: "" }));
    }, [form.investmentId]);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setForm(prev => ({ ...prev, [name]: value }));
    };

    // ✅ Walidacja przed zapisem (frontend)
    const canSubmit = () => {
        if (!form.investmentId) return false; // zawsze wymagamy inwestycji (dla obu typów)
        if (form.type === "apartment" && !form.buildingId) return false;
        if (!form.apartmentNumber) return false;
        if (!form.area || Number(form.area) <= 0) return false;
        if (!form.price || Number(form.price) < 0) return false;
        return true;
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        // 📨 Przygotowanie body zgodne z Twoimi DTO:
        // - apartment  -> buildingId (required), investmentId -> null
        // - house      -> investmentId (required), buildingId -> null
        const body = {
            investmentId: form.investmentId,
            buildingId: form.type === "apartment" ? form.buildingId : null,
            apartmentNumber: form.apartmentNumber,
            type: form.type,
            area: Number(form.area),
            terraceArea: form.terraceArea ? Number(form.terraceArea) : null,
            price: Number(form.price),
            pricePerMeter: Number(form.pricePerMeter),
            status: form.status,
        };

        try {
            const res = await fetch("/api/property", {
                method: "POST",
                headers: { "Content-Type": "application/json",
                    Authorization: `Bearer ${localStorage.getItem("token")}`,},
                body: JSON.stringify(body),
            });
            if (!res.ok) throw new Error("Nie udało się dodać nieruchomości.");
            alert("✅ Dodano nieruchomość!");
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
                    <DialogTitle>Dodaj nieruchomość</DialogTitle>
                </DialogHeader>

                <form onSubmit={handleSubmit} className="space-y-4 mt-4">
                    {/* Typ */}
                    <div>
                        <label className="block text-xs text-gray-600 mb-1">Typ nieruchomości</label>
                        <select
                            name="type"
                            value={form.type}
                            onChange={handleChange}
                            className="w-full border rounded-md p-2 text-sm"
                        >
                            <option value="apartment">Mieszkanie</option>
                            <option value="house">Dom</option>
                        </select>
                    </div>

                    {/* Inwestycja (wymagana dla obu typów – dla apartment służy do filtrowania budynków) */}
                    <div>
                        <label className="block text-xs text-gray-600 mb-1">Inwestycja</label>
                        <select
                            name="investmentId"
                            value={form.investmentId}
                            onChange={handleChange}
                            required
                            className="w-full border rounded-md p-2 text-sm"
                        >
                            <option value="">-- wybierz inwestycję --</option>
                            {(investments ?? []).map(i => (
                                <option key={i.id} value={i.id}>{i.name}</option>
                            ))}
                        </select>
                        <p className="text-xs text-gray-500 mt-1">
                            Najpierw wybierz inwestycję. Dla mieszkania pojawi się wtedy lista budynków z tej inwestycji.
                        </p>
                    </div>

                    {/* Budynek – tylko dla apartment, filtrowany po investmentId */}
                    {form.type === "apartment" && (
                        <div>
                            <label className="block text-xs text-gray-600 mb-1">Budynek</label>
                            <select
                                name="buildingId"
                                value={form.buildingId}
                                onChange={handleChange}
                                required
                                disabled={!form.investmentId}
                                className="w-full border rounded-md p-2 text-sm disabled:bg-gray-100"
                            >
                                <option value="">{form.investmentId ? "-- wybierz budynek --" : "Najpierw wybierz inwestycję"}</option>
                                {filteredBuildings.map(b => (
                                    <option key={b.id} value={b.id}>{b.buildingNumber}</option>
                                ))}
                            </select>
                        </div>
                    )}

                    <div>
                        <label className="block text-xs text-gray-600 mb-1">Numer lokalu / domu</label>
                        <Input
                            name="apartmentNumber"
                            placeholder="Nr lokalu / domu"
                            value={form.apartmentNumber}
                            onChange={handleChange}
                            required
                        />
                    </div>

                    <div>
                        <label className="block text-xs text-gray-600 mb-1">Powierzchnia (m²)</label>
                        <Input
                            name="area"
                            type="number"
                            step="0.01"
                            placeholder="Powierzchnia (m²)"
                            value={form.area}
                            onChange={handleChange}
                            required
                        />
                    </div>

                    <div>
                        <label className="block text-xs text-gray-600 mb-1">Powierzchnia tarasu (opcjonalnie)</label>
                        <Input
                            name="terraceArea"
                            type="number"
                            step="0.01"
                            placeholder="Powierzchnia tarasu (m²)"
                            value={form.terraceArea}
                            onChange={handleChange}
                        />
                    </div>

                    <div>
                        <label className="block text-xs text-gray-600 mb-1">Cena (PLN)</label>
                        <Input
                            name="price"
                            type="number"
                            step="0.01"
                            placeholder="Cena (PLN)"
                            value={form.price}
                            onChange={handleChange}
                            required
                        />
                    </div>

                    <div>
                        <label className="block text-xs text-gray-600 mb-1">Cena za m² (auto)</label>
                        <Input
                            name="pricePerMeter"
                            type="number"
                            step="0.01"
                            placeholder="Cena za m² (auto)"
                            value={form.pricePerMeter}
                            onChange={handleChange}
                            disabled
                        />
                    </div>

                    <div>
                        <label className="block text-xs text-gray-600 mb-1">Status nieruchomości</label>
                        <select
                            name="status"
                            value={form.status}
                            onChange={handleChange}
                            className="w-full border rounded-md p-2 text-sm"
                        >
                            <option value="Aktualne">Aktualne</option>
                            <option value="Zarezerwowane">Zarezerwowane</option>
                            <option value="Sprzedane">Sprzedane</option>
                        </select>
                    </div>

                    <DialogFooter>
                        <Button type="button" variant="outline" onClick={onClose}>Anuluj</Button>
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
