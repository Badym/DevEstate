import { createContext, useContext, useState } from "react";

const NotificationContext = createContext();

export function NotificationProvider({ children }) {
    const [notifications, setNotifications] = useState([]);

    const notify = (type, message) => {
        const id = crypto.randomUUID();
        setNotifications((prev) => [...prev, { id, type, message }]);

        // Auto-remove after 4 seconds
        setTimeout(() => {
            setNotifications((prev) => prev.filter((n) => n.id !== id));
        }, 4000);
    };

    return (
        <NotificationContext.Provider value={{
            notifications,
            notifySuccess: (msg) => notify("success", msg),
            notifyError: (msg) => notify("error", msg),
            notifyInfo: (msg) => notify("info", msg),
        }}>
            {children}
        </NotificationContext.Provider>
    );
}

export const useNotification = () => useContext(NotificationContext);
