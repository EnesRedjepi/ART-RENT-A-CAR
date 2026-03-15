import {
  Container,
  Typography,
  Grid,
  Card,
  CardMedia,
  CardContent,
  Box,
  Button,
} from "@mui/material";
import { Link } from "react-router-dom";
import { cars } from "../data/cars";

export default function FeaturedCars() {
  return (
    <Container sx={{ py: 8 }}>
      <Typography variant="h4" align="center" gutterBottom>
        Featured Cars
      </Typography>

      <Grid container spacing={4}>
        {cars.map((car) => (
          <Grid item xs={12} sm={6} md={4} lg={3} key={car.id}>
            <Card
              sx={{
                height: "100%",
                display: "flex",
                flexDirection: "column",
                transition: "0.3s",
                "&:hover": {
                  transform: "translateY(-6px)",
                  boxShadow: 6,
                },
              }}
            >
              <CardMedia
                component="img"
                image={car.image}
                alt={car.name}
                sx={{
                  height: 180,
                  objectFit: "contain",
                  background: "#f5f5f5",
                  p: 2,
                }}
              />

              <CardContent sx={{ flexGrow: 1 }}>
                <Typography variant="h6">{car.name}</Typography>

                <Box sx={{ fontSize: 14, color: "#777", mt: 1 }}>
                  ⚙ {car.transmission} &nbsp; | &nbsp; ⛽ {car.fuel} &nbsp; |
                  &nbsp; 👤 {car.seats}
                </Box>

                <Typography variant="h6" sx={{ mt: 1 }}>
                  €{car.pricePerDay} / day
                </Typography>

                {!car.available && (
                  <Typography color="error" sx={{ mt: 1 }}>
                    Not Available
                  </Typography>
                )}

                <Button
                  component={Link}
                  to={`/fleet/${car.slug}`}
                  variant="contained"
                  fullWidth
                  sx={{ mt: 2 }}
                >
                  View Details
                </Button>
              </CardContent>
            </Card>
          </Grid>
        ))}
      </Grid>
    </Container>
  );
}
