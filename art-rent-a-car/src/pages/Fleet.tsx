import { Container, Grid, Typography } from "@mui/material";
import { useSearchParams } from "react-router-dom";
import { cars } from "../data/cars";
import CarCard from "../components/ui/CarCard";

const Fleet = () => {
  const [searchParams] = useSearchParams();

  const transmission = searchParams.get("transmission");
  const pickup = searchParams.get("pickup");
  const dropoff = searchParams.get("return");

  // FILTER LOGIC
  const filteredCars = cars.filter((car) => {
    if (!transmission || transmission === "") return true;
    return car.transmission === transmission;
  });

  return (
    <Container sx={{ py: 8 }}>
      <Typography variant="h3" mb={4}>
        Available Cars
      </Typography>

      {/* SHOW SELECTED DATES */}
      {pickup && dropoff && (
        <Typography sx={{ mb: 4 }}>
          Rental from <strong>{pickup}</strong> to <strong>{dropoff}</strong>
        </Typography>
      )}

      <Grid container spacing={4}>
        {filteredCars.map((car) => (
          <Grid item xs={12} sm={6} md={4} lg={3} key={car.id}>
            <CarCard car={car} />
          </Grid>
        ))}
      </Grid>

      {/* NO RESULTS */}
      {filteredCars.length === 0 && (
        <Typography mt={4}>No cars found.</Typography>
      )}
    </Container>
  );
};

export default Fleet;
