import { useState } from "react";
import { useNavigate } from "react-router-dom";

export default function AdminLogin() {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState(null);
    const navigate = useNavigate();

    const handleLogin = async (e) => {
        e.preventDefault();

        try {
            const res = await fetch("/api/Auth/login", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ email, password }),
            });

            if (!res.ok) {
                const text = await res.text();
                throw new Error(text || "Nieprawidłowe dane logowania");
            }

            const data = await res.json();
            localStorage.setItem("token", data.token);
            localStorage.setItem("user", JSON.stringify({
                fullName: data.fullName,
                role: data.role,
                email: data.email,
                
            }));
            navigate("/admin/dashboard");

        } catch (err) {
            setError(err.message);
        }
    };

    return (
        <div className="flex items-center justify-center min-h-screen bg-[#FAF9F6]">
            <form
                onSubmit={handleLogin}
                className="bg-white shadow-2xl rounded-xl p-10 w-full max-w-md border border-gray-100"
            >
                <h2 className="text-3xl font-semibold mb-6 text-center text-[#1A1A1A]">
                    Panel administratora
                </h2>

                {error && (
                    <p className="text-red-600 text-center mb-4 text-sm">{error}</p>
                )}

                <div className="mb-4">
                    <label className="block text-gray-700 text-sm mb-1">E-mail</label>
                    <input
                        type="email"
                        placeholder="admin@devestate.pl"
                        className="w-full border border-gray-300 p-3 rounded-md focus:outline-none focus:ring-2 focus:ring-[#C8A27E]"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        required
                    />
                </div>

                <div className="mb-6">
                    <label className="block text-gray-700 text-sm mb-1">Hasło</label>
                    <input
                        type="password"
                        placeholder="••••••••"
                        className="w-full border border-gray-300 p-3 rounded-md focus:outline-none focus:ring-2 focus:ring-[#C8A27E]"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        required
                    />
                </div>

                <button
                    type="submit"
                    className="w-full bg-[#C8A27E] text-white font-semibold py-3 rounded-md hover:bg-[#b18e6b] transition"
                >
                    Zaloguj się
                </button>

                <p className="text-center text-gray-500 text-sm mt-4">
                    Dostęp wyłącznie dla administratorów
                </p>
            </form>
        </div>
    );
}
