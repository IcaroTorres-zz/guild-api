using Domain.Enums;
using Domain.Models;
using System;
using System.ComponentModel;

namespace Tests.Domain.Models.TestModels
{
    public class TestInvite : Invite, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public override InviteStatuses Status
        {
            get => base.Status; protected internal set
            {
                if (base.Status != value)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Status)));
                    base.Status = value;
                }
            }
        }

        public override Guid? MemberId
        {
            get => base.MemberId; protected internal set
            {
                if (base.MemberId != value)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MemberId)));
                    base.MemberId = value;
                }
            }
        }

        public override Guid? GuildId
        {
            get => base.GuildId; protected internal set
            {
                if (base.GuildId != value)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GuildId)));
                    base.GuildId = value;
                }
            }
        }

        public override Guild Guild
        {
            get => base.Guild; protected internal set
            {
                if (base.Guild != value)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Guild)));
                    base.Guild = value;
                }
            }
        }

        public override Member Member
        {
            get => base.Member; protected internal set
            {
                if (base.Member != value)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Member)));
                    base.Member = value;
                }
            }
        }
    }
}