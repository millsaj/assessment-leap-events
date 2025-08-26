"use client";

import { useEffect, useState } from "react";
import EventTable from "../components/EventTable";
import { getTopEvents } from "../common/api";
import { TopEventType } from "../common/types";


export default function SalesSummary() {
  const [byCount, setByCount] = useState<TopEventType[]>([]);
  const [byRevenue, setByRevenue] = useState<TopEventType[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    setLoading(true);
    setError("");
    Promise.all([
      getTopEvents("count"),
      getTopEvents("revenue")
    ])
      .then(([count, revenue]) => {
        setByCount(count);
        setByRevenue(revenue);
      })
      .catch(() => setError("Failed to load sales summary."))
      .finally(() => setLoading(false));
  }, []);

  return (
    <div className="w-full mx-auto bg-primary border border-accent rounded-lg p-6 min-h-[400px]">
      <h1 className="text-2xl mb-4 text-center text-secondary">Sales Summary</h1>
      {error && <div className="text-red-500 mb-2 text-center">{error}</div>}
      {loading ? (
        <div className="text-center text-secondary">Loading...</div>
      ) : (
        <>
          <h2 className="text-lg mt-4 mb-2 text-secondary">Top 5 Events by Sales Count</h2>
          <EventTable
            data={byCount}
            columns={[
              { accessorKey: "name", header: "Event Name" },
              { accessorKey: "score", header: "Sales Count" },
            ]} />
          <h2 className="text-lg mt-4 mb-2 text-secondary">Top 5 Events by Revenue</h2>
          <EventTable
            data={byRevenue}
            columns={[
              { accessorKey: "name", header: "Event Name" },
              { accessorKey: "score", header: "Revenue ($)", cell: (info: any) => ((info.getValue() as number) / 100).toFixed(2) },
            ]} />
        </>
      )}
    </div>
  );
}
