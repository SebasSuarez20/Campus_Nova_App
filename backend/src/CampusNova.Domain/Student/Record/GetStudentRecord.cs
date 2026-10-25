namespace CampusNova.Domain.Student.Record
{
    public class GetStudentRecord
    {
        public int IdStudent { get; set; }
        public string FullName { get; set; } = string.Empty;
        public long Identification { get; set; }
        public string? Email { get; set; }
        public string SubjectInscription { get; set; } = string.Empty;
        public int Credits { get; set; }
        public bool IsMine { get; set; }
    }
}
