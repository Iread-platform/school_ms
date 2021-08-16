using iread_school_ms.DataAccess.Data;
using iread_school_ms.DataAccess.Interface;

namespace iread_school_ms.DataAccess.Repository
{
    public class PublicRepository : IPublicRepository
    {
        private readonly AppDbContext _context;
        private ISchoolRepository _schoolRepository;
        private IClassRepository _classRepository;
        private IClassMemberRepository _classMemberRepository;


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
        public IClassRepository GetClassRepository
        {
            get
            {
                return _classRepository ??= new ClassRepository(_context);
            }
        }

        public IClassMemberRepository GetClassMemberRepository
        {
            get
            {
                return _classMemberRepository ??= new ClassMemberRepository(_context);
            }

        }
    }
}