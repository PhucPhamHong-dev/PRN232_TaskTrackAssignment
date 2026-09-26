import Link from "next/link";

const items = [["Departments", "/departments/manage"], ["Projects", "/projects/manage"], ["Tasks", "/tasks/manage"], ["Tags", "/tags/manage"]];
export function ManagementNav() { return <div className="mb-7 flex flex-wrap gap-2 rounded-2xl border bg-white p-2 shadow-sm">{items.map(([label, href]) => <Link key={href} href={href} className="rounded-xl px-4 py-2 text-sm font-semibold text-slate-600 hover:bg-indigo-50">{label}</Link>)}</div>; }
