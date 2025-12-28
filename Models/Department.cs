namespace EfCoreSQLiteCrudApp.Models
{
    public class Department
    {
        public int Id { get; set; } // Primary Key
        public string? Name { get; set; }// Name of the Department

        // Navigation property for Students
        public ICollection<Student>? Students { get; set; }
    }
}
