import { Box, Fab, Tooltip } from "@mui/material";
import WhatsAppIcon from "@mui/icons-material/WhatsApp";
import { FaViber } from "react-icons/fa";
import { useLocation } from "react-router-dom";

export default function FloatingContact() {
  const { pathname } = useLocation();
  const whatsapp = "https://wa.me/38975400462";
  const viber = "viber://chat?number=%2B38975400462";

  if (pathname.startsWith("/fleet/")) return null;

  return (
    <Box
      sx={{
        position: "fixed",
        bottom: 20,
        right: 20,
        display: { xs: "none", sm: "flex" },
        flexDirection: "column",
        gap: 2,
        zIndex: 1000,
      }}
    >
      {/* WhatsApp Button */}

      <Tooltip title="Chat on WhatsApp" placement="left">
        <Fab
          href={whatsapp}
          target="_blank"
          sx={{
            backgroundColor: "#25D366",
            color: "white",
            transition: "transform 0.2s ease",
            "&:hover": {
              backgroundColor: "#1ebe5d",
              transform: "scale(1.1)",
            },
          }}
        >
          <WhatsAppIcon />
        </Fab>
      </Tooltip>

      {/* Viber Button */}

      <Tooltip title="Chat on Viber" placement="left">
        <Fab
          href={viber}
          sx={{
            backgroundColor: "#7360F2",
            color: "white",
            transition: "transform 0.2s ease",
            "&:hover": {
              backgroundColor: "#5b4be0",
              transform: "scale(1.1)",
            },
          }}
        >
          <FaViber size={22} />
        </Fab>
      </Tooltip>
    </Box>
  );
}
