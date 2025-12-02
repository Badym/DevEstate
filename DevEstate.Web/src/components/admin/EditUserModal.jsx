import { useEffect, useState } from "react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import {
    Dialog,
    DialogContent,
    DialogHeader,
    DialogTitle,
    DialogFooter,
} from "@/components/ui/dialog";

export default function EditUserModal({ open, onClose, user, onSave }) {
    const [form, setForm] = useState({
        fullName: "",
        email: "",
        password: "",
        role: "Moderator"
    });

    useEffect(() => {
        if (user) {
            setForm({
                fullName: user.fullName,
                email: user.email,
                password: "",
                role: user.role,
            });
        }
    }, [user]);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setForm((prev) => ({ ...prev, [name]: value }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        const token = localStorage.getItem("token");

        const res = await fetch(`/api/User/${user.id}`, {
            method: "PATCH",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${token}`
            },
            body: JSON.stringify(form),
        });

        if (!res.ok) {
            alert("❌ Nie udało się zaktualizować użytkownika");
            return;
        }

        alert("✅ Użytkownik zaktualizowany!");
        onSave();
        onClose();
    };

    return (
        <Dialog open={open} onOpenChange={onClose}>
            <DialogContent className="sm:max-w-md">
                <DialogHeader>
                    <DialogTitle>Edycja użytkownika</DialogTitle>
                </DialogHeader>

                <form onSubmit={handleSubmit} className="space-y-4 mt-4">
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

                    <div>
                        <label className="block text-xs text-gray-600 mb-1">
                            Nowe hasło (opcjonalne)
                        </label>
                        <Input
                            name="password"
                            type="password"
                            value={form.password}
                            onChange={handleChange}
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
                    </div>

                    <DialogFooter>
                        <Button variant="outline" onClick={onClose}>
                            Anuluj
                        </Button>
                        <Button className="bg-blue-600 text-white hover:bg-blue-700" type="submit">
                            Zapisz zmiany
                        </Button>
                    </DialogFooter>
                </form>
            </DialogContent>
        </Dialog>
    );
}
