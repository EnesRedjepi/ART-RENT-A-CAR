import {
  Box,
  Typography,
  Button,
  Paper,
  MenuItem,
  TextField,
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
    const pickup = pickupDate.format("YYYY-MM-DD");
    const dropoff = returnDate.format("YYYY-MM-DD");

    localStorage.setItem("pickupDate", pickup);
    localStorage.setItem("returnDate", dropoff);

    navigate(
      `/fleet?pickup=${pickup}&return=${dropoff}&transmission=${transmission}`,
    );
  };

  return (
    <Box
      sx={{
        height: "80vh",
        backgroundImage: "url(/images/hero.jpg)",
        backgroundSize: "cover",
        backgroundPosition: "center",
        display: "flex",
        alignItems: "center",
        justifyContent: "flex-end",
        pr: 10,
      }}
    >
      <Paper
        sx={{
          mt: 4,
          p: 3,
          display: "flex",
          gap: 2,
          flexWrap: "wrap",
          justifyContent: "center",
          background: "rgba(255,255,255,0.1)",
          backdropFilter: "blur(10px)",
          border: "1px solid rgba(255,255,255,0.2)",
          borderRadius: 3,
        }}
      >
        <Typography variant="h5" mb={3}>
          Search Cars
        </Typography>

        <LocalizationProvider dateAdapter={AdapterDayjs}>
          <DatePicker
            label="Pickup Date"
            value={pickupDate}
            onChange={(v) => v && setPickupDate(v)}
            sx={{ width: "100%", mb: 2 }}
            slotProps={{
              textField: {
                sx: {
                  input: { color: "white" },
                  label: { color: "white" },
                  "& .MuiOutlinedInput-root": {
                    "& fieldset": { borderColor: "white" },
                  },
                },
              },
            }}
          />

          <DatePicker
            label="Return Date"
            value={returnDate}
            onChange={(v) => v && setReturnDate(v)}
            sx={{ width: "100%", mb: 2 }}
            slotProps={{
              textField: {
                sx: {
                  input: { color: "white" },
                  label: { color: "white" },
                  "& .MuiOutlinedInput-root": {
                    "& fieldset": { borderColor: "white" },
                  },
                },
              },
            }}
          />
        </LocalizationProvider>

        <TextField
          select
          label="Transmission"
          value={transmission}
          onChange={(e) => setTransmission(e.target.value)}
          fullWidth
          sx={{ mb: 3 }}
        >
          <MenuItem value="">Any</MenuItem>
          <MenuItem value="Manual">Manual</MenuItem>
          <MenuItem value="Automatic">Automatic</MenuItem>
        </TextField>

        <Button variant="contained" fullWidth onClick={handleSearch}>
          Search Cars
        </Button>
      </Paper>
    </Box>
  );
}
