import { X } from "lucide-react";

export default function Modal({ title, children, open, onClose, footer }) {
  if (!open) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-slate-950/45 p-4 backdrop-blur-sm">
      <div className="max-h-[92vh] w-full max-w-5xl overflow-hidden rounded-lg border border-white/55 bg-white/70 shadow-2xl backdrop-blur-2xl">
        <div className="flex items-center justify-between border-b border-white/60 px-6 py-4">
          <h2 className="text-xl font-bold text-slate-900">{title}</h2>
          <button
            type="button"
            onClick={onClose}
            className="rounded-lg p-2 text-slate-500 transition hover:bg-white/70 hover:text-slate-900"
            aria-label="Close"
          >
            <X size={20} />
          </button>
        </div>
        <div className="max-h-[68vh] overflow-y-auto px-6 py-5">{children}</div>
        {footer ? <div className="border-t border-white/60 px-6 py-4">{footer}</div> : null}
      </div>
    </div>
  );
}
