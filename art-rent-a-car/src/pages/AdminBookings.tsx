import { useEffect, useState } from "react";

interface Booking {
  carId: number;
  carName: string;
  customerName: string;
  customerPhone: string;
  pickupDate: string;
  returnDate: string;
  totalPrice: number;
}

const AdminBookings = () => {
  const [bookings, setBookings] = useState<Booking[]>([]);

  useEffect(() => {
    const storedBookings = JSON.parse(localStorage.getItem("bookings") || "[]");
    setBookings(storedBookings);
  }, []);

  const deleteBooking = (index: number) => {
    const updatedBookings = bookings.filter((_, i) => i !== index);
    localStorage.setItem("bookings", JSON.stringify(updatedBookings));
    setBookings(updatedBookings);
  };

  if (bookings.length === 0) {
    return <h2 style={{ padding: "40px" }}>No bookings yet.</h2>;
  }

  return (
    <div style={{ padding: "40px" }}>
      <h1>Admin - Bookings</h1>

      {bookings.map((booking, index) => (
        <div
          key={index}
          style={{
            border: "1px solid #ccc",
            padding: "20px",
            marginBottom: "20px",
            borderRadius: "8px",
          }}
        >
          <h3>{booking.carName}</h3>
          <p>
            <strong>Name:</strong> {booking.customerName}
          </p>
          <p>
            <strong>Phone:</strong> {booking.customerPhone}
          </p>
          <p>
            <strong>Pickup:</strong> {booking.pickupDate}
          </p>
          <p>
            <strong>Return:</strong> {booking.returnDate}
          </p>
          <p>
            <strong>Total:</strong> €{booking.totalPrice}
          </p>

          <button onClick={() => deleteBooking(index)}>Delete Booking</button>
        </div>
      ))}
    </div>
  );
};

export default AdminBookings;
