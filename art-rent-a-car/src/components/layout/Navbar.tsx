import {
  AppBar,
  Toolbar,
  Typography,
  IconButton,
  Box,
  Button,
} from "@mui/material";
import { Link } from "react-router-dom";
import { useContext } from "react";
import { ThemeContext } from "../../context/theme-context";
import LightModeIcon from "@mui/icons-material/LightMode";
import DarkModeIcon from "@mui/icons-material/DarkMode";

const Navbar = () => {
  const { toggleTheme, mode } = useContext(ThemeContext);

  return (
    <AppBar
      position="sticky"
      elevation={0}
      sx={{
        backdropFilter: "blur(14px)",
        background: "rgba(0,0,0,0.6)",
        borderBottom: "1px solid rgba(255,255,255,0.08)",
      }}
    >
      <Toolbar
        sx={{
          display: "flex",
          justifyContent: "space-between",
          gap: 1,
          px: { xs: 1.5, sm: 3 },
          minHeight: { xs: 60, sm: 64 },
        }}
      >
        <Typography
          variant="h6"
          component={Link}
          to="/"
          sx={{
            textDecoration: "none",
            color: "white",
            fontWeight: 600,
            letterSpacing: { xs: 0.3, sm: 1 },
            fontSize: { xs: "0.78rem", sm: "1.25rem" },
            whiteSpace: "nowrap",
            flexShrink: 0,
          }}
        >
          ART RENT A CAR
        </Typography>

        <Box
          sx={{
            display: "flex",
            gap: { xs: 0.25, sm: 1 },
            alignItems: "center",
          }}
        >
          <Button
            component={Link}
            to="/fleet"
            color="inherit"
            sx={{ textTransform: "none", minWidth: 0, px: { xs: 1, sm: 2 } }}
          >
            Fleet
          </Button>

          <Button
            component={Link}
            to="/admin/login"
            color="inherit"
            sx={{
              textTransform: "none",
              display: { xs: "none", sm: "inline-flex" },
            }}
          >
            Admin
          </Button>

          <IconButton color="inherit" onClick={toggleTheme} sx={{ p: 1 }}>
            {mode === "dark" ? <LightModeIcon /> : <DarkModeIcon />}
          </IconButton>
        </Box>
      </Toolbar>
    </AppBar>
  );
};

export default Navbar;
