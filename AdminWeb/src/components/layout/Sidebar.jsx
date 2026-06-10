import { BarChart3, Compass, FolderTree, LayoutDashboard, Map, Settings, Waypoints } from "lucide-react";
import { NavLink } from "react-router-dom";

const menuItems = [
  { label: "Dashboard", path: "/dashboard", icon: LayoutDashboard },
  { label: "POI Management", path: "/pois", icon: Compass },
  { label: "Category Management", path: "/categories", icon: FolderTree },
  { label: "Tour Management", path: "/tours", icon: Waypoints },
  { label: "Analytics & Heatmap", path: "/analytics", icon: BarChart3 },
  { label: "Settings", path: "/settings", icon: Settings },
];

export default function Sidebar() {
  return (
    <aside className="fixed inset-y-4 left-4 z-30 hidden w-72 rounded-lg border border-white/55 bg-white/35 p-4 shadow-2xl shadow-slate-900/15 backdrop-blur-2xl lg:block">
      <div className="mb-8 flex items-center gap-3 px-2">
        <div className="grid h-11 w-11 place-items-center rounded-lg bg-slate-950 text-white shadow-lg shadow-slate-900/25">
          <Map size={22} />
        </div>
        <div>
          <p className="text-lg font-black text-slate-950">VERSA CMS</p>
          <p className="text-xs font-semibold text-slate-500">Audio Guide Admin</p>
        </div>
      </div>

      <nav className="space-y-2">
        {menuItems.map((item) => {
          const Icon = item.icon;

          return (
            <NavLink
              key={item.path}
              to={item.path}
              className={({ isActive }) =>
                `flex items-center gap-3 rounded-lg px-4 py-3 text-sm font-bold transition ${
                  isActive
                    ? "bg-slate-950 text-white shadow-lg shadow-slate-900/20"
                    : "text-slate-600 hover:bg-white/60 hover:text-slate-950"
                }`
              }
            >
              <Icon size={18} />
              <span>{item.label}</span>
            </NavLink>
          );
        })}
      </nav>
    </aside>
  );
}
