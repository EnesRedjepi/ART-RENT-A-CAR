import { Grid, Container, Typography } from "@mui/material";
import CarCard from "../components/ui/CarCard";
import { cars } from "../data/cars";

const FeaturedCars = () => {
  const featured = cars.slice(0, 3);

  return (
    <Container sx={{ py: 8 }}>
      <Typography variant="h4" sx={{ mb: 4 }}>
        Featured Cars
      </Typography>

      <Grid container spacing={4}>
        {featured.map((car) => (
          <Grid size={{ xs: 12, sm: 6, md: 4 }} key={car.id}>
            <CarCard car={car} />
          </Grid>
        ))}
      </Grid>
    </Container>
  );
};

export default FeaturedCars;
