import React, { useState } from "react";
import { useNavigate } from "react-router-dom"; // <-- import

const InvestmentCarousel = ({ investments }) => {
    const [currentIndex, setCurrentIndex] = useState(0);
    const navigate = useNavigate(); // <-- hook nawigacji

    const nextSlide = () => {
        setCurrentIndex((prevIndex) => (prevIndex + 1) % investments.length);
    };

    const prevSlide = () => {
        setCurrentIndex((prevIndex) =>
            prevIndex === 0 ? investments.length - 1 : prevIndex - 1
        );
    };

    const getImageUrl = (imagePath) => {
        if (imagePath && imagePath.startsWith("http")) {
            return imagePath;
        }
        return `${imagePath}`;
    };

    // ✅ Funkcja do przekierowania po kliknięciu
    const handleOpenInvestment = (investmentName) => {
        const slug = investmentName.toLowerCase().replace(/\s+/g, "_");
        navigate(`/${slug}`);
    };

    return (
        <div className="relative w-full max-w-4xl mx-auto mt-6">
            {investments.length > 0 ? (
                <div className="relative group">
                    {/* Kontener z proporcją 16:9 */}
                    <div
                        className="relative w-full cursor-pointer"
                        style={{ paddingTop: "56.25%" }}
                        onClick={() =>
                            handleOpenInvestment(investments[currentIndex].name)
                        }
                    >
                        <img
                            src={getImageUrl(investments[currentIndex].images[0])}
                            alt={investments[currentIndex].name}
                            className="absolute top-0 left-0 w-full h-full object-cover rounded-lg shadow-lg transition-transform transform group-hover:scale-[1.03]"
                        />
                        <div className="absolute bottom-4 left-4 text-white bg-opacity-50 bg-black p-4 rounded-lg">
                            <h3 className="text-xl font-semibold">
                                {investments[currentIndex].name}
                            </h3>
                            <p>
                                {investments[currentIndex].street},{" "}
                                {investments[currentIndex].city}
                            </p>
                        </div>
                    </div>

                    {/* Strzałki */}
                    <button
                        onClick={prevSlide}
                        className="absolute top-1/2 left-[-50px] transform -translate-y-1/2 p-4 bg-black bg-opacity-50 text-white rounded-full opacity-50 hover:opacity-100 transition-opacity"
                    >
                        &#60;
                    </button>
                    <button
                        onClick={nextSlide}
                        className="absolute top-1/2 right-[-50px] transform -translate-y-1/2 p-4 bg-black bg-opacity-50 text-white rounded-full opacity-50 hover:opacity-100 transition-opacity"
                    >
                        &#62;
                    </button>

                    {/* Kropki */}
                    <div className="absolute bottom-4 left-1/2 transform -translate-x-1/2 flex space-x-2">
                        {investments.map((_, index) => (
                            <div
                                key={index}
                                onClick={() => setCurrentIndex(index)}
                                className={`w-3 h-3 rounded-full cursor-pointer ${
                                    index === currentIndex
                                        ? "bg-white"
                                        : "bg-gray-400"
                                }`}
                            />
                        ))}
                    </div>
                </div>
            ) : (
                <p>Brak inwestycji</p>
            )}
        </div>
    );
};

export default InvestmentCarousel;
