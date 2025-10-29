// src/App.jsx
import { Routes, Route } from "react-router-dom";
import Home from "./pages/Home";
import InvestmentPage from "./pages/InvestmentPage";
import AdminLogin from "./pages/admin/AdminLogin";
import AdminDashboard from "./pages/admin/AdminDashboard";
import InvestmentsAdmin from "@/pages/admin/InvestmentsAdmin.jsx";

export default function App() {
    return (
        <Routes>
            {/* Publiczne strony */}
            <Route path="/" element={<Home />} />
            <Route path="/investment/:slug" element={<InvestmentPage />} />

            {/* Panel administratora */}
            <Route path="/admin/login" element={<AdminLogin />} />
            <Route path="/admin/dashboard" element={<AdminDashboard />} />
            <Route path="/admin/investments" element={<InvestmentsAdmin />} />
        </Routes>
    );
}
