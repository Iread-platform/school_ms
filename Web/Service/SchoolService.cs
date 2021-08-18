using System;
using System.Collections.Generic;
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

        public void Archive(School school)
        {
            // archive the school and it's classes
            _publicRepository.GetClassRepository.ArchiveBySchool(school.SchoolId);
            _publicRepository.GetSchoolRepo.Archive(school);
        }

        internal void Update(School schoolEntity, School oldSchool)
        {
            _publicRepository.GetSchoolRepo.Update(schoolEntity, oldSchool);
        }

        public async Task<List<School>> GetArchived()
        {
            return await _publicRepository.GetSchoolRepo.GetArchived();
        }

        public async Task<List<School>> GetAll()
        {
            return await _publicRepository.GetSchoolRepo.GetAll();
        }
    }
}