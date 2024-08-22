namespace PRN231_FE.Models
{
    public class SubjectDTO
    {
        public string name { get; set; }
        public int subjectId { get; set; }
    }
    public class SubjectData
    {
        public List<SubjectDTO> subjects { get; set; }
    }
}
