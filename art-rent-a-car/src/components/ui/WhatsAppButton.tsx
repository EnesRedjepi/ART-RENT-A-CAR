import { Fab } from "@mui/material";
import WhatsAppIcon from "@mui/icons-material/WhatsApp";

const WhatsAppButton = () => {
  const phoneNumber = "38975400462"; // change later

  return (
    <Fab
      color="success"
      href={`https://wa.me/${phoneNumber}`}
      target="_blank"
      sx={{
        position: "fixed",
        bottom: 20,
        right: 20,
      }}
    >
      <WhatsAppIcon />
    </Fab>
  );
};

export default WhatsAppButton;
