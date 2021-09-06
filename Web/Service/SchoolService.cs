using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using iread_school_ms.DataAccess.Data.Entity;
using iread_school_ms.DataAccess.Interface;

namespace iread_school_ms.Web.Service
{
    public class SchoolService
    {
        private readonly IPublicRepository _publicRepository;

        public SchoolService(IPublicRepository publicRepository)
        {
            _publicRepository = publicRepository;
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

        internal async Task<List<SchoolMember>> GetStudents(int schoolId)
        {
            return await _publicRepository.GetSchoolMemberRepository.GetStudents(schoolId);

        }

        internal async Task<List<SchoolMember>> GetTeachers(int schoolId)
        {
            return await _publicRepository.GetSchoolMemberRepository.GetTeachers(schoolId);

        }

        public async Task<SchoolMember> GetByMemberId(string memberId)
        {
            return await _publicRepository.GetSchoolMemberRepository.GetByMemberId(memberId);
        }
    }
}