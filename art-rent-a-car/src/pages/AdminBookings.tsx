import { Container, Typography } from "@mui/material";

export default function AdminBookings() {
  return (
    <Container sx={{ py: 10 }}>
      <Typography variant="h3">Admin Bookings</Typography>

      <Typography sx={{ mt: 4 }}>Booking list will appear here.</Typography>
    </Container>
  );
}
