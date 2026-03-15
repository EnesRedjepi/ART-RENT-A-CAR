import {
  Dialog,
  DialogTitle,
  DialogContent,
  TextField,
  Button,
  Stack,
} from "@mui/material";
import { useState } from "react";

export default function ReservationModal({ open, onClose, car }) {
  const [name, setName] = useState("");
  const [phone, setPhone] = useState("");
  const [selectedCar, setSelectedCar] = useState(null);
  <Button variant="contained" fullWidth onClick={() => setSelectedCar(car)}>
    Reserve
  </Button>;
  {
    selectedCar && (
      <ReservationModal
        open={true}
        car={selectedCar}
        onClose={() => setSelectedCar(null)}
      />
    );
  }

  const handleSubmit = () => {
    const booking = {
      car: car.name,
      name,
      phone,
      date: new Date(),
    };

    console.log("Booking:", booking);

    onClose();
  };

  return (
    <Dialog open={open} onClose={onClose}>
      <DialogTitle>Reserve {car.name}</DialogTitle>

      <DialogContent>
        <Stack spacing={2} sx={{ mt: 2 }}>
          <TextField
            label="Full Name"
            value={name}
            onChange={(e) => setName(e.target.value)}
          />

          <TextField
            label="Phone"
            value={phone}
            onChange={(e) => setPhone(e.target.value)}
          />

          <Button variant="contained" onClick={handleSubmit}>
            Confirm Reservation
          </Button>
        </Stack>
      </DialogContent>
    </Dialog>
  );
}
