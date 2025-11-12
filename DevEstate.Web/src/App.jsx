import { Routes, Route } from "react-router-dom";
import Home from "./pages/Home";
import InvestmentPage from "./pages/InvestmentPage";
import AdminLogin from "./pages/admin/AdminLogin";
import AdminDashboard from "./pages/admin/AdminDashboard";
import InvestmentsAdmin from "@/pages/admin/InvestmentsAdmin.jsx";
import BuildingsAdmin from "@/pages/admin/BuildingsAdmin.jsx";
import PropertiesAdmin from "@/pages/admin/PropertiesAdmin.jsx";
import ProtectedRoute from "@/components/ProtectedRoute";
import FeaturesAdmin from "@/pages/admin/FeaturesAdmin.jsx";
import FeatureTypesAdmin from "@/pages/admin/FeatureTypesAdmin.jsx";


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
            <Route
                path="/admin/features"
                element={
                    <ProtectedRoute>
                        <FeaturesAdmin />
                    </ProtectedRoute>
                }
            />
            <Route
                path="/admin/feature-types"
                element={
                    <ProtectedRoute>
                        <FeatureTypesAdmin />
                    </ProtectedRoute>
                }
            />

        </Routes>
    );
}
