import {
  Box,
  Button,
  Chip,
  Container,
  Divider,
  Grid,
  IconButton,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Typography,
} from "@mui/material";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";
import { cars } from "../../data/cars";
import DeleteOutlineIcon from "@mui/icons-material/DeleteOutline";
import LogoutIcon from "@mui/icons-material/Logout";
import DirectionsCarIcon from "@mui/icons-material/DirectionsCar";
import EventAvailableIcon from "@mui/icons-material/EventAvailable";
import EuroIcon from "@mui/icons-material/Euro";
import PendingActionsIcon from "@mui/icons-material/PendingActions";
import RefreshIcon from "@mui/icons-material/Refresh";

interface Booking {
  carId: number;
  carName: string;
  customerName: string;
  customerPhone: string;
  pickupDate: string;
  returnDate: string;
  totalPrice: number;
  days?: number;
  createdAt?: string;
}

const StatCard = ({
  icon,
  label,
  value,
  color,
}: {
  icon: React.ReactNode;
  label: string;
  value: string | number;
  color: string;
}) => (
  <Paper sx={{ p: 3, borderRadius: 4, display: "flex", alignItems: "center", gap: 2 }}>
    <Box
      sx={{
        width: 52,
        height: 52,
        borderRadius: 3,
        bgcolor: color,
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
        flexShrink: 0,
      }}
    >
      {icon}
    </Box>
    <Box>
      <Typography variant="body2" color="text.secondary">{label}</Typography>
      <Typography variant="h5" fontWeight={700}>{value}</Typography>
    </Box>
  </Paper>
);

