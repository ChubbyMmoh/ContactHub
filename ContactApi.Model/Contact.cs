using System;
using System.ComponentModel.DataAnnotations;

namespace ContactApi.Model
{
    public class Contact
    {
        public Guid Id { get; set; }

        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "The Full Name field is required.")]
        public string FullName { get; set; }

        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "The Email Address field is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "The Phone Number field is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string Phone { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Photo URL")]
        [Url(ErrorMessage = "Invalid URL.")]
        public string PhotoUrl { get; set; }
    }
}
