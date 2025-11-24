using Microsoft.AspNetCore.Authorization;

namespace WebAPIActors.Attributes
{
    public class JwtAuthAttribute : AuthorizeAttribute
    {
        public JwtAuthAttribute() { Policy = "DefaultPolicy"; }
    }
}
