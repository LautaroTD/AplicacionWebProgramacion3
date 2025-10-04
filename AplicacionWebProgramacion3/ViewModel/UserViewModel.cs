using Microsoft.AspNetCore.Mvc.Rendering;

namespace AplicacionWebProgramacion3.ViewModel
{
    public class UserViewModel
    {
        public string Role { get; set; }

        public List<SelectListItem> Roles { get; set; }
    }

}
