using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace nostromo_muthur
{
    class Program
    {
        static ITelegramBotClient botClient;

        public static async Task Main(string[] args)
        {
            string botId = Environment.GetEnvironmentVariable("BOT_ID");
            if (String.IsNullOrEmpty(botId))
            {
                throw new InvalidOperationException("Need BOT_ID with bot id");
            }

            botClient = new TelegramBotClient(botId);
            var me = await botClient.GetMeAsync();
            Console.Title = me.Username;

            var cts = new CancellationTokenSource();

            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            botClient.StartReceiving(
                new DefaultUpdateHandler(HandleUpdateAsync, HandleErrorAsync),
                cts.Token
            );

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();

            // Send cancellation request to stop bot
            cts.Cancel();
        }

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var handler = update.Type switch
            {
                UpdateType.Message => BotOnMessageReceived(update.Message),
                // UpdateType.EditedMessage => BotOnMessageReceived(update.Message),
                // UpdateType.CallbackQuery => BotOnCallbackQueryReceived(update.CallbackQuery),
                // UpdateType.InlineQuery => BotOnInlineQueryReceived(update.InlineQuery),
                // UpdateType.ChosenInlineResult => BotOnChosenInlineResultReceived(update.ChosenInlineResult),
                // UpdateType.Unknown:
                // UpdateType.ChannelPost:
                // UpdateType.EditedChannelPost:
                // UpdateType.ShippingQuery:
                // UpdateType.PreCheckoutQuery:
                // UpdateType.Poll:
                _ => UnknownUpdateHandlerAsync(update)
            };

            try
            {
                await handler;
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(botClient, exception, cancellationToken);
            }
        }

        // static async void Bot_OnMessage(object sender, MessageEventArgs e)
        private static async Task BotOnMessageReceived(Message message)
        {
            if (message.Text != null)
            {
                Console.WriteLine($"Received a text message in chat {message.Chat.Id}.");
                string text = "You said:\n" + message.Text;
                if (message.Text == "/t" || message.Text == "/t@MUTHUR6000Bot")
                {
                    text = new Redis.Client().ReadOne();
                }
                else
                {

                }
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat,
                    text
                );
            }
        }
        private static async Task UnknownUpdateHandlerAsync(Update update)
        {
            Console.WriteLine($"Unknown update type: {update.Type}");
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
        }
    }
}
