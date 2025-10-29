export default function InvestmentCard({ investment, image }) {
    return (
        <div className="flex flex-col items-center bg-white border border-gray-200 rounded-lg shadow-lg md:flex-row md:max-w-xl hover:bg-gray-100 cursor-pointer">
            <img
                className="object-cover w-full rounded-t-lg h-96 md:h-auto md:w-48 md:rounded-none md:rounded-l-lg"
                src={image}
                alt={investment.name}
            />
            <div className="flex flex-col justify-between p-6 leading-normal">
                <h5 className="mb-3 text-3xl font-bold tracking-tight text-gray-900">{investment.name}</h5>
                <p className="mb-3 font-normal text-gray-700">{investment.city}</p>
                <p className="mb-3 font-normal text-gray-600">{investment.address}</p> {/* Adres inwestycji */}
            </div>
        </div>
    );
}
