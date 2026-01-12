import { useState } from "react";
import { Button } from "@/components/ui/button";

export default function DeveloperPriceSelector({ regions, onSelect }) {
    const [woj, setWoj] = useState("");
    const [pow, setPow] = useState("");

    const powiatyFiltered = regions.powiaty.filter(p => p.woj === woj);

    return (
        <div className="bg-white p-6 rounded-lg shadow mb-10">
            <h2 className="text-xl font-semibold mb-4">Wybierz region do porównania</h2>

            <div className="flex gap-4 items-center">
                {/* WOJEWÓDZTWO */}
                <select
                    className="border p-2 rounded"
                    value={woj}
                    onChange={e => {
                        setWoj(e.target.value);
                        setPow("");
                    }}
                >
                    <option value="">-- wybierz województwo --</option>
                    {regions.wojewodztwa.map(w => (
                        <option key={w} value={w}>{w}</option>
                    ))}
                </select>

                {/* POWIAT */}
                <select
                    className="border p-2 rounded"
                    value={pow}
                    onChange={e => setPow(e.target.value)}
                    disabled={!woj}
                >
                    <option value="">-- wszystkie powiaty --</option>
                    {powiatyFiltered.map(p => (
                        <option key={p.pow} value={p.pow}>{p.pow}</option>
                    ))}
                </select>

                <Button
                    className="bg-[#C8A27E] text-white hover:bg-[#b18e6b]"
                    onClick={() => onSelect(woj, pow)}
                >
                    Dodaj do porównania
                </Button>
            </div>
        </div>
    );
}
