import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Button } from "@/components/ui/button";
import AdminLogsTable from "@/components/admin/AdminLogs/AdminLogsTable";
import AdminLogsFilters from "@/components/admin/AdminLogs/AdminLogsFilters";

export default function AdminLogsPage() {
    const navigate = useNavigate();
    const [logs, setLogs] = useState([]);
    const [loading, setLoading] = useState(true);

    const token = localStorage.getItem("token");

    const fetchLogs = async (filters = {}) => {
        setLoading(true);

        // ✨ Poprawione budowanie parametrów — TYLKO realne wartości
        const params = new URLSearchParams();

        if (filters.userName && filters.userName.trim() !== "") {
            params.append("userName", filters.userName.trim());
        }

        if (filters.action && filters.action.trim() !== "") {
            params.append("action", filters.action.trim());
        }

        if (filters.entity && filters.entity.trim() !== "") {
            params.append("entity", filters.entity.trim());
        }

        const query = params.toString();

        const url = query
            ? `http://localhost:5086/api/AdminLog/filter?${query}`
            : `http://localhost:5086/api/AdminLog`;

        const res = await fetch(url, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });

        const data = await res.json();
        setLogs(data);
        setLoading(false);
    };

    // Auth check + initial load
    useEffect(() => {
        const user = JSON.parse(localStorage.getItem("user") || "{}");

        if (!token || (user.role !== "Admin" && user.role !== "Moderator")) {
            navigate("/admin/login");
            return;
        }

        fetchLogs();
    }, []);

    return (
        <div className="min-h-screen bg-[#FAF9F6] flex flex-col">
            <header className="bg-[#1A1A1A] text-white py-4 px-8 flex justify-between items-center shadow-md">
                <h1 className="text-2xl font-semibold">Logi administratora</h1>
                <Button
                    variant="outline"
                    onClick={() => navigate("/admin/dashboard")}
                    className="border-gray-400 text-gray-200 hover:bg-gray-200 hover:text-[#1A1A1A]"
                >
                    Powrót
                </Button>
            </header>

            <main className="flex">
                {/* lewy sidebar */}
                <aside className="w-64 bg-white shadow-lg border-r border-gray-200 p-6 space-y-4">
                    <h2 className="text-lg font-semibold mb-4">Nawigacja</h2>

                    <Button variant="ghost" className="justify-start" onClick={() => navigate("/admin/dashboard")}>
                        📊 Pulpit
                    </Button>
                    <Button variant="ghost" className="justify-start" onClick={() => navigate("/admin/investments")}>
                        🏢 Inwestycje
                    </Button>
                    <Button variant="ghost" className="justify-start" onClick={() => navigate("/admin/buildings")}>
                        🏗️ Budynki
                    </Button>
                    <Button variant="ghost" className="justify-start" onClick={() => navigate("/admin/properties")}>
                        🏠 Nieruchomości
                    </Button>
                    <Button variant="ghost" className="justify-start" onClick={() => navigate("/admin/features")}>
                        ⚙️ Udogodnienia
                    </Button>
                    <Button variant="ghost" className="justify-start" onClick={() => navigate("/admin/feature-types")}>
                        🧩 Typy cech
                    </Button>
                    <Button variant="ghost" className="justify-start" onClick={() => navigate("/admin/users")}>
                        👤 Użytkownicy
                    </Button>
                </aside>

                {/* content */}
                <section className="flex-1 p-10">
                    <AdminLogsFilters onFilter={fetchLogs} />

                    {loading ? (
                        <p className="text-gray-500 mt-10">Ładowanie logów...</p>
                    ) : (
                        <AdminLogsTable logs={logs} />
                    )}
                </section>
            </main>
        </div>
    );
}
