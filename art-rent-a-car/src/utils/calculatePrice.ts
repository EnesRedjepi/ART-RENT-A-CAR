import dayjs from "dayjs";

export const calculatePrice = (
  pickup: string,
  dropoff: string,
  pricePerDay: number,
) => {
  const start = dayjs(pickup);
  const end = dayjs(dropoff);

  const days = end.diff(start, "day") || 1;

  return {
    days,
    total: days * pricePerDay,
  };
};
