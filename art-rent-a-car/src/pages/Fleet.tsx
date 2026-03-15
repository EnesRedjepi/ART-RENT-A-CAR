import { useState } from "react";
import {
  Container,
  Typography,
  Grid,
  Box,
  TextField,
  MenuItem,
} from "@mui/material";

import { useSearchParams } from "react-router-dom";

import { cars } from "../data/cars";
import CarCard from "../components/ui/CarCard";

export default function Fleet() {
  const [searchParams] = useSearchParams();

  const transmissionParam = searchParams.get("transmission") || "";

  const [transmission, setTransmission] = useState(transmissionParam);
  const [fuel, setFuel] = useState("");

  const filteredCars = cars.filter((car) => {
    return (
      (transmission === "" || car.transmission === transmission) &&
      (fuel === "" || car.fuel === fuel)
    );
  });

  return (
    <Container sx={{ py: 8 }}>
      <Typography variant="h3" align="center" gutterBottom>
        Our Fleet
      </Typography>

      <Box
        sx={{
          display: "flex",
          gap: 2,
          mb: 4,
          flexWrap: "wrap",
          justifyContent: "center",
        }}
      >
        <TextField
          select
          label="Transmission"
          value={transmission}
          onChange={(e) => setTransmission(e.target.value)}
          sx={{ minWidth: 180 }}
        >
          <MenuItem value="">All</MenuItem>
          <MenuItem value="Manual">Manual</MenuItem>
          <MenuItem value="Automatic">Automatic</MenuItem>
        </TextField>

        <TextField
          select
          label="Fuel"
          value={fuel}
          onChange={(e) => setFuel(e.target.value)}
          sx={{ minWidth: 180 }}
        >
          <MenuItem value="">All</MenuItem>
          <MenuItem value="Petrol">Petrol</MenuItem>
          <MenuItem value="Diesel">Diesel</MenuItem>
          <MenuItem value="Hybrid">Hybrid</MenuItem>
        </TextField>
      </Box>

      <Grid container spacing={4}>
        {filteredCars.map((car) => (
          <Grid item xs={12} sm={6} md={4} lg={3} key={car.id}>
            <CarCard car={car} />
          </Grid>
        ))}
      </Grid>
    </Container>
  );
}
