"use client";

export function Modal({ open, title, children, onClose }: { open: boolean; title: string; children: React.ReactNode; onClose: () => void }) {
  if (!open) return null;
  return <div className="fixed inset-0 z-40 flex items-center justify-center bg-slate-950/50 p-4" onMouseDown={onClose}><div className="max-h-[92vh] w-full max-w-2xl overflow-y-auto rounded-2xl bg-white shadow-2xl" onMouseDown={(event) => event.stopPropagation()}><div className="flex items-center justify-between border-b px-6 py-4"><h2 className="text-xl font-black">{title}</h2><button type="button" onClick={onClose} className="rounded-lg px-3 py-1 text-xl text-slate-400 hover:bg-slate-100">×</button></div><div className="p-6">{children}</div></div></div>;
}

export function ConfirmDialog({ open, title, message, busy, onCancel, onConfirm }: { open: boolean; title: string; message: string; busy?: boolean; onCancel: () => void; onConfirm: () => void }) {
  if (!open) return null;
  return <div className="fixed inset-0 z-50 flex items-center justify-center bg-slate-950/50 p-4"><div className="w-full max-w-md rounded-2xl bg-white p-6 shadow-2xl"><h2 className="text-xl font-black">{title}</h2><p className="mt-3 text-sm leading-6 text-slate-600">{message}</p><div className="mt-6 flex justify-end gap-3"><button type="button" onClick={onCancel} disabled={busy} className="rounded-xl border px-4 py-2 text-sm font-semibold">Cancel</button><button type="button" onClick={onConfirm} disabled={busy} className="rounded-xl bg-red-600 px-4 py-2 text-sm font-semibold text-white disabled:opacity-50">{busy ? "Deleting..." : "Delete"}</button></div></div></div>;
}
