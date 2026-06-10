using AdminWeb.Models;
using AdminWeb.ViewModels;

namespace AdminWeb.Services;

public interface ICmsRepository
{
    IReadOnlyList<Poi> GetPois();
    IReadOnlyList<Tour> GetTours();
    IReadOnlyList<Category> GetCategories();
    IReadOnlyList<VisitorPlaybackLog> GetPlaybackLogs();
    IReadOnlyList<SystemSetting> GetSettings();
    IReadOnlyList<DataSyncVersion> GetSyncVersions();
    DashboardViewModel GetDashboard();
    void SavePoi(PoiForm form);
    void SaveTour(TourForm form);
}
