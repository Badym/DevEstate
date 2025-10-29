import { useRef } from "react";
import useFetch from "../hooks/useFetch";
import BuildingCard from "./BuildingCard";

export default function BuildingCarousel({ investmentId, selectedBuildingId, onSelect }) {
    const { data: buildings, loading, error } =
        useFetch(`/api/Investment/${investmentId}/buildings`);
    const scrollerRef = useRef(null);

    if (loading) return null;
    if (error || !buildings || buildings.length === 0) return null;

    const scrollBy = (dx) => scrollerRef.current?.scrollBy({ left: dx, behavior: "smooth" });

    return (
        <section className="max-w-7xl mx-auto px-6 mt-16">
            {/* Nagłówek */}
            <div className="flex items-center justify-between mb-6">
                <h3 className="text-2xl font-semibold text-[#1A1A1A]">
                    Budynki w inwestycji
                </h3>

                <div className="hidden md:flex gap-3">
                    <button
                        onClick={() => scrollBy(-400)}
                        className="px-4 py-2 rounded-full bg-white shadow hover:shadow-md transition"
                    >
                        ‹
                    </button>
                    <button
                        onClick={() => scrollBy(400)}
                        className="px-4 py-2 rounded-full bg-white shadow hover:shadow-md transition"
                    >
                        ›
                    </button>
                </div>
            </div>

            {/* Karuzela budynków */}
            <div className="flex justify-center">
                <div
                    ref={scrollerRef}
                    className="flex gap-6 overflow-x-auto pb-4 snap-x snap-mandatory scrollbar-thin scrollbar-thumb-gray-300 justify-center"
                    style={{ scrollSnapType: "x mandatory", scrollPadding: "1rem" }}
                >
                    {buildings.map((b) => (
                        <div key={b.id} className="flex-shrink-0 snap-center">
                            <BuildingCard
                                building={b}
                                selected={selectedBuildingId === b.id}
                                onSelect={() => onSelect(b.id)} // 🟢 tylko id
                            />
                        </div>
                    ))}
                </div>
            </div>
        </section>
    );
}
