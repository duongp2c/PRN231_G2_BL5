namespace PRN231_FE.Models
{
    public class CombinedViewModel
    {
        public List<SubjectViewModel> AllSubjects { get; set; }
        public List<SubjectOfAccount> SubjectsOfAccount { get; set; }
        public string AccountId { get; set; }
    }
}
