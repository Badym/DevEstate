export default function AdminLogsTable({ logs }) {
    return (
        <div className="overflow-x-auto bg-white rounded-lg shadow">
            <table className="min-w-full text-left">
                <thead className="bg-gray-100 border-b">
                <tr>
                    <th className="p-4">Użytkownik</th>
                    <th className="p-4">Akcja</th>
                    <th className="p-4">Encja</th>
                    <th className="p-4">ID encji</th>
                    <th className="p-4">Czas</th>
                </tr>
                </thead>

                <tbody>
                {logs.map((log) => (
                    <tr key={log.id} className="border-b hover:bg-gray-50">
                        <td className="p-4">{log.userName}</td>
                        <td className="p-4">{log.action}</td>
                        <td className="p-4">{log.entity}</td>
                        <td className="p-4 text-gray-500">{log.entityId}</td>
                        <td className="p-4 text-gray-500">
                            {new Date(log.timestamp).toLocaleString("pl-PL")}
                        </td>
                    </tr>
                ))}
                </tbody>
            </table>

            {logs.length === 0 && (
                <p className="text-center p-6 text-gray-500">Brak wyników.</p>
            )}
        </div>
    );
}
