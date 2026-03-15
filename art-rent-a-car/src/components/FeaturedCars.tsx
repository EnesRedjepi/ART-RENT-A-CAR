import { Container, Typography, Grid, Button } from "@mui/material";
import { cars } from "../data/cars";
import CarCard from "../components/ui/CarCard";
import ReservationModal from "./ReservationModal";
import { useState } from "react";

export default function FeaturedCars() {
  return (
    <Container sx={{ py: 8 }}>
      <Typography variant="h4" align="center" gutterBottom>
        Featured Cars
      </Typography>

      <Grid container spacing={4}>
        {cars.slice(0, 4).map((car) => (
          <Grid item xs={12} sm={6} md={4} lg={3} key={car.id}>
            <CarCard car={car} />
          </Grid>
        ))}
      </Grid>
    </Container>
  );
}
