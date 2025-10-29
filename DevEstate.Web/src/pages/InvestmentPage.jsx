import { useParams } from "react-router-dom";
import { useState } from "react";
import useFetch from "../hooks/useFetch";
import TopBar from "../components/TopBar";
import BuildingCarousel from "../components/BuildingCarousel"; // ⬅️ NOWE
import PropertyGrid from "../components/PropertyGrid";
import BuildingDetailsBox from "../components/BuildingDetailsBox"; // 🆕 dodaj

import {
    Carousel,
    CarouselContent,
    CarouselItem,
    CarouselNext,
    CarouselPrevious,
} from "@/components/ui/carousel";
import { Card, CardContent } from "@/components/ui/card";

export default function InvestmentPage() {
    const { slug } = useParams();
    const { data: investment, loading, error } = useFetch(`/api/Investment/name/${slug}`);
    const [selectedBuilding, setSelectedBuilding] = useState(null);


    // Lightbox do galerii
    const [selectedImage, setSelectedImage] = useState(null);
    const openImage = (img) => setSelectedImage(img);
    const closeImage = () => setSelectedImage(null);

    // Wybrany budynek (do filtrowania lokali)
    const [selectedBuildingId, setSelectedBuildingId] = useState(null);

    if (loading) return <p>Ładowanie...</p>;
    if (error) return <p>Błąd: {error}</p>;
    if (!investment) return <p>Nie znaleziono inwestycji</p>;

    const cover = investment.images?.[0] ?? "https://via.placeholder.com/1200x675";

    return (
        <div className="min-h-screen bg-[#FAF9F6] text-[#1A1A1A]">
            <TopBar />

            {/* HERO */}
            <div className="relative flex flex-col items-center mx-auto pt-32 lg:flex-row-reverse lg:max-w-5xl xl:max-w-6xl">
                <div className="w-full h-64 lg:w-1/2 lg:h-auto">
                    <img className="h-full w-full object-cover" src={cover} alt={investment.name} />
                </div>

                <div
                    className="max-w-lg bg-white md:max-w-2xl md:z-10 md:shadow-lg md:absolute md:top-0 md:mt-48 
                     lg:w-3/5 lg:left-0 lg:mt-20 lg:ml-20 xl:mt-24 xl:ml-12 rounded-xl"
                >
                    <div className="flex flex-col p-10 md:px-14">
                        <h2 className="text-3xl font-bold lg:text-4xl">{investment.name}</h2>

                        <div className="mt-6 text-gray-800 text-sm md:text-base">
                            <p><strong>Adres:</strong> {investment.street}, {investment.city}</p>
                            <p><strong>Kod pocztowy:</strong> {investment.postalCode}</p>
                            <p><strong>Status:</strong> {investment.status}</p>
                        </div>

                        <div className="mt-8">
                            <button
                                onClick={() => window.history.back()}
                                className="inline-block w-full text-center text-lg font-medium text-white bg-[#C8A27E] 
                           py-4 px-10 rounded-full hover:bg-[#b18e6b] transition md:w-48"
                            >
                                Wróć
                            </button>
                        </div>
                    </div>
                </div>
            </div>

            {/* OPIS */}
            <div className="max-w-5xl mx-auto bg-white shadow-lg rounded-xl p-10 mt-20 text-center">
                <h3 className="text-2xl font-semibold mb-4">Opis inwestycji</h3>
                <p className="text-gray-700 leading-relaxed">{investment.description}</p>
            </div>

            {/* GALERIA */}
            {investment.images?.length > 0 && (
                <div className="max-w-5xl mx-auto mt-16 px-4">
                    <h3 className="text-2xl font-semibold mb-6 text-center">Galeria inwestycji</h3>

                    <Carousel className="w-full max-w-4xl mx-auto">
                        <CarouselContent className="flex justify-center">
                            {investment.images.map((img, index) => (
                                <CarouselItem key={index} className="basis-full sm:basis-1/2 md:basis-1/3 flex justify-center">
                                    <div className="p-2">
                                        <Card
                                            className="overflow-hidden rounded-lg shadow-md cursor-pointer hover:shadow-xl transition-transform"
                                            onClick={() => openImage(img)}
                                        >
                                            <CardContent className="p-0">
                                                <img
                                                    src={img}
                                                    alt={`Zdjęcie ${index + 1}`}
                                                    className="w-full h-64 object-cover hover:scale-105 transition-transform duration-300"
                                                />
                                            </CardContent>
                                        </Card>
                                    </div>
                                </CarouselItem>
                            ))}
                        </CarouselContent>
                        <CarouselPrevious />
                        <CarouselNext />
                    </Carousel>
                </div>
            )}

            {/* LIGHTBOX */}
            {selectedImage && (
                <div
                    onClick={closeImage}
                    className="fixed inset-0 bg-black/80 flex justify-center items-center z-50 cursor-zoom-out"
                >
                    <img
                        src={selectedImage}
                        alt="Podgląd"
                        className="max-w-[90%] max-h-[90%] object-contain rounded-lg shadow-2xl"
                    />
                </div>
            )}

            {/* BUDYNKI (karuzela/selector) */}
            <BuildingCarousel
                investmentId={investment.id}
                selectedBuildingId={selectedBuildingId}
                onSelect={(id) => setSelectedBuildingId(prev => (prev === id ? null : id))}
            />

            {selectedBuildingId && (
                <BuildingDetailsBox
                    buildingId={selectedBuildingId}
                    investment={investment}
                />
            )}


            {/* LOKALE (filtrowane po wybranym budynku) */}
            <div className="max-w-6xl mx-auto mt-8 mb-24 px-4">
                <h3 className="text-2xl font-semibold mb-6 text-center">Nieruchomości w tej inwestycji</h3>
                <PropertyGrid
                    investmentId={investment.id}
                    buildingId={selectedBuildingId}   // ⬅️ filtr po budynku (null = wszystkie)
                />
            </div>
        </div>
    );
}
