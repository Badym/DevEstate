export default function BuildingCard({ building, selected, onSelect }) {
    const img =
        (building?.images && building.images[0]) ||
        "https://via.placeholder.com/800x500?text=Brak+zdjęcia";

    const available = building?.availablePropertiesCount || 0;
    const sold = building?.soldPropertiesCount || 0;

    return (
        <button
            onClick={() => onSelect?.(building)} // 👈 przekazujemy cały obiekt
            className={`group block mx-auto bg-white rounded-2xl overflow-hidden shadow-lg hover:shadow-2xl transition-all duration-300 transform hover:-translate-y-1
                ring-2 ${selected ? "ring-[#C8A27E]" : "ring-transparent"}`}
            style={{ width: "320px" }}
        >
            {/* Zdjęcie */}
            <div className="relative w-full" style={{ paddingTop: "62.5%" }}> {/* 16:10 proporcje */}
                <img
                    src={img}
                    alt={`Budynek ${building?.buildingNumber}`}
                    className="absolute inset-0 w-full h-full object-cover group-hover:scale-105 transition-transform duration-300"
                />
            </div>

            {/* Treść */}
            <div className="p-6 text-center">
                <h4 className="text-xl font-semibold text-[#1A1A1A] mb-3">
                    Budynek {building?.buildingNumber}
                </h4>

                {/* Statystyki mieszkań */}
                <div className="flex justify-center gap-10">
                    <div className="flex flex-col items-center text-green-700">
                        <span className="text-sm font-medium">Aktualne</span>
                        <span className="text-2xl font-bold">{available}</span>
                    </div>

                    <div className="h-10 w-[1px] bg-gray-300"></div>

                    <div className="flex flex-col items-center text-red-600">
                        <span className="text-sm font-medium">Sprzedane</span>
                        <span className="text-2xl font-bold">{sold}</span>
                    </div>
                </div>
            </div>
        </button>
    );
}
