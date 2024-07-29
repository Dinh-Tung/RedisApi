using ApiRedis.Models;

using Microsoft.EntityFrameworkCore;

namespace ApiRedis.DatabaseContext;

public class DbContextClass : DbContext
{
  public DbContextClass(DbContextOptions<DbContextClass> options) : base(options)
  {
    
  }
  public DbSet<Products> Products { get; set; }
}