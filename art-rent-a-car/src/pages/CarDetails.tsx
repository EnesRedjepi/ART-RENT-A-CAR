import { Box, Container, Typography, Button, TextField, Chip } from "@mui/material";
import { useParams } from "react-router-dom";
import { cars } from "../data/cars";
import { useState } from "react";
import dayjs, { Dayjs } from "dayjs";
import { LocalizationProvider } from "@mui/x-date-pickers/LocalizationProvider";
import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
import { DatePicker } from "@mui/x-date-pickers/DatePicker";

const CarDetails = () => {
  const { slug } = useParams();
  const car = cars.find((c) => c.slug === slug);

  const [pickupDate, setPickupDate] = useState<Dayjs | null>(null);
  const [returnDate, setReturnDate] = useState<Dayjs | null>(null);
  const [name, setName] = useState("");
  const [phone, setPhone] = useState("");

  if (!car) {
    return (
      <Container sx={{ py: 6 }}>
        <Typography variant="h5">Car not found.</Typography>
      </Container>
    );
  }

  const days = pickupDate && returnDate ? returnDate.diff(pickupDate, "day") : 0;
  const totalPrice = days > 0 ? days * car.pricePerDay : 0;

  const handleReserve = () => {
    if (!pickupDate || !returnDate) {
      alert("Please select dates first");
      return;
    }
    if (days <= 0) {
      alert("Return date must be after pickup date");
      return;
    }
    if (!name || !phone) {
      alert("Please fill all fields");
      return;
    }

    const message =
      `Car: ${car.name}\n` +
      `Pickup: ${pickupDate.format("YYYY-MM-DD")}\n` +
      `Return: ${returnDate.format("YYYY-MM-DD")}\n` +
      `Days: ${days}\n` +
      `Total: €${totalPrice}\n` +
      `Name: ${name}\n` +
      `Phone: ${phone}`;

    window.open(
      "https://wa.me/+38975400462?text=" + encodeURIComponent(message),
      "_blank",
    );
  };

  return (
    <Container sx={{ py: 6, maxWidth: "md" }}>
      {/* IMAGE */}
      <Box
        sx={{
          width: "100%",
          height: 340,
          display: "flex",
          alignItems: "center",
          justifyContent: "center",
          bgcolor: "grey.100",
          borderRadius: 4,
          overflow: "hidden",
          mb: 4,
        }}
      >
        <img
          src={car.image}
          alt={car.name}
          style={{ maxHeight: "300px", maxWidth: "100%", objectFit: "contain" }}
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
        <Button variant="contained" size="large" fullWidth onClick={handleReserve}>
          Reserve via WhatsApp
        </Button>
      </Box>
    </Container>
  );
};

export default CarDetails;
