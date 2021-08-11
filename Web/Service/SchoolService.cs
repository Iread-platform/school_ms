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

        public async Task<School> GetById(int id)
        {
            return await _publicRepository.GetSchoolRepo.GetById(id);
        }

        public bool Insert(School school)
        {
            try
            {
                _publicRepository.GetSchoolRepo.Insert(school);
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public void Delete(School school)
        {
            _publicRepository.GetSchoolRepo.Delete(school.SchoolId);
        }

        internal void Update(School schoolEntity, School oldSchool)
        {
            _publicRepository.GetSchoolRepo.Update(schoolEntity, oldSchool);
        }

    }
}