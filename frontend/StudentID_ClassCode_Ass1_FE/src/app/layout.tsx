import type { Metadata } from "next";
import "./globals.css";

export const metadata: Metadata = { title: "TaskTrack", description: "Public task and team management app" };

export default function RootLayout({ children }: Readonly<{ children: React.ReactNode }>) { return <html lang="en"><body>{children}</body></html>; }
