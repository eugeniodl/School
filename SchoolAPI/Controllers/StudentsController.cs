using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Data;
using SchoolAPI.Models;
using SchoolAPI.Models.Dto;

namespace SchoolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly ILogger<StudentsController> _logger;
        private readonly SchoolContext _db;

        public StudentsController(ILogger<StudentsController> logger, SchoolContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<StudentDto>> GetStudents()
        {
            _logger.LogInformation("Obtener los Estudiantes");
            return Ok(_db.Students.ToList());
        }

        [HttpGet("{id:int}", Name = "GetStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<StudentDto> GetStudent(int id)
        {
            if(id == 0)
            {
                _logger.LogError($"Error al traer el Estudiante con Id {id}");
                return BadRequest();
            }
            var student = _db.Students.FirstOrDefault(s => s.StudentId == id);

            if(student == null)
            {
                _logger.LogError($"Error al traer el Estudiante con Id {id}");
                return NotFound();
            }

            return Ok(student);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult<StudentDto> AddStudent([FromBody] StudentCreateDto studentDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(_db.Students.FirstOrDefault(s => s.StudentName.ToLower() == studentDto.StudentName.ToLower()) != null)
            {
                ModelState.AddModelError("NombreExiste", "¡El Estudiante con ese Nombre ya existe!");
                return BadRequest(ModelState);
            }

            if(studentDto == null)
            {
                return BadRequest(studentDto);
            }

            Student modelo = new()
            {
                StudentName = studentDto.StudentName
            };

            _db.Students.Add(modelo);
            _db.SaveChanges();

            return CreatedAtRoute("GetStudent", new { id = modelo.StudentId }, modelo);

        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteStudent(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }

            var student = _db.Students.FirstOrDefault(s => s.StudentId == id);

            if(id == null)
            {
                return NotFound();
            }

            _db.Students.Remove(student);
            _db.SaveChanges(true);

            return NoContent();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateStudent(int id, [FromBody] StudentUpdateDto studentDTO)
        {
            if(studentDTO == null || id != studentDTO.StudentId)
            {
                return BadRequest();
            }

            Student modelo = new()
            {
                StudentId = studentDTO.StudentId,
                StudentName = studentDTO.StudentName
            };

            _db.Students.Update(modelo);
            _db.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialStudent(int id, JsonPatchDocument<StudentUpdateDto> patchDto)
        {
            if(patchDto == null || id == 0)
            {
                return BadRequest();
            }

            var student = _db.Students.FirstOrDefault(s => s.StudentId == id);

            StudentUpdateDto studentDto = new()
            {
                StudentId = student.StudentId,
                StudentName = student.StudentName
            };
            if(student == null) return BadRequest();

            patchDto.ApplyTo(studentDto, ModelState);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Student modelo = new()
            {
                StudentId = studentDto.StudentId,
                StudentName = studentDto.StudentName
            };

            _db.Students.Update(modelo);
            _db.SaveChanges();

            return NoContent();
        }
    }
}
