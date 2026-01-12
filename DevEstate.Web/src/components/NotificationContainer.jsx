import { useNotification } from "@/components/NotificationProvider";

export default function NotificationContainer() {
    const { notifications } = useNotification();

    return (
        <div className="fixed top-4 right-4 z-[9999] space-y-3">
            {notifications.map((n) => (
                <div
                    key={n.id}
                    className={`
                        px-4 py-3 rounded-lg shadow-lg text-white
                        transition-all duration-300
                        ${n.type === "success" ? "bg-green-600" : ""}
                        ${n.type === "error" ? "bg-red-600" : ""}
                        ${n.type === "info" ? "bg-blue-600" : ""}
                    `}
                >
                    {n.message}
                </div>
            ))}
        </div>
    );
}
