using Discord.Commands;
using Discord;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using WhalesFargo.Services;

namespace DiscordBot.Modules{
    public class Misc : ModuleBase<SocketCommandContext>{
        [Command("Echo")]
        public async Task Echo([Remainder]string message){
            var embed = new EmbedBuilder();
            embed.WithTitle("Echoed message");
            embed.WithDescription(message);
            embed.WithColor(new Color(0,255,0));

            await Context.Channel.SendMessageAsync("", false, embed);
        }



        [Command("pick")]
        public async Task Pick([Remainder]string message){
            string[] options = message.Split(new char[] {'|'}, StringSplitOptions.RemoveEmptyEntries);

            Random r = new Random();
            string selection = options[r.Next(0, options.Length)];

            var embed = new EmbedBuilder();
            embed.WithTitle("Choice for " + Context.User.Username);
            embed.WithDescription("selection");
            embed.WithColor(new Color(255, 255, 0));

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("PermissionCommand")]
        [RequireUserPermission(GuildPermission.Administrator)]

        public async Task Perm([Remainder]string arg = ""){

            if (!IsUserSecretOwner((SocketGuildUser)Context.User)) return;
            var dmChannel = await Context.User.GetOrCreateDMChannelAsync();
            await dmChannel.SendMessageAsync(Utilities.GetAlert("Secret"));
            await Context.Channel.SendMessageAsync(Utilities.GetAlert("Secret"));
        }

        public bool IsUserSecretOwner(SocketGuildUser User){
            string targetRoleName = "SecretOwner";
            var result = from r in User.Guild.Roles where r.Name == targetRoleName select r.Id;
            ulong roleID = result.FirstOrDefault();
            if(roleID == 0) return false;
            var targetRole = User.Guild.GetRole(roleID);
            return User.Roles.Contains(targetRole);
        }

        [Command("data")]
        public async Task GetData(){
            await Context.Channel.SendMessageAsync("Data Has" + DataStorage.pairs.Count + "pairs.");
        }

