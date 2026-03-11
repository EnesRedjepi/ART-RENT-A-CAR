import { Box, Container, Typography } from "@mui/material";

const Footer = () => {
  return (
    <Box sx={{ bgcolor: "#111", color: "#fff", py: 4, mt: 8 }}>
      <Container>
        <Typography variant="h6">ART RENT A CAR</Typography>

        <Typography variant="body2" sx={{ mt: 1 }}>
          Premium Rent a Car in Skopje Airport
        </Typography>

        <Typography variant="body2" sx={{ mt: 1 }}>
          © {new Date().getFullYear()} All rights reserved
        </Typography>
      </Container>
    </Box>
  );
};

export default Footer;
