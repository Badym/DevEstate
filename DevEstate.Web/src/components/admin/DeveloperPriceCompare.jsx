import { BarChart, Bar, XAxis, YAxis, Tooltip, ResponsiveContainer } from "recharts";

export default function DeveloperPriceCompare({ items }) {
    const data = items.map(x => ({
        name: `${x.wojewodztwo}${x.powiat !== "(wszystkie)" ? " - " + x.powiat : ""}`,
        avg: x.avgPriceM2
    }));

    return (
        <div className="bg-white p-6 rounded-lg shadow">
            <h2 className="text-xl font-semibold mb-4">Porównanie średniej ceny m²</h2>

            <div className="w-full h-96">
                <ResponsiveContainer>
                    <BarChart data={data}>
                        <XAxis dataKey="name" tick={{ fontSize: 12 }} />
                        <YAxis />
                        <Tooltip />
                        <Bar dataKey="avg" fill="#C8A27E" />
                    </BarChart>
                </ResponsiveContainer>
            </div>
        </div>
    );
}
