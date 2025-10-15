import { useEffect, useState } from "react";

function App() {
    const [tests, setTests] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    useEffect(() => {
        const fetchTests = async () => {
            try {
                const res = await fetch("/api/test");
                if (!res.ok) throw new Error("Failed to fetch data");
                const data = await res.json();
                setTests(data);
            } catch (err) {
                setError(err.message);
            } finally {
                setLoading(false);
            }
        };

        fetchTests();
    }, []);

    if (loading) return <div className="min-h-screen flex items-center justify-center text-gray-600 text-xl">Loading...</div>;
    if (error) return <div className="min-h-screen flex items-center justify-center text-red-500 text-xl">{error}</div>;

    return (
        <div className="min-h-screen bg-gray-100 flex flex-col items-center justify-center">
            <h1 className="text-4xl font-bold mb-6 text-blue-700">DevEstate – Test Data</h1>

            {tests.length === 0 ? (
                <p className="text-gray-500">No test entries found.</p>
            ) : (
                <div className="bg-white shadow-lg rounded-lg p-6 w-full max-w-lg">
                    <ul className="space-y-4">
                        {tests.map((t) => (
                            <li key={t.id} className="border-b border-gray-200 pb-2">
                                <p className="text-lg font-semibold text-gray-800">{t.name}</p>
                                <p className="text-gray-600">{t.description}</p>
                            </li>
                        ))}
                    </ul>
                </div>
            )}
        </div>
    );
}

export default App;
