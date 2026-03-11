import { useParams } from "react-router-dom";
import { cars } from "../data/cars";
import { Container, Grid, Typography, Box, Button } from "@mui/material";

export default function CarDetails() {
  const { slug } = useParams();
  const car = cars.find((c) => c.slug === slug);
  const [days, setDays] = useState(1);

  if (!car) return null;

  return (
    <Container sx={{ py: 10 }}>
      <Grid container spacing={6}>
        <Grid item md={6}>
          <Box
            component="img"
            src={car.image}
            sx={{
              width: "100%",
              borderRadius: 3,
            }}
          />
        </Grid>

        <Grid item md={6}>
          <Typography variant="h3" fontWeight="bold">
            {car.name}
          </Typography>

          <Box sx={{ mt: 3 }}>
            <Typography>⚙ Transmission: {car.transmission}</Typography>

            <Typography>👤 Seats: {car.seats}</Typography>

            <Typography>⛽ Fuel: {car.fuel}</Typography>

            <Typography>🧳 Luggage: {car.luggage}</Typography>
          </Box>

          <Typography variant="h4" sx={{ mt: 3 }}>
            €{car.price} / day
          </Typography>

          <Typography sx={{ mt: 2 }}>
            Total Price: €{days * car.price}
          </Typography>
          <TextField
            type="number"
            label="Days"
            value={days}
            onChange={(e) => setDays(e.target.value)}
            sx={{ mt: 2 }}
          />

          <Box sx={{ display: "flex", gap: 2, mt: 4 }}>
            <Button
              variant="contained"
              color="success"
              href="https://wa.me/38975400462"
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
