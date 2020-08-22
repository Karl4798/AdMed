using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AdMedAPI.Models;

namespace AdMedAPI.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Application> Applications { get; set; }
        public DbSet<EmergencyContact> EmergencyContacts { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
