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
          minHeight: "90vh",
          backgroundImage: "url('/images/hero.jpg')",
          backgroundSize: "cover",
          backgroundPosition: "center",
          position: "relative",
          display: "flex",
          alignItems: "center",
          color: "#fff",
        }}
      >
        {/* Dark Overlay */}
        <Box
          sx={{
            position: "absolute",
            inset: 0,
            backgroundColor: "rgba(0,0,0,0.6)",
          }}
        />

        <Container
          sx={{
            position: "relative",
            zIndex: 2,
            display: "flex",
            flexWrap: "wrap",
            justifyContent: "space-between",
            alignItems: "center",
            gap: 6,
          }}
        >
          {/* LEFT SIDE TEXT */}
          <motion.div
            initial={{ opacity: 0, y: 40 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.8 }}
          >
            <Box sx={{ maxWidth: 650 }}>
              <Typography variant="h1">
                Premium Rent a Car Skopje Airport
              </Typography>

              <Typography variant="h6" sx={{ mt: 3 }}>
                Fast airport delivery. No hidden fees. 24/7 support.
              </Typography>

              <Stack direction="row" spacing={3} sx={{ mt: 5 }}>
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

          {/* RIGHT BOOKING CARD */}
          <Paper
            elevation={10}
            sx={{
              p: 4,
              width: 340,
              backdropFilter: "blur(10px)",
              background: "rgba(255,255,255,0.12)",
              border: "1px solid rgba(255,255,255,0.25)",
              color: "white",
            }}
          >
            <Typography variant="h6">Book Your Car</Typography>

            <LocalizationProvider dateAdapter={AdapterDayjs}>
              <DatePicker
                label="Pickup Date"
                value={pickupDate}
                onChange={(newValue) => setPickupDate(newValue)}
                slotProps={{
                  textField: {
                    sx: {
                      input: { color: "white" },
                      label: { color: "white" },
                      "& .MuiOutlinedInput-root": {
                        "& fieldset": {
                          borderColor: "rgba(255,255,255,0.5)",
                        },
                      },
                    },
                  },
                }}
              />

              <DatePicker
                label="Return Date"
                value={returnDate}
                onChange={(newValue) => setReturnDate(newValue)}
                slotProps={{
                  textField: {
                    sx: {
                      input: { color: "white" },
                      label: { color: "white" },
                      "& .MuiOutlinedInput-root": {
                        "& fieldset": {
                          borderColor: "rgba(255,255,255,0.5)",
                        },
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
