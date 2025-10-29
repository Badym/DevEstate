import { useEffect, useState } from "react";

export default function HeroSection() {
    const [company, setCompany] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchCompany = async () => {
            try {
                const res = await fetch("/api/company");
                if (!res.ok) throw new Error("Failed to fetch company data");
                const data = await res.json();

                // 🧩 jeśli API zwraca tablicę, weź pierwszy element
                const companyData = Array.isArray(data) ? data[0] : data;

                setCompany(companyData);
            } catch (err) {
                console.error("Error fetching company data:", err);
            } finally {
                setLoading(false);
            }
        };
        fetchCompany();
    }, []);

    return (
        <div className="flex flex-col items-center justify-center min-h-[75vh] text-center px-6">

        {loading ? (
                <p className="text-indigo-200 text-lg animate-pulse">
                    Ładowanie danych firmy...
                </p>
            ) : (
                <>
                    <p className="uppercase tracking-[0.25em] text-indigo-200 text-sm mb-4">
                        Zmieniamy oblicze budownictwa
                    </p>

                    <h1 className="text-7xl font-extrabold mb-6 drop-shadow-lg">
                        {company?.name || "DevEstate Group"}
                    </h1>

                    <p className="text-2xl font-light max-w-3xl mb-10 text-indigo-100 leading-relaxed">
                        {company?.description ||
                            "Nowoczesne inwestycje mieszkaniowe i komercyjne – komfort, design i innowacja w każdym detalu."}
                    </p>

                    <div className="flex flex-col sm:flex-row gap-6">
                        <a
                            href="#investments"
                            className="bg-white text-indigo-700 font-semibold px-10 py-4 rounded-full shadow-xl hover:bg-gray-100 transition text-lg"
                        >
                            Zobacz inwestycje
                        </a>
                        <a
                            href="#contact"
                            className="border-2 border-white text-white font-semibold px-10 py-4 rounded-full hover:bg-white hover:text-indigo-700 transition text-lg"
                        >
                            Skontaktuj się
                        </a>
                    </div>
                </>
            )}
        </div>
    );
}
