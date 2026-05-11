import { useState } from "react";
import { Box, Button, Container, Paper, TextField, Typography, Alert } from "@mui/material";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";
import LockOutlinedIcon from "@mui/icons-material/LockOutlined";

const AdminLogin = () => {
  const { login } = useAuth();
  const navigate = useNavigate();
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState(false);

  const handleLogin = () => {
    const success = login(username, password);
    if (success) {
      navigate("/admin");
    } else {
      setError(true);
    }
  };

  return (
    <Box sx={{ minHeight: "100vh", display: "flex", alignItems: "center", justifyContent: "center" }}>
      <Container maxWidth="xs">
        <Paper sx={{ p: 5, borderRadius: 4, textAlign: "center" }}>
          <Box
            sx={{
              width: 56, height: 56, borderRadius: "50%",
              bgcolor: "primary.main", display: "flex",
              alignItems: "center", justifyContent: "center",
              mx: "auto", mb: 2,
            }}
          >
            <LockOutlinedIcon sx={{ color: "#fff" }} />
          </Box>

          <Typography variant="h5" fontWeight={700} mb={1}>Admin Login</Typography>
          <Typography variant="body2" color="text.secondary" mb={3}>
            ART Rent a Car — Admin Panel
          </Typography>

          {error && (
            <Alert severity="error" sx={{ mb: 2 }}>
              Incorrect username or password.
            </Alert>
          )}

          <TextField
            fullWidth
            label="Username"
            value={username}
            onChange={(e) => { setUsername(e.target.value); setError(false); }}
            onKeyDown={(e) => e.key === "Enter" && handleLogin()}
            sx={{ mb: 2 }}
          />

          <TextField
            fullWidth
            label="Password"
            type="password"
            value={password}
            onChange={(e) => { setPassword(e.target.value); setError(false); }}
            onKeyDown={(e) => e.key === "Enter" && handleLogin()}
            sx={{ mb: 3 }}
          />

          <Button variant="contained" fullWidth size="large" onClick={handleLogin}>
            Login
          </Button>
        </Paper>
      </Container>
    </Box>
  );
};

export default AdminLogin;
