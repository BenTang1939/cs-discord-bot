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
}
