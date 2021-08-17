using System;
using System.Threading.Tasks;
using iread_school_ms.DataAccess.Data.Entity;
using iread_school_ms.DataAccess.Interface;
using Microsoft.EntityFrameworkCore;

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

        public void Delete(School school)
        {
            _publicRepository.GetSchoolRepo.Delete(school);
        }

        internal void Update(School schoolEntity, School oldSchool)
        {
            _publicRepository.GetSchoolRepo.Update(schoolEntity, oldSchool);
        }

        public void AddMember(SchoolMember schoolMember)
        {
            _publicRepository.GetSchoolMemberRepository.Insert(schoolMember);
        }

    }
}