using Microsoft.EntityFrameworkCore;
using RpgCalendar.Commands.ApiModels;
using RpgCalendar.Database;
using RpgCalendar.Database.Models;
using RpgCalendar.Tools;
using RpgCalendar.Tools.Enums;

namespace RpgCalendar.Commands;

public class GroupService(RelationalDb db, ImageService imageService)
{
    private Guid? _groupId;
    private Group? _dbGroup;
    public Group DbGroup => _dbGroup ??= db.Groups.First(x => x.GroupId == _groupId);

    private Guid? _currentMemberId;
    private GroupMember? _currentMember;
    public GroupMember DbCurrentGroupMember => _currentMember ??= db.GroupsMembers
        .First(x => x.GroupId == _groupId && x.UserId == _currentMemberId);
    
    public IQueryable<MemberApiModel> MembersApiModels => db.GroupsMembers
        .Where(x => x.GroupId == _groupId)
        .Include(x => x.User)
        .Select(x => new MemberApiModel(x.User.Id, x.User.Nick, x.PermissionLevel));
    
    public GroupService SelectGroup(Guid groupId, Guid invokerId)
    {
        _groupId = groupId;
        _currentMemberId = invokerId;
        return this;
    }

    public ErrorCode? AddGroup(Guid systemId, Guid owner, string name, Guid? profilePictureId)
    {
        var group = Group.Prepare(owner, name, profilePictureId);
        db.Groups.Add(group);
        db.GroupsMembers.Add(GroupMember.Prepare(owner, group.GroupId, PermissionLevel.Owner, 
            GroupUserProfilePicture.CYCLOPE, "Żołnierz Gwiezdnej Floty", "Kurvinox", "Galaktyka Kurvix"));

        db.SaveChanges();

        _groupId = group.GroupId;
        _currentMemberId = group.OwnerId;
        return null;
    }

    public GroupFullApiModel GetFullApiModel()
    {
        var system = new ApiModels.System(Guid.Empty, "Kapitan Bomba");
        var hero = new GroupUserHeroProfile(DbCurrentGroupMember.HeroPicture, DbCurrentGroupMember.Class, 
            DbCurrentGroupMember.Race, DbCurrentGroupMember.Location);

        return new GroupFullApiModel(DbGroup.GroupId, DbGroup.OwnerId, DbGroup.Name,
            imageService.GetImageUrl(DbGroup.ProfilePicture), DbGroup.CreationDate,
            system, hero);
    }
    
    public MembersListApiModel GetMemberListApiModel()
    {
        return new MembersListApiModel(MembersApiModels, DbGroup.UserLimit);
    }
    
    public ErrorCode? AddMember(Guid memberId, bool save = true)
    {
        if (db.GroupsMembers.FirstOrDefault(x => x.GroupId == DbGroup.GroupId && x.UserId == memberId) is not null)
            return ErrorCode.UserAlreadyRegistered;
        
        if(db.GroupsMembers.Count(x => x.GroupId == DbGroup.GroupId) + 1 > DbGroup.UserLimit)
            return ErrorCode.MembersLimitReached;

        if (db.BlacklistGroups
            .FirstOrDefault(x => x.EntryOwnerId == memberId && x.BlacklistedGroupId == _groupId) is not null)
            return ErrorCode.CannotJoinBlacklistedGroup;

        db.GroupsMembers.Add(GroupMember.Prepare(memberId, DbGroup.GroupId, PermissionLevel.Member, GroupUserProfilePicture.GHOST_OF_ASS,
            "Sarumun", "Duch Dupy", "Labirynt Downa"));
        if(save) db.SaveChanges();
        return null;
    }

    public ErrorCode? Join(Guid inviteId, Guid invokerId)
    {
        var invite = db.GroupsInvites.FirstOrDefault(x => x.InviteId == inviteId);
        if (invite is null) return ErrorCode.InviteNotExists;

        SelectGroup(invite.GroupId, invokerId);
        
        var err = AddMember(invokerId, false);
        if (err is not null) return err;
        
        db.GroupsInvites.Remove(invite);
        db.SaveChanges();
        return null;
    }

    public ErrorCode? UpdateImage(Guid imageId, bool save = true)
    {
        if (imageService.Contains(imageId)) return ErrorCode.ImageNotExists;

        DbGroup.ProfilePicture = imageId;
        
        if(save) db.SaveChanges();
        return null;
    }

    public ErrorCode? UpdateName(string name, bool save = true)
    {
        DbGroup.Name = name;

        if (save) db.SaveChanges();
        return null;
    }

    public ErrorCode? TransferOwnership(Guid memberId, bool save = true)
    {
        var membership = db.GroupsMembers
            .FirstOrDefault(x => x.GroupId == _groupId && x.UserId == memberId);
        if(membership is null) return ErrorCode.MemberNotInGroup;
        
        var newOwner = db.Users.First(x => x.Id == memberId);
        var newOwnerCurrentGroups = db.Groups.Count(x => x.OwnerId == memberId);
        
        if(newOwnerCurrentGroups + 1 > newOwner.GroupsLimit) return ErrorCode.OwnedGroupsLimitReached;

        membership.PermissionLevel = PermissionLevel.Owner;
        DbGroup.OwnerId = memberId;
        DbCurrentGroupMember.PermissionLevel = PermissionLevel.Member;
        
        if(save) db.SaveChanges();
        return null;
    }

    public IApiResponse GetEventsListApiModel(DateTime from, DateTime to)
    {
        var events = db.GroupEvents.Where(x => x.GroupId == _groupId)
            .Where(x => x.StartTime >= from && x.EndTime <= to)
            .Include(x => x.Creator)
            .Select(x => new EventShortModel(x.GroupEventId, x.Title, new UserShortModel(x.CreatorId, x.Creator.Nick),
                x.StartTime, x.EndTime));
        return new EventsListModel(events);
    }
}