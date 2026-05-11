import { Routes, Route } from "react-router-dom";
import Home from "../pages/Home";
import Fleet from "../pages/Fleet";
import CarDetails from "../pages/CarDetails";
import AdminBookings from "../pages/AdminBookings";
import Layout from "../components/Layout";

const AppRoutes = () => {
  return (
    <Layout>
      <Routes>
        <Route path="/" element={<Fleet />} />
        <Route path="/car/:slug" element={<CarDetails />} />
        <Route path="/admin/bookings" element={<AdminBookings />} />
      </Routes>
    </Layout>
  );
};

export default AppRoutes;
