using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using iread_school_ms.Web.Service;
using System.Threading.Tasks;
using iread_school_ms.DataAccess.Data.Entity;
using iread_school_ms.Web.Dto.School;
using iread_school_ms.Web.Util;
using System.Collections.Generic;
using iread_school_ms.Web.Dto.Class;
using iread_school_ms.Web.Dto.User;
using iread_school_ms.DataAccess.Data.Type;
using System;
using System.Linq;
using iread_school_ms.Web.Dto.SchoolMembers;
using iread_school_ms.Web.Dto.UserDto;
using iread_school_ms.Web.Dto.Notification;
using iread_school_ms.Web.Dto.Topic;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;


namespace iread_school_ms.Web.Controller
{

    [ApiController]
    [Route("api/[controller]/")]
    public class SchoolController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly SchoolService _schoolService;
        private readonly ClassService _classService;
        private readonly IConsulHttpClientService _consulHttpClient;

        public SchoolController(SchoolService schoolService, ClassService classService, IMapper mapper,
          IConsulHttpClientService consulHttpClient)
        {
            _schoolService = schoolService;
            _classService = classService;
            _mapper = mapper;
            _consulHttpClient = consulHttpClient;
        }


        // GET: api/School/all
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var schools = await _schoolService.GetAll();

            if (schools == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<List<SchoolDto>>(schools));
        }


        // GET: api/School/get/1
        [HttpGet("get/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            School school = await _schoolService.GetById(id, true);

            if (school == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<SchoolDto>(school));
        }

        // GET: api/School/archived
        [HttpGet("get/archived")]
        public async Task<IActionResult> GetArchived()
        {
            var schools = await _schoolService.GetArchived();

            if (schools == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<List<SchoolDto>>(schools));
        }

        // GET: api/School/1/manager/get
        [HttpGet("{id}/manager/get")]
        public async Task<IActionResult> GetManager([FromRoute] int id)
        {
            var managers = await _schoolService.GetManagers(id);

            if (managers == null || managers.Count == 0)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<List<SchoolMemberDto>>(managers));
        }

        // GET: api/School/1/student/get
        [HttpGet("{id}/student/get")]
        public async Task<IActionResult> GetStudents([FromRoute] int id)
        {
            List<SchoolClassMemberDto> students = await _schoolService.GetStudents(id);

            if (students == null || students.Count == 0)
            {
                return NotFound();
            }

            return Ok(students);
        }


        // GET: api/School/1/teacher/get
        [HttpGet("{id}/teacher/get")]
        public async Task<IActionResult> GetTeachers([FromRoute] int id)
        {
            List<SchoolClassMemberDto> teachers = await _schoolService.GetTeachers(id);

            if (teachers == null || teachers.Count == 0)
            {
                return NotFound();
            }

            return Ok(teachers);
        }

        // GET: api/School/teacher/get
        [HttpGet("teacher/get")]
        [Authorize(Roles = Policies.SchoolManager, AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetTeachersByManagerToken()
        {
            int schoolId = int.Parse(User.Claims.Where(c => c.Type == "SchoolId").Select(c => c.Value).SingleOrDefault());

            List<SchoolClassMemberDto> teachers = await _schoolService.GetTeachers(schoolId);

            return Ok(teachers);
        }

        // GET: api/School/student/get
        [HttpGet("student/get")]
        [Authorize(Roles = Policies.SchoolManager, AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetStudentsByManagerToken()
        {
            int schoolId = int.Parse(User.Claims.Where(c => c.Type == "SchoolId").Select(c => c.Value).SingleOrDefault());

            List<SchoolClassMemberDto> students = await _schoolService.GetStudents(schoolId);

            return Ok(students);
        }

        // GET: api/School/1/class/all
        [HttpGet("{id}/class/all")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetClasses([FromRoute] int id)
        {
            var classes = await _classService.GetBySchool(id);

            if (classes == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<List<InnerClassDto>>(classes));
        }

        // GET: api/School/1/class/all
        [HttpGet("getByMemberId/{memberId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByMemberId([FromRoute] string memberId)
        {
            SchoolAndClassDto schoolMember = _schoolService.GetSchoolAndClassId(memberId);

            if (schoolMember == null)
            {
                return NotFound();
            }
            return Ok(schoolMember);
        }

        // POST: api/School/1/class/add
        [HttpPost("{id}/class/add")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddClassToSchool([FromBody] ClassCreateDto classObj, [FromRoute] int id)
        {

            School school = _schoolService.GetById(id, false).Result;

            if (school == null)
            {
                ModelState.AddModelError("School", "School not found");
                return BadRequest(ErrorMessage.ModelStateParser(ModelState));
            }

            Class classEntity = _mapper.Map<Class>(classObj);
            classEntity.SchoolId = id;
            _classService.Insert(classEntity);

            // NOTIFICATION_MS
            // Add school class topic
            try
            {
                AddTopicDto topic = await CreateTopic(NotificationUtil.ClassTopicTitle(classEntity));
                AddTopicDto teacherTopic = await CreateTopic(NotificationUtil.ClassTeachersTopicTitle(classEntity));
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.ToString());
            }
            return CreatedAtAction(nameof(ClassController.GetById), new { id = classEntity.ClassId }, _mapper.Map<ClassDto>(classEntity));
        }



        // POST: api/School/1/manager/add
        [HttpPost("{id}/manager/add")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult AddManagerToSchool([FromBody] ManagerDto manager, [FromRoute] int id)
        {

            if (manager == null)
            {
                return BadRequest();
            }

            SchoolMember managerMember = _mapper.Map<SchoolMember>(manager);
            managerMember.SchoolId = id;
            managerMember.SchoolMembershipType = SchoolMembershipType.SchoolManager.ToString();
            ValidationLogicForAddMember(managerMember);

            if (ModelState.ErrorCount != 0)
            {
                return BadRequest(ErrorMessage.ModelStateParser(ModelState));
            }

            _schoolService.AddMember(managerMember);

            return NoContent();
        }

        // POST: api/School/1/teacher/add
        [HttpPost("{id}/teacher/add")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddTeacherToSchool([FromBody] TeacherDto teacher, [FromRoute] int id)
        {

            if (teacher == null)
            {
                return BadRequest();
            }

            SchoolMember teacherMember = _mapper.Map<SchoolMember>(teacher);
            teacherMember.SchoolId = id;
            teacherMember.SchoolMembershipType = SchoolMembershipType.Teacher.ToString();
            ValidationLogicForAddMember(teacherMember);

            if (ModelState.ErrorCount != 0)
            {
                return BadRequest(ErrorMessage.ModelStateParser(ModelState));
            }

            _schoolService.AddMember(teacherMember);

            // NOTIFICATION_MS
            // Add teacher to database.
            // subscribe to the school topic
            try
            {
                AddNotificationUserDto addNotificationUserDto = await AddUser(teacherMember.MemberId);
                if (addNotificationUserDto != null && addNotificationUserDto.UserId != null)
                {
                    TopicSubscribeDto topic = await subscribeToTopic(NotificationUtil.SchoolTeachersTopicTitle(teacherMember.School), new List<string>() { teacherMember.MemberId });
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.ToString());
            }
            return NoContent();
        }

        // POST: api/School/1/student/add
        [HttpPost("{id}/student/add")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddStudentToSchool([FromBody] StudentDto student, [FromRoute] int id)
        {

            if (student == null)
            {
                return BadRequest();
            }

            SchoolMember studentMember = _mapper.Map<SchoolMember>(student);
            studentMember.SchoolId = id;
            studentMember.SchoolMembershipType = SchoolMembershipType.Student.ToString();
            ValidationLogicForAddMember(studentMember);

            if (ModelState.ErrorCount != 0)
            {
                return BadRequest(ErrorMessage.ModelStateParser(ModelState));
            }

            _schoolService.AddMember(studentMember);

            // NOTIFICATION_MS
            // Add student for the first time to notifications ms db.
            // supscripe to the school topics 
            try
            {
                AddNotificationUserDto addNotificationUserDto = await AddUser(studentMember.MemberId);
                TopicSubscribeDto topic = await subscribeToTopic(NotificationUtil.SchoolTopicTitle(studentMember.School), new List<string>() { studentMember.MemberId });

            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.ToString());
            }
            return NoContent();
        }

        private void ValidationLogicForAddMember(SchoolMember managerMember)
        {

            UserDto user = _consulHttpClient.GetAsync<UserDto>("identity_ms", $"/api/Identity/{managerMember.MemberId}/get").Result;
            if (user == null)
            {
                ModelState.AddModelError("User", "User not found");
            }
            else
            {
                if (user.Role != managerMember.SchoolMembershipType)
                {
                    ModelState.AddModelError("User", $"User not {managerMember.SchoolMembershipType}");
                }
                else
                {
                    managerMember.FirstName = user.FirstName;
                    managerMember.LastName = user.LastName;
                }

            }

            School school = _schoolService.GetById((int)managerMember.SchoolId, true).Result;
            if (school == null)
            {
                ModelState.AddModelError("School", "School not found");
            }
            else
            {
                if (school.Members.Find(m => m.MemberId == managerMember.MemberId) != null)
                {
                    ModelState.AddModelError("Member", "User already exists in this school");
                }
                else
                {
                    managerMember.School = school;
                }

            }
        }


        // DELETE: api/School/1/class/delete/1
        // [HttpDelete("{id}/class/delete/{classId}")]
        // public IActionResult Delete([FromRoute] int id, [FromRoute] int classId)
        // {

        //     School school = _schoolService.GetById(id, true).Result;
        //     Class classObj = _classService.GetById(classId, false).Result;


        //     if (school == null)
        //     {
        //         ModelState.AddModelError("School", "School not found");
        //     }

        //     if (classObj == null)
        //     {
        //         ModelState.AddModelError("Class", "Class not found");
        //     }

        //     if (ModelState.ErrorCount != 0)
        //     {
        //         return BadRequest(ErrorMessage.ModelStateParser(ModelState));
        //     }
        //     if (school.Classes.Find(c => c.ClassId == classObj.ClassId) == null)
        //     {
        //         ModelState.AddModelError("Class", "Class not related to this school");
        //         return BadRequest(ErrorMessage.ModelStateParser(ModelState));
        //     }

        //     _classService.Archive(classObj);

        //     return NoContent();
        // }


        //POST: api/School/add
        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] SchoolCreateDto schoolCreateDto)
        {
            if (schoolCreateDto == null)
            {
                return BadRequest();
            }

            School schoolEntity = _mapper.Map<School>(schoolCreateDto);

            _schoolService.Insert(schoolEntity);
            // NOTIFICATION_MS
            // create a topic for the school. 
            try
            {
                AddTopicDto topic = await CreateTopic(NotificationUtil.SchoolTopicTitle(schoolEntity));
                AddTopicDto tteachersTopic = await CreateTopic(NotificationUtil.SchoolTeachersTopicTitle(schoolEntity));
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.ToString());
            }
            return CreatedAtAction("GetById", new { id = schoolEntity.SchoolId }, _mapper.Map<SchoolDto>(schoolEntity));
        }

        [HttpPut("UpdateMemberInfo/{memberId}")]
        public async Task<IActionResult> UpdateMemberInfo([FromRoute] string memberId, [FromBody] UpdateMemberDto member)
        {

            if (member == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorMessage.ModelStateParser(ModelState));
            }

            SchoolMember memberEntity = _mapper.Map<SchoolMember>(member);

            SchoolMember oldMember = _schoolService.GetByMemberId(memberId).GetAwaiter().GetResult();
            if (oldMember == null)
            {
                return NotFound();
            }

            _schoolService.Update(memberEntity, oldMember);
            return NoContent();
        }

        [HttpPut("{id}/update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Update([FromBody] SchoolUpdateDto school, [FromRoute] int id)
        {
            if (school == null)
            {
                return BadRequest();
            }

            School oldSchool = _schoolService.GetById(id, false).Result;
            if (oldSchool == null)
            {
                return NotFound();
            }

            School schoolEntity = _mapper.Map<School>(school);

            schoolEntity.SchoolId = id;
            _schoolService.Update(schoolEntity, oldSchool);

            return NoContent();
        }


        //Put: api/School/5/archive
        [HttpPut("{id}/archive")]
        public IActionResult Archive([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorMessage.ModelStateParser(ModelState));
            }
            var school = _schoolService.GetById(id, true).Result;
            if (school == null)
            {
                return NotFound();
            }

            if (school.Archived)
            {
                ModelState.AddModelError("Archive", "school already archive");
                return BadRequest(ErrorMessage.ModelStateParser(ModelState));
            }

            _schoolService.Archive(school, true);
            return NoContent();
        }


        //Put: api/School/5/unarchive
        [HttpPut("{id}/unarchive")]
        public IActionResult Unarchive([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorMessage.ModelStateParser(ModelState));
            }
            var school = _schoolService.GetById(id, true).Result;
            if (school == null)
            {
                return NotFound();
            }

            if (!school.Archived)
            {
                ModelState.AddModelError("Archive", "school already unarchive");
                return BadRequest(ErrorMessage.ModelStateParser(ModelState));
            }

            _schoolService.Archive(school, false);
            return NoContent();
        }

        private async Task<SingletNotificationDto> SendSingleNotification(string title, string body, int userId, string message, string route)
        {
            SingletNotificationDto response = new SingletNotificationDto() { Body = body, UserId = 1, Title = title, ExtraData = new ExtraDataDto() { GoTo = route, Messsage = message } };
            response = await _consulHttpClient.PostBodyAsync<SingletNotificationDto>("notifications_ms", $"/api/Notification/Send",
             response);

            return response;
        }

        private async Task<TopicNotificationAddDto> SendTopicNotification(string title, string body, string topicName, string message, string route)
        {
            TopicNotificationAddDto response = new TopicNotificationAddDto() { Body = body, TopicName = topicName, Title = title, ExtraData = new ExtraDataDto() { GoTo = route, Messsage = message } };
            response = await _consulHttpClient.PostBodyAsync<TopicNotificationAddDto>("notifications_ms", $"/api/Notification/broadcast-by-topic-title",
             response);

            return response;
        }

        private async Task<AddTopicDto> CreateTopic(string topicName)
        {
            AddTopicDto response = new AddTopicDto() { Title = topicName };
            response = await _consulHttpClient.PostBodyAsync<AddTopicDto>("notifications_ms", $"/api/Topic/Add",
             response);

            return response;
        }

        private async Task<TopicSubscribeDto> subscribeToTopic(string topicName, List<string> users)
        {
            TopicSubscribeDto response = new TopicSubscribeDto() { TopicTitle = topicName, Users = users };
            response = await _consulHttpClient.PostBodyAsync<TopicSubscribeDto>("notifications_ms", $"/api/Topic/Subscribe",
             response);

            return response;
        }

        private async Task<AddNotificationUserDto> AddUser(string memberId)
        {
            AddNotificationUserDto response = new AddNotificationUserDto() { UserId = memberId };
            response = await _consulHttpClient.PostBodyAsync<AddNotificationUserDto>("notifications_ms", $"/api/User/Add",
             response);

            return response;
        }
    }
}