const AdminDashboard = () => {
  const { logout } = useAuth();
  const navigate = useNavigate();
  const [bookings, setBookings] = useState<Booking[]>([]);

  const loadBookings = () => {
    const stored = JSON.parse(localStorage.getItem("bookings") || "[]");
    setBookings(stored);
  };

  useEffect(() => {
    loadBookings();
    window.addEventListener("focus", loadBookings);
    return () => window.removeEventListener("focus", loadBookings);
  }, []);

  const deleteBooking = (index: number) => {
    const updated = bookings.filter((_, i) => i !== index);
    localStorage.setItem("bookings", JSON.stringify(updated));
    setBookings(updated);
  };

  const handleLogout = () => {
    logout();
    navigate("/admin/login");
  };

  const totalRevenue = bookings.reduce((sum, b) => sum + b.totalPrice, 0);
  const pendingCount = bookings.filter((b) => b.customerName === "Pending").length;
  const availableCars = cars.filter((c) => c.available).length;

  const getStatus = (booking: Booking) => {
    const today = new Date();
    const pickup = new Date(booking.pickupDate);
    const returnD = new Date(booking.returnDate);
    if (today < pickup) return { label: "Upcoming", color: "info" as const };
    if (today >= pickup && today <= returnD) return { label: "Active", color: "success" as const };
    return { label: "Completed", color: "default" as const };
  };

  return (
    <Box sx={{ minHeight: "100vh", bgcolor: "background.default" }}>
      {/* HEADER */}
      <Box sx={{ bgcolor: "background.paper", borderBottom: "1px solid", borderColor: "divider", px: 4, py: 2, display: "flex", alignItems: "center", justifyContent: "space-between" }}>
        <Typography variant="h6" fontWeight={700}>🚗 ART Rent a Car — Admin</Typography>
        <Box sx={{ display: "flex", gap: 1 }}>
          <Button startIcon={<RefreshIcon />} onClick={loadBookings} color="inherit">Refresh</Button>
          <Button startIcon={<LogoutIcon />} onClick={handleLogout} color="inherit">Logout</Button>
        </Box>
      </Box>

      <Container sx={{ py: 5 }}>
        {/* STATS */}
        <Typography variant="h4" fontWeight={700} mb={4}>Dashboard</Typography>

        <Grid container spacing={3} mb={5}>
          <Grid size={{ xs: 12, sm: 6, md: 3 }}>
            <StatCard icon={<EventAvailableIcon sx={{ color: "#fff" }} />} label="Total Bookings" value={bookings.length} color="#1976d2" />
          </Grid>
          <Grid size={{ xs: 12, sm: 6, md: 3 }}>
            <StatCard icon={<EuroIcon sx={{ color: "#fff" }} />} label="Total Revenue" value={`€${totalRevenue}`} color="#2e7d32" />
          </Grid>
          <Grid size={{ xs: 12, sm: 6, md: 3 }}>
            <StatCard icon={<DirectionsCarIcon sx={{ color: "#fff" }} />} label="Available Cars" value={availableCars} color="#ed6c02" />
          </Grid>
          <Grid size={{ xs: 12, sm: 6, md: 3 }}>
            <StatCard icon={<PendingActionsIcon sx={{ color: "#fff" }} />} label="Pending Info" value={pendingCount} color="#9c27b0" />
          </Grid>
        </Grid>

        <Divider sx={{ mb: 4 }} />

        {/* BOOKINGS TABLE */}
        <Typography variant="h5" fontWeight={700} mb={3}>Reservations</Typography>

        {bookings.length === 0 ? (
          <Paper sx={{ p: 6, textAlign: "center", borderRadius: 4 }}>
            <Typography color="text.secondary">No reservations yet.</Typography>
          </Paper>
        ) : (
          <TableContainer component={Paper} sx={{ borderRadius: 4 }}>
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell><strong>Car</strong></TableCell>
                  <TableCell><strong>Customer</strong></TableCell>
                  <TableCell><strong>Phone</strong></TableCell>
                  <TableCell><strong>Pickup</strong></TableCell>
                  <TableCell><strong>Return</strong></TableCell>
                  <TableCell><strong>Days</strong></TableCell>
                  <TableCell><strong>Total</strong></TableCell>
                  <TableCell><strong>Status</strong></TableCell>
                  <TableCell><strong>Submitted</strong></TableCell>
                  <TableCell align="center"><strong>Action</strong></TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {bookings.map((booking, index) => {
                  const status = getStatus(booking);
                  return (
                    <TableRow key={index} hover>
                      <TableCell>{booking.carName}</TableCell>
                      <TableCell>{booking.customerName}</TableCell>
                      <TableCell>{booking.customerPhone}</TableCell>
                      <TableCell>{booking.pickupDate}</TableCell>
                      <TableCell>{booking.returnDate}</TableCell>
                      <TableCell>{booking.days ?? "—"}</TableCell>
                      <TableCell>€{booking.totalPrice}</TableCell>
                      <TableCell>
                        <Chip label={status.label} color={status.color} size="small" />
                      </TableCell>
                      <TableCell>{booking.createdAt ? new Date(booking.createdAt).toLocaleString() : "—"}</TableCell>
                      <TableCell align="center">
                        <IconButton color="error" onClick={() => deleteBooking(index)}>
                          <DeleteOutlineIcon />
                        </IconButton>
                      </TableCell>
                    </TableRow>
                  );
                })}
              </TableBody>
            </Table>
          </TableContainer>
        )}

        {/* FLEET OVERVIEW */}
        <Typography variant="h5" fontWeight={700} mt={6} mb={3}>Fleet Overview</Typography>
        <Grid container spacing={3}>
          {cars.map((car) => (
            <Grid size={{ xs: 12, sm: 6, md: 4 }} key={car.id}>
              <Paper sx={{ p: 3, borderRadius: 4, display: "flex", alignItems: "center", gap: 2 }}>
                <Box
                  component="img"
                  src={car.image}
                  alt={car.name}
                  sx={{ width: 80, height: 55, objectFit: "contain", mixBlendMode: "multiply" }}
                />
                <Box sx={{ flexGrow: 1 }}>
                  <Typography fontWeight={600}>{car.name}</Typography>
                  <Typography variant="body2" color="text.secondary">€{car.pricePerDay} / day • {car.transmission}</Typography>
                </Box>
                <Chip
                  label={car.available ? "Available" : "Unavailable"}
                  color={car.available ? "success" : "error"}
                  size="small"
                />
              </Paper>
            </Grid>
          ))}
        </Grid>
      </Container>
    </Box>
  );
};

export default AdminDashboard;
