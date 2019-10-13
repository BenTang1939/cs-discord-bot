using Discord.WebSocket;
using Discord.Commands;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace csbot{
    class CommandHandler{
        DiscordSocketClient _clientside;
        CommandService _serverside;

        public async Task InitialiseAsync(DiscordSocketClient client){
            _clientside = client;
            _serverside = new CommandService();
            await _serverside.AddModulesAsync(Assembly.GetEntryAssembly());
            _clientside.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage x){
            var msg = x  as SocketUserMessage; // Occurs once bot receives a message.
            if (msg == null){
                return;
            }
            var context = new SocketCommandContext(_clientside, msg);
            int argPos = 0;
            if(msg.HasStringPrefix(config.bot.cmdPrefix, ref argPos) || msg.HasMentionPrefix(_clientside.CurrentUser, ref argPos)){
                var result = await _serverside.ExecuteAsync(context, argPos);
                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand){
                    Console.WriteLine(result.ErrorReason);
                }
            }
        }

    }

}