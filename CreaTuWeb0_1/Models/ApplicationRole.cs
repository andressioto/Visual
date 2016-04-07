using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CreaTuWeb0_1.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string name, string description)
            : base(name)
        {
            this.Description = description;
        }
        public ApplicationRole(string name) : base(name) { }
        public virtual string Description { get; set; }
    }
}