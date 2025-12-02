import { useEffect, useState } from "react";
import { Button } from "@/components/ui/button";
import { useNavigate } from "react-router-dom";
import { jwtDecode } from "jwt-decode";
import AddUserModal from "@/components/admin/AddUserModal";
import EditUserModal from "@/components/admin/EditUserModal";

export default function AdminUsers() {
    const [userData, setUserData] = useState([]);
    const [userRole, setUserRole] = useState("");
    const [userId, setUserId] = useState("");

    const [openAddModal, setOpenAddModal] = useState(false);
    const [openEditModal, setOpenEditModal] = useState(false);
    const [selectedUser, setSelectedUser] = useState(null);

    const navigate = useNavigate();

    // 🔹 Wczytanie roli i ID z JWT
    useEffect(() => {
        const token = localStorage.getItem("token");
        const storedUser = JSON.parse(localStorage.getItem("user") || "{}");

        if (!token || (storedUser.role !== "Admin" && storedUser.role !== "Moderator")) {
            navigate("/admin/login");
            return;
        }

        setUserRole(storedUser.role);

        try {
            const decoded = jwtDecode(token);
            setUserId(decoded.sub);
        } catch {
            localStorage.removeItem("token");
            navigate("/admin/login");
        }
    }, [navigate]);

    // 🔹 Pobranie danych
    useEffect(() => {
        if (!userRole || !userId) return;

        const fetchData = async () => {
            let url;

            if (userRole === "Admin") {
                url = "/api/user/all";
            } else if (userRole === "Moderator") {
                url = `/api/User/${userId}`;
            }

            if (!url) return;

            const res = await fetch(url);
            const data = await res.json();

            setUserData(userRole === "Admin" ? data : [data]);
        };

        fetchData();
    }, [userRole, userId]);

    // 🔥 Usuwanie
    const handleDelete = async (user) => {
        if (!window.confirm(`Czy na pewno chcesz usunąć użytkownika: ${user.fullName}?`)) {
            return;
        }

        try {
            const token = localStorage.getItem("token");

            const res = await fetch(`/api/User/${user.id}`, {
                method: "DELETE",
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });

            if (!res.ok) throw new Error("Nie udało się usunąć użytkownika.");

            alert("🗑️ Użytkownik usunięty.");

            // 🌟 odświeżanie tylko dla Admina
            if (userRole === "Admin") {
                const refresh = await fetch("/api/user/all");
                setUserData(await refresh.json());
            } else {
                // Moderator nie może usuwać, ale dla bezpieczeństwa:
                setUserData([]);
            }
        } catch (err) {
            alert(`❌ Błąd: ${err.message}`);
        }
    };

    return (
        <div className="min-h-screen bg-[#FAF9F6] flex flex-col">
            <header className="bg-[#1A1A1A] text-white py-4 px-8 flex justify-between items-center shadow-md">
                <h1 className="text-2xl font-semibold">Zarządzanie użytkownikami</h1>
            </header>

            <main className="flex-1 p-10">
                <h2 className="text-3xl font-semibold mb-6">Lista użytkowników</h2>

                {userRole === "Admin" && (
                    <Button
                        className="mb-6 bg-[#C8A27E] text-white hover:bg-[#b18e6b]"
                        onClick={() => setOpenAddModal(true)}
                    >
                        ➕ Dodaj użytkownika
                    </Button>
                )}

                <div className="grid grid-cols-1 md:grid-cols-5 gap-6">
                    {userData.map((user) => (
                        <div key={user.id} className="bg-white p-6 rounded-lg shadow hover:shadow-md transition">
                            <h3 className="font-semibold text-lg mb-2">{user.fullName}</h3>
                            <p className="text-gray-500">{user.email}</p>

                            <div className="flex gap-2 mt-4">
                                <Button
                                    className="bg-[#C8A27E] text-white hover:bg-[#b18e6b]"
                                    onClick={() => {
                                        setSelectedUser(user);
                                        setOpenEditModal(true);
                                    }}
                                >
                                    Edytuj
                                </Button>

                                {userRole === "Admin" && user.role !== "Admin" && (
                                    <Button
                                        className="bg-red-600 text-white hover:bg-red-700"
                                        onClick={() => handleDelete(user)}
                                    >
                                        Usuń
                                    </Button>
                                )}

                            </div>
                        </div>
                    ))}
                </div>

                {/* Modale */}
                <AddUserModal
                    open={openAddModal}
                    onClose={() => setOpenAddModal(false)}
                    onSave={async () => {
                        // tylko admin powinien odświeżać ALL
                        if (userRole === "Admin") {
                            const res = await fetch("/api/user/all");
                            setUserData(await res.json());
                        }
                    }}
                />

                <EditUserModal
                    open={openEditModal}
                    onClose={() => setOpenEditModal(false)}
                    user={selectedUser}
                    onSave={async () => {
                        if (userRole === "Admin") {
                            const res = await fetch("/api/user/all");
                            setUserData(await res.json());
                        } else {
                            const res = await fetch(`/api/User/${userId}`);
                            setUserData([await res.json()]);
                        }
                    }}
                />
            </main>
        </div>
    );
}
