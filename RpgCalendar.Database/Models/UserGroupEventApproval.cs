using Microsoft.EntityFrameworkCore;
using RpgCalendar.Tools.Enums;

namespace RpgCalendar.Database.Models;

[PrimaryKey(nameof(GroupEventId), nameof(UserId))]
public class UserGroupEventApproval
{
    public Guid GroupEventId { get; set; }
    public GroupEvent GroupEvent { get; set; } = null!;
    
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    public RelationTowardsEventEnum RelationTowardsEvent { get; set; }
}