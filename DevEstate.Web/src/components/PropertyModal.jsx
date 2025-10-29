import { useEffect } from "react";

export default function PropertyModal({ property, onClose }) {
    // zamykanie ESC
    useEffect(() => {
        const handleEsc = (e) => e.key === "Escape" && onClose();
        window.addEventListener("keydown", handleEsc);
        return () => window.removeEventListener("keydown", handleEsc);
    }, [onClose]);

    if (!property) return null;

    const {
        images,
        apartmentNumber,
        type,
        area,
        terraceArea,
        price,
        pricePerMeter,
        status,
    } = property;

    return (
        <div
            className="fixed inset-0 bg-black/70 z-50 flex justify-center items-center p-4"
            onClick={onClose}
        >
            {/* Kontener modala */}
            <div
                className="bg-white rounded-2xl shadow-2xl max-w-4xl w-full max-h-[90vh] overflow-y-auto relative animate-fadeIn"
                onClick={(e) => e.stopPropagation()} // blokuje zamykanie przy kliknięciu wnętrza
            >
                {/* Header */}
                <div className="flex justify-between items-center p-6 border-b border-gray-200 sticky top-0 bg-white z-10">
                    <h2 className="text-2xl font-semibold text-[#1A1A1A]">
                        Lokal {apartmentNumber} ({type})
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
                    <div className="w-full h-80 overflow-hidden">
                        <img
                            src={images[0]}
                            alt={`Zdjęcie ${apartmentNumber}`}
                            className="w-full h-full object-cover"
                        />
                    </div>
                )}

                {/* Treść główna */}
                <div className="p-8 grid grid-cols-1 sm:grid-cols-2 gap-6 text-gray-700">
                    <div>
                        <p><strong>Typ:</strong> {type === "apartment" ? "Mieszkanie" : "Dom"}</p>
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
                        <p><strong>Powierzchnia:</strong> {area} m²</p>
                        {terraceArea && <p><strong>Taras:</strong> {terraceArea} m²</p>}
                    </div>
                    <div>
                        <p><strong>Cena:</strong> {price.toLocaleString("pl-PL")} zł</p>
                        <p><strong>Cena za m²:</strong> {pricePerMeter} zł/m²</p>
                        <p><strong>ID lokalu:</strong> {property.id}</p>
                    </div>
                </div>

                {/* 🧩 Dodatkowe sekcje */}
                <div className="border-t border-gray-200 p-8 text-gray-600">
                    <h4 className="text-lg font-semibold mb-4 text-[#1A1A1A]">
                        Dodatkowe informacje
                    </h4>
                    <p>
                        Tutaj można dodać np. opis techniczny, dokumenty PDF, rzuty mieszkania,
                        albo przyciski do pobrania materiałów.
                    </p>
                    {/* 👉 miejsce na załączniki, przyciski, linki */}
                </div>
            </div>
        </div>
    );
}
