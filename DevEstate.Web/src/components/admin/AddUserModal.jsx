import { useState } from "react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import {
    Dialog,
    DialogContent,
    DialogHeader,
    DialogTitle,
    DialogFooter,
} from "@/components/ui/dialog";

export default function AddUserModal({ open, onClose, onSave }) {
    const [form, setForm] = useState({
        fullName: "",
        email: "",
        password: "",
        role: "Moderator", // domyślnie Moderator
    });

    const handleChange = (e) => {
        const { name, value } = e.target;
        setForm((prev) => ({ ...prev, [name]: value }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const token = localStorage.getItem("token");

            const res = await fetch("/api/User", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${token}`,
                },
                body: JSON.stringify(form),
            });

            if (!res.ok) throw new Error("Nie udało się utworzyć użytkownika.");

            alert("✅ Użytkownik został dodany!");
            onSave();
            onClose();
        } catch (err) {
            alert(`❌ Błąd: ${err.message}`);
        }
    };

    return (
        <Dialog open={open} onOpenChange={onClose}>
            <DialogContent className="sm:max-w-md">
                <DialogHeader>
                    <DialogTitle>Dodaj nowego użytkownika</DialogTitle>
                </DialogHeader>

                <form onSubmit={handleSubmit} className="space-y-4 mt-4">
                    {/* Imię i nazwisko */}
                    <div>
                        <label className="block text-xs text-gray-600 mb-1">
                            Imię i nazwisko *
                        </label>
                        <Input
                            name="fullName"
                            value={form.fullName}
                            onChange={handleChange}
                            required
                        />
                    </div>

                    {/* Email */}
                    <div>
                        <label className="block text-xs text-gray-600 mb-1">
                            Email *
                        </label>
                        <Input
                            name="email"
                            type="email"
                            value={form.email}
                            onChange={handleChange}
                            required
                        />
                    </div>

                    {/* Hasło */}
                    <div>
                        <label className="block text-xs text-gray-600 mb-1">
                            Hasło *
                        </label>
                        <Input
                            name="password"
                            type="password"
                            value={form.password}
                            onChange={handleChange}
                            required
                        />
                    </div>

                    {/* Rola */}
                    <div>
                        <label className="block text-xs text-gray-600 mb-1">
                            Rola *
                        </label>

                        <select
                            name="role"
                            value={form.role}
                            onChange={handleChange}
                            className="w-full border border-gray-300 p-2 rounded-md"
                            required
                        >
                            <option value="Moderator">Moderator</option>
                        </select>

                        <p className="text-xs text-gray-500 mt-1">
                            Administratora nie można dodać z tego poziomu
                        </p>
                    </div>

                    <DialogFooter>
                        <Button type="button" variant="outline" onClick={onClose}>
                            Anuluj
                        </Button>

                        <Button
                            type="submit"
                            className="bg-green-600 text-white hover:bg-green-700"
                        >
                            Dodaj użytkownika
                        </Button>
                    </DialogFooter>
                </form>
            </DialogContent>
        </Dialog>
    );
}
