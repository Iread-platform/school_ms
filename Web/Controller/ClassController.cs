using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using iread_school_ms.Web.Service;
using System.Threading.Tasks;
using iread_school_ms.DataAccess.Data.Entity;
using iread_school_ms.Web.Util;
using iread_school_ms.Web.Dto.Class;
using iread_school_ms.Web.Dto.User;
using System;
using iread_school_ms.Web.Dto.UserDto;
using iread_school_ms.Web.Dto.Notification;
using iread_school_ms.Web.Dto.Topic;
using iread_school_ms.DataAccess.Data.Type;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

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
            ValidationLogicForAddStudent(studentMember);

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

            // NOTIFICATION_MS
            // Add student for the first time to notifications ms db.
            // subscribe to the class topic. 
            try
            {
                AddNotificationUserDto addNotificationUserDto = AddUser(studentMember.MemberId).GetAwaiter().GetResult();
                TopicSubscribeDto topicSubscribeDto = subscribeToTopic(NotificationUtil.ClassTopicTitle(studentMember.Class), new List<string>() { studentMember.MemberId }).GetAwaiter().GetResult();

            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.ToString());
            }
            return NoContent();
        }

        [HttpPut("edit-student-class")]
        public IActionResult EditStudentClass([FromBody] UpdateStudentClassDto student)
        {
            if (student == null)
            {
                return BadRequest();
            }

            ClassMember oldClassMember =
                _classService.GetClassMemberById(student.ClassMemberId).GetAwaiter().GetResult();

            ValidationLogicForUpdateStudentClass(oldClassMember, student);

            if (ModelState.ErrorCount != 0)
            {
                return BadRequest(ErrorMessage.ModelStateParser(ModelState));
            }

            oldClassMember.ClassId = student.ClassId;

            _classService.UpdateClassInClassMember(oldClassMember);

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
            ValidationLogicForAddTeacher(teacherMember);

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

            // NOTIFICATION_MS
            // Add teacher for the first time to notifications ms db.
            // subscribe to the class teacher topic. 
            try
            {
                AddNotificationUserDto addNotificationUserDto = AddUser(teacherMember.MemberId).GetAwaiter().GetResult();
                TopicSubscribeDto topicSubscribeDto =  subscribeToTopic(NotificationUtil.ClassTeachersTopicTitle(teacherMember.Class), new List<string>() { teacherMember.MemberId }).GetAwaiter().GetResult();

            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.ToString());
            }
            return NoContent();
        }

        [HttpPut("edit-teacher-class")]
        public IActionResult EditTeacherClass([FromBody] UpdateTeacherClassDto teacher)
        {
            if (teacher == null)
            {
                return BadRequest();
            }

            ClassMember oldClassMember =
                _classService.GetClassMemberById(teacher.ClassMemberId).GetAwaiter().GetResult();

            ValidationLogicForUpdateTeacherClass(oldClassMember, teacher);

            if (ModelState.ErrorCount != 0)
            {
                return BadRequest(ErrorMessage.ModelStateParser(ModelState));
            }

            oldClassMember.ClassId = teacher.ClassId;

            _classService.UpdateClassInClassMember(oldClassMember);

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


        private void ValidationLogicForAddTeacher(ClassMember member)

        {
            UserDto user = _consulHttpClient.GetAsync<UserDto>("identity_ms", $"/api/Identity/{member.MemberId}/get")
                .Result;
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

            Class classObj = _classService.GetById((int)member.ClassId, true).GetAwaiter().GetResult();
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

        private void ValidationLogicForAddStudent(ClassMember member)
        {
            UserDto user = _consulHttpClient.GetAsync<UserDto>("identity_ms", $"/api/Identity/{member.MemberId}/get")
                .GetAwaiter().GetResult();

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

            Class classObj = _classService.GetById((int)member.ClassId, true).GetAwaiter().GetResult();
            if (classObj == null)
            {
                ModelState.AddModelError("Class", "Class not found");
            }
            else
            {
                if (_classService.ExistsStudent(member.MemberId))
                {
                    ModelState.AddModelError("Member", "User already exists in author class");
                }
                else
                {
                    member.Class = classObj;
                }
            }
        }

        private void ValidationLogicForUpdateStudentClass(ClassMember oldClassMember, UpdateStudentClassDto student)
        {
            if (oldClassMember == null)
            {
                ModelState.AddModelError("ClassMember", "Class member not found.");
            }
            else
            {
                if (oldClassMember.ClassMembershipType != Policies.Student)
                {
                    ModelState.AddModelError("Role", "This class member is not for a student account.");
                }
            }

            Class classObj = _classService.GetById(student.ClassId, true).GetAwaiter().GetResult();
            if (classObj == null)
            {
                ModelState.AddModelError("Class", "Class not found");
            }

            if (classObj != null && oldClassMember != null)
            {
                if (classObj.Members.Find(m => m.MemberId == oldClassMember.MemberId) != null)
                {
                    ModelState.AddModelError("Member", "User already exists in this class");
                }
            }
        }

        private void ValidationLogicForUpdateTeacherClass(ClassMember oldClassMember, UpdateTeacherClassDto teacher)
        {
            if (oldClassMember == null)
            {
                ModelState.AddModelError("ClassMember", "Class member not found.");
            }
            else
            {
                if (oldClassMember.ClassMembershipType != Policies.Teacher)
                {
                    ModelState.AddModelError("Role", "This class member is not for a teacher account.");
                }
            }

            Class classObj = _classService.GetById(teacher.ClassId, true).GetAwaiter().GetResult();
            if (classObj == null)
            {
                ModelState.AddModelError("Class", "Class not found");
            }

            if (classObj != null && oldClassMember != null)
            {
                if (classObj.Members.Find(m => m.MemberId == oldClassMember.MemberId) != null)
                {
                    ModelState.AddModelError("Member", "User already exists in this class");
                }
            }
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
            Regex rgx = new Regex(@"[a-zA-Z0-9-_.~%]+");
            var cahrs = topicName.Where((character) => rgx.IsMatch(character.ToString()));
            string processedName = new string(cahrs.ToArray());
            AddTopicDto response = new AddTopicDto() { Title = processedName };
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