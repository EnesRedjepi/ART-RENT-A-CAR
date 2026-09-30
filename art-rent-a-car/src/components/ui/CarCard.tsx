import {
  Card,
  CardContent,
  Typography,
  Button,
  Box,
  Chip,
} from "@mui/material";
import { useNavigate } from "react-router-dom";
import type { Car } from "../../types/Car";

const CarCard = ({ car }: { car: Car }) => {
  const navigate = useNavigate();

  return (
    <Card
      sx={{
        height: "100%",
        display: "flex",
        flexDirection: "column",
        overflow: "hidden",
        transition: "transform 180ms ease, box-shadow 180ms ease",
        "&:hover": {
          transform: "translateY(-3px)",
          boxShadow: "0 12px 28px rgba(0,0,0,0.1)",
        },
      }}
    >
      {/* IMAGE */}
      <Box
        sx={{
          height: 220,
          display: "flex",
          alignItems: "center",
          justifyContent: "center",
          bgcolor: "#ffffff",
          overflow: "hidden",
          p: 2.5,
        }}
      >
        <img
          src={car.image}
          alt={car.name}
          style={{
            maxHeight: "180px",
            width: "100%",
            objectFit: "contain",
            mixBlendMode: "multiply",
          }}
        />
      </Box>

      {/* CONTENT */}
      <CardContent
        sx={{
          flexGrow: 1,
          display: "flex",
          flexDirection: "column",
          justifyContent: "space-between",
          p: 3,
        }}
      >
        <Box>
          <Typography variant="h5" fontWeight={700} mb={0.5}>
            {car.name}
          </Typography>

          <Typography variant="body2" color="text.secondary" mb={2}>
            {car.year} • {car.fuel}
          </Typography>

          {/* CHIPS */}
          <Box sx={{ display: "flex", flexWrap: "wrap", gap: 1, mb: 2 }}>
            <Chip label={car.transmission} size="small" />
            <Chip label={`${car.seats} seats`} size="small" />
            <Chip label={`${car.luggage} bags`} size="small" />
            {car.airConditioning && <Chip label="A/C" size="small" />}
          </Box>

          <Box sx={{ display: "flex", alignItems: "baseline", gap: 0.75 }}>
            <Typography variant="h5" fontWeight={700} color="primary">
              €{car.pricePerDay}
            </Typography>
            <Typography variant="body2" color="text.secondary">
              / day
            </Typography>
          </Box>
        </Box>

        <Box sx={{ mt: 3 }}>
          <Button
            fullWidth
            variant="contained"
            size="medium"
            onClick={() => navigate(`/fleet/${car.slug}`)}
          >
            View details and reserve
          </Button>
        </Box>
      </CardContent>
    </Card>
  );
};

export default CarCard;
