namespace FinStack.Domain.Entities
{
    public class Job
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string JobType { get; set; }
        public bool Success { get; set; }
        public long ElapsedMs { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
        public string Message { get; set; } = string.Empty;

        public Job()
        {
            Guid = Guid.NewGuid();
        }
    }
}