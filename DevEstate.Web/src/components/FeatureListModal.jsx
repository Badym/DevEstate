import { useEffect, useState } from "react";
import { motion, AnimatePresence } from "framer-motion";
import { FaTimes } from "react-icons/fa";

export default function FeatureListModal({ open, onClose, featureType, buildingId }) {
    const [features, setFeatures] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        if (!open || !featureType || !buildingId) return;

        const fetchFeatures = async () => {
            try {
                const res = await fetch(
                    `/api/Feature/byBuilding/${buildingId}/type/${featureType.featureTypeId}`
                );
                const data = await res.json();
                setFeatures(data);
            } catch (err) {
                console.error("Błąd przy pobieraniu feature’ów:", err);
            } finally {
                setLoading(false);
            }
        };

        fetchFeatures();
    }, [open, featureType, buildingId]);

    if (!open) return null;

    return (
        <AnimatePresence>
            <motion.div
                initial={{ opacity: 0 }}
                animate={{ opacity: 1 }}
                exit={{ opacity: 0 }}
                className="fixed inset-0 bg-black bg-opacity-40 backdrop-blur-sm flex justify-center items-center z-50"
                onClick={onClose}
            >
                <motion.div
                    onClick={(e) => e.stopPropagation()}
                    initial={{ y: 60, opacity: 0 }}
                    animate={{ y: 0, opacity: 1 }}
                    exit={{ y: 60, opacity: 0 }}
                    transition={{ type: "spring", stiffness: 120, damping: 18 }}
                    className="relative bg-white rounded-2xl shadow-2xl w-[90%] md:w-[700px] max-h-[80vh] overflow-y-auto p-8"
                >
                    {/* Przycisk zamknięcia */}
                    <button
                        onClick={onClose}
                        className="absolute top-4 right-4 text-gray-400 hover:text-gray-700 transition"
                    >
                        <FaTimes size={20} />
                    </button>

                    {/* Tytuł */}
                    <h2 className="text-3xl font-bold text-[#1A1A1A] text-center mb-6">
                        {featureType?.featureTypeName}
                    </h2>

                    {/* Treść */}
                    {loading ? (
                        <p className="text-center text-gray-500 py-8">Ładowanie danych...</p>
                    ) : features.length === 0 ? (
                        <p className="text-center text-gray-600 py-8">
                            Brak dostępnych elementów tego typu.
                        </p>
                    ) : (
                        <div className="space-y-4">
                            {features.map((f) => (
                                <motion.div
                                    key={f.id}
                                    initial={{ opacity: 0, y: 10 }}
                                    animate={{ opacity: 1, y: 0 }}
                                    transition={{ duration: 0.2 }}
                                    className="flex justify-between items-start bg-[#FAF9F6] hover:bg-[#f4ede3] transition rounded-xl p-5 shadow-sm"
                                >
                                    <div className="flex-1">
                                        <p className="text-lg font-semibold text-[#1A1A1A]">
                                            {f.description || "Brak opisu"}
                                        </p>
                                        {f.isAvailable === false && (
                                            <p className="text-sm text-red-500 mt-1">
                                                Niedostępne
                                            </p>
                                        )}
                                    </div>
                                    <div className="text-right">
                                        {f.price ? (
                                            <p className="text-[#C8A27E] font-semibold text-lg">
                                                {f.price.toLocaleString("pl-PL")} zł
                                            </p>
                                        ) : (
                                            <p className="text-gray-500 text-sm">Bezpłatne</p>
                                        )}
                                    </div>
                                </motion.div>
                            ))}
                        </div>
                    )}

                    {/* Stopka */}
                    <div className="flex justify-center mt-8">
                        <button
                            onClick={onClose}
                            className="px-6 py-2 bg-[#C8A27E] hover:bg-[#b18e6b] text-white rounded-lg shadow-md font-medium transition"
                        >
                            Zamknij
                        </button>
                    </div>
                </motion.div>
            </motion.div>
        </AnimatePresence>
    );
}
