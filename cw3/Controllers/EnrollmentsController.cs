using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using cw3.DAL;
using cw3.DTOs.Requests;

namespace cw3.Controllers
{
    [Route("api/enrollments")]
    [ApiController]

    public class EnrollmentsController : ControllerBase
    {
        private IStudentsDbService _service;

        public EnrollmentsController(IStudentsDbService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Roles = "employee")]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
             try
            {
                var response = _service.EnrollStudent(request);
         
                if (response != null)
                {
               return Created("",response);
                }
                else
                {
                return BadRequest();
            }
            }
            catch (SqlException except)
            {
                return BadRequest();
            }


        }

        [HttpPost("/api/enrollments/promotions")]
        [Authorize(Roles = "employee")]
        public IActionResult PromoteStudents(PromoteStudentsRequest request)
        {
            try
            {
                var response = _service.PromoteStudents(request);

                if (response != null)
                {
                    return Created("",response);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (SqlException except)
            {
                return BadRequest();
            }
        }

    }
}