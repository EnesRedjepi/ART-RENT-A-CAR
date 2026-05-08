import { Box, Container, Typography, Button, TextField } from "@mui/material";
import { useParams } from "react-router-dom";
import { cars } from "../data/cars";
import { useState, useEffect } from "react";
import dayjs from "dayjs";
import { LocalizationProvider } from "@mui/x-date-pickers/LocalizationProvider";
import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
import { DatePicker } from "@mui/x-date-pickers/DatePicker";

const CarDetails = () => {
  const { slug } = useParams();
  const car = cars.find((c) => c.slug === slug);

  const [name, setName] = useState("");
  const [phone, setPhone] = useState("");

  // ✅ DEFINE FIRST
  const [pickupDate, setPickupDate] = useState(null);
  const [returnDate, setReturnDate] = useState(null);

  useEffect(() => {
    if (pickupDate) {
      localStorage.setItem("pickupDate", pickupDate.format("YYYY-MM-DD"));
    }
    if (returnDate) {
      localStorage.setItem("returnDate", returnDate.format("YYYY-MM-DD"));
    }
  }, [pickupDate, returnDate]);

  if (!car) return <Typography>Car not found</Typography>;

  // ✅ CALCULATE AFTER
  let totalDays = 1;

  if (pickupDate && returnDate) {
    const diff = returnDate.diff(pickupDate, "day");
    totalDays = diff > 0 ? diff : 1;
  }

  const totalPrice = totalDays * car.pricePerDay;

  const handleReserve = () => {
    if (!pickupDate || !returnDate) {
      alert("Please select dates first");
      return;
    }

    if (!name || !phone) {
      alert("Please fill all fields");
      return;
    }

    const days = returnDate.diff(pickupDate, "day");

    if (days <= 0) {
      alert("Return date must be after pickup date");
      return;
    }

    const totalPrice = days * car.pricePerDay;

    const message = `
Car: ${car.name}
Pickup: ${pickupDate.format("YYYY-MM-DD")}
Return: ${returnDate.format("YYYY-MM-DD")}
Days: ${days}
Total: €${totalPrice}
Name: ${name}
Phone: ${phone}
`;

    const whatsappUrl = `https://wa.me/38975400462?text=${encodeURIComponent(message)}`;

    window.open(whatsappUrl, "_blank");
  };

  return (
    <Container sx={{ py: 6 }}>
      <img src={car.image} style={{ width: "100%" }} />

      <Typography variant="h3" mt={3}>
        {car.name}
      </Typography>

      <Typography mt={2}>
        {car.transmission} • {car.fuel}
      </Typography>

      <Typography mt={1}>
        {car.seats} seats • {car.luggage} bags
      </Typography>

      <Typography mt={2} fontWeight={600}>
        €{car.pricePerDay} / day
      </Typography>

      {/* ✅ DATE PICKERS HERE */}
      <LocalizationProvider dateAdapter={AdapterDayjs}>
        <Box mt={3} sx={{ maxWidth: 400 }}>
          <DatePicker
            label="Pickup Date"
            value={pickupDate}
            onChange={(v) => setPickupDate(v)}
            sx={{ mb: 2, width: "100%" }}
          />

          <DatePicker
            label="Return Date"
            value={returnDate}
            onChange={(v) => setReturnDate(v)}
            sx={{ mb: 2, width: "100%" }}
          />
        </Box>
      </LocalizationProvider>

      {/* ✅ SHOW DATES */}
      {pickupDate && returnDate && (
        <Typography mt={2} fontWeight={600}>
          Total: €{totalPrice}
        </Typography>
      )}

      {/* FORM */}
      <Box mt={4} sx={{ maxWidth: 400 }}>
        <TextField
          fullWidth
          label="Your Name"
          value={name}
          onChange={(e) => setName(e.target.value)}
          sx={{ mb: 2 }}
        />

        <TextField
          fullWidth
          label="Phone"
          value={phone}
          onChange={(e) => setPhone(e.target.value)}
          sx={{ mb: 2 }}
        />

        <Button variant="contained" fullWidth onClick={handleReserve}>
          Reserve Now
        </Button>
      </Box>
    </Container>
  );
};

export default CarDetails;
