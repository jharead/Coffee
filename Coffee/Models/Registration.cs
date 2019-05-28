using System;
using System.ComponentModel.DataAnnotations;

namespace Coffee.Models
{
    public class Registration
    {

        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public string Role { get; set; }


    }
}
