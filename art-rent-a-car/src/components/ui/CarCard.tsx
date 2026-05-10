import { Card, CardContent, Typography, Button, Box, Chip } from "@mui/material";
import { useNavigate } from "react-router-dom";

const CarCard = ({ car }: any) => {
  const navigate = useNavigate();
  const params = new URLSearchParams(window.location.search);
  const pickup = params.get("pickup");
  const dropoff = params.get("return");

  return (
    <Card
      sx={{
        borderRadius: 4,
        cursor: "pointer",
        transition: "0.3s",
        height: "100%",
        display: "flex",
        flexDirection: "column",
        "&:hover": {
          transform: "translateY(-6px)",
          boxShadow: "0 16px 40px rgba(0,0,0,0.2)",
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
          bgcolor: "grey.100",
          borderTopLeftRadius: 16,
          borderTopRightRadius: 16,
          overflow: "hidden",
          p: 2,
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

          <Typography variant="h5" fontWeight={700} color="primary">
            €{car.pricePerDay} <Typography component="span" variant="body2" color="text.secondary">/ day</Typography>
          </Typography>
        </Box>

        {/* BUTTONS */}
        <Box sx={{ mt: 3, display: "flex", gap: 1.5 }}>
          <Button
            fullWidth
            variant="contained"
            size="large"
            onClick={() => navigate(`/fleet/${car.slug}`)}
          >
            View Details
          </Button>

          <Button
            fullWidth
            variant="outlined"
            size="large"
            onClick={() => {
              const days =
                pickup && dropoff
                  ? Math.round(
                      (new Date(dropoff).getTime() - new Date(pickup).getTime()) /
                        (1000 * 60 * 60 * 24),
                    )
                  : 0;
              const total = days > 0 ? days * car.pricePerDay : null;
              const msg = pickup && dropoff && total
                ? `New Reservation\n\nCar: ${car.name}\nPickup: ${pickup}\nReturn: ${dropoff}\nDays: ${days}\nTotal: €${total}`
                : `Hello, I'm interested in renting the ${car.name}`;
              window.open(
                "https://wa.me/+38975400462?text=" + encodeURIComponent(msg),
                "_blank",
              );
            }}
          >
            WhatsApp
          </Button>
        </Box>
      </CardContent>
    </Card>
  );
};

export default CarCard;
