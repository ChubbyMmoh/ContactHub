using System.ComponentModel.DataAnnotations;

namespace ContactApi.Model
{
    public class UpdateContactRequest
    {
        [Required(ErrorMessage = "Full Name is required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Url(ErrorMessage = "Invalid URL")]
        public string PhotoUrl { get; set; }
    }

}
