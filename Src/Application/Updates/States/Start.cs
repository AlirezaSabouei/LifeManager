using Application.Common.Interfaces;
using Application.Updates.Commands;
using Application.Users.Commands;

public class Start(
    ITelegramBot telegramBot,
    IRequestHandler<CreateUserCommand, Domain.Entities.Users.User> createUsersCommandHandler) : IState
{
    private readonly ITelegramBot _telegramBot = telegramBot;
    private readonly IRequestHandler<CreateUserCommand, Domain.Entities.Users.User> _createUserCommandHandler = createUsersCommandHandler;

    public string StateText => "/start";

    public async Task HandleAsync(ProcessUpdateCommand processUpdateCommand)
    {
        try
        {
            await CreateUserAsync(processUpdateCommand);
            await SendAnswerAsync(processUpdateCommand);
        }
        catch (Exception ex)
        {
            await SendErrorMessageAsync(processUpdateCommand);
        }
    }

    private async Task CreateUserAsync(ProcessUpdateCommand processUpdateCommand)
    {
        CreateUserCommand createUserCommand = CreateCommand(processUpdateCommand);

        await _createUserCommandHandler.Handle(createUserCommand, new CancellationToken());
    }

    private static CreateUserCommand CreateCommand(ProcessUpdateCommand processUpdateCommand)
    {
        return new CreateUserCommand()
        {
            User = new Domain.Entities.Users.User()
            {
                Id = processUpdateCommand.UserId,
                Name = $"{processUpdateCommand.FirstName}",
                TelegramChatId = processUpdateCommand.ChatId,
                WaterIntake = new Domain.ValueObjects.WaterIntake()
                {
                    Goal = 8,
                    MeasurementUnit = Domain.Enums.WaterMeasurementUnit.Glass,
                    CurrentIntake = 0,
                    LastDay = DateTime.UtcNow
                }
            }
        };
    }

    private async Task SendAnswerAsync(ProcessUpdateCommand processUpdateCommand)
    {
        await _telegramBot.SendMessageAsync(
            processUpdateCommand.ChatId,
            CreateAnswerText(processUpdateCommand),
            CreateKeyboard(processUpdateCommand));
    }

    private string CreateAnswerText(ProcessUpdateCommand processUpdateCommand)
    {
        return $"Welcome Dear {processUpdateCommand.FirstName} 🌸" + 
            Environment.NewLine + 
            Environment.NewLine +
            "What Can I Do For You? 👇";
    }

    private Dictionary<string, string> CreateKeyboard(ProcessUpdateCommand processUpdateCommand)
    {
        Dictionary<string,string> keyboard = new()
        {
            { "Monitor water intake", "/water" },
            { "Monitor sitting time", "/sitting" },
            { "Button3", "button3" }
        };
        return keyboard;
    }

    private async Task SendErrorMessageAsync(ProcessUpdateCommand processUpdateCommand)
    {
        await _telegramBot.SendMessageAsync(processUpdateCommand.ChatId, $"Some errors happened");
    }
}