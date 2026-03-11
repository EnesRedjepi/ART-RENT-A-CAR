import {
  Card,
  CardMedia,
  CardContent,
  Typography,
  Button,
} from "@mui/material";
import { Link } from "react-router-dom";
import type { Car } from "../../types/Car";

interface Props {
  car: Car;
}

const CarCard = ({ car }: Props) => {
  return (
    <Card sx={{ borderRadius: 3 }}>
      <CardMedia
        component="img"
        height="200"
        image={car.image}
        alt={car.name}
      />

      <CardContent>
        <Typography variant="h6">{car.name}</Typography>

        <Typography color="text.secondary">€{car.pricePerDay} / day</Typography>

        <Button
          component={Link}
          to={`/car/${car.id}`}
          variant="contained"
          sx={{ mt: 2 }}
        >
          View Details
        </Button>
      </CardContent>
    </Card>
  );
};

export default CarCard;
