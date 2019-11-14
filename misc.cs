using Discord.Commands;
using Discord;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

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
    }
}
