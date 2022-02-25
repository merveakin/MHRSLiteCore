using MHRSLiteEntityLayer.IdentityModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MHRSLiteUI.ClaimProvider
{
    public class ClaimProvider : IClaimsTransformation
    {
        private UserManager<AppUser> _userManager { get; set; }
        public ClaimProvider(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal != null && principal.Identity.IsAuthenticated)
            {
                ClaimsIdentity identity = principal.Identity as ClaimsIdentity;
                AppUser user = await _userManager.FindByNameAsync(identity.Name);
                if (user != null)
                {
                    if (!principal.HasClaim(c => c.Type == "gender"))
                    {
                        //prensiplerde gender diye tanımlı bir claim yoksa ekle
                        Claim genderClaim = new Claim("gender", user.Gender.ToString(), ClaimValueTypes.Integer32, "Internal");
                        identity.AddClaim(genderClaim);
                    }
                }
            }
            return principal;
        }
    }
}
