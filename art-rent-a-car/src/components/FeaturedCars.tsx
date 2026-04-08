import { Grid, Container, Typography } from "@mui/material";
import CarCard from "../components/ui/CarCard";
import { cars } from "../data/cars";

const FeaturedCars = () => {
  return (
    <Container sx={{ py: 8 }}>
      <Typography variant="h4" sx={{ mb: 4 }}>
        Featured Cars
      </Typography>

      <Grid container spacing={4}>
        {cars.map((car) => (
          <Grid item xs={12} sm={6} md={3} key={car.id}>
            <CarCard car={car} />
          </Grid>
        ))}
      </Grid>
    </Container>
  );
};

export default FeaturedCars;
