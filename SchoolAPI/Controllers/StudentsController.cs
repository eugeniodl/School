using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Data;
using SchoolAPI.Models.Dto;

namespace SchoolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly SchoolContext _db;
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(ILogger<StudentsController> logger, SchoolContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<StudentDto>> GetStudents()
        {
            _logger.LogInformation("Obtener los estudiantes");
            return Ok(_db.Students.ToList());
        }

        [HttpGet("{id:int}", Name ="GetStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<StudentDto> GetStudent(int id)
        {
            if(id == 0)
            {
                _logger.LogError($"Error al traer Estudiante con Id {id}");
                return BadRequest();
            }
            var student = _db.Students.FirstOrDefault(s => s.StudentId == id);

            if(student == null)
            {
                _logger.LogError($"Error al traer Estudiante con Id {id}");
                return NotFound();
            }

            return Ok(student);
        }
    }
}
