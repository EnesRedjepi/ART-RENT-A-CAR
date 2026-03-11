export interface Car {
  id: number;
  name: string;
  image: string;
  description: string;
  pricePerDay: number;
  //price: number; (in order to keep the code simple, we will use pricePerDay instead of price)

  transmission: "Manual" | "Automatic";
  fuel: "Petrol" | "Diesel" | "Hybrid" | "Electric";
  seats: number;
  luggage: number;
  airConditioning: boolean;
  year: number;
  available: boolean;
}
