using CampusNova.Domain.Student.Record;

namespace CampusNova.Domain.Student.Interfaces
{
    public interface IStudentRepository
    {
        public Task<IEnumerable<GetStudentRecord>> GetStudent();
        public Task<int> CreateStudent(CampusNova.Domain.Student.Model.Student model, long institutionCode, string passwordHash, CancellationToken ct = default);
        public Task<bool> UpdateStudent(CampusNova.Domain.Student.Model.Student model, string? passwordHash, CancellationToken ct = default);
        public Task<bool> DeleteStudent(CancellationToken ct = default);
    }
}
