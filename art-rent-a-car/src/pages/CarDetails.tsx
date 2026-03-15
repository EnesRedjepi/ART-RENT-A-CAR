import { useParams } from "react-router-dom";
import { cars } from "../data/cars";
import { Container, Typography, Grid, Box, Button } from "@mui/material";

export default function CarDetails() {
  const { slug } = useParams();

  const car = cars.find((c) => c.slug === slug);

  if (!car) {
    return (
      <Container sx={{ py: 10 }}>
        <Typography variant="h4">Car not found</Typography>
      </Container>
    );
  }

  return (
    <Container sx={{ py: 8 }}>
      <Grid container spacing={6}>
        <Grid item md={6}>
          <Box
            component="img"
            src={car.image}
            alt={car.name}
            sx={{
              width: "100%",
              borderRadius: 2,
              background: "#f5f5f5",
              p: 3,
            }}
          />
        </Grid>

        <Grid item md={6}>
          <Typography variant="h3">{car.name}</Typography>

          <Typography sx={{ mt: 2 }}>{car.description}</Typography>

          <Box sx={{ mt: 3 }}>
            <Typography>⚙ Transmission: {car.transmission}</Typography>
            <Typography>⛽ Fuel: {car.fuel}</Typography>
            <Typography>👤 Seats: {car.seats}</Typography>
            <Typography>🧳 Luggage: {car.luggage}</Typography>
            <Typography>
              ❄ Air Conditioning: {car.airConditioning ? "Yes" : "No"}
            </Typography>
            <Typography>📅 Year: {car.year}</Typography>
          </Box>

          <Typography variant="h4" sx={{ mt: 3 }}>
            €{car.pricePerDay} / day
          </Typography>

          <Box sx={{ display: "flex", gap: 2, mt: 4 }}>
            <Button
              variant="contained"
              color="success"
              href="https://wa.me/38975400462"
              target="_blank"
            >
              Reserve via WhatsApp
            </Button>

            <Button
              variant="contained"
              color="secondary"
              href="viber://chat?number=%2B38975400462"
            >
              Reserve via Viber
            </Button>
          </Box>
        </Grid>
      </Grid>
    </Container>
  );
}
