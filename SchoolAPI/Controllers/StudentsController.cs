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
    public class StudentsController : ControllerBase
    {
        private readonly ILogger<StudentsController> _logger;
        private readonly IStudentRepository _studentRepo;
        private readonly IMapper _mapper;

        public StudentsController(ILogger<StudentsController> logger, IStudentRepository studentRepo, IMapper mapper)
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
            _logger.LogInformation("Obtener los estudiantes");

            var studentList = await _studentRepo.GetAll();

            return Ok(_mapper.Map<IEnumerable<StudentDto>>(studentList));
        }

        [HttpGet("{id:int}", Name ="GetStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StudentDto>> GetStudent(int id)
        {
            if(id == 0)
            {
                _logger.LogError($"Error al traer Estudiante con Id {id}");
                return BadRequest();
            }
            var student = await _studentRepo.Get(s => s.StudentId == id);

            if(student == null)
            {
                _logger.LogError($"Error al traer Estudiante con Id {id}");
                return NotFound();
            }

            return Ok(_mapper.Map<StudentDto>(student));
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<StudentDto>> AddStudent([FromBody] StudentCreateDto studentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _studentRepo.Get(s => s.StudentName.ToLower() == studentDto.StudentName.ToLower()) != null)
            {
                ModelState.AddModelError("NombreExiste", "¡El Estudiante con ese Nombre ya existe!");
                return BadRequest(ModelState);
            }

            if (studentDto == null)
            {
                return BadRequest(studentDto);
            }

            Student modelo = _mapper.Map<Student>(studentDto);

            //Student modelo = new()
            //{
            //    StudentName = studentDto.StudentName
            //};

            await _studentRepo.Create(modelo);

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

            var student = await _studentRepo.Get(s => s.StudentId == id);

            if (id == null)
            {
                return NotFound();
            }

            await _studentRepo.Remove(student);

            return NoContent();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] StudentUpdateDto studentDTO)
        {
            if (studentDTO == null || id != studentDTO.StudentId)
            {
                return BadRequest();
            }

            Student modelo = _mapper.Map<Student>(studentDTO);

            //Student modelo = new()
            //{
            //    StudentId = studentDTO.StudentId,
            //    StudentName = studentDTO.StudentName
            //};

            await _studentRepo.Update(modelo);

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

            StudentUpdateDto studentDto = _mapper.Map<StudentUpdateDto>(student);

            //StudentUpdateDto studentDto = new()
            //{
            //    StudentId = student.StudentId,
            //    StudentName = student.StudentName
            //};

            if (student == null) return BadRequest();

            patchDto.ApplyTo(studentDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Student modelo = _mapper.Map<Student>(studentDto);

            //Student modelo = new()
            //{
            //    StudentId = studentDto.StudentId,
            //    StudentName = studentDto.StudentName
            //};

            await _studentRepo.Update(modelo);

            return NoContent();
        }

    }
}
