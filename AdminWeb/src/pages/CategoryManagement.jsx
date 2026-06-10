import GlassCard from "../components/ui/GlassCard.jsx";
import PageHeader from "../components/ui/PageHeader.jsx";

export default function CategoryManagement() {
  return (
    <>
      <PageHeader
        eyebrow="Taxonomy"
        title="Category Management"
        description="Placeholder for categories and category_translations management."
      />
      <GlassCard className="p-6 text-sm text-slate-600">
        Category table and multilingual category form can be connected to categories and category_translations.
      </GlassCard>
    </>
  );
}
