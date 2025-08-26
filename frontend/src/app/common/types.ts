export interface EventType {
  id: number;
  name: string;
  startsOn: string;
  endsOn: string;
  location: string;
}

export interface TicketType {
  id: number;
  eventId: number;
  userId: number;
  purchaseDate: string;
  priceInCents: number;
}

export interface TopEventType {
  eventId: number;
  name: string;
  salesCount: number;
  revenue: number;
}
