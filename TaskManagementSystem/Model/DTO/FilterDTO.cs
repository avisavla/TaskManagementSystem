namespace TaskManagementSystem.Model.DTO
{
    public class FilterDTO
    {
        public string? Status { get; set; }
        public string? Text { get; set; }
        public int PageNo { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
