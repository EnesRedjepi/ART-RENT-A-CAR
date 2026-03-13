import {
  Container,
  Typography,
  Card,
  CardMedia,
  CardContent,
  Button,
  Select,
  MenuItem,
  Box,
} from "@mui/material";
import { useState } from "react";
import { Link } from "react-router-dom";
import { cars } from "../data/cars";

export default function Fleet() {
  const [transmission, setTransmission] = useState("all");
  const filteredCars = cars.filter((car) => {
    if (transmission === "all") return true;
    return car.transmission === transmission;
  });

  return (
    <Container sx={{ py: 10 }}>
      <Typography variant="h3" textAlign="center" fontWeight="bold" mb={6}>
        Our Fleet
      </Typography>

      <Box sx={{ mb: 4, display: "flex", justifyContent: "center" }}>
        <Select
          value={transmission}
          onChange={(e) => setTransmission(e.target.value)}
        >
          <MenuItem value="all">All</MenuItem>
          <MenuItem value="Manual">Manual</MenuItem>
          <MenuItem value="Automatic">Automatic</MenuItem>
        </Select>
      </Box>

      <Grid container spacing={4}>
        {filteredCars.map((car) => (
          <Grid item xs={12} sm={6} md={4} key={car.id}>
            <Card sx={{ borderRadius: 3 }}>
              <CardMedia
                component="img"
                height="200"
                image={car.image}
                alt={car.name}
              />

              <CardContent>
                <Typography variant="h6" fontWeight="bold">
                  {car.name}
                </Typography>

                <Typography color="text.secondary">
                  {car.transmission} • {car.seats} seats
                </Typography>

                <Typography variant="h6" sx={{ mt: 1 }}>
                  €{car.price} / day
                </Typography>

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
