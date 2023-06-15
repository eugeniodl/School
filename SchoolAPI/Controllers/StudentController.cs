using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.Models;
using SchoolAPI.Models.Dto;
using SchoolAPI.Repository.IRepository;

namespace SchoolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;
        private readonly IStudentRepository _studentRepo;
        private readonly IMapper _mapper;

        public StudentController(ILogger<StudentController> logger, IStudentRepository studentRepo, IMapper mapper)
        {
            _logger = logger;
            _studentRepo = studentRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudents()
        {
            _logger.LogInformation("Obtener los Estudiantes");

            var studentList = await _studentRepo.GetAll();

            return Ok(_mapper.Map<IEnumerable<StudentDto>>(studentList));
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
            var student = await _studentRepo.Get(s => s.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<StudentDto>(student));
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

            if (await _studentRepo.Get(s => s.StudentName.ToLower() == studentCreateDto.StudentName.ToLower()) != null)
            {
                ModelState.AddModelError("NombreExiste", "¡El Estudiante con ese Nombre ya existe!");
                return BadRequest(ModelState);
            }

            if (studentCreateDto == null)
            {
                return BadRequest(studentCreateDto);
            }

            Student modelo = _mapper.Map<Student>(studentCreateDto);

            //Student modelo = new()
            //{
            //    StudentName = studentCreateDto.StudentName
            //};

            await _studentRepo.Add(modelo);

            return CreatedAtRoute("GetStudent", new { id = modelo.StudentId }, modelo);

        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var student = await _studentRepo.Get(s => s.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            _studentRepo.Remove(student);

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

            Student modelo = _mapper.Map<Student>(studentUpdateDto);

            //Student modelo = new()
            //{
            //    StudentId = studentUpdateDto.StudentId,
            //    StudentName = studentUpdateDto.StudentName
            //};

            _studentRepo.Update(modelo);

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

            var student = await _studentRepo.Get(s => s.StudentId == id, tracked:false);

            StudentUpdateDto studentUpdateDto = _mapper.Map<StudentUpdateDto>(student);
            //StudentUpdateDto studentUpdateDto = new()
            //{
            //    StudentId = student.StudentId,
            //    StudentName = student.StudentName
            //};
            if (student == null) return BadRequest();

            patchDto.ApplyTo(studentUpdateDto, ModelState);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Student modelo = _mapper.Map<Student>(studentUpdateDto);
            //Student modelo = new()
            //{
            //    StudentId = studentUpdateDto.StudentId,
            //    StudentName = studentUpdateDto.StudentName
            //};
            _studentRepo.Update(modelo);

            return NoContent();
        }
    }
}
