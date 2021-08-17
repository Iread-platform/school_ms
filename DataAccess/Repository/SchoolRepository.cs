﻿using System.Linq;
using System.Threading.Tasks;
using iread_school_ms.DataAccess.Data;
using iread_school_ms.DataAccess.Data.Entity;
using iread_school_ms.DataAccess.Interface;
using Microsoft.EntityFrameworkCore;

namespace iread_school_ms.DataAccess.Repository
{
    public class SchoolRepository : ISchoolRepository
    {
        private readonly AppDbContext _context;

        public SchoolRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<School> GetById(int id, bool includeClasses)
        {
            if (includeClasses)
                return await _context.Schools.Include(s => s.Classes)
                .FirstOrDefaultAsync(s => s.SchoolId == id && !s.Archived);

            return await _context.Schools
            .FirstOrDefaultAsync(s => s.SchoolId == id && !s.Archived);
        }

        public void Insert(School audio)
        {
            _context.Schools.Add(audio);
            _context.SaveChangesAsync();
        }

        public void Delete(School school)
        {
            _context.Schools.Remove(school);
            _context.SaveChanges();
        }

        public bool Exists(int id)
        {
            return _context.Schools.Any(s => s.SchoolId == id && !s.Archived);
        }

        public void Update(School school, School oldSchool)
        {
            _context.Entry(oldSchool).State = EntityState.Deleted;
            _context.Schools.Attach(school);
            _context.Entry(school).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Archive(School school)
        {
            school.Archived = true;
            _context.SaveChanges();
        }
    }
}