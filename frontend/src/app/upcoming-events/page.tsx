"use client";
import { useEffect, useState } from "react";

import EventTable from "../components/EventTable";
import EventFilters from "../components/EventFilters";
import { getEvents } from "../common/api";
import { EventType } from "../common/types";

export default function UpcomingEvents() {
  const [events, setEvents] = useState<EventType[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [filters, setFilters] = useState({
    days: 30,
    sortBy: "startDate",
    sortOrder: "asc",
    page: 1,
    pageSize: 50,
  });

  useEffect(() => {
    setLoading(true);
    setError("");
    getEvents(filters)
      .then(setEvents)
      .catch(() => setError("Failed to load events."))
      .finally(() => setLoading(false));
  }, [filters]);

  const columns = [
    { accessorKey: 'name', header: 'Name' },
    { accessorKey: 'startsOn', header: 'Start', cell: (info: any) => new Date(info.getValue() as string).toLocaleString() },
    { accessorKey: 'endsOn', header: 'End', cell: (info: any) => new Date(info.getValue() as string).toLocaleString() },
    { accessorKey: 'location', header: 'Location' },
  ];

  return (
  <div className="w-full bg-primary border border-accent rounded-lg p-6 min-h-[400px] mx-auto">
  <h1 className="text-2xl mb-4 text-center text-secondary">Upcoming Events</h1>
      <EventFilters
        days={filters.days}
        sortBy={filters.sortBy}
        sortOrder={filters.sortOrder}
        page={filters.page}
        pageSize={filters.pageSize}
        onChange={setFilters}
      />
  {error && <div className="text-red-500 mb-2 text-center">{error}</div>}
      {loading ? (
  <div className="text-center text-secondary">Loading...</div>
      ) : (
        <EventTable data={events} columns={columns} />
      )}
    </div>
  );
}
