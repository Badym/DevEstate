import { useNavigate } from "react-router-dom";
import { Button } from "@/components/ui/button";

export default function AdminTopBar() {
    const navigate = useNavigate();
    const user = JSON.parse(localStorage.getItem("user") || "{}");

    return (
        <header className="bg-[#1A1A1A] text-white py-4 px-8 flex justify-between items-center shadow-md">
            <h1 className="text-2xl font-semibold">Panel administratora</h1>

            <div className="flex items-center gap-4">

                <p className="text-sm text-gray-200">
                    Zalogowano jako: <strong>{user.fullName || "Admin"}</strong>
                </p>

                {/* 🔥 Dashboard */}
                <Button
                    className="
                        bg-[#C8A27E] 
                        text-white 
                        hover:bg-[#b18e6b] 
                        transition 
                        px-4
                    "
                    onClick={() => navigate("/admin/dashboard")}
                >
                    Dashboard
                </Button>

                {/* 🔥 Strona główna */}
                <Button
                    className="
                        bg-[#C8A27E] 
                        text-white 
                        hover:bg-[#b18e6b] 
                        transition 
                        px-4
                    "
                    onClick={() => navigate("/")}
                >
                    Strona główna
                </Button>

                {/* 🔥 Wyloguj */}
                <Button
                    className="
                        bg-[#C8A27E] 
                        text-white 
                        hover:bg-[#b18e6b] 
                        transition 
                        px-4
                    "
                    onClick={() => {
                        localStorage.removeItem("token");
                        localStorage.removeItem("user");
                        navigate("/login");
                    }}
                >
                    Wyloguj
                </Button>
            </div>
        </header>
    );
}
