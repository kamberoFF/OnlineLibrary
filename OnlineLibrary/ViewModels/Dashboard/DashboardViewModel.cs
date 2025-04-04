using OnlineLibrary.Models;
using OnlineLibrary.ViewModels.Book;

namespace OnlineLibrary.ViewModels.Dashboard
{
    public class DashboardViewModel
    {
        public List<BorrowedBookViewModel> BorrowedBooks { get; set; }
        public List<ReadingHistoryViewModel> ReadingHistory { get; set; }
        public ApplicationUser User { get; set; }
    }
}