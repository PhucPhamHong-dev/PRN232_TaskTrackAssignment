"use client";

import Link from "next/link";
import { useParams } from "next/navigation";
import { useEffect, useState } from "react";
import { ErrorState, LoadingState } from "@/components/Feedback";
import { Shell } from "@/components/Shell";
import { PriorityBadge, StatusBadge } from "@/components/StatusBadge";
import { getTask, WorkTask } from "@/lib/api";

export default function TaskDetailPage() {
  const { id } = useParams<{ id: string }>(); const [item, setItem] = useState<WorkTask | null>(null); const [loading, setLoading] = useState(true); const [error, setError] = useState("");
  useEffect(() => { getTask(id).then(setItem).catch((e) => setError(e.message)).finally(() => setLoading(false)); }, [id]);
  return <Shell>{loading ? <LoadingState /> : error || !item ? <ErrorState message={error || "Task not found."} /> : <><Link href={`/projects/${item.projectId}`} className="text-sm font-semibold text-indigo-600">← {item.projectName}</Link><section className="mt-5 rounded-3xl border bg-white p-7 shadow-sm"><p className="text-sm font-semibold uppercase tracking-widest text-indigo-600">Task #{item.taskId}</p><h1 className="mt-2 text-3xl font-black">{item.title}</h1><div className="mt-4 flex flex-wrap gap-2"><StatusBadge value={item.status} task /><PriorityBadge value={item.priority} />{item.tags.map((tag) => <span key={tag.tagId} className="rounded-full px-3 py-1 text-xs font-semibold" style={{ backgroundColor: `${tag.color || "#64748b"}22`, color: tag.color || "#475569" }}>{tag.tagName}</span>)}</div><div className="mt-7 grid gap-5 md:grid-cols-2"><div><p className="text-xs font-bold uppercase text-slate-400">Description</p><p className="mt-2 leading-7 text-slate-600">{item.description || "No description"}</p></div><div className="rounded-2xl bg-slate-50 p-5 text-sm text-slate-600"><p><b>Project:</b> {item.projectName}</p><p className="mt-2"><b>Due date:</b> {item.dueDate || "No due date"}</p><p className="mt-2"><b>Created:</b> {item.createdDate ? new Date(item.createdDate).toLocaleString() : "—"}</p><p className="mt-2"><b>Modified:</b> {item.modifiedDate ? new Date(item.modifiedDate).toLocaleString() : "Never"}</p></div></div></section></>}</Shell>;
}
