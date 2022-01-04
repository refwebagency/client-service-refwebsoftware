using System.ComponentModel.DataAnnotations;

namespace ClientService.Dtos
{
    public class CreateClientDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Company { get; set; }

        // public Meet 
    }
}