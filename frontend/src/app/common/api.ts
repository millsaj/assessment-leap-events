// api.ts
// Central place for shared API constants and helpers


import { EventType, TicketType, TopEventType } from "./types";

export const API_URL = process.env.NEXT_PUBLIC_API_URL || "http://localhost:3000";

async function handleResponse<T>(res: Response): Promise<T> {
	if (!res.ok) throw new Error("API error");
	return res.json();
}

export async function getEvents(params: {
	days: number;
	sortBy?: string;
	sortOrder?: string;
	page?: number;
	pageSize?: number;
}): Promise<EventType[]> {
	const { days, sortBy = "startDate", sortOrder = "asc", page = 1, pageSize = 50 } = params;
	const url = `${API_URL}/events?days=${days}&sortBy=${sortBy}&sortOrder=${sortOrder}&page=${page}&pageSize=${pageSize}`;
	const res = await fetch(url);
	return handleResponse<EventType[]>(res);
}

export async function getTickets(eventId: number): Promise<TicketType[]> {
	const url = `${API_URL}/events/${eventId}/tickets`;
	const res = await fetch(url);
	return handleResponse<TicketType[]>(res);
}

export async function getTopEvents(by: "count" | "revenue"): Promise<TopEventType[]> {
	const url = `${API_URL}/top-events?by=${by}`;
	const res = await fetch(url);
	return handleResponse<TopEventType[]>(res);
}
