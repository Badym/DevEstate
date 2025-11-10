import { Routes, Route } from "react-router-dom";
import Home from "./pages/Home";
import InvestmentPage from "./pages/InvestmentPage";
import AdminLogin from "./pages/admin/AdminLogin";
import AdminDashboard from "./pages/admin/AdminDashboard";
import InvestmentsAdmin from "@/pages/admin/InvestmentsAdmin.jsx";
import BuildingsAdmin from "@/pages/admin/BuildingsAdmin.jsx";
import PropertiesAdmin from "@/pages/admin/PropertiesAdmin.jsx";
import ProtectedRoute from "@/components/ProtectedRoute";

export default function App() {
    return (
        <Routes>
            {/* 🌍 Publiczne strony */}
            <Route path="/" element={<Home />} />
            <Route path="/:slug" element={<InvestmentPage />} />
            <Route path="/login" element={<AdminLogin />} />

            {/* 🔒 Panel administratora */}
            <Route
                path="/admin/dashboard"
                element={
                    <ProtectedRoute>
                        <AdminDashboard />
                    </ProtectedRoute>
                }
            />
            <Route
                path="/admin/investments"
                element={
                    <ProtectedRoute>
                        <InvestmentsAdmin />
                    </ProtectedRoute>
                }
            />
            <Route
                path="/admin/buildings"
                element={
                    <ProtectedRoute>
                        <BuildingsAdmin />
                    </ProtectedRoute>
                }
            />
            <Route
                path="/admin/properties"
                element={
                    <ProtectedRoute>
                        <PropertiesAdmin />
                    </ProtectedRoute>
                }
            />
        </Routes>
    );
}
