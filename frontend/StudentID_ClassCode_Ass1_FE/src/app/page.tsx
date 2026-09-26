"use client";

import Link from "next/link";
import { useEffect, useRef, useState } from "react";
import { getDepartments, getProjects, searchTasks, Project, WorkTask } from "@/lib/api";
import { Shell } from "@/components/Shell";
import { StatusBadge } from "@/components/StatusBadge";
import { EmptyState, ErrorState, LoadingState } from "@/components/Feedback";

export default function Home() {
  const heroRef = useRef<HTMLElement>(null);
  const [departments, setDepartments] = useState(0);
  const [projects, setProjects] = useState<Project[]>([]);
  const [tasks, setTasks] = useState<WorkTask[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    Promise.all([getDepartments(), getProjects(), searchTasks("")])
      .then(([departmentData, projectData, taskData]) => {
        setDepartments(departmentData.length);
        setProjects(projectData);
        setTasks(taskData);
      })
      .catch((reason: Error) => setError(reason.message))
      .finally(() => setLoading(false));
  }, []);

  return <Shell>
    <section
      ref={heroRef}
      onPointerMove={(event) => {
        const bounds = event.currentTarget.getBoundingClientRect();
        event.currentTarget.style.setProperty("--pointer-x", `${event.clientX - bounds.left}px`);
        event.currentTarget.style.setProperty("--pointer-y", `${event.clientY - bounds.top}px`);
      }}
      className="hero-card group relative isolate overflow-hidden rounded-[2rem] text-white shadow-[0_24px_70px_-30px_rgba(79,70,229,0.75)]"
    >
      <div className="hero-grid" aria-hidden="true" />
      <div className="hero-orb hero-orb-one" aria-hidden="true" />
      <div className="hero-orb hero-orb-two" aria-hidden="true" />

      <div className="relative z-10 grid min-h-[380px] items-center gap-12 px-7 py-10 sm:px-10 lg:grid-cols-[1.15fr_0.85fr] lg:px-14">
        <div>
          <div className="hero-reveal hero-delay-1 flex items-center gap-2.5 text-xs font-bold uppercase tracking-[0.22em] text-indigo-100 sm:text-sm">
            <span className="hero-status-dot" />
            Public task management
          </div>
          <h1 className="hero-reveal hero-delay-2 mt-5 max-w-3xl text-4xl font-black leading-[1.05] tracking-[-0.04em] sm:text-5xl lg:text-6xl">
            Organize work.
            <span className="hero-highlight block">Move projects forward.</span>
          </h1>
          <p className="hero-reveal hero-delay-3 mt-5 max-w-2xl text-base leading-7 text-indigo-100/90 sm:text-lg">
            Explore active projects, follow tasks, and keep every team aligned in one simple workspace.
          </p>

          <div className="hero-reveal hero-delay-4 mt-7 flex flex-wrap gap-3">
            {[["Departments", departments], ["Projects", projects.length], ["Active tasks", tasks.length]].map(([label, value]) => (
              <div key={label} className="hero-stat">
                <span className="text-xl font-black">{loading ? "—" : value}</span>
                <span className="text-xs font-semibold text-indigo-100/80">{label}</span>
              </div>
            ))}
          </div>

          <div className="hero-reveal hero-delay-5 mt-7 flex flex-wrap items-center gap-4">
            <Link href="/departments" className="hero-primary-link">Explore departments <span aria-hidden="true">→</span></Link>
            <Link href="/search" className="hero-secondary-link">Search tasks</Link>
          </div>
        </div>

        <div className="hero-reveal hero-delay-4 relative hidden lg:block" aria-hidden="true">
          <div className="hero-preview">
            <div className="flex items-center justify-between border-b border-white/10 px-5 py-4">
              <div>
                <p className="text-xs font-bold uppercase tracking-[0.18em] text-indigo-200">Team workspace</p>
                <p className="mt-1 text-sm font-bold">This week&apos;s progress</p>
              </div>
              <div className="flex gap-1.5"><span className="preview-dot bg-rose-300" /><span className="preview-dot bg-amber-300" /><span className="preview-dot bg-emerald-300" /></div>
            </div>
            <div className="space-y-3 p-5">
              <div className="preview-task preview-task-one"><span className="preview-check">✓</span><div className="flex-1"><span className="preview-line w-3/4" /><span className="preview-line mt-2 w-2/5 opacity-40" /></div><span className="preview-pill bg-emerald-300/20 text-emerald-100">Done</span></div>
              <div className="preview-task preview-task-two"><span className="preview-check text-indigo-100">●</span><div className="flex-1"><span className="preview-line w-4/5" /><span className="preview-line mt-2 w-1/2 opacity-40" /></div><span className="preview-pill bg-amber-300/20 text-amber-100">Review</span></div>
              <div className="preview-task preview-task-three"><span className="preview-check text-indigo-100">○</span><div className="flex-1"><span className="preview-line w-2/3" /><span className="preview-line mt-2 w-1/3 opacity-40" /></div><span className="preview-pill bg-indigo-300/20 text-indigo-100">To do</span></div>
            </div>
          </div>
          <div className="hero-floating-pill hero-floating-pill-top"><span className="h-2 w-2 rounded-full bg-emerald-300" /> Live sync</div>
          <div className="hero-floating-pill hero-floating-pill-bottom">+12% productivity <span aria-hidden="true">↗</span></div>
        </div>
      </div>
    </section>
    <div className="mt-10 flex items-center justify-between"><h2 className="text-2xl font-black">Active projects</h2><Link href="/departments" className="text-sm font-semibold text-indigo-600">View departments →</Link></div>
    <div className="mt-4">
      {loading ? <LoadingState label="Loading dashboard..." /> : error ? <ErrorState message={error} /> : projects.length === 0 ? <EmptyState message="No active projects found." /> : <div className="grid gap-5 md:grid-cols-2 lg:grid-cols-3">{projects.map((project) => <Link key={project.projectId} href={`/projects/${project.projectId}`} className="rounded-2xl border bg-white p-5 shadow-sm transition hover:-translate-y-0.5 hover:shadow-md"><div className="flex items-start justify-between gap-3"><h3 className="font-bold">{project.projectName}</h3><StatusBadge value={project.status} /></div><p className="mt-3 line-clamp-3 text-sm text-slate-500">{project.description || "No description"}</p><p className="mt-5 text-xs font-semibold text-slate-400">{project.departmentName}</p></Link>)}</div>}
    </div>
  </Shell>;
}
