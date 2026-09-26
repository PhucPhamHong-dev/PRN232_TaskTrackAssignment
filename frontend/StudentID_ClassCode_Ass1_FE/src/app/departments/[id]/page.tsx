"use client";

import Link from "next/link";
import { useParams } from "next/navigation";
import { useEffect, useState } from "react";
import { Shell } from "@/components/Shell";
import { EmptyState, ErrorState, LoadingState } from "@/components/Feedback";
import { StatusBadge } from "@/components/StatusBadge";
import { Department, getDepartment } from "@/lib/api";

export default function DepartmentDetailPage() {
  const { id } = useParams<{ id: string }>(); const [item, setItem] = useState<Department | null>(null); const [loading, setLoading] = useState(true); const [error, setError] = useState("");
  useEffect(() => { getDepartment(id).then(setItem).catch((e) => setError(e.message)).finally(() => setLoading(false)); }, [id]);
  return <Shell>{loading ? <LoadingState /> : error || !item ? <ErrorState message={error || "Department not found."} /> : <><Link href="/departments" className="text-sm font-semibold text-indigo-600">← Departments</Link><section className="mt-5 rounded-3xl border bg-white p-7 shadow-sm"><p className="text-sm font-semibold uppercase tracking-widest text-indigo-600">Department</p><h1 className="mt-2 text-3xl font-black">{item.departmentName}</h1><p className="mt-3 max-w-3xl leading-7 text-slate-600">{item.departmentDescription}</p></section><h2 className="mb-4 mt-9 text-2xl font-black">Projects</h2>{!item.projects?.length ? <EmptyState message="This department has no active projects." /> : <div className="grid gap-5 md:grid-cols-2">{item.projects.map((project) => <Link key={project.projectId} href={`/projects/${project.projectId}`} className="rounded-2xl border bg-white p-5 shadow-sm hover:shadow-md"><div className="flex items-start justify-between gap-3"><h3 className="font-black">{project.projectName}</h3><StatusBadge value={project.status} /></div><p className="mt-3 line-clamp-2 text-sm text-slate-500">{project.description || "No description"}</p><p className="mt-4 text-xs font-semibold text-slate-400">{project.startDate} — {project.endDate || "Open-ended"}</p></Link>)}</div>}</>}</Shell>;
}
