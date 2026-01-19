import { useEffect, useState } from "react";
import { ChevronLeft, ChevronRight } from "lucide-react";
import PriceHistoryChart from "@/components/PriceHistoryChart";
import { Button } from "@/components/ui/button";

export default function PropertyModal({ property, onClose }) {
    const [index, setIndex] = useState(0);
    const [historyOpen, setHistoryOpen] = useState(false);
    const [documents, setDocuments] = useState([]);
    const [lightboxOpen, setLightboxOpen] = useState(false);

    // Zamknięcie ESC: najpierw lightbox, potem modal
    useEffect(() => {
        const handleEsc = (e) => {
            if (e.key !== "Escape") return;

            if (lightboxOpen) {
                setLightboxOpen(false);
                return;
            }

            onClose();
        };

        window.addEventListener("keydown", handleEsc);
        return () => window.removeEventListener("keydown", handleEsc);
    }, [onClose, lightboxOpen]);

    // Reset po zmianie nieruchomości
    useEffect(() => {
        setIndex(0);
        setLightboxOpen(false);
        setHistoryOpen(false);
    }, [property?.id]);

    // Pobieranie dokumentów przypisanych do nieruchomości
    useEffect(() => {
        const fetchDocuments = async () => {
            try {
                const res = await fetch(`/api/Document/property/${property.id}`);
                if (!res.ok) throw new Error("Nie udało się pobrać dokumentów.");
                const data = await res.json();
                setDocuments(data);
            } catch (error) {
                console.error("Błąd podczas pobierania dokumentów: ", error);
                setDocuments([]);
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

    const hasImages = Array.isArray(images) && images.length > 0;

    // Nawigacja strzałkami
    const next = () => hasImages && setIndex((prev) => (prev + 1) % images.length);
    const prev = () => hasImages && setIndex((prev) => (prev - 1 + images.length) % images.length);

    return (
        <div
            className="fixed inset-0 bg-black/70 z-50 flex justify-center items-center p-4"
            onClick={onClose}
        >
            {/* Kontener modala */}
            <div
                className="bg-white rounded-2xl shadow-2xl max-w-4xl w-full max-h-[90vh] overflow-y-auto relative animate-fadeIn"
                onClick={(e) => e.stopPropagation()}
            >
                {/* Header */}
                <div className="flex justify-between items-center p-6 border-b border-gray-200 sticky top-0 bg-white z-10">
                    <h2 className="text-2xl font-semibold text-[#1A1A1A]">
                        Lokal {apartmentNumber} ({type === "apartment" ? "Mieszkanie" : "Dom"})
                    </h2>
                    <button
                        onClick={onClose}
                        className="text-gray-400 hover:text-gray-600 text-3xl font-light"
                        aria-label="Zamknij"
                    >
                        ×
                    </button>
                </div>

                {/* Galeria */}
                {hasImages && (
                    <div className="relative w-full h-80 overflow-hidden">
                        <img
                            key={index}
                            src={images[index]}
                            alt={`Zdjęcie ${apartmentNumber}`}
                            className="w-full h-full object-cover transition-all duration-500 cursor-zoom-in"
                            onClick={() => setLightboxOpen(true)}
                            draggable={false}
                        />

                        {/* Strzałki */}
                        {images.length > 1 && (
                            <>
                                <button
                                    type="button"
                                    onClick={prev}
                                    className="absolute top-1/2 left-4 -translate-y-1/2 bg-black/40 hover:bg-black/60 text-white rounded-full p-2 z-10"
                                    aria-label="Poprzednie zdjęcie"
                                >
                                    <ChevronLeft size={24} />
                                </button>
                                <button
                                    type="button"
                                    onClick={next}
                                    className="absolute top-1/2 right-4 -translate-y-1/2 bg-black/40 hover:bg-black/60 text-white rounded-full p-2 z-10"
                                    aria-label="Następne zdjęcie"
                                >
                                    <ChevronRight size={24} />
                                </button>
                            </>
                        )}

                        {/* Kropki */}
                        {images.length > 1 && (
                            <div className="absolute bottom-3 w-full flex justify-center gap-2 z-10">
                                {images.map((_, i) => (
                                    <button
                                        key={i}
                                        type="button"
                                        onClick={() => setIndex(i)}
                                        className={`w-2.5 h-2.5 rounded-full transition-all ${
                                            i === index ? "bg-white" : "bg-white/50 hover:bg-white/70"
                                        }`}
                                        aria-label={`Przejdź do zdjęcia ${i + 1}`}
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

                {/* Dokumenty */}
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
                    <h4 className="text-lg font-semibold mb-4 text-[#1A1A1A]">Historia cen</h4>

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

            {/* LIGHTBOX */}
            {lightboxOpen && hasImages && (
                <div
                    className="fixed inset-0 z-[100] bg-black/90 flex items-center justify-center p-4"
                    onClick={() => setLightboxOpen(false)}
                >
                    <div
                        className="relative w-full max-w-6xl max-h-[90vh] flex items-center justify-center"
                        onClick={(e) => e.stopPropagation()}
                    >
                        {/* Zamknij */}
                        <button
                            type="button"
                            onClick={() => setLightboxOpen(false)}
                            className="absolute top-4 right-4 text-white text-4xl font-light z-20 hover:opacity-80"
                            aria-label="Zamknij podgląd"
                        >
                            ×
                        </button>

                        {/* Obraz */}
                        <img
                            src={images[index]}
                            alt={`Zdjęcie ${apartmentNumber} (podgląd)`}
                            className="max-h-[90vh] max-w-full object-contain rounded-xl shadow-2xl cursor-zoom-out"
                            onClick={() => setLightboxOpen(false)}
                            draggable={false}
                        />

                        {/* Strzałki */}
                        {images.length > 1 && (
                            <>
                                <button
                                    type="button"
                                    onClick={(e) => {
                                        e.stopPropagation();
                                        prev();
                                    }}
                                    className="absolute left-4 top-1/2 -translate-y-1/2 bg-white/10 hover:bg-white/20 text-white rounded-full p-3 z-20"
                                    aria-label="Poprzednie zdjęcie"
                                >
                                    <ChevronLeft size={32} />
                                </button>

                                <button
                                    type="button"
                                    onClick={(e) => {
                                        e.stopPropagation();
                                        next();
                                    }}
                                    className="absolute right-4 top-1/2 -translate-y-1/2 bg-white/10 hover:bg-white/20 text-white rounded-full p-3 z-20"
                                    aria-label="Następne zdjęcie"
                                >
                                    <ChevronRight size={32} />
                                </button>
                            </>
                        )}

                        {/* Licznik */}
                        {images.length > 1 && (
                            <div className="absolute bottom-4 left-1/2 -translate-x-1/2 text-white/80 text-sm bg-black/40 px-3 py-1 rounded-full">
                                {index + 1} / {images.length}
                            </div>
                        )}
                    </div>
                </div>
            )}
        </div>
    );
}
