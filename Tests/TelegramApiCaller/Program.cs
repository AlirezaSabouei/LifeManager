using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;


// See https://aka.ms/new-console-template for more information
var _httpClient = new HttpClient();
_httpClient.BaseAddress = new Uri("https://localhost:7217");

using var cts = new CancellationTokenSource();

var bot = new TelegramBotClient(GetTelegramBotToken(), cancellationToken: cts.Token);
var me = await bot.GetMe();
int offset = 0;
while (true)
{
    var updates = (await  bot.GetUpdates(offset)).ToList();
    if (updates.Any())
    {
        offset = updates[0].Id + 1;
        var result = await _httpClient.PostAsJsonAsync("api/v1/Updates", updates[0]);
    }
    
    Thread.Sleep(1000);
}

static string GetTelegramBotToken()
{
    var test = Directory.GetCurrentDirectory();
    // Build configuration
    var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: true)// Optional: read from system environment variables too
        .Build();

    // Bind to strongly typed settings (optional)
    var settings = configuration.GetSection("Telegram");
    return settings.GetValue<string>("BotToken");
}