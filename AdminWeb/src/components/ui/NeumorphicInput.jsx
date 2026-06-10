export default function NeumorphicInput({ label, as = "input", className = "", ...props }) {
  const Control = as;

  return (
    <label className={`block ${className}`}>
      <span className="mb-2 block text-sm font-semibold text-slate-700">{label}</span>
      <Control
        className="w-full rounded-lg border border-white/70 bg-slate-100/70 px-4 py-3 text-sm text-slate-800 shadow-neo-inset outline-none transition focus:border-cyan-300 focus:bg-white/80"
        {...props}
      />
    </label>
  );
}
