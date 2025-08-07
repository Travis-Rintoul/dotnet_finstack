namespace FinStack.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        
        public User() {}

        public User(string email, string name)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email is required");
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required");

            Guid = Guid.NewGuid();
            Email = email;
            Name = name;
            CreatedDate = DateTime.UtcNow;
        }
    }
}