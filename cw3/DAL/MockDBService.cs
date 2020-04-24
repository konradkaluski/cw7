using cw3.Models;
using System.Collections.Generic;

namespace cw3.Services
{
    public class MockDBService : IDBService
    {
        private static IEnumerable<Student> _students = new List<Student>
        {
            new Student{IndexNumber="s1234", FirstName="Jan", LastName="Kowalski" },
            new Student{IndexNumber="s2345", FirstName="Andrzej", LastName="Malina"},
            new Student{IndexNumber="s3456", FirstName="Monika", LastName="Nowak"}
        };

        public IEnumerable<Student> GetStudents()
        {
            return _students;
        }
    }
}
