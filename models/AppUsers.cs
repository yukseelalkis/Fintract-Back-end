using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace api.models
{
    // IdentityUser sınıfı zaten Username, Email, PhoneNumber gibi temel alanları içerir.

    public class AppUsers : IdentityUser
    {
        public List<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
    }
}