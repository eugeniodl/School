using AutoMapper;
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
        private readonly ILogger<StudentsController> _logger;
        private readonly SchoolContext _db;
        private readonly IMapper _mapper;

        public StudentsController(ILogger<StudentsController> logger, SchoolContext db, IMapper mapper)
        {
            _logger = logger;
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudents()
        {
            _logger.LogInformation("Obtener los Estudiantes");

            var studentList = await _db.Students.ToListAsync();

            return Ok(_mapper.Map<IEnumerable<StudentDto>>(studentList));
        }

        [HttpGet("{id:int}", Name = "GetStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StudentDto>> GetStudent(int id)
        {
            if(id == 0)
            {
                _logger.LogError($"Error al traer el Estudiante con Id {id}");
                return BadRequest();
            }
            var student = await _db.Students.FirstOrDefaultAsync(s => s.StudentId == id);

            if(student == null)
            {
                _logger.LogError($"Error al traer el Estudiante con Id {id}");
                return NotFound();
            }

            return Ok(_mapper.Map<StudentDto>(student));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<StudentDto>> AddStudent([FromBody] StudentCreateDto studentDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(await _db.Students.FirstOrDefaultAsync(s => s.StudentName.ToLower() == studentDto.StudentName.ToLower()) != null)
            {
                ModelState.AddModelError("NombreExiste", "¡El Estudiante con ese Nombre ya existe!");
                return BadRequest(ModelState);
            }

            if(studentDto == null)
            {
                return BadRequest(studentDto);
            }

            Student modelo = _mapper.Map<Student>(studentDto);

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
            if(id == 0)
            {
                return BadRequest();
            }

            var student = await _db.Students.FirstOrDefaultAsync(s => s.StudentId == id);

            if(id == null)
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
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] StudentUpdateDto studentDTO)
        {
            if(studentDTO == null || id != studentDTO.StudentId)
            {
                return BadRequest();
            }

            Student modelo = _mapper.Map<Student>(studentDTO);

            _db.Students.Update(modelo);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialStudent(int id, JsonPatchDocument<StudentUpdateDto> patchDto)
        {
            if(patchDto == null || id == 0)
            {
                return BadRequest();
            }

            var student = await _db.Students.AsNoTracking().FirstOrDefaultAsync(s => s.StudentId == id);

            StudentUpdateDto studentDto = _mapper.Map<StudentUpdateDto>(student);

            if(student == null) return BadRequest();

            patchDto.ApplyTo(studentDto, ModelState);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Student modelo = _mapper.Map<Student>(studentDto);

            _db.Students.Update(modelo);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
