import { TextField, Button, Box } from "@mui/material";

export default function BookingForm() {
  return (
    <Box sx={{ mt: 4 }}>
      <TextField
        label="Pick-up Date"
        type="date"
        fullWidth
        sx={{ mb: 2 }}
        slotProps={{ inputLabel: { shrink: true } }}
      />

      <TextField
        label="Return Date"
        type="date"
        fullWidth
        sx={{ mb: 2 }}
        slotProps={{ inputLabel: { shrink: true } }}
      />

      <Button variant="contained" fullWidth>
        Check Availability
      </Button>
    </Box>
  );
}
