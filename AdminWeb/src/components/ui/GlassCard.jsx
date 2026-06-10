export default function GlassCard({ children, className = "" }) {
  return (
    <div
      className={`rounded-lg border border-white/55 bg-white/45 shadow-xl shadow-slate-900/10 backdrop-blur-xl ${className}`}
    >
      {children}
    </div>
  );
}
