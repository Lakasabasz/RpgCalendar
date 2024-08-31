using RpgCalendar.Commands.ApiModels;
using RpgCalendar.Database;
using RpgCalendar.Database.Models;
using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs;

public class GroupService(RelationalDb db, ImageService imageService)
{  
    public record GroupDto(Group group, GroupMember Member, ImageService ImageService)
    {
        public GroupFull ToFullGroup()
        {
            var system = new ApiModels.System(Guid.Empty, "Kapitan Bomba");
            var hero = new GroupUserHeroProfile(Member.HeroPicture, Member.Class, Member.Race,
                Member.Location);

            return new GroupFull(group.GroupId, group.OwnerId, group.Name,
                ImageService.GetImageUrl(group.ProfilePicture), group.CreationDate,
                system, hero);
        }
    }

    public ErrorCode? AddGroup(out GroupDto? groupDto, Guid systemId, Guid owner, string name, Guid? profilePictureId)
    {
        var group = Group.Prepare(owner, name, profilePictureId);
        db.Groups.Add(group);
        db.GroupsMembers.Add(GroupMember.Prepare(owner, group.GroupId, PermissionLevel.Owner, 
            GroupUserProfilePicture.CYCLOPE, "Żołnierz Gwiezdnej Floty", "Kurvinox", "Galaktyka Kurvix"));

        db.SaveChanges();
        
        groupDto = GetGroupInfo(group.GroupId, group.OwnerId);

        return null;
    }
    
    public GroupDto GetGroupInfo(Guid groupId, Guid memberId)
    {
        var dbGroup = db.Groups.First(x => groupId == x.GroupId);
        var membership = db.GroupsMembers.First(x => groupId == x.GroupId && x.UserId == memberId);
        
        return new GroupDto(dbGroup, membership, imageService);
    }
    
    public ErrorCode? AddMember(Guid groupId, Guid memberId, bool save = true)
    {
        if (db.GroupsMembers.FirstOrDefault(x => x.GroupId == groupId && x.UserId == memberId) is not null)
            return ErrorCode.UserAlreadyRegistered;
        
        var group = db.Groups.First(x => x.GroupId == groupId);
        if(db.GroupsMembers.Count(x => x.GroupId == groupId) + 1 > group.UserLimit)
            return ErrorCode.MembersLimitReached;

        db.GroupsMembers.Add(GroupMember.Prepare(memberId, groupId, PermissionLevel.Member, GroupUserProfilePicture.GHOST_OF_ASS,
            "Sarumun", "Duch Dupy", "Labirynt Downa"));
        if(save) db.SaveChanges();
        return null;
    }

    public ErrorCode? Join(out GroupDto? groupDto, Guid inviteId, Guid invokerId)
    {
        groupDto = null;
        var invite = db.GroupsInvites.FirstOrDefault(x => x.InviteId == inviteId);
        if (invite is null) return ErrorCode.InviteNotExists;

        var err = AddMember(invite.GroupId, invokerId, false);
        if (err is not null) return err;
        
        db.GroupsInvites.Remove(invite);
        db.SaveChanges();
        return null;
    }

    public ErrorCode? UpdateImage(out GroupDto? groupDto, Guid groupId, Guid imageId, Guid invokerId, bool save = true)
    {
        groupDto = null;
        if (imageService.Contains(imageId)) return ErrorCode.ImageNotExists;

        var group = db.Groups.First(x => x.GroupId == groupId);
        group.ProfilePicture = imageId;
        
        if(!save) return null;
        db.SaveChanges();
        groupDto = GetGroupInfo(groupId, invokerId);
        return null;
    }

    public ErrorCode? UpdateName(out GroupDto? groupDto, Guid groupId, string name, Guid invokerId, bool save = true)
    {
        groupDto = null;
        var group = db.Groups.First(x => x.GroupId == groupId);
        group.Name = name;
        
        if(!save) return null;
        db.SaveChanges();
        groupDto = GetGroupInfo(groupId, invokerId);
        return null;
    }

    public ErrorCode? TransferOwnership(out GroupDto? groupDto, Guid groupId, Guid memberId, Guid invokerId, bool save = true)
    {
        groupDto = null;
        var membership = db.GroupsMembers
            .FirstOrDefault(x => x.GroupId == groupId && x.UserId == memberId);
        if(membership is null) return ErrorCode.MemberNotInGroup;
        
        var newOwner = db.Users.First(x => x.Id == memberId);
        var newOwnerCurrentGroups = db.Groups.Count(x => x.OwnerId == memberId);
        
        if(newOwnerCurrentGroups + 1 > newOwner.GroupsLimit) return ErrorCode.OwnedGroupsLimitReached;

        membership.PermissionLevel = PermissionLevel.Owner;
        var group = db.Groups.First(x => x.GroupId == groupId);
        var ownerMembership = db.GroupsMembers
            .First(x => x.GroupId == groupId && x.UserId == group.OwnerId);
        group.OwnerId = memberId;
        ownerMembership.GroupId = groupId;
        
        if(!save) return null;
        db.SaveChanges();
        groupDto = GetGroupInfo(groupId, membership.GroupId);
        return null;
    }
}