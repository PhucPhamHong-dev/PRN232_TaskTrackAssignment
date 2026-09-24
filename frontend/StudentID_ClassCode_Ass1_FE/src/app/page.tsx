"use client";

import Link from "next/link";
import { useEffect, useState } from "react";
import { getDepartments, getProjects, searchTasks, Project, WorkTask } from "@/lib/api";
import { Shell } from "@/components/Shell";
import { StatusBadge } from "@/components/StatusBadge";

export default function Home() {
  const [departments, setDepartments] = useState(0); const [projects, setProjects] = useState<Project[]>([]); const [tasks, setTasks] = useState<WorkTask[]>([]); const [loading, setLoading] = useState(true);
  useEffect(() => { Promise.all([getDepartments(), getProjects(), searchTasks("")]).then(([d, p, t]) => { setDepartments(d.length); setProjects(p); setTasks(t); }).finally(() => setLoading(false)); }, []);
  return <Shell><section className="rounded-3xl bg-indigo-600 p-8 text-white shadow-lg"><p className="text-sm font-semibold uppercase tracking-widest text-indigo-200">Public task management</p><h1 className="mt-3 text-4xl font-black">Organize work. Move projects forward.</h1><p className="mt-3 max-w-2xl text-indigo-100">Explore active projects, follow tasks, and keep every team aligned in one simple workspace.</p></section><div className="mt-8 grid gap-4 md:grid-cols-3">{[["Departments", departments], ["Active projects", projects.length], ["Active tasks", tasks.length]].map(([label, value]) => <div key={label} className="rounded-2xl border bg-white p-5 shadow-sm"><p className="text-sm text-slate-500">{label}</p><p className="mt-2 text-3xl font-black">{loading ? "—" : value}</p></div>)}</div><div className="mt-10 flex items-center justify-between"><h2 className="text-2xl font-black">Active projects</h2><Link href="/departments" className="text-sm font-semibold text-indigo-600">View departments →</Link></div><div className="mt-4 grid gap-5 md:grid-cols-2 lg:grid-cols-3">{projects.map((project) => <Link key={project.projectId} href={`/projects/${project.projectId}`} className="rounded-2xl border bg-white p-5 shadow-sm transition hover:-translate-y-0.5 hover:shadow-md"><div className="flex items-start justify-between gap-3"><h3 className="font-bold">{project.projectName}</h3><StatusBadge value={project.status} /></div><p className="mt-3 line-clamp-3 text-sm text-slate-500">{project.description || "No description"}</p><p className="mt-5 text-xs font-semibold text-slate-400">{project.departmentName}</p></Link>)}</div></Shell>;
}
