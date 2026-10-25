using CampusNova.Domain.Professor.Record;

namespace CampusNova.Domain.Professor.Interfaces
{
    public interface IProfessorRepository
    {
        Task<SubjectsResultRecord> GetSubjects(CancellationToken ct = default);
    }
}
