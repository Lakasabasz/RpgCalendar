using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using RpgCalendar.Database.Models;

namespace RpgCalendar.Database;

public class RelationalDb: DbContext
{
    public DbSet<User> Users { get; set; }

    public RelationalDb(){}
    public RelationalDb(DbContextOptions options): base(options){}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if(optionsBuilder.IsConfigured) return;

        optionsBuilder.UseMySql(ServerVersion.Create(new Version(11, 5), ServerType.MariaDb));
        base.OnConfiguring(optionsBuilder);
    }
}