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

export default function InvestmentEditModal({ open, onClose, investment, onSave }) {
    const [form, setForm] = useState({
        name: "",
        city: "",
        street: "",
        postalCode: "",
        description: "",
        status: "",
        investmentProvince: "",
        investmentCounty: "",
        investmentMunicipality: "",
    });

    const [images, setImages] = useState([]);
    const [documents, setDocuments] = useState([]);

    // 🔄 Wczytaj dane inwestycji przy otwarciu
    useEffect(() => {
        if (investment) {
            setForm({
                name: investment.name || "",
                city: investment.city || "",
                street: investment.street || "",
                postalCode: investment.postalCode || "",
                description: investment.description || "",
                status: investment.status || "",
                investmentProvince: investment.investmentProvince || "",
                investmentCounty: investment.investmentCounty || "",
                investmentMunicipality: investment.investmentMunicipality || "",
            });

            setImages(investment.images || []);

            // Pobierz dokumenty przypisane do inwestycji
            fetch(`/api/document/investment/${investment.id}`)
                .then((res) => (res.ok ? res.json() : []))
                .then((data) => setDocuments(data))
                .catch(() => setDocuments([]));
        }
    }, [investment]);

    // 🖊️ Zmiana pól formularza
    const handleChange = (e) => {
        const { name, value } = e.target;
        setForm((prev) => ({ ...prev, [name]: value }));
    };

    // 💾 Zapis inwestycji
    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const res = await fetch(`/api/investment/${investment.id}`, {
                method: "PATCH",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(form),
            });

            if (!res.ok) throw new Error("Nie udało się zaktualizować inwestycji.");
            onSave();
            onClose();
        } catch (err) {
            alert(err.message);
        }
    };

    // 📤 Upload zdjęcia
    const handleUploadImage = async (e) => {
        const file = e.target.files[0];
        if (!file) return;

        const formData = new FormData();
        formData.append("file", file);

        try {
            const res = await fetch(`/api/image/investment/${investment.id}`, {
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
                `/api/image/investment/${investment.id}?imageUrl=${encodeURIComponent(imageUrl)}`,
                { method: "DELETE" }
            );
            if (!res.ok) throw new Error("Nie udało się usunąć zdjęcia.");

            setImages((prev) => prev.filter((img) => img !== imageUrl));
        } catch (err) {
            alert(err.message);
        }
    };

    // 📤 Upload dokumentu
    const handleUploadDocument = async (e) => {
        const file = e.target.files[0];
        if (!file) return;

        const formData = new FormData();
        formData.append("file", file);

        try {
            const res = await fetch(`/api/document/investment/${investment.id}`, {
                method: "POST",
                body: formData,
            });

            if (!res.ok) throw new Error("Nie udało się przesłać dokumentu.");
            const data = await res.json();

            setDocuments((prev) => [
                ...prev,
                { fileName: data.fileName, fileUrl: data.fileUrl, fileType: data.fileType },
            ]);
            e.target.value = "";

            alert("✅ Dokument został pomyślnie dodany!");
        } catch (err) {
            alert(err.message);
        }
    };

    // 🗑️ Usuwanie dokumentu
    const handleDeleteDocument = async (docId) => {
        if (!confirm("Czy na pewno chcesz usunąć ten dokument?")) return;

        try {
            const res = await fetch(`/api/document/${docId}`, { method: "DELETE" });
            if (!res.ok) throw new Error("Nie udało się usunąć dokumentu.");

            setDocuments((prev) => prev.filter((doc) => doc.id !== docId));
        } catch (err) {
            alert(err.message);
        }
    };

    return (
        <Dialog open={open} onOpenChange={onClose}>
            <DialogContent className="sm:max-w-xl">
                <DialogHeader>
                    <DialogTitle>Edytuj inwestycję</DialogTitle>
                </DialogHeader>

                <form onSubmit={handleSubmit} className="space-y-4 mt-4">
                    {/* 🔹 Pola edycji */}
                    <div className="grid grid-cols-2 gap-4">
                        <div>
                            <label className="block text-sm font-medium text-gray-700">Nazwa</label>
                            <Input name="name" value={form.name} onChange={handleChange} />
                        </div>

                        <div>
                            <label className="block text-sm font-medium text-gray-700">Miasto</label>
                            <Input name="city" value={form.city} onChange={handleChange} />
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
                            className="w-full border border-gray-300 rounded-md p-2 text-sm"
                        >
                            <option value="Aktualne">Aktualne</option>
                            <option value="Sprzedane">Sprzedane</option>
                        </select>
                    </div>

                    {/* 🏢 Dodatkowe pola */}
                    <div>
                        <label className="block text-sm font-medium text-gray-700">Województwo</label>
                        <Input
                            name="investmentProvince"
                            value={form.investmentProvince}
                            onChange={handleChange}
                        />
                    </div>

                    <div>
                        <label className="block text-sm font-medium text-gray-700">Powiat</label>
                        <Input
                            name="investmentCounty"
                            value={form.investmentCounty}
                            onChange={handleChange}
                        />
                    </div>

                    <div>
                        <label className="block text-sm font-medium text-gray-700">Gmina</label>
                        <Input
                            name="investmentMunicipality"
                            value={form.investmentMunicipality}
                            onChange={handleChange}
                        />
                    </div>

                    {/* 🖼️ Sekcja zdjęć */}
                    <div className="mt-6">
                        <div className="flex items-center justify-between mb-3">
                            <h3 className="text-sm font-semibold text-gray-800">Zdjęcia inwestycji</h3>
                            <label className="cursor-pointer text-sm font-medium text-green-600 hover:underline">
                                ➕ Dodaj zdjęcie
                                <input
                                    type="file"
                                    accept="image/*"
                                    onChange={handleUploadImage}
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
                                            alt="investment"
                                            className="w-full h-28 object-cover rounded-md border"
                                        />

                                        {/* 🔥 Widoczny przycisk usuwania */}
                                        <button
                                            type="button"
                                            onClick={() => handleDeleteImage(img)}
                                            className="
                        absolute top-1 right-1
                        bg-red-600 text-white
                        rounded-full p-1
                        opacity-0 group-hover:opacity-100
                        transition-opacity duration-200
                        shadow-md
                    "
                                        >
                                            ✕
                                        </button>
                                    </div>
                                ))}
                            </div>
                        ) : (
                            <p className="text-sm text-gray-500">Brak zdjęć dla tej inwestycji.</p>
                        )}

                    </div>

                    {/* 📄 Sekcja dokumentów */}
                    <div className="mt-6">
                        <div className="flex items-center justify-between mb-3">
                            <h3 className="text-sm font-semibold text-gray-800">Dokumenty inwestycji</h3>
                            <label className="cursor-pointer text-sm font-medium text-green-600 hover:underline">
                                ➕ Dodaj dokument
                                <input
                                    type="file"
                                    accept=".pdf,.doc,.docx"
                                    onChange={handleUploadDocument}
                                    className="hidden"
                                />
                            </label>
                        </div>

                        {documents.length > 0 ? (
                            <ul className="space-y-2">
                                {documents.map((doc, i) => (
                                    <li
                                        key={i}
                                        className="flex justify-between items-center bg-gray-50 px-3 py-2 rounded-md"
                                    >
                                        <a
                                            href={doc.fileUrl}
                                            target="_blank"
                                            rel="noopener noreferrer"
                                            className="text-sm text-blue-600 hover:underline"
                                        >
                                            📎 {doc.fileName}
                                        </a>
                                        <button
                                            type="button"
                                            onClick={() => handleDeleteDocument(doc.id)}
                                            className="text-red-500 text-xs hover:underline"
                                        >
                                            Usuń
                                        </button>
                                    </li>
                                ))}
                            </ul>
                        ) : (
                            <p className="text-sm text-gray-500">Brak dokumentów dla tej inwestycji.</p>
                        )}
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
