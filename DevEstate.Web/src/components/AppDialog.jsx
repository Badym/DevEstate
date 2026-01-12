import {
    Dialog,
    DialogContent,
    DialogHeader,
    DialogTitle,
    DialogFooter
} from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";

export default function AppDialog({ open, onClose, type = "info", message }) {

    const getColor = () => {
        switch (type) {
            case "success":
                return "text-green-600";
            case "error":
                return "text-red-600";
            default:
                return "text-blue-600";
        }
    };

    const getTitle = () => {
        switch (type) {
            case "success":
                return "Sukces";
            case "error":
                return "Błąd";
            default:
                return "Informacja";
        }
    };

    return (
        <Dialog open={open} onOpenChange={onClose}>
            <DialogContent className="max-w-sm">
                <DialogHeader>
                    <DialogTitle className={getColor()}>
                        {getTitle()}
                    </DialogTitle>
                </DialogHeader>

                <div className="py-4 text-gray-700 text-sm">
                    {message}
                </div>

                <DialogFooter>
                    <Button
                        className="bg-gray-700 text-white hover:bg-gray-800"
                        onClick={onClose}
                    >
                        OK
                    </Button>
                </DialogFooter>
            </DialogContent>
        </Dialog>
    );
}
