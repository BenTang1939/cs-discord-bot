using Discord.Commands;
using Discord;
using System.Threading.Tasks;
using System;
using System.Net;
using Newtonsoft.Json.Linq;
using System.IO;

    public class ApiModule : ModuleBase<SocketCommandContext>
    {

        EmbedBuilder errorBuilder = new EmbedBuilder().WithColor(Color.Red);

        #region ApiKeys
        string dictApiKey = "lbqfelNAZKIF3X5TdyJXH8LNZMC76h";
        string steamDevKey = "sNEEsdct7fVFpRohz0l9Qzu7aTGx9grXBmpV18qg";
        string owmApiKey = "A1cAtugQ0ERISwhqv6pYZh2zQi0";
        #endregion
   
        [Command("dict")]
        public async Task SearchDictionary(string wordToSearch)
        {

            string json = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://od-api.oxforddictionaries.com:443/api/v1/entries/en/" + wordToSearch.ToLower());
            if (request != null)
            {
                request.Method = "GET";
                request.ContinueTimeout = 20000;
                request.Accept = "application/json";
                request.Headers["app_id"] = "0ff6fba7";
                request.Headers["app_key"] = dictApiKey;

                try
                {
                    using (HttpWebResponse response = (HttpWebResponse)(await request.GetResponseAsync()))
                    {
                        using (Stream stream = response.GetResponseStream())
                        using (StreamReader reader = new StreamReader(stream))
                            json += await reader.ReadToEndAsync();

                        JObject jsonParsed = JObject.Parse(json);
                        Console.WriteLine(jsonParsed);

                        var wordDefinition = jsonParsed["results"][0]["lexicalEntries"][0]["entries"][0]["senses"][0]["definitions"][0];
                        var wordExample = jsonParsed["results"][0]["lexicalEntries"][0]["entries"][0]["senses"][0]["examples"][0]["text"];
                        var lexicalCategory = jsonParsed["lexicalCategory"];


                        string wordMeaning = ("**" + wordToSearch.ToUpper() + " Meaning**" + "\n" + lexicalCategory + "\n\n**Definition:**\n  " + wordDefinition + "\n\n**Example:**\n  " + wordExample);
                        Console.WriteLine("\n\n" + lexicalCategory);

                        await ReplyAsync(wordMeaning);
                    }
                }
                catch (WebException e)
                {
                    if (e.Status == WebExceptionStatus.ProtocolError)
                    {
                        HttpStatusCode code = ((HttpWebResponse)e.Response).StatusCode;
                        if (code == HttpStatusCode.NotFound)
                            await ReplyAsync("Word requested not found.");
                        if (code == HttpStatusCode.BadRequest)
                            await ReplyAsync("Word not supported.");
                    }
                }

            }
        }

        #region SteamAPI

        [Command("steam", RunMode = RunMode.Async)]
        public async Task GetSteamInfo(string userId)
        {
            string id64response = null;

            //Variables
            string steamName = null;
            string realName = null;
            ulong steam64ID = 0;
            string avatarUrl = null;
            int onlineStatusGet = 0;
            string profileUrl = null;
            string countryCode = null;            
            int profileVisibilityGet = 0;
            int steamUserLevel = 0;

            if (!helper.IsDigitsOnly(userId))
            {
                id64response = await GetSteamId64(userId);
                steamUserLevel = await GetSteamLevel(id64response);
            }
            else
            {
                id64response = userId;
                steamUserLevel = await GetSteamLevel(id64response);
            }

            try
            {
                string profileResponse = null;
                HttpWebRequest steamProfileRequest = (HttpWebRequest)WebRequest.Create("https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v2/?key=" + steamDevKey + "&steamids=" + id64response);

                using (HttpWebResponse steamProfileResponse = (HttpWebResponse)(await steamProfileRequest.GetResponseAsync()))
                {
                    using (Stream stream = steamProfileResponse.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                        profileResponse += await reader.ReadToEndAsync();

                    JObject profileJsonResponse = JObject.Parse(profileResponse);

                    //Give values to the variables
                    try
                    {
                        steamName = (string)profileJsonResponse["response"]["players"][0]["personaname"];
                        realName = (string)profileJsonResponse["response"]["players"][0]["realname"];
                        steam64ID = (ulong)profileJsonResponse["response"]["players"][0]["steamid"];
                        avatarUrl = (string)profileJsonResponse["response"]["players"][0]["avatarfull"];
                        onlineStatusGet = (int)profileJsonResponse["response"]["players"][0]["personastate"];
                        profileUrl = (string)profileJsonResponse["response"]["players"][0]["profileurl"];
                        countryCode = (string)profileJsonResponse["response"]["players"][0]["loccountrycode"];
                        profileVisibilityGet = (int)profileJsonResponse["response"]["players"][0]["communityvisibilitystate"];
                    }
                    catch (Exception)
                    {
                        errorBuilder.WithDescription("**User not found!** Please check your SteamID and try again.");
                        await ReplyAsync("", false, errorBuilder.Build());
                        return;
                    }

                    string onlineStatus = null;
                    switch (onlineStatusGet)
                    {
                        case 0:
                            onlineStatus = "Offline";
                            break;
                        case 1:
                            onlineStatus = "Online";
                            break;
                        case 2:
                            onlineStatus = "Busy";
                            break;
                        case 3:
                            onlineStatus = "Away";
                            break;
                        case 4:
                            onlineStatus = "Snooze";
                            break;
                        case 5:
                            onlineStatus = "Looking to Trade";
                            break;
                        case 6:
                            onlineStatus = "Looking to Play";
                            break;

                    }

                    string profileVisibility = null;
                    switch (profileVisibilityGet)
                    {
                        case 1:
                            profileVisibility = "Private";
                            break;
                        case 2:
                            profileVisibility = "Friends Only";
                            break;
                        case 3:
                            profileVisibility = "Public";
                            break;
                    }

                    var embed = new EmbedBuilder();
                    embed.WithTitle(steamName + " Steam Info")
                        .WithDescription("\nSteam Name: " + "**" + steamName + "**" + "\nSteam Level: " + "**" + steamUserLevel + "**" + "\nReal Name: " + "**" + realName + "**" + "\nSteam ID64: " + "**" + steam64ID + "**" + "\nStatus: " + "**" + onlineStatus + "**" + "\nProfile Privacy: " + "**" + profileVisibility + "**" + "\nCountry: " + "**" + countryCode + "**" + "\n\nProfile URL: " + profileUrl)
                        .WithThumbnailUrl(avatarUrl)
                        .WithColor(Color.Blue);

                    await ReplyAsync("", false, embed.Build());

                }
            }
            catch (WebException)
            {
                errorBuilder.WithDescription("**An error ocurred**");
                await ReplyAsync("", false, errorBuilder.Build());
            }

        }


        public async Task<string> GetSteamId64(string userId)
        {
            TaskCompletionSource<String> tcs = new TaskCompletionSource<String>();

            string id64response = null;
            string userIdResolved;

            HttpWebRequest steamRequest = (HttpWebRequest)WebRequest.Create("http://api.steampowered.com/ISteamUser/ResolveVanityURL/v0001/?key=" + steamDevKey + "&vanityurl=" + userId);
            try
            {
                using (HttpWebResponse steamResponse = (HttpWebResponse)(await steamRequest.GetResponseAsync()))
                {
                    using (Stream stream = steamResponse.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                        id64response += await reader.ReadToEndAsync();

                    JObject jsonParsed = JObject.Parse(id64response);

                    userIdResolved = jsonParsed["response"]["steamid"].ToString();

                    tcs.SetResult(userIdResolved);
                }
            }
            catch (NullReferenceException)
            {
                errorBuilder.WithDescription("**User not found!** Please check your SteamID and try again.");
                await ReplyAsync("", false, errorBuilder.Build());
            }

            string result = await tcs.Task;

            return result;
        }


        public async Task<int>GetSteamLevel(string userId)
        {
            TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();

            string steamLevelJson = null;
            int userLevel = 0;
            HttpWebRequest steamLevelRequest = (HttpWebRequest)WebRequest.Create("http://api.steampowered.com/IPlayerService/GetSteamLevel/v1/?key=" + steamDevKey + "&steamid=" + userId);

            try
            {
                using (HttpWebResponse steamResponse = (HttpWebResponse)(await steamLevelRequest.GetResponseAsync()))
                {
                    using (Stream stream = steamResponse.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                        steamLevelJson += await reader.ReadToEndAsync();

                    JObject jsonParsed = JObject.Parse(steamLevelJson);

                    userLevel = (int)jsonParsed["response"]["player_level"];

                    tcs.SetResult(userLevel);
                }
            }
            catch (NullReferenceException)
            {
                errorBuilder.WithDescription("**Couldn't fetch level**");
                await ReplyAsync("", false, errorBuilder.Build());
            }

            int result = await tcs.Task;

            return result;
        }

        #endregion

        [Command("lmgtfy")]
        public async Task Lmgtfy(params string[] textToSearch)
        {
            if (textToSearch == null || textToSearch.Length == 0)
            {
                errorBuilder.WithDescription("**Please specify some text**");
                await ReplyAsync("", false, errorBuilder.Build());
                return;
            }

            try
            {
                string textSearchConverted = String.Join(" ", textToSearch);

                if (textSearchConverted.Contains(" "))
                {
                    string textSearchWithoutWhiteSpace = textSearchConverted.Replace(" ", "+");
                    await ReplyAsync("http://lmgtfy.com/?q=" + textSearchWithoutWhiteSpace);
                }
                else
                {
                    await ReplyAsync("http://lmgtfy.com/?q=" + textSearchConverted);
                }
            }
            catch (Exception)
            {
                await ReplyAsync("Why the fuck did lmgtfy command throw an error?");
            }
            
        }

        [Command("weather")]
        public async Task Weather(params string[] city)
        {
            if (city.Length == 0 || city == null)
            {
                errorBuilder.WithDescription("**Please specify a location**");
                await ReplyAsync("", false, errorBuilder.Build());
                return;
            }

            string cityConverted = String.Join(" ", city);
            string searchQuery = null;

            if(cityConverted.Contains(" "))
            {
                searchQuery = cityConverted.Replace(" ", "+");
            }
            else
            {
                searchQuery = cityConverted;
            }
            

            string weatherJsonResponse = null;

            try
            {
                HttpWebRequest currentWeather = (HttpWebRequest)WebRequest.Create("http://api.openweathermap.org/data/2.5/weather?q=" + searchQuery + "&appid=" + owmApiKey + "&units=metric");

                using (HttpWebResponse weatherResponse = (HttpWebResponse)(await currentWeather.GetResponseAsync()))
                {
                    using (Stream stream = weatherResponse.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                        weatherJsonResponse += await reader.ReadToEndAsync();

                    JObject weatherParsedJson = JObject.Parse(weatherJsonResponse);

                    string weatherMain = (string)weatherParsedJson["weather"][0]["main"];
                    string weatherDescription = (string)weatherParsedJson["weather"][0]["description"];
                    string thumbnailIcon = (string)weatherParsedJson["weather"][0]["icon"];
                    string cityName = (string)weatherParsedJson["name"];
                    string cityCountry = (string)weatherParsedJson["sys"]["country"];
                    double actualTemperature = (double)weatherParsedJson["main"]["temp"];
                    double maxTemperature = (double)weatherParsedJson["main"]["temp_max"];
                    double minTemperature = (double)weatherParsedJson["main"]["temp_min"];
                    double humidity = (double)weatherParsedJson["main"]["humidity"];

                    weatherDescription = helper.FirstLetterToUpper(weatherDescription);

                    string thumbnailUrl = "http://openweathermap.org/img/w/" + thumbnailIcon + ".png";

                    var embed = new EmbedBuilder();
                    embed.WithTitle("Current Weather for: " + cityName + ", " + cityCountry)
                        .WithThumbnailUrl(thumbnailUrl)
                        .WithDescription("**Conditions: " + weatherMain + ", " + weatherDescription + "**\nTemperature: **" + actualTemperature + "ºC**\n" + "Max Temperature: **" + maxTemperature + "ºC**\n" + "Min Temperature: **" + minTemperature + "ºC**\n" + "Humidity: **" + humidity + "%**\n")
                        .WithColor(102, 204, 0);

                    await ReplyAsync("", false, embed.Build());
                }
            }
            
        }
        
    