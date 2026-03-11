import { Box, Container, Typography, Button, Stack } from "@mui/material";
import { motion } from "framer-motion";
import { Link } from "react-router-dom";
import FeaturedCars from "../components/FeaturedCars";

const Home = () => {
  return (
    <Box>
      {/* HERO SECTION */}
      <Box
        sx={{
          height: "90vh",
          backgroundImage: "url('images/hero.jpg')",
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

        <Container sx={{ position: "relative", zIndex: 2 }}>
          <motion.div
            initial={{ opacity: 0, y: 40 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.8 }}
          >
            <Typography variant="h1" sx={{ maxWidth: 700 }}>
              Premium Rent a Car Skopje Airport
            </Typography>

            <Typography variant="h6" sx={{ mt: 3, maxWidth: 600 }}>
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
          </motion.div>
        </Container>
      </Box>

      <FeaturedCars />

      {/* BELOW HERO CONTENT */}
      <Container sx={{ py: 10 }}>
        <Typography variant="h2">Why Choose ART RENT A CAR?</Typography>

        <Typography sx={{ mt: 3 }}>
          Airport focused. Tourist friendly. Transparent pricing.
        </Typography>
      </Container>
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
              We deliver directly to Skopje International Airport. No waiting.
              No stress.
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
      {/* SEO AIRPORT SECTION */}
      <Box sx={{ bgcolor: "#111", color: "#fff", py: 12 }}>
        <Container>
          <Typography variant="h2">
            Rent a Car Skopje Airport – Fast & Reliable
          </Typography>

          <Typography sx={{ mt: 4, maxWidth: 800 }}>
            Looking for a reliable rent a car in Skopje Airport? ART RENT A CAR
            provides premium vehicles with fast airport delivery, flexible
            rental periods and competitive pricing. Perfect for tourists
            visiting Macedonia and business travelers.
          </Typography>
        </Container>
      </Box>
      {/* GOOGLE MAP */}
      <Container sx={{ py: 12 }}>
        <Typography variant="h2" textAlign="center">
          Find Us
        </Typography>

        <Box sx={{ mt: 6 }}>
          <iframe
            title="Skopje Airport Location"
            width="100%"
            height="400"
            style={{ border: 0 }}
            loading="lazy"
            allowFullScreen
            src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d2965.207912281876!2d21.621381!3d41.961745"
          />
        </Box>
      </Container>
    </Box>
  );
};

export default Home;
