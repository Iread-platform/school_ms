using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using iread_school_ms.DataAccess.Data.Entity;
using iread_school_ms.DataAccess.Data.Type;
using iread_school_ms.DataAccess.Interface;

namespace iread_school_ms.Web.Service
{
    public class ClassService
    {
        private readonly IPublicRepository _publicRepository;

        public ClassService(IPublicRepository publicRepository)
        {
            _publicRepository = publicRepository;
        }

        public async Task<Class> GetById(int id, bool includeMambers)
        {
            return await _publicRepository.GetClassRepository.GetById(id, includeMambers);
        }

        public void Insert(Class classObj)
        {
            _publicRepository.GetClassRepository.Insert(classObj);
        }

        public void Delete(Class classObj)
        {
            _publicRepository.GetClassRepository.Delete(classObj);
        }

        internal void Update(Class classEntity, Class oldEntity)
        {
            _publicRepository.GetClassRepository.Update(classEntity, oldEntity);
        }

        public async Task<List<Class>> GetBySchool(int schoolId)
        {
            return await _publicRepository.GetClassRepository.GetBySchool(schoolId);
        }

        public void AddStudent(ClassMember studentMember)
        {
            studentMember.ClassMembershipType = ClassMembershipType.Student.ToString();
            _publicRepository.GetClassMemberRepository.Insert(studentMember);
        }
    }
}