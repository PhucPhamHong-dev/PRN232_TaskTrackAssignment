"use client";

import Link from "next/link";
import { useParams } from "next/navigation";
import { useEffect, useState } from "react";
import { EmptyState, ErrorState, LoadingState } from "@/components/Feedback";
import { Shell } from "@/components/Shell";
import { StatusBadge } from "@/components/StatusBadge";
import { TaskCard } from "@/components/TaskCard";
import { getProject, Project } from "@/lib/api";

export default function ProjectDetailPage() {
  const { id } = useParams<{ id: string }>(); const [item, setItem] = useState<Project | null>(null); const [loading, setLoading] = useState(true); const [error, setError] = useState("");
  useEffect(() => { getProject(id).then(setItem).catch((e) => setError(e.message)).finally(() => setLoading(false)); }, [id]);
  return <Shell>{loading ? <LoadingState /> : error || !item ? <ErrorState message={error || "Project not found."} /> : <><Link href={`/departments/${item.departmentId}`} className="text-sm font-semibold text-indigo-600">← {item.departmentName}</Link><section className="mt-5 rounded-3xl border bg-white p-7 shadow-sm"><div className="flex flex-wrap items-start justify-between gap-4"><div><p className="text-sm font-semibold uppercase tracking-widest text-indigo-600">Project</p><h1 className="mt-2 text-3xl font-black">{item.projectName}</h1></div><StatusBadge value={item.status} /></div><p className="mt-4 max-w-3xl leading-7 text-slate-600">{item.description || "No description"}</p><div className="mt-6 flex flex-wrap gap-6 text-sm text-slate-500"><span><b className="text-slate-800">Department:</b> {item.departmentName}</span><span><b className="text-slate-800">Start:</b> {item.startDate}</span><span><b className="text-slate-800">End:</b> {item.endDate || "Open-ended"}</span></div></section><h2 className="mb-4 mt-9 text-2xl font-black">Tasks</h2>{!item.tasks?.length ? <EmptyState message="This project has no active tasks." /> : <div className="grid gap-4 md:grid-cols-2">{item.tasks.map((task) => <TaskCard key={task.taskId} task={task} />)}</div>}</>}</Shell>;
}
