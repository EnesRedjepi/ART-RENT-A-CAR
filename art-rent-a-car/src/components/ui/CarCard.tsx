import {
  Card,
  CardMedia,
  CardContent,
  Typography,
  Box,
  Button,
  Chip,
} from "@mui/material";
import { Link } from "react-router-dom";

type Car = {
  id: number;
  name: string;
  slug: string;
  image: string;
  pricePerDay: number;
  transmission: string;
  fuel: string;
  seats: number;
  available: boolean;
};

export default function CarCard({ car }: { car: Car }) {
  return (
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
          ⚙ {car.transmission} | ⛽ {car.fuel} | 👤 {car.seats}
        </Box>

        <Typography variant="h6" sx={{ mt: 1 }}>
          €{car.pricePerDay} / day
        </Typography>

        {!car.available && <Typography color="error">Not Available</Typography>}
        <Button disabled={!car.available}>Reserve</Button>

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
  );
}
