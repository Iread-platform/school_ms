
namespace iread_school_ms.DataAccess.Interface
{
    public interface IPublicRepository
    {
        ISchoolRepository GetSchoolRepo { get; }
        IClassRepository GetClassRepository { get; }
        IClassMemberRepository GetClassMemberRepository { get; }
        ISchoolMemberRepository GetSchoolMemberRepository { get; }


    }
}