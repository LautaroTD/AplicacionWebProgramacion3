using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AplicacionWebProgramacion3.DTOs
{
    public class RegisterDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PasswordForVerification { get; set; } = string.Empty;

        public string Role { get; set; } = "basico";
        public string Imagen { get; set; } = string.Empty;
    }
}
