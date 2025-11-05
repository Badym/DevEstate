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

export default function PropertyEditModal({ open, onClose, property, onSave }) {
    const [form, setForm] = useState({
        price: "",
        status: "Aktualne",
    });
    const [images, setImages] = useState([]);

    // 🧠 Wczytaj dane nieruchomości przy otwarciu
    useEffect(() => {
        if (property) {
            setForm({
                price: property.price ?? "",
                status: property.status ?? "Aktualne",
            });
            setImages(property.images || []);
        }
    }, [property]);

    // 🔁 Zmiana pól formularza
    const handleChange = (e) => {
        const { name, value } = e.target;
        setForm((prev) => ({ ...prev, [name]: value }));
    };

    // 💾 Zapis zmian
    const handleSubmit = async (e) => {
        e.preventDefault();
        const body = {
            price: form.price !== "" ? Number(form.price) : null,
            status: form.status || null,
        };

        try {
            const res = await fetch(`/api/property/${property.id}`, {
                method: "PATCH",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${localStorage.getItem("token")}`,
                },
                body: JSON.stringify(body),
            });
            if (!res.ok) throw new Error("Nie udało się zaktualizować nieruchomości.");
            alert("✅ Zaktualizowano nieruchomość!");
            onSave();
            onClose();
        } catch (err) {
            alert(`❌ Błąd: ${err.message}`);
        }
    };

    // 📤 Upload zdjęcia
    const handleUpload = async (e) => {
        const file = e.target.files[0];
        if (!file) return;

        const formData = new FormData();
        formData.append("file", file);

        try {
            const res = await fetch(`/api/image/property/${property.id}`, {
                method: "POST",
                body: formData,
            });

            if (!res.ok) throw new Error("Nie udało się przesłać zdjęcia.");
            const data = await res.json();

            setImages((prev) => [...prev, data.fileUrl]);
            e.target.value = "";

            alert("✅ Zdjęcie zostało pomyślnie dodane!");
        } catch (err) {
            alert(err.message);
        }
    };

    // 🗑️ Usuwanie zdjęcia
    const handleDeleteImage = async (imageUrl) => {
        if (!confirm("Czy na pewno chcesz usunąć to zdjęcie?")) return;

        try {
            const res = await fetch(
                `/api/image/property/${property.id}?imageUrl=${encodeURIComponent(imageUrl)}`,
                { method: "DELETE" }
            );
            if (!res.ok) throw new Error("Nie udało się usunąć zdjęcia.");
            setImages((prev) => prev.filter((img) => img !== imageUrl));
        } catch (err) {
            alert(err.message);
        }
    };

    return (
        <Dialog open={open} onOpenChange={onClose}>
            <DialogContent className="sm:max-w-xl">
                <DialogHeader>
                    <DialogTitle>Edytuj nieruchomość</DialogTitle>
                </DialogHeader>

                <form onSubmit={handleSubmit} className="space-y-4 mt-4">
                    {/* 🔹 Dane podstawowe */}
                    <div className="grid grid-cols-2 gap-3">
                        <div>
                            <label className="block text-xs text-gray-500 mb-1">Nr lokalu / domu</label>
                            <Input value={property?.apartmentNumber ?? "-"} disabled />
                        </div>
                        <div>
                            <label className="block text-xs text-gray-500 mb-1">Typ</label>
                            <Input value={property?.type === "apartment" ? "Mieszkanie" : "Dom"} disabled />
                        </div>
                    </div>

                    <div className="grid grid-cols-2 gap-3">
                        <div>
                            <label className="block text-xs text-gray-500 mb-1">Cena (PLN)</label>
                            <Input
                                name="price"
                                type="number"
                                step="0.01"
                                value={form.price}
                                onChange={handleChange}
                            />
                        </div>

                        <div>
                            <label className="block text-xs text-gray-500 mb-1">Status</label>
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
                    </div>

                    {/* 🖼️ Sekcja zdjęć */}
                    <div className="mt-6">
                        <div className="flex items-center justify-between mb-3">
                            <h3 className="text-sm font-semibold text-gray-800">Zdjęcia nieruchomości</h3>
                            <label className="cursor-pointer text-sm font-medium text-green-600 hover:underline">
                                ➕ Dodaj zdjęcie
                                <input
                                    type="file"
                                    accept="image/*"
                                    onChange={handleUpload}
                                    className="hidden"
                                />
                            </label>
                        </div>

                        {images.length > 0 ? (
                            <div className="grid grid-cols-3 gap-3">
                                {images.map((img, i) => (
                                    <div key={i} className="relative group">
                                        <img
                                            src={img}
                                            alt="nieruchomość"
                                            className="w-full h-28 object-cover rounded-md border"
                                        />
                                        <button
                                            type="button"
                                            onClick={() => handleDeleteImage(img)}
                                            className="absolute top-1 right-1 bg-red-600 text-white rounded-full p-1 opacity-0 group-hover:opacity-100 transition"
                                        >
                                            ✕
                                        </button>
                                    </div>
                                ))}
                            </div>
                        ) : (
                            <p className="text-sm text-gray-500">Brak zdjęć dla tej nieruchomości.</p>
                        )}
                    </div>

                    {/* 📅 Data utworzenia */}
                    <div>
                        <label className="block text-xs text-gray-500 mb-1">Data utworzenia</label>
                        <Input
                            value={
                                property?.createdAt
                                    ? new Date(property.createdAt).toLocaleDateString("pl-PL")
                                    : "-"
                            }
                            disabled
                        />
                    </div>

                    <DialogFooter>
                        <Button type="button" variant="outline" onClick={onClose}>
                            Anuluj
                        </Button>
                        <Button type="submit" className="bg-green-600 text-white hover:bg-green-700">
                            Zapisz zmiany
                        </Button>
                    </DialogFooter>
                </form>
            </DialogContent>
        </Dialog>
    );
}
