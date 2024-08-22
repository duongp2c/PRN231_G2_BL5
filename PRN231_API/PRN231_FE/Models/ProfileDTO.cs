using System.ComponentModel.DataAnnotations;

namespace PRN231_FE.Models
{
    public class ProfileDTO
    {
        [Required(ErrorMessage = "Empty Name")]
        public string? name { get; set; }
        [Required]
        public int? age { get; set; }
        [Required]
        public string? address { get; set; }
        public string? additionalInfo { get; set; }
        [Required]
        public string? phone { get; set; }
        public string? image { get; set; }
    }
}
