using System.ComponentModel.DataAnnotations;

namespace PRN231_FE.Models
{
    public class ActivateViewModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string ActivationCode { get; set; }
    }
}
