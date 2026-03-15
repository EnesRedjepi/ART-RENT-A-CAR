import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Navbar from "./components/layout/Navbar";

import Home from "./pages/Home";
import Fleet from "./pages/Fleet";
import CarDetails from "./pages/CarDetails";
import FloatingContact from "./components/FloatingContact";

function App() {
  return (
    <Router>
      <Navbar />

      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/fleet" element={<Fleet />} />
        <Route path="/fleet/:slug" element={<CarDetails />} />
      </Routes>
      <FloatingContact />
    </Router>
  );
}

export default App;
