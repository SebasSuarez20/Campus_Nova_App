namespace CampusNova.Domain.Professor.Record
{
    public class SubjectsResultRecord
    {
        public IEnumerable<SubjectRecord> Subjects { get; set; } = Enumerable.Empty<SubjectRecord>();
        public IEnumerable<ProfessorRecord> Professors { get; set; } = Enumerable.Empty<ProfessorRecord>();
    }
}
