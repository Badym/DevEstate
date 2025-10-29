import React from "react";

export default function PropertyCard({ property }) {
  if (!property) return null;

  const { images, price, area, apartmentNumber, type, status } = property;

  return (
    <div className="block rounded-lg p-4 shadow-md shadow-gray-200 hover:shadow-lg transition-transform transform hover:scale-[1.02] bg-white">
      <img
        alt={apartmentNumber}
        src={images && images.length > 0 ? images[0] : "https://via.placeholder.com/400x300"}
        className="h-56 w-full rounded-md object-cover"
      />

      <div className="mt-3">
        <dl>
          <div>
            <dt className="sr-only">Cena</dt>
            <dd className="text-sm text-gray-500">
              {price?.toLocaleString("pl-PL")} zł
            </dd>
          </div>

          <div>
            <dt className="sr-only">Numer</dt>
            <dd className="font-medium text-gray-800">
              Lokal {apartmentNumber} ({type})
            </dd>
          </div>
        </dl>

        <div className="mt-4 flex items-center gap-6 text-xs">
          <div className="inline-flex items-center gap-2">
            <svg
              className="w-4 h-4 text-[#C8A27E]"
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
              stroke="currentColor"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth="2"
                d="M8 14v3m4-3v3m4-3v3M3 21h18M3 10h18M3 7l9-4 9 4M4 10h16v11H4V10z"
              />
            </svg>
            <p className="text-gray-500">Powierzchnia</p>
            <p className="font-medium">{area} m²</p>
          </div>

          <div className="inline-flex items-center gap-2">
            <svg
              className="w-4 h-4 text-[#C8A27E]"
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
              stroke="currentColor"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth="2"
                d="M20.354 15.354A9 9 0 018.646 3.646 9.003 9.003 0 0012 21a9.003 9.003 0 008.354-5.646z"
              />
            </svg>
            <p className="text-gray-500">Status</p>
            <p
              className={`font-medium ${
                status === "Sprzedane" ? "text-red-600" : "text-green-700"
              }`}
            >
              {status}
            </p>
          </div>
        </div>
      </div>
    </div>
  );
}