        [Command("8ball")]
        [Alias("ask")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        public async Task AskEightBall([Remainder]string args = null)
        {
            
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();

            var replies = new List<string>();

            replies.Add("Yes");
            replies.Add("No");
            replies.Add("Maybe");
            replies.Add("Possibly");

            embed.WithColor(new Color(0, 255,0));
            embed.Title = "8-ball Command.";
            
            sb.AppendLine($"{Context.User.Username},");
            sb.AppendLine();

            if (args == null)
            {
                sb.AppendLine("Re-enter this time with a question.");
            }
            else 
            {
                var ans = r[new Random().Next(r.Count - 1)];
                
                sb.AppendLine($"Your question was: [**{args}**]...");
                sb.AppendLine();
                sb.AppendLine($"...your answer is [**{ans}**]");

                switch (answer) 
                {
                    case "Yes":
                    {
                        embed.WithColor(new Color(0, 255, 0));
                        break;
                    }
                    case "No":
                    {
                        embed.WithColor(new Color(255, 0, 0));
                        break;
                    }
                    case "Maybe":
                    {
                        embed.WithColor(new Color(255,255,0));
                        break;
                    }
                    case "Possibly":
                    {
                        embed.WithColor(new Color(255,0,255));
                        break;
                    }
                }
            }
            
        [Name("Moderator")]
        [RequireContext(ContextType.Guild)]
        public class ModeratorModule : ModuleBase<SocketCommandContext>{
        [Command("Kick")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        [RequireBotPermission(GuildPermission.KickMembers)]
        public async Task Kick([Remainder]SocketGuildUser user)
        {
            await ReplyAsync($"The following user: {user.Mention} has been kicked.");
            await user.KickAsync();
        }
        }

        [Name("Audio")]
        public class AudioModule : CustomModule{
            private readonly AudioService m_Service;

            public AudioModule(AudioService service)
            {
                m_Service = service;
                m_Service.SetParentModule(this);.
            }

            [Command("join", RunMode = RunMode.Async)]
            public async Task JoinVoiceChannel()
            {
                if (m_Service.GetDelayAction()) return;
                await m_Service.JoinAudioAsync(Context.Guild, (Context.User as IVoiceState).VoiceChannel);

                await m_Service.CheckAutoPlayAsync(Context.Guild, Context.Channel);
            }

            [Command("leave", RunMode = RunMode.Async)]
            public async Task LeaveVoiceChannel()
            {
                await m_Service.LeaveAudioAsync(Context.Guild);
            }

            [Command("play", RunMode = RunMode.Async)]
            public async Task PlayVoiceChannel([Remainder] string song)
            {
                await m_Service.ForcePlayAudioAsync(Context.Guild, Context.Channel, song);
                if (m_Service.GetNumPlaysCalled() == 0) await m_Service.CheckAutoPlayAsync(Context.Guild, Context.Channel);
            }

            [Command("play", RunMode = RunMode.Async)]
            public async Task PlayVoiceChannelByIndex(int index)
            {
                await PlayVoiceChannel(m_Service.GetLocalSong(index)); 
            }

            [Command("pause", RunMode = RunMode.Async)]
            public async Task PauseVoiceChannel()
            {
                m_Service.PauseAudio();
                await Task.Delay(0);
            }

            [Command("resume", RunMode = RunMode.Async)]
            public async Task ResumeVoiceChannel()
            {
                m_Service.ResumeAudio();
                await Task.Delay(0);
            }

            [Command("stop", RunMode = RunMode.Async)]
            public async Task StopVoiceChannel()
            {
                m_Service.StopAudio();
                await Task.Delay(0);
            }

            [Command("volume")]
            public async Task VolumeVoiceChannel(int v)
            {
                m_Service.AdjustVolume((float)v / 100.0f);
                await Task.Delay(0);
            }

            [Command("add", RunMode = RunMode.Async)]
            public async Task AddVoiceChannel([Remainder] string song)
            {

                await m_Service.PlaylistAddAsync(song);
                await m_Service.CheckAutoPlayAsync(Context.Guild, Context.Channel);
            }

            [Command("add", RunMode = RunMode.Async)]
            public async Task AddVoiceChannelByIndex(int index)
            {
                await AddVoiceChannel(m_Service.GetLocalSong(index));
            }

            [Command("skip", RunMode = RunMode.Async)]
            public async Task SkipVoiceChannel()
            {
                m_Service.PlaylistSkip();
                await Task.Delay(0);
            }

            [Command("playlist", RunMode = RunMode.Async)]
            public async Task PrintPlaylistVoiceChannel()
            {
                m_Service.PrintPlaylist();
                await Task.Delay(0);
            }

            [Command("autoplay", RunMode = RunMode.Async)]
            public async Task AutoPlayVoiceChannel(bool enable)
            {
                m_Service.SetAutoPlay(enable);
                await m_Service.CheckAutoPlayAsync(Context.Guild, Context.Channel);
            }

            [Command("download", RunMode = RunMode.Async)]
            public async Task DownloadSong([Remainder] string path)
            {
                await m_Service.DownloadSongAsync(path);
            }

            [Command("songs", RunMode = RunMode.Async)]
            public async Task PrintSongDirectory(int page = 0)
            {
                m_Service.PrintLocalSongs(page);
                await Task.Delay(0);
            }

            [Command("cleanupsongs", RunMode = RunMode.Async)]
            public async Task CleanSongDirectory()
            {
                await m_Service.RemoveDuplicateSongsAsync();
            }

            FunService funService = new FunService();

            [Name("SS"), Summary("Screenshare")]
            [Command("SS", RunMode = RunMode.Async), RequireContext(ContextType.Guild), Alias("s")]
            public async Task Screenshare([Remainder]SocketVoiceChannel channel = null)
                => await ReplyAsync(embed: funService.GetScreenshareEmbed(Context.Guild.CurrentUser, channel, Context.Guild));

            [Name("Avatar"), Summary("Returns the user's pfp")]
            [Command("ava", RunMode = RunMode.Async), Alias("a")]
            public async Task Avatar([Remainder]SocketUser user = null)
                => await ReplyAsync(embed: funService.GetAvatarEmbed(Context.Message, user));

            [Name("ID"), Summary("Returns an user ID")]
            [Command("RandomID", RunMode = RunMode.Async), Alias("rID")]
            public async Task RandomUser()
                => await ReplyAsync($"<@{funService.GetRandomUserId(Context.Client)}>");
            
            }

            [Name ("User Info")]
        [Command ("userinfo")]
        [Summary ("Gets information about the specified user")]
        [CommandOptions (typeof (UserInfoOptions))]
        public async Task UserInfo ([Remainder] IGuildUser user)
        {
            var options = GetOptions<UserInfoOptions> ();

            EmbedBuilder emb = new EmbedBuilder();

            string userRoles = DiscordHelpers.GetListOfUsersRoles (user);
            IRole highestRole = DiscordHelpers.GetUsersHigherstRole (user);

            if (highestRole != null)
                emb.Color = highestRole.Color;
            EmbedAuthorBuilder author = new EmbedAuthorBuilder ();
            author.Name = user.Username;
            if (user.IsBot)
                author.Name += " (Bot)";
            else if (user.IsWebhook)
                author.Name += " (Webhook)";

            emb.Author = author;
            if (string.IsNullOrEmpty (user.AvatarId))
                emb.ThumbnailUrl = $"https://discordapp.com/assets/dd4dbc0016779df1378e7812eabaa04d.png";
            else
                emb.ThumbnailUrl = $"https://cdn.discordapp.com/avatars/{user.Id}/{user.AvatarId}.png";

            EmbedFooterBuilder footer = new EmbedFooterBuilder();
            footer.Text = $"User info requested by {Context.User.Username}";
            if (string.IsNullOrEmpty (Context.User.AvatarId))
                footer.IconUrl = $"https://discordapp.com/assets/dd4dbc0016779df1378e7812eabaa04d.png";
            else
                footer.IconUrl = $"https://cdn.discordapp.com/avatars/{Context.User.Id}/{Context.User.AvatarId}.png";
            emb.Footer = footer;

            emb.Description = $"User information for {user.Username}#{user.Discriminator} | {user.Id}";

            emb.AddField("Created account at", user.CreatedAt.ToString("dd MMM yyyy, HH:mm"));

            emb.AddField("Joined server at", ((DateTimeOffset)user.JoinedAt).ToString("dd MMM yyyy, HH:mm"));
            if (string.IsNullOrEmpty(userRoles) == false)
                emb.AddField("Role(s)", userRoles);

            // Display the list of all of user's permissions
            string userPermissions = GetUserPermissions (user);

            if (string.IsNullOrEmpty (userPermissions) == false)
                emb.AddField ("Permissions", userPermissions);

            emb.AddField("Online status", user.Status == UserStatus.DoNotDisturb ? "Do Not Disturb" : user.Status.ToString());

            if (user.Game.HasValue)
            {
                if (user.Game.Value.StreamType == StreamType.Twitch)
                {
                    emb.AddField("Streaming", user.Game.Value.StreamUrl);
                }
                else
                {
                    emb.AddField("Playing", user.Game.Value.Name);
                }
            }

            await SendMessage (options, "", false, emb.Build());
        }

            private string GetUserPermissions (IGuildUser user)
            {
                string permissions = "";

                if (Context.Guild.OwnerId == user.Id)
                {
                    permissions += "Owner";
                    return permissions;
                }

                if (user.GuildPermissions.Administrator)
                {
                    permissions += "Administrator";
                    return permissions;
                }

                if (user.GuildPermissions.BanMembers)
                    permissions += "Ban Memebers, ";
            
                if (user.GuildPermissions.DeafenMembers)
                    permissions += "Deafen Members, ";

                if (user.GuildPermissions.KickMembers)
                    permissions += "Kick Members, ";

                if (user.GuildPermissions.ManageChannels)
                    permissions += "Manage Channels, ";

                if (user.GuildPermissions.ManageEmojis)
                    permissions += "Manage Emojis, ";

                if (user.GuildPermissions.ManageGuild)
                    permissions += "Manage Guild, ";

                if (user.GuildPermissions.ManageMessages)
                    permissions += "Manage Messages, ";

                if (user.GuildPermissions.ManageNicknames)
                    permissions += "Manage Nicknames, ";

                if (user.GuildPermissions.ManageRoles)
                    permissions += "Manage Roles, ";

                if (user.GuildPermissions.ManageWebhooks)
                    permissions += "Manage Webhooks, ";

                if (user.GuildPermissions.MentionEveryone)
                    permissions += "Mention Everyone, ";

                if (user.GuildPermissions.MoveMembers)
                    permissions += "Move Members, ";

                if (user.GuildPermissions.MuteMembers)
                    permissions += "Mute Members, ";

                return permissions.Remove (permissions.Length - 2);
        }

        private readonly Daily _DailyReward;
        private readonly MoneyTransfer _MoneyTransfer;
        private readonly UserAccounts _UserAccounts;

        public Economy(Daily Daily, MoneyTransfer MoneyTransfer, UserAccounts UserAccounts)
        {
            _DailyReward = Daily;
            _MoneyTransfer = MoneyTransfer;
            _UserAccounts = UserAccounts;
        }

        [Command("Daily"))]
        public async Task GetDaily()
        {
            try
            {
                _DailyMoney.GetDaily(Context.User.Id);
                await ReplyAsync($"You have earned {Constants.Daily}.");
            }
            catch (InvalidOperationException e)
            {
                var timeSpanString = string.Format("{0:%h} hours {0:%m} minutes {0:%s} seconds", new TimeSpan(24,0,0).Subtract((TimeSpan)e.Data["sinceLastDaily"]));
                await ReplyAsync($"You have already claimed your daily reward.");
            }
        }

        [Command("Balance")]
        [Alias("Bal", "bal")]
        public async Task CheckMiunies()
        {
            var account = _UserAccounts.GetById(Context.User.Id);
            await ReplyAsync(ReturnMoney(account.Money, Context.User.Mention));
        }

        [Command("Balance")]
        [Alias("Bal", "bal")]
        public async Task CheckMiuniesOther(IGuildUser target)
        {
            var account = _UserAccounts.GetById(target.Id);
            await ReplyAsync(ReturnMoney(account.Money, target.Mention));
        }


        [Command("Pay")]
        public async Task TransferMinuies(IGuildUser target, ulong amount)
        {
            try
            {
                MoneyTransfer.UserToUser(Context.User.Id, target.Id, amount);
                await ReplyAsync($"{Context.User.Username} has given {target.Username} {amount}.");
            }
            catch (InvalidOperationException e)
            {
                await ReplyAsync($"{e.Message}");
            }
        }
            account.Money -= amount;
            _UserAccounts.SaveAccounts(Context.User.Id);

            var slotEmojis = Global.Slot.Spin();
            var payoutAndFlavour = Global.Slot.GetPayoutAndFlavourText(amount);

            if (payoutAndFlavour.Item1 > 0)
            {
                account.Money += payoutAndFlavour.Item1;
                _UserAccounts.SaveAccounts();
            }            

            await ReplyAsync(slotEmojis);
            await Task.Delay(1000);
            await ReplyAsync(payoutAndFlavour.Item2);
        }
    }
}
