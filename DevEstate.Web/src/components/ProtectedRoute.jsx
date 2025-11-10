import { jwtDecode } from "jwt-decode";

export default function ProtectedRoute({ children }) {
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
    } catch (err) {
        console.error("Niepoprawny token:", err);
        localStorage.removeItem("token");
        window.location.href = "/login";
        return null;
    }

    return children;
}
