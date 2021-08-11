using iread_school_ms.DataAccess.Data;
using iread_school_ms.DataAccess.Interface;

namespace iread_school_ms.DataAccess.Repository
{
    public class PublicRepository : IPublicRepository
    {
        private readonly AppDbContext _context;
        private ISchoolRepository _schoolRepository;


        public PublicRepository(AppDbContext context)
        {
            _context = context;
        }

        public ISchoolRepository GetSchoolRepo
        {
            get
            {
                return _schoolRepository ??= new SchoolRepository(_context);
            }
        }
    }
}