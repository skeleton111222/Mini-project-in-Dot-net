using EfCoreSQLiteCrudApp.Data;
using EfCoreSQLiteCrudApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EfCoreSQLiteCrudApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new AppDbContext())
            {
                context.Database.EnsureCreated();

                bool exit = false;
                while (!exit)
                {
                    Console.Clear();
                    Console.WriteLine("Menu:");
                    Console.WriteLine("1. Add a Student");
                    Console.WriteLine("2. View Students");
                    Console.WriteLine("3. Update a Student");
                    Console.WriteLine("4. Delete a Student");
                    Console.WriteLine("5. Exit");
                    Console.WriteLine("6. Add a Department");
                    Console.Write("Select an option: ");
                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            AddStudent(context);
                            break;
                        case "2":
                            ViewStudents(context);
                            break;
                        case "3":
                            UpdateStudent(context);
                            break;
                        case "4":
                            DeleteStudent(context);
                            break;
                        case "5":
                            exit = true;
                            Console.WriteLine("Exiting the Application");
                            break;
                        case "6":
                            AddDepartment(context);
                            break;
                        default:
                            Console.WriteLine("Invalid option, please try again.");
                            break;
                    }
                    if (!exit)
                    {
                        Console.WriteLine("Press any key to return to the menu...");
                        Console.ReadKey();
                    }
                }
            }
        }

        static void AddStudent(AppDbContext context)
        {
            Console.WriteLine("Enter Student Name: ");
            string name = Console.ReadLine();

            Console.WriteLine("Enter Student Age: ");
            int age = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter Department Name: ");
            string departmentName = Console.ReadLine();

            try
            {
                // Find the department by name (case-insensitive)
                var department = context.Departments
                    .FirstOrDefault(d => d.Name.ToLower() == departmentName.ToLower());

                if (department == null)
                {
                    Console.WriteLine("Department not found. Would you like to create this department? (y/n)");
                    if (Console.ReadLine().ToLower() == "y")
                    {
                        department = new Department { Name = departmentName };
                        context.Departments.Add(department);
                        context.SaveChanges();
                        Console.WriteLine($"Department '{departmentName}' created successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Student not added. Please try again with a valid department.");
                        return;
                    }
                }

                // Create and add the student
                var student = new Student { Name = name, Age = age, DepartmentId = department.Id };
                context.Students.Add(student);
                context.SaveChanges();
                Console.WriteLine("Student added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while adding the student or department.");
                Console.WriteLine($"Error: {ex.Message}");
            }
        }


        static void ViewStudents(AppDbContext context)
        {
            try
            {
                var students = context.Students.Include(s => s.Department).ToList();
                Console.WriteLine("\nStudent Information:");
                foreach (var student in students)
                {
                    Console.WriteLine($"ID: {student.Id}, Name: {student.Name}, Age: {student.Age}, Department: {student.Department?.Name ?? "No Department"}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while retrieving the student data.");
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void UpdateStudent(AppDbContext context)
        {
            try
            {
                Console.WriteLine("Enter the Student ID to update: ");
                int id = int.Parse(Console.ReadLine());

                var student = context.Students.Find(id);
                if (student != null)
                {
                    Console.WriteLine("Enter new Name: ");
                    student.Name = Console.ReadLine();
                    Console.WriteLine("Enter new Age: ");
                    student.Age = int.Parse(Console.ReadLine());

                    context.SaveChanges();
                    Console.WriteLine("Student updated successfully!");
                }
                else
                {
                    Console.WriteLine("Student not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while updating the student.");
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void DeleteStudent(AppDbContext context)
        {
            try
            {
                Console.WriteLine("Enter the Student ID to delete: ");
                int id = int.Parse(Console.ReadLine());

                var student = context.Students.Find(id);
                if (student != null)
                {
                    context.Students.Remove(student);
                    context.SaveChanges();
                    Console.WriteLine("Student deleted successfully!");
                }
                else
                {
                    Console.WriteLine("Student not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while deleting the student.");
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void AddDepartment(AppDbContext context)
        {

            try
            {
                Console.WriteLine("Enter Department Name: ");
                string departmentName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(departmentName))
                {
                    Console.WriteLine("Department name cannot be empty.");
                    return;
                }
                var department = new Department { Name = departmentName };
                context.Departments.Add(department);
                context.SaveChanges();
                Console.WriteLine("Department added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while adding the department.");
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
