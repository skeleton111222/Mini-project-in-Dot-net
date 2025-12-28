namespace EfCoreSQLiteCrudApp.Models
{
    public class Student
    {
        public int Id { get; set; } // Primary Key

        public string? Name { get; set; }// Name of the Student
        public int Age { get; set; } // Age of the Student

        // Foreign Key for Department
        public int DepartmentId { get; set; }

        // Navigation property for Department
        public Department? Department { get; set; }
    }
}
