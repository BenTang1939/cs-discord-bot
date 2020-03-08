using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace csbot
{
    class DataStorage
    {
        public void Initialize(DiscordClient client)
        {
            client.MessageCreated += Client_MessageCreated;
        }

        private static async Task Client_MessageCreated(MessageCreateEventArgs message)
        {
            if(message.Author.IsBot) return;
            using (var botContext = new BotContext())
            {
                var Guild = await message.Guild(botContext);
                if (!message.Message.Content.StartsWith(Guild.Prefix))
                {
                    var User = await message.Author.User(Guild, botContext);

                    if (!User.LastExpMessage.HasValue || (DateTime.Now - kuvuUser.LastExpMessage.Value).Minutes >= 1)
                    {
                        if (kuvuGuild.ShowLevelUp)
                        {
                            await User.AddExp(new Random().Next(1, 5), message.Channel, message.Author.Mention);
                        }
                        else
                        {
                            await User.AddExp(new Random().Next(1, 5));
                        }

                        User.LastExpMessage = DateTime.Now;

                        await botContext.SaveChangesAsync();
                    }
                }

            }
        }
    }
}