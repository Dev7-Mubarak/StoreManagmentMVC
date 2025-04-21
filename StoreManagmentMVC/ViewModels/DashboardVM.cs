using StoreManagmentMVC.Models;

namespace StoreManagmentMVC.ViewModels
{
    public class DashboardVM
    {
        public List<Sale>? Sales { get; set; }
        public List<TopProductVM>? TopProducts { get; set; }
    }
}
