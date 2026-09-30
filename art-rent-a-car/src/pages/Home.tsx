import {
  Box,
  Container,
  Typography,
  Button,
  Stack,
  Paper,
  TextField,
  MenuItem,
} from "@mui/material";
import { motion } from "framer-motion";
import { Link, useNavigate } from "react-router-dom";
import FeaturedCars from "../components/FeaturedCars";

import { LocalizationProvider } from "@mui/x-date-pickers/LocalizationProvider";
import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
import { DatePicker } from "@mui/x-date-pickers/DatePicker";

import dayjs from "dayjs";
import { useState } from "react";

const Home = () => {
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
    <Box>
      {/* HERO SECTION */}
      <Box
        sx={{
          minHeight: { xs: "auto", md: "min(780px, calc(100vh - 64px))" },
          backgroundImage: "url('/images/hero.jpg')",
          backgroundSize: "cover",
          backgroundPosition: "center",
          position: "relative",
          display: "flex",
          flexDirection: { xs: "column-reverse", md: "row" },
          alignItems: "center",
          justifyContent: "center",
          gap: { xs: 3, md: 0 },
          color: "#fff",
        }}
      >
        {/* Dark Overlay */}
        <Box
          sx={{
            position: "absolute",
            inset: 0,
            background:
              "linear-gradient(90deg, rgba(9,17,15,0.86), rgba(9,17,15,0.5))",
          }}
        />

        {/* RIGHT BOOKING CARD — fixed to right edge, vertically centered */}
        <Paper
          elevation={0}
          sx={{
            position: { xs: "relative", md: "absolute" },
            right: { xs: "auto", md: 0 },
            top: { xs: "auto", md: "50%" },
            transform: { xs: "none", md: "translateY(-50%)" },
            zIndex: 3,
            p: { xs: 2.5, sm: 3 },
            width: { xs: "calc(100% - 32px)", md: 320 },
            maxWidth: 440,
            background: "rgba(15,24,21,0.88)",
            backdropFilter: "blur(14px)",
            border: "1px solid rgba(255,255,255,0.18)",
            borderRadius: 2,
            color: "white",
            mx: { xs: "auto", md: 0 },
          }}
        >
          <Typography variant="h6" sx={{ mb: 2 }}>
            Book Your Car
          </Typography>

          <LocalizationProvider dateAdapter={AdapterDayjs}>
            <DatePicker
              label="Pickup Date"
              value={pickupDate}
              onChange={(newValue) => {
                if (newValue) setPickupDate(newValue);
              }}
              slotProps={{
                textField: {
                  fullWidth: true,
                  size: "small",
                  sx: {
                    mb: 2,
                    input: { color: "white" },
                    label: { color: "rgba(255,255,255,0.7)" },
                    "& .MuiOutlinedInput-root fieldset": {
                      borderColor: "rgba(255,255,255,0.3)",
                    },
                    "& .MuiSvgIcon-root": { color: "white" },
                  },
                },
              }}
            />
            <DatePicker
              label="Return Date"
              value={returnDate}
              onChange={(newValue) => {
                if (newValue) setReturnDate(newValue);
              }}
              slotProps={{
                textField: {
                  fullWidth: true,
                  size: "small",
                  sx: {
                    mb: 2,
                    input: { color: "white" },
                    label: { color: "rgba(255,255,255,0.7)" },
                    "& .MuiOutlinedInput-root fieldset": {
                      borderColor: "rgba(255,255,255,0.3)",
                    },
                    "& .MuiSvgIcon-root": { color: "white" },
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
            size="small"
            sx={{
              mb: 2,
              label: { color: "rgba(255,255,255,0.7)" },
              "& .MuiOutlinedInput-root": { color: "white" },
              "& .MuiOutlinedInput-root fieldset": {
                borderColor: "rgba(255,255,255,0.3)",
              },
              "& .MuiSvgIcon-root": { color: "white" },
            }}
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

        <Container
          sx={{
            position: "relative",
            zIndex: 2,
            display: "flex",
            alignItems: "center",
          }}
        >
          {/* LEFT SIDE TEXT */}
          <motion.div
            initial={{ opacity: 0, y: 40 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.8 }}
          >
            <Box sx={{ maxWidth: 650 }}>
              <Typography
                variant="h1"
                sx={{
                  fontSize: { xs: "2.5rem", sm: "3.25rem", md: "4rem" },
                  lineHeight: 1.05,
                  maxWidth: 600,
                }}
              >
                Your journey starts with the right car.
              </Typography>

              <Typography variant="h6" sx={{ mt: 3 }}>
                Fast airport delivery. No hidden fees. 24/7 support.
              </Typography>

              <Stack
                direction="row"
                spacing={1.5}
                sx={{ mt: 4, flexWrap: "wrap" }}
              >
                <Button
                  variant="contained"
                  color="secondary"
                  size="large"
                  component={Link}
                  to="/fleet"
                >
                  View Fleet
                </Button>

                <Button
                  variant="outlined"
                  size="large"
                  sx={{
                    borderColor: "#fff",
                    color: "#fff",
                    "&:hover": { borderColor: "#C9A227" },
                  }}
                  href="tel:+38975400462"
                >
                  Call Now
                </Button>
              </Stack>
            </Box>
          </motion.div>
        </Container>
      </Box>

      {/* FEATURED CARS */}
      <FeaturedCars />

      {/* WHY CHOOSE US */}
      <Container sx={{ py: 12 }}>
        <Typography variant="h2" textAlign="center">
          Why Choose ART RENT A CAR?
        </Typography>

        <Box
          sx={{
            display: "grid",
            gridTemplateColumns: { xs: "1fr", md: "repeat(3, 1fr)" },
            gap: 6,
            mt: 8,
          }}
        >
          <Box>
            <Typography variant="h5">✈ Airport Delivery</Typography>
            <Typography sx={{ mt: 2 }}>
              We deliver directly to Skopje International Airport.
            </Typography>
          </Box>

          <Box>
            <Typography variant="h5">💰 No Hidden Fees</Typography>
            <Typography sx={{ mt: 2 }}>
              Transparent pricing. What you see is what you pay.
            </Typography>
          </Box>

          <Box>
            <Typography variant="h5">🕒 24/7 Support</Typography>
            <Typography sx={{ mt: 2 }}>
              Always available for tourists and business travelers.
            </Typography>
          </Box>
        </Box>
      </Container>

      {/* SEO SECTION */}
      <Box sx={{ bgcolor: "#111", color: "#fff", py: 12 }}>
        <Container>
          <Typography variant="h2">
            Rent a Car Skopje Airport – Fast & Reliable
          </Typography>

          <Typography sx={{ mt: 4, maxWidth: 800 }}>
            Looking for a reliable rent a car in Skopje Airport? ART RENT A CAR
            provides premium vehicles with fast airport delivery, flexible
            rental periods and competitive pricing.
          </Typography>
        </Container>
      </Box>
    </Box>
  );
};

export default Home;
