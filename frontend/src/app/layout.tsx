import type { Metadata } from "next";
import "./globals.css";

export const metadata: Metadata = {
  title: "Leap Events Assessment",
  description: "",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <body className="bg-primary text-secondary">
        <nav className="w-full bg-secondary py-3 mb-8 flex justify-center border-b border-accent">
          <div className="flex gap-6">
            <a href="/upcoming-events" className="text-2xl text-primary underline hover:no-underline hover:text-accent">Events</a>
            <a href="/sales-summary" className="text-2xl text-primary underline hover:no-underline hover:text-accent">Sales Summary</a>
          </div>
        </nav>
        <main className="max-w-6xl w-full mx-auto px-4">{children}</main>
      </body>
    </html>
  );
}
