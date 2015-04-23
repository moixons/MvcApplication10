using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcApplication10.Models
{
    public class UserModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Please provide NAME", AllowEmptyStrings = false)]
        [StringLength(50)] //longitud maxima del campo
        [Display(Name = "Name ")] //Mensaje indicar obligatorio
        public string Name { get; set; }

        [Required(ErrorMessage = "Please provide EMAIL ADRESS", AllowEmptyStrings = false)]
        [EmailAddress(ErrorMessage = "Please provide valid email")] //Validar que se ingrese un email valido
        [StringLength(150)] //longitud maxima del campo
        [Display(Name = "Email address ")] //Mensaje indicar obligatorio
        public string Email { get; set; }

        [Required(ErrorMessage = "Please provide Password", AllowEmptyStrings = false)]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be 6 char long.")]
        public string Password { get; set; }

        //[Compare("Password", ErrorMessage = "Confirm password dose not match.")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> BirthDay { get; set; }

        public string Country { get; set; }
    }
}