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

        public async Task<School> GetById(int id)
        {
            return await _context.Schools.FirstOrDefaultAsync(a => a.SchoolId == id);
        }

        public void Insert(School audio)
        {
            _context.Schools.Add(audio);
            _context.SaveChangesAsync();
        }

        public void Delete(int id)
        {
            var schoolToRemove = new School() { SchoolId = id };
            _context.Schools.Attach(schoolToRemove);
            _context.Schools.Remove(schoolToRemove);
            _context.SaveChangesAsync();
        }

        public bool Exists(int id)
        {
            return _context.Schools.Any(a => a.SchoolId == id);
        }

        public void Update(School school, School oldSchool)
        {
            _context.Entry(oldSchool).State = EntityState.Deleted;
            _context.Schools.Attach(school);
            _context.Entry(school).State = EntityState.Modified;
            _context.SaveChanges();
        }

    }
}