﻿using System.Collections.Generic;
using System.Threading.Tasks;
using iread_school_ms.DataAccess.Data.Entity;

namespace iread_school_ms.DataAccess.Interface
{
    public interface ISchoolRepository
    {
        public Task<School> GetById(int id, bool includeClasses);

        public void Insert(School audio);

        public void Delete(School school);

        public void Archive(School school, bool archive);

        public bool Exists(int id);

        public void Update(School schoolEntity, School oldSchool);
        public Task<List<School>> GetArchived();
        public Task<List<School>> GetAll();
    }
}