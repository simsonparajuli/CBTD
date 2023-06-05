using CBTD.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using CBTD.ApplicationCore.Models;

namespace CBTDWeb.Pages.Users
{
    public class UpdateModel : PageModel
    {

        private readonly UnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UpdateModel(UnitOfWork unitOfWork, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [BindProperty]
        public ApplicationUser AppUser { get; set; }
        public List<string> UsersRoles { get; set; }
        public List<string> AllRoles { get; set; }
        public List<string> OldRoles { get; set; }

        public async Task OnGet(string id)
        {
            AppUser = _unitOfWork.ApplicationUser.Get(u => u.Id == id);
            var roles = await _userManager.GetRolesAsync(AppUser);
            UsersRoles = roles.ToList();
            OldRoles = roles.ToList();
            AllRoles = _roleManager.Roles.Select(r => r.Name).ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var newRoles = Request.Form["roles"];
            UsersRoles = newRoles.ToList();
            var OldRoles = await _userManager.GetRolesAsync(AppUser);  //ones in DB
            var rolesToAdd = new List<string>();
            var user = _unitOfWork.ApplicationUser.Get(u => u.Id == AppUser.Id);

            user.FirstName = AppUser.FirstName;
            user.LastName = AppUser.LastName;
            user.Email = AppUser.Email;
            user.PhoneNumber = AppUser.PhoneNumber;
            _unitOfWork.ApplicationUser.Update(user);
            _unitOfWork.Commit();

            //update their roles

            foreach (var r in UsersRoles)
            {
                if (!OldRoles.Contains(r)) //new Role
                {
                    rolesToAdd.Add(r);
                }
            }

            foreach (var r in OldRoles)
            {
                if (!UsersRoles.Contains(r))  //remove
                {
                    var result = await _userManager.RemoveFromRoleAsync(user, r);
                }
            }

            var result1 = await _userManager.AddToRolesAsync(user, rolesToAdd.AsEnumerable());
            return RedirectToPage("./Index", new { sucess = true, message = "Update Successful" });
        }
    }
}
