using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagmentMVC.ViewModels.DashboardMVs
{
    public class SalesSummary
    {
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        [NotMapped]
        public string FormattedDate => OrderDate.ToString("MMM dd, yyyy");
    }
}
