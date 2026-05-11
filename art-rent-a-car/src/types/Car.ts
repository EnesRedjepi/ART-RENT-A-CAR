export interface Car {
  id: number;
  name: string;
  image: string;
  slug: string;
  description: string;
  pricePerDay: number;
  transmission: "Manual" | "Automatic";
  fuel: "Petrol" | "Diesel" | "Hybrid" | "Electric";
  seats: number;
  luggage: number;
  airConditioning: boolean;
  year: number;
  available: boolean;
}
