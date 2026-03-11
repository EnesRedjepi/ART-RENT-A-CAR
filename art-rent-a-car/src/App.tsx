import { BrowserRouter, Routes, Route } from "react-router-dom";

import Navbar from "./components/Navbar";
import Home from "./pages/Home";
import Fleet from "./pages/Fleet";
import CarDetails from "./pages/CarDetails";

function App() {
  return (
    <BrowserRouter>
      <Navbar />

      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/fleet" element={<Fleet />} />
        <Route path="/fleet/:slug" element={<CarDetails />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
