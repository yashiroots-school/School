namespace SchoolManagement.Website.ViewModels
{
    public class TcAmountViewModel
    {
        public long Id { get; set; }
        public long TypeId { get; set; }
        public string TypeName { get; set; }
        public decimal Amount { get; set; }
        public bool IsEdit { get; set; }
    }
}