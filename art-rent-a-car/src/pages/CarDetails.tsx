import { Box, Container, Typography, Button, TextField, Chip, Dialog, DialogContent, DialogActions } from "@mui/material";
import { useParams } from "react-router-dom";
import { cars } from "../data/cars";
import { useState } from "react";
import dayjs, { Dayjs } from "dayjs";
import { LocalizationProvider } from "@mui/x-date-pickers/LocalizationProvider";
import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
import { DatePicker } from "@mui/x-date-pickers/DatePicker";
import CheckCircleOutlineIcon from "@mui/icons-material/CheckCircleOutline";

const CarDetails = () => {
  const { slug } = useParams();
  const car = cars.find((c) => c.slug === slug);

  const [pickupDate, setPickupDate] = useState<Dayjs | null>(null);
  const [returnDate, setReturnDate] = useState<Dayjs | null>(null);
  const [name, setName] = useState("");
  const [phone, setPhone] = useState("");

  const [successOpen, setSuccessOpen] = useState(false);

  if (!car) {
    return (
      <Container sx={{ py: 6 }}>
        <Typography variant="h5">Car not found.</Typography>
      </Container>
    );
  }

  const days = pickupDate && returnDate ? returnDate.diff(pickupDate, "day") : 0;
  const totalPrice = days > 0 ? days * car.pricePerDay : 0;

  const buildMessage = () =>
    `Car: ${car.name}\n` +
    `Pickup: ${pickupDate!.format("YYYY-MM-DD")}\n` +
    `Return: ${returnDate!.format("YYYY-MM-DD")}\n` +
    `Days: ${days}\n` +
    `Total: €${totalPrice}\n` +
    `Name: ${name}\n` +
    `Phone: ${phone}`;

  const validate = () => {
    if (!pickupDate || !returnDate) { alert("Please select dates first"); return false; }
    if (days <= 0) { alert("Return date must be after pickup date"); return false; }
    if (!name || !phone) { alert("Please fill all fields"); return false; }
    return true;
  };

  const saveBooking = () => {
    const booking = {
      carId: car.id,
      carName: car.name,
      customerName: name,
      customerPhone: phone,
      pickupDate: pickupDate!.format("YYYY-MM-DD"),
      returnDate: returnDate!.format("YYYY-MM-DD"),
      totalPrice,
      days,
      createdAt: new Date().toISOString(),
    };
    const existing = JSON.parse(localStorage.getItem("bookings") || "[]");
    existing.push(booking);
    localStorage.setItem("bookings", JSON.stringify(existing));
  };

  const handleWhatsApp = () => {
    if (!validate()) return;
    saveBooking();
    setSuccessOpen(true);
    window.open("https://wa.me/+38975400462?text=" + encodeURIComponent(buildMessage()), "_blank");
  };

  const handleViber = () => {
    if (!validate()) return;
    saveBooking();
    setSuccessOpen(true);
    window.open("viber://chat?number=%2B38975400462&text=" + encodeURIComponent(buildMessage()), "_blank");
  };

  return (
    <Container sx={{ py: 6, maxWidth: "md" }}>
      {/* SUCCESS DIALOG */}
      <Dialog open={successOpen} onClose={() => setSuccessOpen(false)} PaperProps={{ sx: { borderRadius: 4, p: 2, textAlign: "center" } }}>
        <DialogContent>
          <CheckCircleOutlineIcon sx={{ fontSize: 64, color: "#25D366", mb: 2 }} />
          <Typography variant="h5" fontWeight={700} mb={1}>Reservation Confirmed!</Typography>
          <Typography color="text.secondary" mb={1}>Your reservation for <strong>{car.name}</strong> has been submitted successfully.</Typography>
          <Typography variant="body2" color="text.secondary">
            {pickupDate?.format("DD MMM YYYY")} → {returnDate?.format("DD MMM YYYY")} • {days} day{days > 1 ? "s" : ""} • <strong>€{totalPrice}</strong>
          </Typography>
        </DialogContent>
        <DialogActions sx={{ justifyContent: "center", pb: 2 }}>
          <Button variant="contained" onClick={() => setSuccessOpen(false)}>Close</Button>
        </DialogActions>
      </Dialog>

      {/* IMAGE */}
      <Box
        sx={{
          width: "100%",
          height: 340,
          display: "flex",
          alignItems: "center",
          justifyContent: "center",
          bgcolor: "#ffffff",
          borderRadius: 4,
          overflow: "hidden",
          mb: 4,
        }}
      >
        <img
          src={car.image}
          alt={car.name}
          style={{ maxHeight: "300px", maxWidth: "100%", objectFit: "contain", mixBlendMode: "multiply" }}
        />
      </Box>

      {/* CAR INFO */}
      <Typography variant="h3" fontWeight={700}>{car.name}</Typography>
      <Typography color="text.secondary" mt={1} mb={2}>{car.description}</Typography>

      <Box sx={{ display: "flex", flexWrap: "wrap", gap: 1, mb: 3 }}>
        <Chip label={car.transmission} />
        <Chip label={car.fuel} />
        <Chip label={`${car.seats} seats`} />
        <Chip label={`${car.luggage} bags`} />
        <Chip label={car.year.toString()} />
        {car.airConditioning && <Chip label="A/C" />}
      </Box>

      <Typography variant="h4" fontWeight={700} color="primary" mb={4}>
        €{car.pricePerDay} <Typography component="span" variant="body1" color="text.secondary">/ day</Typography>
      </Typography>

      {/* DATE PICKERS */}
      <LocalizationProvider dateAdapter={AdapterDayjs}>
        <Box sx={{ display: "flex", gap: 2, flexWrap: "wrap", mb: 3 }}>
          <DatePicker
            label="Pickup Date"
            value={pickupDate}
            onChange={(v) => setPickupDate(v)}
            sx={{ flex: 1, minWidth: 200 }}
          />
          <DatePicker
            label="Return Date"
            value={returnDate}
            onChange={(v) => setReturnDate(v)}
            sx={{ flex: 1, minWidth: 200 }}
          />
        </Box>
      </LocalizationProvider>

      {days > 0 && (
        <Typography variant="h6" fontWeight={600} mb={3}>
          {days} day{days > 1 ? "s" : ""} — Total: €{totalPrice}
        </Typography>
      )}

      {/* FORM */}
      <Box sx={{ display: "flex", flexDirection: "column", gap: 2, maxWidth: 500 }}>
        <TextField
          fullWidth
          label="Your Name"
          value={name}
          onChange={(e) => setName(e.target.value)}
        />
        <TextField
          fullWidth
          label="Phone"
          value={phone}
          onChange={(e) => setPhone(e.target.value)}
        />
        <Button variant="contained" size="large" fullWidth onClick={handleWhatsApp}
          sx={{ bgcolor: "#25D366", "&:hover": { bgcolor: "#1ebe57" } }}
        >
          Reserve via WhatsApp
        </Button>
        <Button variant="contained" size="large" fullWidth onClick={handleViber}
          sx={{ bgcolor: "#7360F2", "&:hover": { bgcolor: "#5b4ac4" } }}
        >
          Reserve via Viber
        </Button>
      </Box>
    </Container>
  );
};

export default CarDetails;
