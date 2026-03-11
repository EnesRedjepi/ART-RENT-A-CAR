import {
  Container,
  Typography,
  Grid,
  Card,
  CardContent,
  CardMedia,
  Box,
} from "@mui/material";

import { Link } from "react-router-dom";
import { cars } from "../data/cars";

export default function FeaturedCars() {
  return (
    <Container sx={{ py: 10 }}>
      <Typography variant="h3" textAlign="center" fontWeight="bold" mb={6}>
        Featured Cars
      </Typography>

      <Grid container spacing={4} justifyContent="center">
        {cars.map((car) => (
          <Grid key={car.id} xs={12} sm={6} md={4} lg={3}>
            <Card
              component={Link}
              to={`/fleet/${car.slug}`}
              sx={{
                textDecoration: "none",
                borderRadius: 4,
                overflow: "hidden",
                transition: "all 0.3s ease",
                "&:hover": {
                  transform: "translateY(-8px)",
                  boxShadow: 8,
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

              <CardContent>
                <Typography variant="h6" fontWeight="bold">
                  {car.name}
                </Typography>

                <Box
                  sx={{
                    display: "flex",
                    gap: 2,
                    mt: 1,
                    color: "#666",
                    fontSize: 14,
                  }}
                >
                  ⚙ {car.transmission}
                  👤 {car.seats}
                </Box>

                <Typography
                  sx={{
                    mt: 2,
                    fontSize: 20,
                    fontWeight: "bold",
                    color: "#1976d2",
                  }}
                >
                  €{car.price} / day
                </Typography>
              </CardContent>
            </Card>
          </Grid>
        ))}
      </Grid>
    </Container>
  );
}
