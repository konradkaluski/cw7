using cw3.DTOs.Requests;
using cw3.DTOs.Responses;
using cw3.Models;

namespace cw3.DAL
{
    public interface IStudentsDbService
    {
        EnrollStudentResponse EnrollStudent(EnrollStudentRequest rqt);
        PromoteStudentsResponse PromoteStudents(PromoteStudentsRequest rqt);
        Student GetStudent(string IndexNumer);
        LoginResponse Login(LoginRequest rqt);
    }
}
