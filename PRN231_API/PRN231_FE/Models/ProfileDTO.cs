using System.ComponentModel.DataAnnotations;

namespace PRN231_FE.Models
{
    public class ProfileDTO
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the name")]
        [StringLength(maximumLength: 25, MinimumLength = 5, ErrorMessage = "Length must be between 5 to 25")]
        public string? name { get; set; }
        [Required]
        public int? age { get; set; }
        public string? address { get; set; }
        public string? additionalInfo { get; set; }
        [Phone(ErrorMessage = "Please enter a valid Phone number")]
        [StringLength(maximumLength: 15, MinimumLength = 7, ErrorMessage = "Length must be between 7 to 15")]
        public string? phone { get; set; }
        public string? image { get; set; }
    }
}
