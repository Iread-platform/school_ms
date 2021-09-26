using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using iread_school_ms.DataAccess.Data.Entity;
using iread_school_ms.DataAccess.Interface;
using iread_school_ms.Web.Dto.Class;
using iread_school_ms.Web.Dto.School;
using iread_school_ms.Web.Dto.User;

namespace iread_school_ms.Web.Service
{
    public class SchoolService
    {
        private readonly IPublicRepository _publicRepository;
        private readonly IMapper _mapper;


        public SchoolService(IPublicRepository publicRepository,
        IMapper mapper)
        {
            _publicRepository = publicRepository;
            _mapper = mapper;
        }

        public async Task<School> GetById(int id, bool includeClasses)
        {
            return await _publicRepository.GetSchoolRepo.GetById(id, includeClasses);
        }

        public void Insert(School school)
        {
            _publicRepository.GetSchoolRepo.Insert(school);
        }

        public void Archive(School school, bool archive)
        {
            // archive the school and it's classes
            _publicRepository.GetClassRepository.ArchiveBySchool(school.SchoolId);
            _publicRepository.GetSchoolRepo.Archive(school, archive);
        }

        internal void Update(School schoolEntity, School oldSchool)
        {
            _publicRepository.GetSchoolRepo.Update(schoolEntity, oldSchool);
        }

        internal void Update(SchoolMember schoolMemberEntity, SchoolMember oldSchoolMember)
        {
            _publicRepository.GetSchoolMemberRepository.Update(schoolMemberEntity, oldSchoolMember);
        }

        public void AddMember(SchoolMember schoolMember)
        {
            _publicRepository.GetSchoolMemberRepository.Insert(schoolMember);
        }

        public async Task<List<School>> GetArchived()
        {
            return await _publicRepository.GetSchoolRepo.GetArchived();
        }

        public async Task<List<School>> GetAll()
        {
            return await _publicRepository.GetSchoolRepo.GetAll();
        }

        internal async Task<List<SchoolMember>> GetManagers(int schoolId)
        {
            return await _publicRepository.GetSchoolMemberRepository.GetManagers(schoolId);
        }

        internal async Task<List<SchoolClassMemberDto>> GetStudents(int schoolId)
        {
            var res = _mapper.Map<List<SchoolClassMemberDto>>(await _publicRepository.GetSchoolMemberRepository.GetStudents(schoolId));
            res.ForEach(s => s.Classes = _mapper.Map<List<InnerClassDto>>
            (_publicRepository.GetClassMemberRepository.GetByStudent(s.MemberId))
            );
            return res;
        }

        internal async Task<List<SchoolClassMemberDto>> GetTeachers(int schoolId)
        {

            var res = _mapper.Map<List<SchoolClassMemberDto>>(await _publicRepository.GetSchoolMemberRepository.GetTeachers(schoolId));
            res.ForEach(s => s.Classes = _mapper.Map<List<InnerClassDto>>
            (_publicRepository.GetClassMemberRepository.GetByTeacher(s.MemberId))
            );
            return res;
        }

        public SchoolAndClassDto GetSchoolAndClassId(string memberId)
        {
            return  _publicRepository.GetSchoolMemberRepository.GetSchoolAndClassId(memberId);
        }
        
        public async Task<SchoolMember> GetByMemberId(string memberId)
        {
            return await _publicRepository.GetSchoolMemberRepository.GetByMemberId(memberId);
        }
        
    }
}