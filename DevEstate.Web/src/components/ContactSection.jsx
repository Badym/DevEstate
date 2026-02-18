import React from "react";

const ContactSection = ({ companyData }) => {
    const logoUrl = companyData?.logoImage;

    return (
        <div className="bg-white py-20">
            <div className="max-w-7xl mx-auto px-6">
                <div className="flex flex-col md:flex-row justify-between items-start space-y-10 md:space-y-0">
                    {/* Lewa kolumna - nazwa firmy i logo */}
                    <div className="flex-1 text-center md:text-left">
                        <h1 className="text-4xl font-semibold text-[#1A1A1A]">{companyData?.name}</h1>

                        {/* Logo firmy - responsywne */}
                        {logoUrl ? (
                            <img
                                src={logoUrl}
                                alt="Logo"
                                className="mt-6 mx-auto md:mx-0 max-w-[200px] sm:max-w-[300px] md:max-w-[400px] lg:max-w-[500px] h-auto object-contain"
                            />
                        ) : (
                            <p>Brak logo</p> // Jeśli logo jest undefined
                        )}
                    </div>

                    {/* Prawa kolumna - dane kontaktowe i firma */}
                    <div className="flex-1 space-y-10">
                        {/* Dane kontaktowe */}
                        <div className="bg-[#F2F2F2] p-6 rounded-lg shadow-md">
                            <h3 className="text-2xl font-semibold text-[#1A1A1A] mb-4">DANE KONTAKTOWE</h3>
                            <p><strong>Email:</strong> <a href={`mailto:${companyData?.email}`} className="text-blue-600">{companyData?.email}</a></p>
                            <p><strong>Telefon:</strong> {companyData?.phone}</p>
                            <p><strong>REGON:</strong> {companyData?.regon}</p>
                            <p><strong>NIP:</strong> {companyData?.nip}</p>
                        </div>

                        {/* Siedziba firmy */}
                        <div className="bg-[#F2F2F2] p-6 rounded-lg shadow-md">
                            <h3 className="text-2xl font-semibold text-[#1A1A1A] mb-4">SIEDZIBA FIRMY</h3>
                            <p><strong>Firma:</strong> {companyData?.name}</p>
                            <p><strong>Adres:</strong> {companyData?.city + ", " + companyData?.street + " " + companyData?.buildingNumber + " " + companyData?.postalCode}</p>
                            <p><strong>KRS:</strong> {companyData?.krs}</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default ContactSection;
