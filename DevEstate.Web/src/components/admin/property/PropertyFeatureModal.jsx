import { useEffect, useState } from "react";
import {
    Dialog,
    DialogContent,
    DialogHeader,
    DialogTitle,
    DialogFooter,
} from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";

export default function PropertyFeatureModal({
                                                 open,
                                                 onClose,
                                                 property,
                                                 selectedFeatureIds,
                                                 onSave,
                                             }) {
    const [features, setFeatures] = useState([]);
    const [selected, setSelected] = useState([]);

    useEffect(() => {
        if (!open || !property) return;

        setSelected(selectedFeatureIds || []);

        const loadFeatures = async () => {
            try {
                // 1️⃣ ZAWSZE investment
                const investmentRes = await fetch(
                    `/api/feature/byInvestment/${property.investmentId}`,
                    {
                        headers: {
                            Authorization: `Bearer ${localStorage.getItem("token")}`,
                        },
                    }
                );

                if (!investmentRes.ok) throw new Error();
                let investmentFeatures = await investmentRes.json();

                // 2️⃣ JEŚLI jest building → dociągamy building
                let buildingFeatures = [];
                if (property.buildingId) {
                    const buildingRes = await fetch(
                        `/api/feature/byBuilding/${property.buildingId}`,
                        {
                            headers: {
                                Authorization: `Bearer ${localStorage.getItem("token")}`,
                            },
                        }
                    );

                    if (!buildingRes.ok) throw new Error();
                    buildingFeatures = await buildingRes.json();
                }

                // 3️⃣ ŁĄCZYMY + tylko required + uniq
                const all = [...investmentFeatures, ...buildingFeatures]
                    .filter(f => f.isRequired)
                    .reduce((acc, f) => {
                        if (!acc.find(x => x.id === f.id)) acc.push(f);
                        return acc;
                    }, []);

                setFeatures(all);
            } catch {
                alert("❌ Nie udało się pobrać dodatków");
                setFeatures([]);
            }
        };

        loadFeatures();
    }, [open, property, selectedFeatureIds]);

    const toggleFeature = (id) => {
        setSelected(prev =>
            prev.includes(id)
                ? prev.filter(f => f !== id)
                : [...prev, id]
        );
    };

    const handleSave = () => {
        onSave(selected);
        onClose();
    };

    return (
        <Dialog open={open} onOpenChange={onClose}>
            <DialogContent className="sm:max-w-md">
                <DialogHeader>
                    <DialogTitle>Dodatki do nieruchomości</DialogTitle>
                </DialogHeader>

                <div className="space-y-3 max-h-72 overflow-y-auto mt-2">
                    {features.length > 0 ? (
                        features.map(f => (
                            <label
                                key={f.id}
                                className="flex items-start gap-3 p-3 border rounded-md cursor-pointer hover:bg-gray-50"
                            >
                                <input
                                    type="checkbox"
                                    checked={selected.includes(f.id)}
                                    onChange={() => toggleFeature(f.id)}
                                    className="mt-1"
                                />

                                <div className="flex-1">
                                    <div className="flex justify-between items-start">
                                        <span className="font-medium text-sm text-gray-900">
                                            {f.name}
                                        </span>

                                        {f.price != null && (
                                            <span className="text-sm text-gray-600 whitespace-nowrap">
                                                (+{f.price} PLN)
                                            </span>
                                        )}
                                    </div>

                                    {f.description && (
                                        <p className="text-xs text-gray-500 mt-1">
                                            {f.description}
                                        </p>
                                    )}
                                </div>
                            </label>
                        ))
                    ) : (
                        <p className="text-sm text-gray-500">
                            Brak dostępnych dodatków
                        </p>
                    )}
                </div>

                <DialogFooter>
                    <Button variant="outline" onClick={onClose}>
                        Anuluj
                    </Button>
                    <Button
                        className="bg-green-600 text-white hover:bg-green-700"
                        onClick={handleSave}
                    >
                        Zapisz
                    </Button>
                </DialogFooter>
            </DialogContent>
        </Dialog>
    );
}
