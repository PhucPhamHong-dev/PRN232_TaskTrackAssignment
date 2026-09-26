"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";
import { useState } from "react";

const navItems = [
  { label: "Departments", href: "/departments", icon: "departments" },
  { label: "Search Tasks", href: "/search", icon: "search" },
  { label: "Manage", href: "/departments/manage", icon: "manage" },
];

function NavIcon({ name }: { name: string }) {
  if (name === "search") {
    return <svg viewBox="0 0 24 24" aria-hidden="true"><circle cx="11" cy="11" r="6" /><path d="m16 16 4 4" /></svg>;
  }
  if (name === "manage") {
    return <svg viewBox="0 0 24 24" aria-hidden="true"><path d="M4 7h10M18 7h2M4 17h2M10 17h10" /><circle cx="16" cy="7" r="2" /><circle cx="8" cy="17" r="2" /></svg>;
  }
  return <svg viewBox="0 0 24 24" aria-hidden="true"><path d="M4 5.5h6v6H4zM14 5.5h6v6h-6zM4 15.5h6v3H4zM14 15.5h6v3h-6z" /></svg>;
}

export function Shell({ children }: { children: React.ReactNode }) {
  const pathname = usePathname();
  const [menuOpen, setMenuOpen] = useState(false);

  const isActive = (href: string) => {
    if (href.includes("/manage")) return pathname.includes("/manage");
    if (href === "/departments") return pathname.startsWith("/departments") && !pathname.includes("/manage");
    return pathname === href;
  };

  return <div className="min-h-screen bg-slate-50 text-slate-900">
    <header className="site-header sticky top-0 z-50 border-b border-slate-200/70 bg-white/80 backdrop-blur-xl">
      <div className="nav-glow" aria-hidden="true" />
      <div className="mx-auto flex h-[72px] max-w-7xl items-center justify-between px-6">
        <Link href="/" className="brand-link group" aria-label="TaskTrack home" onClick={() => setMenuOpen(false)}>
          <span className="brand-mark" aria-hidden="true"><i /><i /><i /></span>
          <span className="brand-word">Task<span>Track</span></span>
        </Link>

        <nav className="nav-desktop" aria-label="Primary navigation">
          {navItems.map((item) => {
            const active = isActive(item.href);
            return <Link
              key={item.href}
              href={item.href}
              aria-current={active ? "page" : undefined}
              className={`nav-link ${active ? "nav-link-active" : ""}`}
            >
              <span className="nav-icon"><NavIcon name={item.icon} /></span>
              {item.label}
              <span className="nav-active-dot" aria-hidden="true" />
            </Link>;
          })}
        </nav>

        <button
          type="button"
          className={`nav-toggle ${menuOpen ? "nav-toggle-open" : ""}`}
          aria-expanded={menuOpen}
          aria-controls="mobile-navigation"
          aria-label={menuOpen ? "Close navigation" : "Open navigation"}
          onClick={() => setMenuOpen((current) => !current)}
        >
          <span /><span /><span />
        </button>
      </div>

      <div id="mobile-navigation" className={`nav-mobile ${menuOpen ? "nav-mobile-open" : ""}`} aria-hidden={!menuOpen} inert={!menuOpen}>
        <nav className="mx-auto grid w-full max-w-7xl gap-1 px-4 pb-4" aria-label="Mobile navigation">
          {navItems.map((item) => {
            const active = isActive(item.href);
            return <Link
              key={item.href}
              href={item.href}
              aria-current={active ? "page" : undefined}
              className={`nav-mobile-link ${active ? "nav-mobile-link-active" : ""}`}
              onClick={() => setMenuOpen(false)}
            >
              <span className="nav-icon"><NavIcon name={item.icon} /></span>
              <span className="flex-1">{item.label}</span>
              <span aria-hidden="true">→</span>
            </Link>;
          })}
        </nav>
      </div>
    </header>
    <main className="mx-auto max-w-7xl px-6 py-8">{children}</main>
  </div>;
}
