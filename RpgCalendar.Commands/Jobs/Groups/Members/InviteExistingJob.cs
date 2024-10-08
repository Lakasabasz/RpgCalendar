﻿using System.Data;
using Microsoft.EntityFrameworkCore;
using RpgCalendar.Commands.ApiModels;
using RpgCalendar.Database;
using RpgCalendar.Database.Models;
using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Groups.Members;

public class InviteExistingJob(RelationalDb db, GroupService groupService): IJob
{
    public record JobData(Guid GroupId, string PrivateCode, Guid InvokerId);

    public ErrorCode? Error { get; private set; }
    public IApiResponse? ApiResponse { get; private set; }

    public void Execute(JobData data)
    {
        var user = db.Users.FirstOrDefault(x => x.PrivateCode == data.PrivateCode);
        if (user is null)
        {
            Error = ErrorCode.UserNotExists;
            return;
        }

        var blacklistEntry =
            db.BlacklistUsers.FirstOrDefault(x => x.EntryOwnerId == user.Id && x.BlacklistedUserId == data.InvokerId);
        if(blacklistEntry is not null)
        {
            Error = ErrorCode.UserBlacklistedInvoker;
            return;
        }

        Error = groupService.SelectGroup(data.GroupId, data.InvokerId).AddMember(user.Id);

        ApiResponse = groupService.GetMemberListApiModel();
    }
}