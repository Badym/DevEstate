// src/App.jsx
import { Routes, Route } from "react-router-dom";
import Home from "./pages/Home";
import InvestmentPage from "./pages/InvestmentPage";
import AdminLogin from "./pages/admin/AdminLogin";
import AdminDashboard from "./pages/admin/AdminDashboard";
import InvestmentsAdmin from "@/pages/admin/InvestmentsAdmin.jsx";
import BuildingsAdmin from "@/pages/admin/BuildingsAdmin.jsx";
import PropertiesAdmin from "@/pages/admin/PropertiesAdmin.jsx";

export default function App() {
    return (
        <Routes>
            {/* Publiczne strony */}
            <Route path="/" element={<Home />} />
            <Route path="/:slug" element={<InvestmentPage />} />

            {/* Panel administratora */}
            <Route path="/admin/login" element={<AdminLogin />} />
            <Route path="/admin/dashboard" element={<AdminDashboard />} />
            <Route path="/admin/investments" element={<InvestmentsAdmin />} />
            <Route path="/admin/buildings" element={<BuildingsAdmin />} />
            <Route path="/admin/properties" element={<PropertiesAdmin />} />
        </Routes>
    );
}
