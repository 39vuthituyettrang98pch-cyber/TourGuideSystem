import { ArrowDown, ArrowUp, Edit3, Plus, Save } from "lucide-react";
import { useState } from "react";
import GlassCard from "../components/ui/GlassCard.jsx";
import Modal from "../components/ui/Modal.jsx";
import NeumorphicInput from "../components/ui/NeumorphicInput.jsx";
import PageHeader from "../components/ui/PageHeader.jsx";
import PrimaryButton from "../components/ui/PrimaryButton.jsx";
import StatusBadge from "../components/ui/StatusBadge.jsx";
import { languages } from "../data/mockData.js";
import { useCmsStore } from "../store/cmsStore.js";

const emptyTour = {
  estimated_time: 60,
  status: "active",
  translations: {
    vi: { title: "", description: "" },
    en: { title: "", description: "" },
  },
  poiIds: [],
};

export default function TourManagement() {
  const { tours, pois, upsertTour } = useCmsStore();
  const [activeLanguage, setActiveLanguage] = useState("vi");
  const [editingTour, setEditingTour] = useState(null);

  const getPoiName = (id) => {
    const poi = pois.find((item) => item.id === id);
    return poi?.translations.vi?.name || poi?.translations.en?.name || `POI #${id}`;
  };

  const openCreate = () => {
    setActiveLanguage("vi");
    setEditingTour(emptyTour);
  };

  const openEdit = (tour) => {
    setActiveLanguage("vi");
    setEditingTour(JSON.parse(JSON.stringify(tour)));
  };

  const updateTour = (field, value) => setEditingTour((tour) => ({ ...tour, [field]: value }));

  const updateTranslation = (field, value) => {
    setEditingTour((tour) => ({
      ...tour,
      translations: {
        ...tour.translations,
        [activeLanguage]: {
          ...tour.translations[activeLanguage],
          [field]: value,
        },
      },
    }));
  };

  const togglePoi = (poiId) => {
    setEditingTour((tour) => ({
      ...tour,
      poiIds: tour.poiIds.includes(poiId) ? tour.poiIds.filter((id) => id !== poiId) : [...tour.poiIds, poiId],
    }));
  };

  const movePoi = (index, direction) => {
    setEditingTour((tour) => {
      const nextIndex = index + direction;
      if (nextIndex < 0 || nextIndex >= tour.poiIds.length) return tour;

      const poiIds = [...tour.poiIds];
      [poiIds[index], poiIds[nextIndex]] = [poiIds[nextIndex], poiIds[index]];
      return { ...tour, poiIds };
    });
  };

  const saveTour = () => {
    upsertTour({
      ...editingTour,
      estimated_time: Number(editingTour.estimated_time),
    });
    setEditingTour(null);
  };

  return (
    <>
      <PageHeader
        eyebrow="Routes"
        title="Tour Management"
        description="Manage tours, tour_translations, and ordered tour_pois sequence data."
        action={
          <PrimaryButton onClick={openCreate}>
            <Plus size={18} />
            New Tour
          </PrimaryButton>
        }
      />

      <GlassCard className="overflow-hidden">
        <div className="overflow-x-auto">
          <table className="w-full min-w-[820px] text-left text-sm">
            <thead className="border-b border-white/60 bg-white/35 text-xs uppercase text-slate-500">
              <tr>
                <th className="px-5 py-4">Tour</th>
                <th className="px-5 py-4">Estimated Time</th>
                <th className="px-5 py-4">POI Sequence</th>
                <th className="px-5 py-4">Status</th>
                <th className="px-5 py-4 text-right">Actions</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-white/55">
              {tours.map((tour) => (
                <tr key={tour.id} className="bg-white/20 transition hover:bg-white/45">
                  <td className="px-5 py-4">
                    <p className="font-black text-slate-950">{tour.translations.vi.title}</p>
                    <p className="mt-1 text-xs text-slate-500">{tour.translations.en.title}</p>
                  </td>
                  <td className="px-5 py-4 text-slate-700">{tour.estimated_time} minutes</td>
                  <td className="px-5 py-4 text-slate-700">{tour.poiIds.map(getPoiName).join(" -> ")}</td>
                  <td className="px-5 py-4">
                    <StatusBadge status={tour.status} />
                  </td>
                  <td className="px-5 py-4 text-right">
                    <button
                      type="button"
                      onClick={() => openEdit(tour)}
                      className="inline-flex items-center gap-2 rounded-lg bg-white/65 px-3 py-2 font-bold text-slate-700 shadow-neo transition hover:bg-white"
                    >
                      <Edit3 size={15} />
                      Edit
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </GlassCard>

      <Modal
        open={Boolean(editingTour)}
        title={editingTour?.id ? "Edit Tour" : "Create Tour"}
        onClose={() => setEditingTour(null)}
        footer={
          <div className="flex justify-end gap-3">
            <button type="button" onClick={() => setEditingTour(null)} className="rounded-lg px-4 py-3 text-sm font-bold text-slate-600">
              Cancel
            </button>
            <PrimaryButton onClick={saveTour}>
              <Save size={18} />
              Save Tour
            </PrimaryButton>
          </div>
        }
      >
        {editingTour ? (
          <div className="space-y-6">
            <section className="grid gap-4 md:grid-cols-2">
              <NeumorphicInput
                label="Estimated time (minutes)"
                type="number"
                value={editingTour.estimated_time}
                onChange={(event) => updateTour("estimated_time", event.target.value)}
              />
              <label className="block">
                <span className="mb-2 block text-sm font-semibold text-slate-700">Status</span>
                <select
                  className="w-full rounded-lg border border-white/70 bg-slate-100/70 px-4 py-3 text-sm text-slate-800 shadow-neo-inset outline-none"
                  value={editingTour.status}
                  onChange={(event) => updateTour("status", event.target.value)}
                >
                  <option value="active">active</option>
                  <option value="disabled">disabled</option>
                </select>
              </label>
            </section>

            <section>
              <div className="mb-4 flex flex-wrap gap-2">
                {languages.map((language) => (
                  <button
                    key={language.code}
                    type="button"
                    onClick={() => setActiveLanguage(language.code)}
                    className={`rounded-lg px-4 py-2 text-sm font-bold transition ${
                      activeLanguage === language.code ? "bg-slate-950 text-white shadow-lg" : "bg-white/60 text-slate-600"
                    }`}
                  >
                    {language.label}
                  </button>
                ))}
              </div>
              <div className="grid gap-4">
                <NeumorphicInput
                  label="Title"
                  value={editingTour.translations[activeLanguage].title}
                  onChange={(event) => updateTranslation("title", event.target.value)}
                />
                <NeumorphicInput
                  as="textarea"
                  rows={4}
                  label="Description"
                  value={editingTour.translations[activeLanguage].description}
                  onChange={(event) => updateTranslation("description", event.target.value)}
                />
              </div>
            </section>

            <section className="grid gap-4 lg:grid-cols-2">
              <div>
                <h3 className="mb-3 text-base font-black text-slate-950">Select POIs</h3>
                <div className="space-y-2">
                  {pois.map((poi) => (
                    <label key={poi.id} className="flex items-center gap-3 rounded-lg bg-white/55 px-4 py-3 text-sm font-bold text-slate-700">
                      <input
                        type="checkbox"
                        checked={editingTour.poiIds.includes(poi.id)}
                        onChange={() => togglePoi(poi.id)}
                        className="h-4 w-4 accent-slate-950"
                      />
                      {getPoiName(poi.id)}
                    </label>
                  ))}
                </div>
              </div>
              <div>
                <h3 className="mb-3 text-base font-black text-slate-950">Sequence order</h3>
                <div className="space-y-2">
                  {editingTour.poiIds.map((poiId, index) => (
                    <div key={poiId} className="flex items-center justify-between gap-3 rounded-lg bg-slate-100/70 px-4 py-3 shadow-neo-inset">
                      <span className="text-sm font-bold text-slate-800">
                        {index + 1}. {getPoiName(poiId)}
                      </span>
                      <div className="flex gap-2">
                        <button type="button" onClick={() => movePoi(index, -1)} className="rounded-lg bg-white/70 p-2 text-slate-700 shadow-neo">
                          <ArrowUp size={16} />
                        </button>
                        <button type="button" onClick={() => movePoi(index, 1)} className="rounded-lg bg-white/70 p-2 text-slate-700 shadow-neo">
                          <ArrowDown size={16} />
                        </button>
                      </div>
                    </div>
                  ))}
                </div>
              </div>
            </section>
          </div>
        ) : null}
      </Modal>
    </>
  );
}
