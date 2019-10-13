using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord.Commands;
using Discord.WebSocket;
using Discord;


namespace csbot
{
    class Program
    {
        DiscordSocketClient _clientside;
        CommandHandler _serverside;
        static void Main(string[] args)
        => new Program().StartAsync().GetAwaiter().GetResult();
        public async Task StartAsync(){
            if (config.bot.token == "" || config.bot.token == null){
                return;
                _clientside = new DiscordSocketClient(new DiscordSocketConfig{LogLevel = LogSeverity.Verbose});
                _clientside.Log += Log;
                _serverside = new CommandHandler();
                await _clientside.LoginAsync(TokenType.Bot, config.bot.token);
                await _clientside.StartAsync();
                await _serverside.InitialiseAsync(_clientside);
                await Task.Delay(-1);
            }

        }

        private async Task Log(LogMessage msg){
            Console.WriteLine(msg.Message);
        }
    }
}
