namespace CampusNova.Domain.Subject.Model
{
    public class ClassmateRecord
    {
        public string FullName { get; set; } = string.Empty;
        public string FullNameTeacher { get; set; } = string.Empty;
        public int IdFkSubject { get; set; }
    }
}
