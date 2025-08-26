"use client";

import { useReactTable, getCoreRowModel, flexRender, ColumnDef } from '@tanstack/react-table';
import { EventType } from "../common/types";

interface EventTableProps<T> {
  data: T[];
  columns: ColumnDef<T>[];
}

export default function EventTable<T>({ data, columns }: EventTableProps<T>) {
  const table = useReactTable({
    data,
    columns,
    getCoreRowModel: getCoreRowModel(),
  });

  return (
    <div className="overflow-x-auto min-h-[280px]">
      <table className="w-full border border-accent rounded bg-primary">
        <thead>
          {table.getHeaderGroups().map(headerGroup => (
            <tr key={headerGroup.id} className="bg-secondary/10">
              {headerGroup.headers.map(header => (
                <th key={header.id} className="px-3 py-2 text-left text-secondary font-medium border-b border-accent">
                  {flexRender(header.column.columnDef.header, header.getContext())}
                </th>
              ))}
            </tr>
          ))}
        </thead>
        <tbody>
          {table.getRowModel().rows.length === 0 ? (
            <tr>
              <td colSpan={columns.length} className="text-center text-secondary/40 py-8">No events found.</td>
            </tr>
          ) : (
            table.getRowModel().rows.map(row => (
              <tr key={row.id} className="border-t border-accent hover:bg-accent/10">
                {row.getVisibleCells().map(cell => (
                  <td key={cell.id} className="px-3 py-2 text-secondary">
                    {flexRender(cell.column.columnDef.cell, cell.getContext())}
                  </td>
                ))}
              </tr>
            ))
          )}
        </tbody>
      </table>
    </div>
  );
}
