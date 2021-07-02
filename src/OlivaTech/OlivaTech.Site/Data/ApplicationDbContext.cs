using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OlivaTech.Site.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OlivaTech.Site.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUserCustom>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
    }
}
