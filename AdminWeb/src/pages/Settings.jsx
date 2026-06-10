import GlassCard from "../components/ui/GlassCard.jsx";
import PageHeader from "../components/ui/PageHeader.jsx";

export default function Settings() {
  return (
    <>
      <PageHeader
        eyebrow="System"
        title="Settings"
        description="Placeholder for system_settings, sync versions, and administration preferences."
      />
      <GlassCard className="p-6 text-sm text-slate-600">
        Add API-backed settings here when backend endpoints are ready.
      </GlassCard>
    </>
  );
}
