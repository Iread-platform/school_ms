using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using iread_interaction_ms.Web.Service;
using iread_school_ms.Web.Service;
using System.Threading.Tasks;
using iread_school_ms.DataAccess.Data.Entity;
using iread_school_ms.Web.Dto.School;
using iread_school_ms.Web.Util;
using iread_school_ms.Web.Dto.Class;
using iread_school_ms.Web.Dto.User;
using System;
using iread_school_ms.Web.Dto.UserDto;
using iread_identity_ms.DataAccess.Data.Type;
using iread_school_ms.DataAccess.Data.Type;

namespace iread_school_ms.Web.Controller
{

    [ApiController]
    [Route("api/School/[controller]/")]
    public class ClassController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ClassService _classService;
        private readonly IConsulHttpClientService _consulHttpClient;

        public ClassController(ClassService classService, IMapper mapper,
          IConsulHttpClientService consulHttpClient)
        {
            _classService = classService;
            _mapper = mapper;
            _consulHttpClient = consulHttpClient;
        }

        // GET: api/School/Class/get/1
        [HttpGet("get/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            Class classObj = await _classService.GetById(id, true);

            if (classObj == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ClassDto>(classObj));
        }


        [HttpPut("{id}/add-student")]
        public IActionResult AddStudent([FromBody] StudentDto student, [FromRoute] int id)
        {
            if (student == null)
            {
                return BadRequest();
            }

            ClassMember studentMember = _mapper.Map<ClassMember>(student);
            studentMember.ClassId = id;
            studentMember.ClassMembershipType = ClassMembershipType.Student.ToString();
            ValidationLogicForAddMember(studentMember);

            if (ModelState.ErrorCount != 0)
            {
                return BadRequest(ErrorMessage.ModelStateParser(ModelState));
            }

            _classService.AddMember(studentMember);

            return NoContent();
        }



        [HttpPut("{id}/add-teacher")]
        public IActionResult AddTeacher([FromBody] TeacherDto teacher, [FromRoute] int id)
        {
            if (teacher == null)
            {
                return BadRequest();
            }

            ClassMember teacherMember = _mapper.Map<ClassMember>(teacher);
            teacherMember.ClassId = id;
            teacherMember.ClassMembershipType = ClassMembershipType.Teacher.ToString();
            ValidationLogicForAddMember(teacherMember);

            if (ModelState.ErrorCount != 0)
            {
                return BadRequest(ErrorMessage.ModelStateParser(ModelState));
            }

            _classService.AddMember(teacherMember);

            return NoContent();
        }



        //DELETE: api/class/5/archive
        [HttpDelete("{id}/archive")]
        public IActionResult Archive([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorMessage.ModelStateParser(ModelState));
            }
            var classObj = _classService.GetById(id, false).Result;
            if (classObj == null)
            {
                return NotFound();
            }
            if (classObj.Archived)
            {
                ModelState.AddModelError("Archive", "class is archived");
                return BadRequest(ErrorMessage.ModelStateParser(ModelState));
            }

            _classService.Archive(classObj);
            return NoContent();
        }



        private void ValidationLogicForAddMember(ClassMember student)
        {

            UserDto user = _consulHttpClient.GetAsync<UserDto>("identity_ms", $"/api/Identity/{student.MemberId}/get").Result;
            if (user == null)
            {
                ModelState.AddModelError("User", "User not found");
            }
            else
            {
                if (user.Role != student.ClassMembershipType)
                {
                    ModelState.AddModelError("User", $"User not {student.ClassMembershipType}");
                }
                else
                {
                    student.FirstName = user.FirstName;
                    student.LastName = user.LastName;
                }

            }

            Class classObj = _classService.GetById((int)student.ClassId, true).Result;
            if (classObj == null)
            {
                ModelState.AddModelError("Class", "Class not found");
            }
            else
            {
                if (classObj.Members.Find(m => m.MemberId == student.MemberId) != null)
                {
                    ModelState.AddModelError("Member", "User already exists in this class");
                }

            }
        }


    }

}
