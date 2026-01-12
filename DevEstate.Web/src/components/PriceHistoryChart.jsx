import { useEffect, useState } from "react";
import {
    Dialog,
    DialogContent,
    DialogHeader,
    DialogTitle,
    DialogFooter,
} from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";
import {
    LineChart,
    Line,
    XAxis,
    YAxis,
    Tooltip,
    ResponsiveContainer,
    CartesianGrid,
} from "recharts";

const CustomTooltip = ({ active, payload, label }) => {
    if (!active || !payload || payload.length === 0) return null;
    const price = payload[0].value;

    return (
        <div className="bg-white border border-gray-300 rounded-md px-3 py-2 shadow text-sm">
            <p className="font-medium text-gray-700">{label}</p>
            <p className="text-indigo-600 font-semibold">
                {price.toLocaleString("pl-PL")} zł
            </p>
        </div>
    );
};

export default function PriceHistoryChart({ propertyId, open, onClose }) {
    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        if (!open || !propertyId) return;

        const fetchHistory = async () => {
            setLoading(true);
            try {
                const res = await fetch(`/api/pricehistory/property/${propertyId}`);
                if (!res.ok) throw new Error("Nie udało się pobrać historii cen.");
                const history = await res.json();

                // Formatowanie
                const formatted = history.map((h) => ({
                    date: new Date(h.date).toLocaleDateString("pl-PL"),
                    newPrice: h.newPrice,
                }));

                // Odwrócenie kolejności
                const reversed = formatted.reverse();

                // ➕ Przedłużenie wykresu o 2 dni
                if (reversed.length > 0) {
                    const last = reversed[reversed.length - 1];

                    const extendDate = new Date();
                    extendDate.setDate(extendDate.getDate() + 2);

                    reversed.push({
                        date: extendDate.toLocaleDateString("pl-PL"),
                        newPrice: last.newPrice,
                    });
                }

                setData(reversed);
            } catch (err) {
                console.error(err);
            } finally {
                setLoading(false);
            }
        };

        fetchHistory();
    }, [open, propertyId]);

    return (
        <Dialog open={open} onOpenChange={onClose}>
            <DialogContent className="max-w-3xl">
                <DialogHeader>
                    <DialogTitle>Historia zmian ceny</DialogTitle>
                </DialogHeader>

                {loading ? (
                    <div className="text-center text-gray-500 py-10">
                        Ładowanie danych...
                    </div>
                ) : data.length === 0 ? (
                    <div className="text-center text-gray-500 py-10">
                        Brak danych o historii cen dla tej nieruchomości.
                    </div>
                ) : (
                    <div className="w-full h-80">
                        <ResponsiveContainer width="100%" height="100%">
                            <LineChart
                                data={data}
                                margin={{ top: 20, right: 30, left: 10, bottom: 10 }}
                            >
                                <CartesianGrid strokeDasharray="3 3" />

                                <XAxis
                                    dataKey="date"
                                    tick={{ fontSize: 12 }}
                                />

                                <YAxis
                                    tickFormatter={(v) =>
                                        v.toLocaleString("pl-PL", {
                                            style: "currency",
                                            currency: "PLN",
                                            maximumFractionDigits: 0,
                                        })
                                    }
                                    tick={{ fontSize: 12 }}
                                />

                                <Tooltip
                                    content={<CustomTooltip />}
                                    cursor={{ stroke: "#999", strokeWidth: 1 }}
                                    isAnimationActive={false}
                                />

                                <Line
                                    type="stepAfter"
                                    dataKey="newPrice"
                                    stroke="#4f46e5"
                                    strokeWidth={3}
                                    activeDot={{
                                        r: 7,
                                        stroke: "#fff",
                                        strokeWidth: 2,
                                        fill: "#4f46e5",
                                    }}
                                    dot={{
                                        r: 5,
                                        stroke: "#fff",
                                        strokeWidth: 2,
                                        fill: "#4f46e5",
                                    }}
                                    isAnimationActive={false}
                                />
                            </LineChart>
                        </ResponsiveContainer>
                    </div>
                )}

                <DialogFooter>
                    <Button
                        onClick={onClose}
                        className="bg-gray-600 text-white hover:bg-gray-700"
                    >
                        Zamknij
                    </Button>
                </DialogFooter>
            </DialogContent>
        </Dialog>
    );
}
