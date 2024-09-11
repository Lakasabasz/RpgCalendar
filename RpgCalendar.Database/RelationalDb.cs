using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using RpgCalendar.Database.Models;

namespace RpgCalendar.Database;

public class RelationalDb: DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<PrivateEvent> PrivateEvents { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupMember> GroupsMembers { get; set; }
    public DbSet<Invite> GroupsInvites { get; set; }
    public DbSet<BlacklistUser> BlacklistUsers { get; set; }
    public DbSet<BlacklistGroup> BlacklistGroups { get; set; }
    public DbSet<GroupEvent> GroupEvents { get; set; }
    public DbSet<UserGroupEventApproval> UserGroupEventApprovals { get; set; }

    public RelationalDb(){}
    public RelationalDb(DbContextOptions options): base(options){}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        if(!optionsBuilder.IsConfigured) 
            optionsBuilder.UseMySql(ServerVersion.Create(new Version(11, 5), ServerType.MariaDb));
    }
}