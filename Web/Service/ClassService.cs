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

        public void Archive(Class classObj, bool archived)
        {
            _publicRepository.GetClassRepository.Archive(classObj, archived);
        }

        public void ArchiveBySchool(int schoolId)
        {
            _publicRepository.GetClassRepository.ArchiveBySchool(schoolId);
        }

        internal void Update(Class classEntity, Class oldEntity)
        {
            _publicRepository.GetClassRepository.Update(classEntity, oldEntity);
        }

        public async Task<List<Class>> GetBySchool(int schoolId)
        {
            return await _publicRepository.GetClassRepository.GetBySchool(schoolId);
        }

        public void AddMember(ClassMember classMember)
        {
            _publicRepository.GetClassMemberRepository.Insert(classMember);
        }

        public async Task<List<Class>> GetArchived(bool archived)
        {
            return await _publicRepository.GetClassRepository.GetArchived(archived);
        }

        public async Task<List<Class>> GetAll()
        {
            return await _publicRepository.GetClassRepository.GetAll();
        }

        public bool ExistsStudent(string memberId)
        {
            return  _publicRepository.GetClassRepository.ExistsStudent(memberId);
        }

        public async Task<ClassMember> GetClassMemberById(int classMemberId)
        {
            return await _publicRepository.GetClassMemberRepository.GetById(classMemberId);
        }

        public void UpdateClassInClassMember(ClassMember oldClassMember)
        {
            _publicRepository.GetClassMemberRepository.Update(oldClassMember);
        }
    }
}