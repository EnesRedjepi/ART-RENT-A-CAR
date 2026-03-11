import { Box, Typography, Container, TextField, Button } from "@mui/material";

export default function HeroSection() {
  return (
    <Box
      sx={{
        height: "90vh",
        backgroundImage: "url('/images/hero.jpg')",
        backgroundSize: "cover",
        backgroundPosition: "center",
        display: "flex",
        alignItems: "center",
        position: "relative",
      }}
    >
      <Box
        sx={{
          position: "absolute",
          inset: 0,
          background: "rgba(0,0,0,0.6)",
        }}
      />

      <Container sx={{ position: "relative", color: "#fff" }}>
        <Typography variant="h2" fontWeight="bold">
          Rent a Car Skopje Airport
        </Typography>

        <Typography sx={{ mt: 2 }}>
          Airport delivery • No hidden fees • 24/7 support
        </Typography>

        <Box
          sx={{
            mt: 6,
            p: 3,
            borderRadius: 3,
            background: "#fff",
            display: "grid",
            gridTemplateColumns: {
              xs: "1fr",
              md: "repeat(4,1fr)",
            },
            gap: 2,
          }}
        >
          <TextField
            label="Pick-up location"
            defaultValue="Skopje Airport"
            fullWidth
          />

          <TextField
            label="Pick-up date"
            type="date"
            slotProps={{ inputLabel: { shrink: true } }}
            fullWidth
          />

          <TextField
            label="Return date"
            type="date"
            slotProps={{ inputLabel: { shrink: true } }}
            fullWidth
          />

          <Button variant="contained" size="large">
            Search Cars
          </Button>
        </Box>
      </Container>
    </Box>
  );
}
