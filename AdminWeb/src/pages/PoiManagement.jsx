import { Edit3, Plus, Save, Upload } from "lucide-react";
import { useMemo, useState } from "react";
import GlassCard from "../components/ui/GlassCard.jsx";
import Modal from "../components/ui/Modal.jsx";
import NeumorphicInput from "../components/ui/NeumorphicInput.jsx";
import PageHeader from "../components/ui/PageHeader.jsx";
import PrimaryButton from "../components/ui/PrimaryButton.jsx";
import StatusBadge from "../components/ui/StatusBadge.jsx";
import { languages } from "../data/mockData.js";
import { useCmsStore } from "../store/cmsStore.js";

const emptyPoi = {
  latitude: "",
  longitude: "",
  radius: 50,
  status: "active",
  qr_code_token: "",
  translations: {
    vi: { name: "", short_description: "", full_description: "", audio_url: "" },
    en: { name: "", short_description: "", full_description: "", audio_url: "" },
  },
};

export default function PoiManagement() {
  const { pois, upsertPoi } = useCmsStore();
  const [activeLanguage, setActiveLanguage] = useState("vi");
  const [editingPoi, setEditingPoi] = useState(null);

  const tableRows = useMemo(
    () =>
      pois.map((poi) => ({
        ...poi,
        displayName: poi.translations.vi?.name || poi.translations.en?.name || "Untitled POI",
      })),
    [pois]
  );

  const openCreate = () => {
    setActiveLanguage("vi");
    setEditingPoi(emptyPoi);
  };

  const openEdit = (poi) => {
    setActiveLanguage("vi");
    setEditingPoi(JSON.parse(JSON.stringify(poi)));
  };

  const closeModal = () => setEditingPoi(null);

  const updateGlobal = (field, value) => {
    setEditingPoi((poi) => ({ ...poi, [field]: value }));
  };

  const updateTranslation = (field, value) => {
    setEditingPoi((poi) => ({
      ...poi,
      translations: {
        ...poi.translations,
        [activeLanguage]: {
          ...poi.translations[activeLanguage],
          [field]: value,
        },
      },
    }));
  };

  const savePoi = () => {
    upsertPoi({
      ...editingPoi,
      latitude: Number(editingPoi.latitude),
      longitude: Number(editingPoi.longitude),
      radius: Number(editingPoi.radius),
      qr_code_token: editingPoi.qr_code_token || `POI-${Date.now()}`,
    });
    closeModal();
  };

  return (
    <>
      <PageHeader
        eyebrow="Core CMS"
        title="POI Management"
        description="Manage pois and poi_translations with global geofence data plus language-specific narration content."
        action={
          <PrimaryButton onClick={openCreate}>
            <Plus size={18} />
            New POI
          </PrimaryButton>
        }
      />

      <GlassCard className="overflow-hidden">
        <div className="overflow-x-auto">
          <table className="w-full min-w-[860px] text-left text-sm">
            <thead className="border-b border-white/60 bg-white/35 text-xs uppercase text-slate-500">
              <tr>
                <th className="px-5 py-4">POI</th>
                <th className="px-5 py-4">Coordinates</th>
                <th className="px-5 py-4">Radius</th>
                <th className="px-5 py-4">QR Token</th>
                <th className="px-5 py-4">Status</th>
                <th className="px-5 py-4 text-right">Actions</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-white/55">
              {tableRows.map((poi) => (
                <tr key={poi.id} className="bg-white/20 transition hover:bg-white/45">
                  <td className="px-5 py-4">
                    <p className="font-black text-slate-950">{poi.displayName}</p>
                    <p className="mt-1 text-xs text-slate-500">{poi.translations.en?.name}</p>
                  </td>
                  <td className="px-5 py-4 font-mono text-xs text-slate-700">
                    {poi.latitude}, {poi.longitude}
                  </td>
                  <td className="px-5 py-4 text-slate-700">{poi.radius}m</td>
                  <td className="px-5 py-4 text-slate-700">{poi.qr_code_token}</td>
                  <td className="px-5 py-4">
                    <StatusBadge status={poi.status} />
                  </td>
                  <td className="px-5 py-4 text-right">
                    <button
                      type="button"
                      onClick={() => openEdit(poi)}
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
        open={Boolean(editingPoi)}
        title={editingPoi?.id ? "Edit POI" : "Create POI"}
        onClose={closeModal}
        footer={
          <div className="flex justify-end gap-3">
            <button type="button" onClick={closeModal} className="rounded-lg px-4 py-3 text-sm font-bold text-slate-600">
              Cancel
            </button>
            <PrimaryButton onClick={savePoi}>
              <Save size={18} />
              Save POI
            </PrimaryButton>
          </div>
        }
      >
        {editingPoi ? (
          <div className="space-y-6">
            <section>
              <h3 className="mb-4 text-base font-black text-slate-950">Global info</h3>
              <div className="grid gap-4 md:grid-cols-4">
                <NeumorphicInput label="Latitude" type="number" step="0.000001" value={editingPoi.latitude} onChange={(event) => updateGlobal("latitude", event.target.value)} />
                <NeumorphicInput label="Longitude" type="number" step="0.000001" value={editingPoi.longitude} onChange={(event) => updateGlobal("longitude", event.target.value)} />
                <NeumorphicInput label="Radius (meters)" type="number" value={editingPoi.radius} onChange={(event) => updateGlobal("radius", event.target.value)} />
                <label className="block">
                  <span className="mb-2 block text-sm font-semibold text-slate-700">Status</span>
                  <select
                    className="w-full rounded-lg border border-white/70 bg-slate-100/70 px-4 py-3 text-sm text-slate-800 shadow-neo-inset outline-none"
                    value={editingPoi.status}
                    onChange={(event) => updateGlobal("status", event.target.value)}
                  >
                    <option value="active">active</option>
                    <option value="hidden">hidden</option>
                  </select>
                </label>
              </div>
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
                  label="Name"
                  value={editingPoi.translations[activeLanguage].name}
                  onChange={(event) => updateTranslation("name", event.target.value)}
                />
                <NeumorphicInput
                  label="Short description"
                  value={editingPoi.translations[activeLanguage].short_description}
                  onChange={(event) => updateTranslation("short_description", event.target.value)}
                />
                <NeumorphicInput
                  as="textarea"
                  rows={5}
                  label="Full description"
                  value={editingPoi.translations[activeLanguage].full_description}
                  onChange={(event) => updateTranslation("full_description", event.target.value)}
                />
                <div className="grid gap-3 md:grid-cols-[1fr_auto] md:items-end">
                  <NeumorphicInput
                    label="Audio URL"
                    value={editingPoi.translations[activeLanguage].audio_url}
                    onChange={(event) => updateTranslation("audio_url", event.target.value)}
                  />
                  <button type="button" className="inline-flex items-center justify-center gap-2 rounded-lg bg-white/70 px-4 py-3 text-sm font-bold text-slate-700 shadow-neo">
                    <Upload size={17} />
                    Upload
                  </button>
                </div>
              </div>
            </section>
          </div>
        ) : null}
      </Modal>
    </>
  );
}
