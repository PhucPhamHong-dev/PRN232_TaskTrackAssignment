"use client";

import { useEffect } from "react";

export function LoadingState({ label = "Loading data..." }: { label?: string }) {
  return <div className="flex min-h-56 items-center justify-center gap-3 text-sm font-medium text-slate-500"><span className="h-5 w-5 animate-spin rounded-full border-2 border-indigo-200 border-t-indigo-600" />{label}</div>;
}

export function ErrorState({ message }: { message: string }) {
  return <div className="rounded-2xl border border-red-200 bg-red-50 p-5 text-sm text-red-700">{message}</div>;
}

export function EmptyState({ message }: { message: string }) {
  return <div className="rounded-2xl border border-dashed bg-white p-10 text-center text-sm text-slate-500">{message}</div>;
}

export function Toast({ message, type = "success", onClose }: { message: string; type?: "success" | "error"; onClose: () => void }) {
  useEffect(() => { const timer = setTimeout(onClose, 3200); return () => clearTimeout(timer); }, [onClose]);
  return <div className={`fixed right-5 top-5 z-50 rounded-xl px-5 py-3 text-sm font-semibold text-white shadow-xl ${type === "success" ? "bg-emerald-600" : "bg-red-600"}`}>{message}</div>;
}
