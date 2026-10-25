namespace CampusNova.Domain.Professor.Record
{
    public class SubjectRecord
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int Credits { get; set; }
        public int TotalStudents { get; set; }
        public int ProfessorId { get; set; }
        public int subjectId { get; set; }
        public string result { get; set; }
    }
}
