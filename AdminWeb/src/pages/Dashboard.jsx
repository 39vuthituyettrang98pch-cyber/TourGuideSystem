import { Clock3, Headphones, MapPin, Users } from "lucide-react";
import { Line, LineChart, ResponsiveContainer, Tooltip, XAxis, YAxis } from "recharts";
import { CircleMarker, MapContainer, TileLayer, Tooltip as MapTooltip } from "react-leaflet";
import GlassCard from "../components/ui/GlassCard.jsx";
import PageHeader from "../components/ui/PageHeader.jsx";
import { heatmapPoints, playbackSeries } from "../data/mockData.js";

const stats = [
  { label: "Điểm tham quan", value: "1", icon: MapPin, tone: "text-blue-700" },
  { label: "Tour du lịch", value: "0", icon: MapPin, tone: "text-amber-700" },
  { label: "Tổng lượt phát", value: "0", icon: Headphones, tone: "text-emerald-700" },
  { label: "File audio", value: "0", icon: Clock3, tone: "text-rose-700" },
];

export default function Dashboard() {
  return (
    <>
      <PageHeader
        eyebrow="Overview"
        title="Dashboard & Analytics"
        description="Tổng quan các chỉ số hoạt động chính của hệ thống VERSA."
      />

      <section className="grid gap-4 md:grid-cols-2 xl:grid-cols-4">
        {stats.map((stat) => {
          const Icon = stat.icon;

          return (
            <div key={stat.label} className="rounded-lg bg-slate-100/70 p-5 shadow-neo">
              {/* Đã sửa thành flex-col và canh giữa toàn bộ (items-center text-center) */}
              <div className="flex flex-col items-center justify-center text-center gap-3">
                <div
                  className={`grid h-12 w-12 place-items-center rounded-lg bg-white/70 shadow-neo-inset ${stat.tone}`}
                  aria-hidden="true"
                >
                  <Icon size={22} />
                </div>

                <div className="min-w-0">
                  <p className="text-sm font-bold leading-5 text-slate-500">{stat.label}</p>
                  <p className="mt-2 stat-value text-2xl font-black leading-none text-slate-950">{stat.value}</p>
                </div>
              </div>
            </div>
          );
        })}
      </section>

      <section className="mt-6 grid gap-6 xl:grid-cols-[1.1fr_0.9fr]">
        <GlassCard className="p-5">
          <div className="mb-5">
            <h2 className="text-lg font-black text-slate-950">Biểu đồ Lượt nghe 7 ngày qua</h2>
            <p className="text-sm text-slate-600">Placeholder Recharts line chart from visitor_playback_logs.</p>
          </div>
          <div className="h-80">
            <ResponsiveContainer width="100%" height="100%">
              <LineChart data={playbackSeries}>
                <XAxis dataKey="day" stroke="#64748b" />
                <YAxis stroke="#64748b" />
                <Tooltip />
                <Line type="monotone" dataKey="listens" stroke="#0f172a" strokeWidth={3} dot={{ r: 4 }} />
                <Line type="monotone" dataKey="avgTime" stroke="#0891b2" strokeWidth={3} dot={{ r: 4 }} />
              </LineChart>
            </ResponsiveContainer>
          </div>
        </GlassCard>

        <GlassCard className="overflow-hidden">
          <div className="p-5">
            <h2 className="text-lg font-black text-slate-950">Tỷ lệ Ngôn ngữ được chọn</h2>
            <p className="text-sm text-slate-600">React Leaflet placeholder using visitor coordinates.</p>
          </div>
          <div className="h-80">
            <MapContainer center={[10.7757, 106.6968]} zoom={13} scrollWheelZoom={false}>
              <TileLayer
                attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a>'
                url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
              />
              {heatmapPoints.map((point) => (
                <CircleMarker
                  key={point.id}
                  center={[point.visitor_latitude, point.visitor_longitude]}
                  radius={12}
                  pathOptions={{ color: "#0f172a", fillColor: "#06b6d4", fillOpacity: 0.45 }}
                >
                  <MapTooltip>{point.listen_duration}s listened</MapTooltip>
                </CircleMarker>
              ))}
            </MapContainer>
          </div>
        </GlassCard>
      </section>
    </>
  );
}