using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Angular_Authentication_with_JWT.Data
{
    public class ApplictionDbContext:IdentityDbContext
    {
        public ApplictionDbContext(DbContextOptions<ApplictionDbContext>options):base(options)
        {
                
        }
    }
}
