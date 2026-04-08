export const createReservation = (
  car: any,
  pickupDate: string,
  returnDate: string,
) => {
  const start = new Date(pickupDate);
  const end = new Date(returnDate);

  const days = Math.ceil(
    (end.getTime() - start.getTime()) / (1000 * 60 * 60 * 24),
  );

  const totalPrice = days * car.pricePerDay;

  const booking = {
    carId: car.id,
    carName: car.name,
    customerName: "Pending",
    customerPhone: "Pending",
    pickupDate,
    returnDate,
    totalPrice,
  };

  const existing = JSON.parse(localStorage.getItem("bookings") || "[]");

  existing.push(booking);

  localStorage.setItem("bookings", JSON.stringify(existing));

  return { booking, days, totalPrice };
};
