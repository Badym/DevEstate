export default function InvestmentOverviewSection({ investment }) {
    const features = [
        {
            name: "Lokalizacja",
            value: investment.city,
        },
        {
            name: "Rok rozpoczęcia",
            value: investment.startYear || "brak danych",
        },
        {
            name: "Liczba budynków",
            value: investment.buildingsCount || "–",
        },
        {
            name: "Deweloper",
            value: investment.companyName || "nieznany",
        },
    ];

    return (
        <div className="bg-gray-900 py-24 sm:py-32 text-white">
            <div className="mx-auto max-w-7xl px-6 lg:px-8">
                {/* Nagłówek */}
                <div className="mx-auto max-w-2xl lg:text-center">
                    <h2 className="text-indigo-400 font-semibold text-lg">Inwestycja</h2>
                    <p className="mt-2 text-4xl font-semibold tracking-tight">{investment.name}</p>
                    <p className="mt-6 text-gray-300 text-lg">{investment.description}</p>
                </div>

                {/* Feature grid */}
                <div className="mx-auto mt-16 max-w-2xl sm:mt-20 lg:mt-24 lg:max-w-4xl">
                    <dl className="grid max-w-xl grid-cols-1 gap-x-8 gap-y-10 lg:max-w-none lg:grid-cols-2 lg:gap-y-16">
                        {features.map((feature) => (
                            <div key={feature.name} className="relative pl-16">
                                <dt className="font-semibold text-white">
                                    {feature.name}
                                </dt>
                                <dd className="mt-2 text-gray-400">{feature.value}</dd>
                            </div>
                        ))}
                    </dl>
                </div>
            </div>
        </div>
    );
}
