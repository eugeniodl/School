using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.Models;
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
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudents()
        {
            _logger.LogInformation("Obtener los Estudiantes");
            return Ok(await _db.Students.ToListAsync());
        }

        [HttpGet("{id:int}", Name = "GetStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StudentDto>> GetStudent(int id)
        {
            if (id == 0)
            {
                _logger.LogError($"Error al traer Estudiante con Id {id}");
                return BadRequest();
            }
            var student = await _db.Students.FirstOrDefaultAsync(s => s.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<StudentDto>> AddStudent([FromBody] StudentCreateDto studentCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _db.Students.FirstOrDefaultAsync(s => s.StudentName.ToLower() == studentCreateDto.StudentName.ToLower()) != null)
            {
                ModelState.AddModelError("NombreExiste", "¡El Estudiante con ese Nombre ya existe!");
                return BadRequest(ModelState);
            }

            if (studentCreateDto == null)
            {
                return BadRequest(studentCreateDto);
            }

            Student modelo = new()
            {
                StudentName = studentCreateDto.StudentName
            };

            await _db.Students.AddAsync(modelo);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("GetStudent", new { id = modelo.StudentId }, modelo);

        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var student = await _db.Students.FirstOrDefaultAsync(s => s.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            _db.Students.Remove(student);
            await _db.SaveChangesAsync(true);

            return NoContent();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] StudentUpdateDto studentUpdateDto)
        {
            if (studentUpdateDto == null || id != studentUpdateDto.StudentId)
            {
                return BadRequest();
            }

            Student modelo = new()
            {
                StudentId = studentUpdateDto.StudentId,
                StudentName = studentUpdateDto.StudentName
            };

            _db.Students.Update(modelo);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialStudent(int id, JsonPatchDocument<StudentUpdateDto> patchDto)
        {
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }
            var student = await _db.Students.AsNoTracking().FirstOrDefaultAsync(s => s.StudentId == id);
            StudentUpdateDto studentDto = new()
            {
                StudentId = student.StudentId,
                StudentName = student.StudentName
            };
            if (student == null) return BadRequest();

            patchDto.ApplyTo(studentDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Student modelo = new()
            {
                StudentId = studentDto.StudentId,
                StudentName = studentDto.StudentName
            };
            _db.Students.Update(modelo);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
