namespace JobBoard.Domain.Entities.Models
{
    public class Post
    {
        public int ID { get; set; }
        public string? Job { get; set; }
        public string? Requirements { get; set; }
        public string? Salary { get; set; }
        public string? Contact { get; set; }
        
    }
}