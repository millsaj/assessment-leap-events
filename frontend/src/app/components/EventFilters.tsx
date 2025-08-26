"use client";

interface EventFiltersProps {
  days: number;
  sortBy: string;
  sortOrder: string;
  page: number;
  pageSize: number;
  onChange: (filters: { days: number; sortBy: string; sortOrder: string; page: number; pageSize: number }) => void;
}

const DAYS_OPTIONS = [30, 60, 180];

export default function EventFilters({ days, sortBy, sortOrder, page, pageSize, onChange }: EventFiltersProps) {
  return (
  <div className="flex flex-wrap gap-4 mb-4 justify-between items-end bg-primary border border-accent rounded p-4">
      <div>
  <label className="block text-sm text-secondary mb-1">Upcoming in</label>
  <select value={days} onChange={e => onChange({ days: Number(e.target.value), sortBy, sortOrder, page, pageSize })} className="border border-accent rounded px-2 py-1 bg-primary">
          {DAYS_OPTIONS.map(d => <option key={d} value={d}>{d} days</option>)}
        </select>
      </div>
      <div>
  <label className="block text-sm text-secondary mb-1">Sort by</label>
  <select value={sortBy} onChange={e => onChange({ days, sortBy: e.target.value, sortOrder, page, pageSize })} className="border border-accent rounded px-2 py-1 bg-primary">
          <option value="name">Name</option>
          <option value="startDate">Start Date</option>
        </select>
      </div>
      <div>
  <label className="block text-sm text-secondary mb-1">Order</label>
  <select value={sortOrder} onChange={e => onChange({ days, sortBy, sortOrder: e.target.value, page, pageSize })} className="border border-accent rounded px-2 py-1 bg-primary">
          <option value="asc">Ascending</option>
          <option value="desc">Descending</option>
        </select>
      </div>
      <div className="flex items-end gap-2">
        <button
          className="px-3 py-1 rounded bg-accent/10 text-secondary border border-accent hover:bg-accent/20 disabled:opacity-50"
          onClick={() => onChange({ days, sortBy, sortOrder, page: Math.max(1, page - 1), pageSize })}
          disabled={page <= 1}
        >
          Prev
        </button>
  <span className="text-secondary">Page {page}</span>
        <button
          className="px-3 py-1 rounded bg-accent/10 text-secondary border border-accent hover:bg-accent/20"
          onClick={() => onChange({ days, sortBy, sortOrder, page: page + 1, pageSize })}
        >
          Next
        </button>
      </div>
      <div>
  <label className="block text-sm text-secondary mb-1">Page Size</label>
  <select value={pageSize} onChange={e => onChange({ days, sortBy, sortOrder, page, pageSize: Number(e.target.value) })} className="border border-accent rounded px-2 py-1 bg-primary">
          {[10, 25, 50, 100].map(size => <option key={size} value={size}>{size}</option>)}
        </select>
      </div>
    </div>
  );
}
