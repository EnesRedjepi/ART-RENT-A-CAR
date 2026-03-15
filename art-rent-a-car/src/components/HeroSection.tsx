import {
  Box,
  Typography,
  Button,
  Paper,
  MenuItem,
  TextField,
  Container,
} from "@mui/material";
import { useNavigate } from "react-router-dom";
import { useState } from "react";

import { LocalizationProvider } from "@mui/x-date-pickers/LocalizationProvider";
import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
import { DatePicker } from "@mui/x-date-pickers/DatePicker";

import dayjs from "dayjs";

export default function HeroSection() {
  const navigate = useNavigate();

  const [pickupDate, setPickupDate] = useState(dayjs());
  const [returnDate, setReturnDate] = useState(dayjs().add(1, "day"));
  const [transmission, setTransmission] = useState("");

  const handleSearch = () => {
    navigate(
      `/fleet?transmission=${transmission}&pickup=${pickupDate.format(
        "YYYY-MM-DD",
      )}&return=${returnDate.format("YYYY-MM-DD")}`,
    );
  };

  return (
    <Box
      sx={{
        minHeight: "85vh",
        backgroundImage: "url(/images/hero.jpg)",
        backgroundSize: "cover",
        backgroundPosition: "center",
        display: "flex",
        alignItems: "center",
      }}
    >
      <Container maxWidth="lg">
        <Box
          sx={{
            display: "flex",
            flexWrap: "wrap",
            alignItems: "center",
            justifyContent: "space-between",
            gap: 6,
          }}
        >
          {/* LEFT SIDE */}
          <Box sx={{ color: "white", maxWidth: 520 }}>
            <Typography variant="h2" sx={{ fontWeight: 700 }}>
              Premium Rent a Car Skopje Airport
            </Typography>

            <Typography sx={{ mt: 2 }}>
              Fast airport delivery. No hidden fees. 24/7 support.
            </Typography>

            <Box sx={{ mt: 3, display: "flex", gap: 2 }}>
              <Button variant="contained" color="secondary">
                View Fleet
              </Button>

              <Button
                variant="outlined"
                sx={{ color: "white", borderColor: "white" }}
              >
                Call Now
              </Button>
            </Box>
          </Box>

          {/* RIGHT SIDE BOOKING CARD */}
          <Paper
            elevation={6}
            sx={{
              p: 4,
              width: 320,
              display: "flex",
              flexDirection: "column",
              gap: 2,
              backgroundColor: "white",
            }}
          >
            <Typography variant="h6">Book Your Car</Typography>

            <LocalizationProvider dateAdapter={AdapterDayjs}>
              <DatePicker
                label="Pickup Date"
                value={pickupDate}
                onChange={(newValue) => setPickupDate(newValue)}
              />

              <DatePicker
                label="Return Date"
                value={returnDate}
                onChange={(newValue) => setReturnDate(newValue)}
              />
            </LocalizationProvider>

            <TextField
              select
              label="Transmission"
              value={transmission}
              onChange={(e) => setTransmission(e.target.value)}
            >
              <MenuItem value="">Any</MenuItem>
              <MenuItem value="Manual">Manual</MenuItem>
              <MenuItem value="Automatic">Automatic</MenuItem>
            </TextField>

            <Button
              variant="contained"
              size="large"
              fullWidth
              onClick={handleSearch}
            >
              Search Cars
            </Button>
          </Paper>
        </Box>
      </Container>
    </Box>
  );
}
