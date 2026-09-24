import { priorityLabels, statusLabels, taskStatusLabels } from "@/lib/api";

const colors = ["bg-slate-100 text-slate-700", "bg-blue-100 text-blue-700", "bg-emerald-100 text-emerald-700", "bg-amber-100 text-amber-700"];
export function StatusBadge({ value, task = false }: { value: number; task?: boolean }) { const labels = task ? taskStatusLabels : statusLabels; return <span className={`rounded-full px-3 py-1 text-xs font-semibold ${colors[value] || colors[0]}`}>{labels[value] || "Unknown"}</span>; }
export function PriorityBadge({ value }: { value: number }) { const color = value === 3 ? "bg-red-100 text-red-700" : value === 2 ? "bg-orange-100 text-orange-700" : value === 1 ? "bg-blue-100 text-blue-700" : "bg-slate-100 text-slate-700"; return <span className={`rounded-full px-3 py-1 text-xs font-semibold ${color}`}>{priorityLabels[value] || "Unknown"}</span>; }
