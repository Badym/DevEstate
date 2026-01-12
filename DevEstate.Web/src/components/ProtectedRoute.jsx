import { jwtDecode } from "jwt-decode";

export default function ProtectedRoute({ children, roles = [] }) {
    const token = localStorage.getItem("token");

    if (!token) {
        window.location.href = "/login";
        return null;
    }

    try {
        const decoded = jwtDecode(token);
        const now = Date.now() / 1000;

        if (decoded.exp < now) {
            localStorage.removeItem("token");
            window.location.href = "/login";
            return null;
        }

        // 🔥 Pobieramy rolę niezależnie od formatu
        const userRole =
            decoded.role ||
            decoded.Role ||
            decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];

        // Jeśli ProtectedRoute ma wymagane role → sprawdzamy czy pasuje
        if (roles.length > 0 && !roles.includes(userRole)) {
            window.location.href = "/admin/dashboard";
            return null;
        }

    } catch (err) {
        console.error("Niepoprawny token:", err);
        localStorage.removeItem("token");
        window.location.href = "/login";
        return null;
    }

    return children;
}
