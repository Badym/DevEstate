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
import UserAdmin from "@/pages/admin/UsersAdmin.jsx";
import LogsAdmin from "@/pages/admin/AdminLogsPage.jsx";
import CompanyAdmin from "@/pages/admin/AdminCompanyInfoPage.jsx";
import AdminReportsPage  from "@/pages/admin/AdminReportsPage.jsx";
import AdminComparisonPage   from "@/pages/admin/AdminComparisonPage.jsx";
import { NotificationProvider } from "@/components/NotificationProvider";
import NotificationContainer from "@/components/NotificationContainer";



export default function App() {
    return (
        <NotificationProvider>
            <NotificationContainer />
                <Routes>
                    {/* 🌍 Publiczne strony */}
                    <Route path="/" element={<Home />} />
                    <Route path="/:slug" element={<InvestmentPage />} />
                    <Route path="/login" element={<AdminLogin />} />
        
                    {/* 🔒 Panel administratora */}
                    <Route
                        path="/admin/dashboard"
                        element={
                            <ProtectedRoute roles={["Admin", "Moderator"]}>
                                <AdminDashboard />
                            </ProtectedRoute>
                        }
                    />
                    <Route
                        path="/admin/users"
                        element={
                            <ProtectedRoute roles={["Admin", "Moderator"]}>
                                <UserAdmin />
                            </ProtectedRoute>
                        }
                    />
                    <Route
                        path="/admin/investments"
                        element={
                            <ProtectedRoute roles={["Admin", "Moderator"]}>
                                <InvestmentsAdmin />
                            </ProtectedRoute>
                        }
                    />
                    <Route
                        path="/admin/buildings"
                        element={
                            <ProtectedRoute roles={["Admin", "Moderator"]}>
                                <BuildingsAdmin />
                            </ProtectedRoute>
                        }
                    />
                    <Route
                        path="/admin/properties"
                        element={
                            <ProtectedRoute roles={["Admin", "Moderator"]}>
                                <PropertiesAdmin />
                            </ProtectedRoute>
                        }
                    />
                    <Route
                        path="/admin/features"
                        element={
                            <ProtectedRoute roles={["Admin", "Moderator"]}>
                                <FeaturesAdmin />
                            </ProtectedRoute>
                        }
                    />
                    <Route
                        path="/admin/feature-types"
                        element={
                            <ProtectedRoute roles={["Admin", "Moderator"]}>
                                <FeatureTypesAdmin />
                            </ProtectedRoute>
                        }
                    />
                    <Route
                        path="/admin/logs"
                        element={
                            <ProtectedRoute roles={["Admin"]}>
                                <LogsAdmin />
                            </ProtectedRoute>
                        }
                    />
                    <Route
                        path="/admin/company-info"
                        element={
                            <ProtectedRoute roles={["Admin", "Moderator"]}>
                                <CompanyAdmin />
                            </ProtectedRoute>
                        }
                    />
                    <Route
                        path="/admin/reports"
                        element={
                            <ProtectedRoute roles={["Admin"]}>
                                <AdminReportsPage />
                            </ProtectedRoute>
                        }
                    />
                    <Route
                        path="/admin/compare-prices"
                        element={
                            <ProtectedRoute roles={["Admin", "Moderator"]}>
                                <AdminComparisonPage />
                            </ProtectedRoute>
                        }
                    />
        
                </Routes>
        </NotificationProvider>
    );
}
