namespace CampusNova.Domain.Subject.Model
{
    public class StudentClassesResultRecord
    {
        public IEnumerable<StudentClassRecord> Subjects { get; set; } = Enumerable.Empty<StudentClassRecord>();
        public IEnumerable<ClassmateRecord> Students { get; set; } = Enumerable.Empty<ClassmateRecord>();
    }
}
