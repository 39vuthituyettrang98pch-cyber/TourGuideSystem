import { Menu, Search } from "lucide-react";
import { Outlet } from "react-router-dom";
import Sidebar from "./Sidebar.jsx";

export default function AdminLayout() {
  return (
    <div className="min-h-screen bg-[radial-gradient(circle_at_top_left,#bde8f4_0,#edf2f8_34%,#f7e8df_68%,#d9e4ff_100%)]">
      <Sidebar />

      <div className="lg:pl-80">
        <header className="safe-area-top sticky top-0 z-20 border-b border-white/45 bg-white/35 px-4 py-3 backdrop-blur-2xl sm:px-6 lg:px-8">


          <div className="flex items-center justify-between gap-4">
            <div className="flex items-center gap-3">
              <button className="rounded-lg bg-white/55 p-2 text-slate-700 shadow-neo lg:hidden" aria-label="Open menu">
                <Menu size={20} />
              </button>
              <div>
                <p className="text-sm font-bold text-slate-500">Multilingual Automatic Audio Guide</p>
                <p className="text-lg font-black text-slate-950">Admin workspace</p>
              </div>
            </div>
            <div className="hidden min-w-72 items-center gap-2 rounded-lg border border-white/60 bg-white/45 px-3 py-2 shadow-neo-inset sm:flex">
              <Search size={17} className="text-slate-500" />
              <input className="w-full bg-transparent text-sm outline-none" placeholder="Search POI, tour, category..." />
            </div>
          </div>
        </header>

        <main className="px-4 py-6 sm:px-6 lg:px-8">
          <Outlet />
        </main>
      </div>
    </div>
  );
}
