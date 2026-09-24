import Link from "next/link";
import { PriorityBadge, StatusBadge } from "@/components/StatusBadge";
import { WorkTask } from "@/lib/api";

export function TaskCard({ task }: { task: WorkTask }) { return <Link href={`/tasks/${task.taskId}`} className="block rounded-2xl border bg-white p-5 shadow-sm transition hover:-translate-y-0.5 hover:shadow-md"><div className="flex items-start justify-between gap-3"><h3 className="font-bold">{task.title}</h3><StatusBadge value={task.status} task /></div><p className="mt-2 line-clamp-2 text-sm text-slate-500">{task.description || "No description"}</p><div className="mt-4 flex flex-wrap gap-2"><PriorityBadge value={task.priority} />{task.tags.map((tag) => <span key={tag.tagId} className="rounded-full px-3 py-1 text-xs font-medium" style={{ backgroundColor: `${tag.color || "#64748b"}22`, color: tag.color || "#475569" }}>{tag.tagName}</span>)}</div></Link>; }
