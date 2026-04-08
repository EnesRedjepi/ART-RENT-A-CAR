import { Card, CardContent, Typography, Button, Box } from "@mui/material";
import { useNavigate } from "react-router-dom";

const CarCard = ({ car }: any) => {
  const navigate = useNavigate();

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
          transform: "translateY(-5px)",
          boxShadow: "0 12px 30px rgba(0,0,0,0.15)",
        },
      }}
    >
      {/* IMAGE */}
      <Box
        sx={{
          height: 160,
          display: "flex",
          alignItems: "center",
          justifyContent: "center",
          bgcolor: "#f5f5f5",
          borderTopLeftRadius: 16,
          borderTopRightRadius: 16,
        }}
      >
        <img
          src={car.image}
          style={{
            maxHeight: "120px",
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
        }}
      >
        <Box>
          <Typography variant="h6">{car.name}</Typography>

          <Typography variant="body2" color="text.secondary">
            {car.transmission} • {car.fuel}
          </Typography>

          <Typography variant="body2" color="text.secondary">
            {car.seats} seats • {car.luggage} bags
          </Typography>

          <Typography sx={{ mt: 2, fontWeight: 600 }}>
            €{car.pricePerDay} / day
          </Typography>
        </Box>

        {/* BUTTONS */}
        <Box
          sx={{
            mt: 2,
            display: "flex",
            gap: 1,
          }}
        >
          <Button
            fullWidth
            variant="contained"
            onClick={() => navigate(`/fleet/${car.slug}`)}
          >
            Details
          </Button>

          <Button
            fullWidth
            variant="outlined"
            onClick={() => {
              const msg = `Hello, I'm interested in ${car.name}`;
              window.open(
                "https://wa.me/38975400462?text=" + encodeURIComponent(msg),
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
