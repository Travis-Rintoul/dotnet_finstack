namespace FinStack.Domain.Entities
{
    public class Job
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public DateTime CreatedDate { get; set; }
        public Job()
        {
            Guid = Guid.NewGuid();
            CreatedDate = DateTime.UtcNow;
        }
    }
}