using System.ComponentModel.DataAnnotations;

namespace PRN231_FE.Models
{
    public class GradeDTO
    {
        
        public string name { get; set; }
        public decimal? weight { get; set; }
        public decimal? grade { get; set; }
        public string additionalInfo { get; set; }
    }
}
