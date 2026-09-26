"use client";

import Link from "next/link";
import { useEffect, useState } from "react";
import { Shell } from "@/components/Shell";
import { EmptyState, ErrorState, LoadingState } from "@/components/Feedback";
import { Department, getDepartments } from "@/lib/api";

export default function DepartmentsPage() {
  const [items, setItems] = useState<Department[]>([]); const [loading, setLoading] = useState(true); const [error, setError] = useState("");
  useEffect(() => { getDepartments().then(setItems).catch((e) => setError(e.message)).finally(() => setLoading(false)); }, []);
  return <Shell><div className="mb-7"><p className="text-sm font-semibold uppercase tracking-widest text-indigo-600">Teams</p><h1 className="mt-2 text-3xl font-black">Active departments</h1><p className="mt-2 text-slate-500">Choose a department to explore its projects.</p></div>{loading ? <LoadingState /> : error ? <ErrorState message={error} /> : items.length === 0 ? <EmptyState message="No active departments found." /> : <div className="grid gap-5 md:grid-cols-2 lg:grid-cols-3">{items.map((item) => <Link key={item.departmentId} href={`/departments/${item.departmentId}`} className="rounded-2xl border bg-white p-6 shadow-sm hover:-translate-y-0.5 hover:shadow-md"><div className="mb-4 flex h-11 w-11 items-center justify-center rounded-xl bg-indigo-100 font-black text-indigo-600">{item.departmentName.charAt(0)}</div><h2 className="text-lg font-black">{item.departmentName}</h2><p className="mt-2 line-clamp-3 text-sm leading-6 text-slate-500">{item.departmentDescription}</p><p className="mt-5 text-sm font-semibold text-indigo-600">View projects →</p></Link>)}</div>}</Shell>;
}
