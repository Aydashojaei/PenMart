using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PenMart.Models
{
    public class ApplicationUser:IdentityUser
    {
        
        public DateTime RegisterDate { get; set; }
        public bool IsAdmin { get; set; }

        public List<Order> orders { get; set; } //Navigation Property
    }
}
