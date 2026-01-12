import { useEffect, useState } from "react";
import { ChevronLeft, ChevronRight } from "lucide-react";
import PriceHistoryChart from "@/components/PriceHistoryChart";
import { Button } from "@/components/ui/button";

export default function PropertyModal({ property, onClose }) {
    const [index, setIndex] = useState(0);
    const [historyOpen, setHistoryOpen] = useState(false);
    const [documents, setDocuments] = useState([]);

    // Zamknięcie ESC
    useEffect(() => {
        const handleEsc = (e) => e.key === "Escape" && onClose();
        window.addEventListener("keydown", handleEsc);
        return () => window.removeEventListener("keydown", handleEsc);
    }, [onClose]);

    // Pobieranie dokumentów przypisanych do nieruchomości
    useEffect(() => {
        const fetchDocuments = async () => {
            try {
                const res = await fetch(`/api/Document/property/${property.id}`);
                if (!res.ok) throw new Error("Nie udało się pobrać dokumentów.");
                const data = await res.json();
                setDocuments(data);  // Zapisanie dokumentów w stanie
            } catch (error) {
                console.error("Błąd podczas pobierania dokumentów: ", error);
            }
        };
        if (property) fetchDocuments();
    }, [property]);

    if (!property) return null;

    const {
        images = [],
        apartmentNumber,
        type,
        area,
        terraceArea,
        price,
        pricePerMeter,
        status,
    } = property;

    // Nawigacja strzałkami
    const next = () => setIndex((prev) => (prev + 1) % images.length);
    const prev = () => setIndex((prev) => (prev - 1 + images.length) % images.length);

    return (
        <div
            className="fixed inset-0 bg-black/70 z-50 flex justify-center items-center p-4"
            onClick={onClose}
        >
            {/* Kontener modala */}
            <div
                className="bg-white rounded-2xl shadow-2xl max-w-4xl w-full max-h-[90vh] overflow-y-auto relative animate-fadeIn"
                onClick={(e) => e.stopPropagation()} // Blokuje zamykanie przy kliknięciu wnętrza
            >
                {/* Header */}
                <div className="flex justify-between items-center p-6 border-b border-gray-200 sticky top-0 bg-white z-10">
                    <h2 className="text-2xl font-semibold text-[#1A1A1A]">
                        Lokal {apartmentNumber} ({type === "apartment" ? "Mieszkanie" : "Dom"})
                    </h2>
                    <button
                        onClick={onClose}
                        className="text-gray-400 hover:text-gray-600 text-3xl font-light"
                    >
                        ×
                    </button>
                </div>

                {/* Galeria */}
                {images && images.length > 0 && (
                    <div className="relative w-full h-80 overflow-hidden">
                        {/* Zdjęcie */}
                        <img
                            key={index}
                            src={images[index]}
                            alt={`Zdjęcie ${apartmentNumber}`}
                            className="w-full h-full object-cover transition-all duration-500"
                        />

                        {/* Strzałki */}
                        {images.length > 1 && (
                            <>
                                <button
                                    onClick={prev}
                                    className="absolute top-1/2 left-4 -translate-y-1/2 bg-black/40 hover:bg-black/60 text-white rounded-full p-2"
                                >
                                    <ChevronLeft size={24} />
                                </button>
                                <button
                                    onClick={next}
                                    className="absolute top-1/2 right-4 -translate-y-1/2 bg-black/40 hover:bg-black/60 text-white rounded-full p-2"
                                >
                                    <ChevronRight size={24} />
                                </button>
                            </>
                        )}

                        {/* Kropki (indykatory) */}
                        {images.length > 1 && (
                            <div className="absolute bottom-3 w-full flex justify-center gap-2">
                                {images.map((_, i) => (
                                    <button
                                        key={i}
                                        onClick={() => setIndex(i)}
                                        className={`w-2.5 h-2.5 rounded-full transition-all ${
                                            i === index ? "bg-white" : "bg-white/50 hover:bg-white/70"
                                        }`}
                                    />
                                ))}
                            </div>
                        )}
                    </div>
                )}

                {/* Treść główna */}
                <div className="p-8 grid grid-cols-1 sm:grid-cols-2 gap-6 text-gray-700">
                    <div>
                        <p>
                            <strong>Typ:</strong> {type === "apartment" ? "Mieszkanie" : "Dom"}
                        </p>
                        <p>
                            <strong>Status:</strong>{" "}
                            <span
                                className={`font-semibold ${
                                    status === "Sprzedane"
                                        ? "text-red-600"
                                        : status === "Zarezerwowane"
                                            ? "text-yellow-600"
                                            : "text-green-700"
                                }`}
                            >
                                {status}
                            </span>
                        </p>
                        <p>
                            <strong>Powierzchnia:</strong> {area} m²
                        </p>
                        {terraceArea && (
                            <p>
                                <strong>Taras:</strong> {terraceArea} m²
                            </p>
                        )}
                    </div>
                    <div>
                        <p>
                            <strong>Cena:</strong> {price.toLocaleString("pl-PL")} zł
                        </p>
                        <p>
                            <strong>Cena za m²:</strong> {pricePerMeter} zł/m²
                        </p>
                    </div>
                </div>

                {/* Dokumenty (jeśli są) */}
                {documents && documents.length > 0 && (
                    <div className="border-t border-gray-200 p-8 text-gray-600">
                        <h4 className="text-lg font-semibold mb-4 text-[#1A1A1A]">Dokumenty</h4>
                        <div className="space-y-4">
                            {documents.map((doc) => (
                                <a
                                    key={doc.id}
                                    href={doc.fileUrl}
                                    target="_blank"
                                    rel="noopener noreferrer"
                                    className="block text-[#C8A27E] hover:text-[#b18e6b] font-semibold text-lg"
                                >
                                    {doc.fileName}
                                </a>
                            ))}
                        </div>
                    </div>
                )}

                {/* Historia cen */}
                <div className="border-t border-gray-200 p-8 text-gray-600">
                    <h4 className="text-lg font-semibold mb-4 text-[#1A1A1A]">
                        Historia cen
                    </h4>

                    <Button
                        onClick={() => setHistoryOpen(true)}
                        className="bg-indigo-600 text-white hover:bg-indigo-700"
                    >
                        Zobacz historię cen
                    </Button>

                    <PriceHistoryChart
                        propertyId={property.id}
                        open={historyOpen}
                        onClose={() => setHistoryOpen(false)}
                    />
                </div>
            </div>
        </div>
    );
}
