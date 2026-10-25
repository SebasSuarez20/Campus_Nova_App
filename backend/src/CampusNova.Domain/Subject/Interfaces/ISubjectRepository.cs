using CampusNova.Domain.Subject.Model;

namespace CampusNova.Domain.Subject.Interfaces
{
    public interface ISubjectRepository
    {
        Task InsertStudentSubject(string subjectCode, CancellationToken ct = default);
        Task DeleteStudentSubject(string subjectCode, CancellationToken ct = default);
        Task<StudentClassesResultRecord> GetStudentClasses(CancellationToken ct = default);
    }
}
