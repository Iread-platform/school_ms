﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using iread_interaction_ms.Web.Service;
using iread_school_ms.Web.Service;
using System.Threading.Tasks;
using iread_school_ms.DataAccess.Data.Entity;
using iread_school_ms.Web.Util;
using iread_school_ms.Web.Dto.Class;
using iread_school_ms.Web.Dto.User;
using System;
using iread_school_ms.Web.Dto.UserDto;
using iread_school_ms.DataAccess.Data.Type;
using System.Collections.Generic;

namespace iread_school_ms.Web.Controller
{

    [ApiController]
    [Route("api/School/[controller]/")]
    public class ClassController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ClassService _classService;
        private readonly SchoolService _schoolService;
        private readonly IConsulHttpClientService _consulHttpClient;

        public ClassController(ClassService classService, IMapper mapper,
        SchoolService schoolService,
          IConsulHttpClientService consulHttpClient)
        {
            _classService = classService;
            _mapper = mapper;
            _schoolService = schoolService;
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


        // GET: api/School/Class/all
        [HttpGet("all")]
        public async Task<IActionResult> GetArchived()
        {
            var classes = await _classService.GetAll();

            if (classes == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<List<InnerClassDto>>(classes));
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

            // register as school member
            SchoolMember schoolMember = _mapper.Map<SchoolMember>(studentMember);
            schoolMember.SchoolId = studentMember.Class.SchoolId;
            schoolMember.SchoolMembershipType = SchoolMembershipType.Student.ToString();
            _schoolService.AddMember(schoolMember);


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

            // register as school member
            SchoolMember schoolMember = _mapper.Map<SchoolMember>(teacherMember);
            schoolMember.SchoolId = teacherMember.Class.SchoolId;
            schoolMember.SchoolMembershipType = SchoolMembershipType.Teacher.ToString();
            _schoolService.AddMember(schoolMember);

            return NoContent();
        }

        //Put: api/class/5/archive
        [HttpPut("{id}/archive")]
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
                ModelState.AddModelError("Archive", "class already archived");
                return BadRequest(ErrorMessage.ModelStateParser(ModelState));
            }

            _classService.Archive(classObj, true);
            return NoContent();
        }

        //Put: api/class/5/unarchive
        [HttpPut("{id}/unarchive")]
        public IActionResult Unarchive([FromRoute] int id)
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
            if (!classObj.Archived)
            {
                ModelState.AddModelError("Archive", "class already unarchived");
                return BadRequest(ErrorMessage.ModelStateParser(ModelState));
            }

            _classService.Archive(classObj, false);
            return NoContent();
        }



        private void ValidationLogicForAddMember(ClassMember member)

        {

            UserDto user = _consulHttpClient.GetAsync<UserDto>("identity_ms", $"/api/Identity/{member.MemberId}/get").Result;
            if (user == null)
            {
                ModelState.AddModelError("User", "User not found");
            }
            else
            {
                if (user.Role != member.ClassMembershipType)
                {
                    ModelState.AddModelError("User", $"User not {member.ClassMembershipType}");
                }
                else
                {
                    member.FirstName = user.FirstName;
                    member.LastName = user.LastName;
                }

            }

            Class classObj = _classService.GetById((int)member.ClassId, true).Result;
            if (classObj == null)
            {
                ModelState.AddModelError("Class", "Class not found");
            }
            else
            {
                if (classObj.Members.Find(m => m.MemberId == member.MemberId) != null)
                {
                    ModelState.AddModelError("Member", "User already exists in this class");
                }
                else
                {
                    member.Class = classObj;
                }

            }
        }


    }

}
