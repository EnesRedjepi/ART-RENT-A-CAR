import {
  Box,
  Container,
  Typography,
  Button,
  TextField,
  Chip,
  Dialog,
  DialogContent,
  DialogActions,
  Paper,
} from "@mui/material";
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

  const days =
    pickupDate && returnDate ? returnDate.diff(pickupDate, "day") : 0;
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
    if (!pickupDate || !returnDate) {
      alert("Please select dates first");
      return false;
    }
    if (days <= 0) {
      alert("Return date must be after pickup date");
      return false;
    }
    if (!name || !phone) {
      alert("Please fill all fields");
      return false;
    }
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
    window.open(
      "https://wa.me/+38975400462?text=" + encodeURIComponent(buildMessage()),
      "_blank",
    );
  };

  const handleViber = () => {
    if (!validate()) return;
    saveBooking();
    setSuccessOpen(true);
    window.open(
      "viber://chat?number=%2B38975400462&text=" +
        encodeURIComponent(buildMessage()),
      "_blank",
    );
  };

  return (
    <Container sx={{ py: { xs: 3, md: 6 }, maxWidth: "lg" }}>
      {/* SUCCESS DIALOG */}
      <Dialog
        open={successOpen}
        onClose={() => setSuccessOpen(false)}
        PaperProps={{ sx: { borderRadius: 4, p: 2, textAlign: "center" } }}
      >
        <DialogContent>
          <CheckCircleOutlineIcon
            sx={{ fontSize: 64, color: "#25D366", mb: 2 }}
          />
          <Typography variant="h5" fontWeight={700} mb={1}>
            Request ready to send
          </Typography>
          <Typography color="text.secondary" mb={1}>
            Your message for <strong>{car.name}</strong> is ready in your
            selected app. Send it there so we can confirm availability.
          </Typography>
          <Typography variant="body2" color="text.secondary">
            {pickupDate?.format("DD MMM YYYY")} →{" "}
            {returnDate?.format("DD MMM YYYY")} • {days} day
            {days > 1 ? "s" : ""} • <strong>€{totalPrice}</strong>
          </Typography>
        </DialogContent>
        <DialogActions sx={{ justifyContent: "center", pb: 2 }}>
          <Button variant="contained" onClick={() => setSuccessOpen(false)}>
            Close
          </Button>
        </DialogActions>
      </Dialog>

      <Box
        sx={{
          display: "grid",
          gridTemplateColumns: {
            xs: "1fr",
            md: "minmax(0, 1.15fr) minmax(360px, 0.85fr)",
          },
          alignItems: "start",
          gap: { xs: 3, md: 5 },
        }}
      >
        <Box>
          {/* IMAGE */}
          <Box
            sx={{
              width: "100%",
              aspectRatio: "16 / 10",
              display: "flex",
              alignItems: "center",
              justifyContent: "center",
              bgcolor: "#ffffff",
              borderRadius: 2,
              overflow: "hidden",
              p: { xs: 2, md: 4 },
            }}
          >
            <img
              src={car.image}
              alt={car.name}
              style={{
                maxHeight: "100%",
                maxWidth: "100%",
                objectFit: "contain",
                mixBlendMode: "multiply",
              }}
            />
          </Box>

          {/* CAR INFO */}
          <Typography
            variant="h3"
            sx={{ fontSize: { xs: "2rem", md: "2.5rem" } }}
          >
            {car.name}
          </Typography>
          <Typography color="text.secondary" mt={1} mb={2}>
            {car.description}
          </Typography>

          <Box sx={{ display: "flex", flexWrap: "wrap", gap: 1, mb: 3 }}>
            <Chip label={car.transmission} />
            <Chip label={car.fuel} />
            <Chip label={`${car.seats} seats`} />
            <Chip label={`${car.luggage} bags`} />
            <Chip label={car.year.toString()} />
            {car.airConditioning && <Chip label="A/C" />}
          </Box>
        </Box>

        <Paper
          component="section"
          elevation={0}
          sx={{
            p: { xs: 2.5, md: 3 },
            border: "1px solid",
            borderColor: "divider",
            borderRadius: 2,
          }}
        >
          <Typography
            variant="overline"
            color="text.secondary"
            fontWeight={700}
          >
            RESERVATION REQUEST
          </Typography>
          <Typography variant="h4" fontWeight={700} color="primary" mb={3}>
            €{car.pricePerDay}{" "}
            <Typography component="span" variant="body1" color="text.secondary">
              / day
            </Typography>
          </Typography>

          {/* DATE PICKERS */}
          <LocalizationProvider dateAdapter={AdapterDayjs}>
            <Box
              sx={{
                display: "grid",
                gridTemplateColumns: { xs: "1fr", sm: "1fr 1fr" },
                gap: 1.5,
                mb: 2,
              }}
            >
              <DatePicker
                label="Pickup Date"
                value={pickupDate}
                onChange={(v) => setPickupDate(v)}
                minDate={dayjs()}
              />
              <DatePicker
                label="Return Date"
                value={returnDate}
                onChange={(v) => setReturnDate(v)}
                minDate={
                  pickupDate ? pickupDate.add(1, "day") : dayjs().add(1, "day")
                }
              />
            </Box>
          </LocalizationProvider>

          {days > 0 && (
            <Box
              sx={{
                display: "flex",
                justifyContent: "space-between",
                mb: 2,
                p: 1.5,
                bgcolor: "action.hover",
                borderRadius: 1,
              }}
            >
              <Typography color="text.secondary">
                {days} day{days > 1 ? "s" : ""}
              </Typography>
              <Typography fontWeight={700}>Total €{totalPrice}</Typography>
            </Box>
          )}

          {/* FORM */}
          <Box
            sx={{
              display: "grid",
              gridTemplateColumns: { xs: "1fr", sm: "1fr 1fr" },
              gap: 1.5,
              mb: 2,
            }}
          >
            <TextField
              fullWidth
              label="Your Name"
              value={name}
              onChange={(e) => setName(e.target.value)}
            />
            <TextField
              fullWidth
              label="Phone"
              type="tel"
              value={phone}
              onChange={(e) => setPhone(e.target.value)}
            />
          </Box>
          <Box
            sx={{
              display: "grid",
              gridTemplateColumns: { xs: "1fr", sm: "1fr 1fr" },
              gap: 1.5,
            }}
          >
            <Button
              variant="contained"
              size="large"
              fullWidth
              onClick={handleWhatsApp}
              sx={{
                bgcolor: "#25D366",
                color: "#fff",
                "&:hover": { bgcolor: "#1ebe57" },
              }}
            >
              Reserve via WhatsApp
            </Button>
            <Button
              variant="contained"
              size="large"
              fullWidth
              onClick={handleViber}
              sx={{
                bgcolor: "#6655C7",
                color: "#fff",
                "&:hover": { bgcolor: "#5544B3" },
              }}
            >
              Reserve via Viber
            </Button>
          </Box>
          <Typography
            variant="caption"
            color="text.secondary"
            sx={{ display: "block", mt: 1.5 }}
          >
            Availability is confirmed by our team after you send the message.
          </Typography>
        </Paper>
      </Box>
    </Container>
  );
};

export default CarDetails;
