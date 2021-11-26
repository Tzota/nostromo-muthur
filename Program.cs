using System;
using System.Threading;
using System.Threading.Tasks;
using nostromo_muthur.Domain.Messages;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace nostromo_muthur
{
    class Program
    {
        static ITelegramBotClient? botClient;
        static Redis.Client? redisClient;

        public static async Task Main(string[] args)
        {
            string? botId = Environment.GetEnvironmentVariable("BOT_ID");
            if (String.IsNullOrEmpty(botId))
            {
                throw new InvalidOperationException("Need BOT_ID with bot id");
            }

            string? redisServer = Environment.GetEnvironmentVariable("REDIS_SERVER");
            if (String.IsNullOrEmpty(redisServer))
            {
                throw new InvalidOperationException("Need REDIS_SERVER with redis host");
            }

            redisClient = new Redis.Client(redisServer);

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

            while (true)
            {
                System.Threading.Thread.Sleep(Int32.MaxValue);
            }
            // Console.ReadLine();
            // Console.WriteLine("Bye-bye");
            // // Send cancellation request to stop bot
            // cts.Cancel();
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
            if (redisClient == null) throw new InvalidOperationException(nameof(redisClient));
            if (botClient == null) throw new InvalidOperationException(nameof(botClient));
            if (message.Text == null) return;

            Console.WriteLine($"Received a text message in chat {message.Chat.Id}.");
            string text = "You said:\n" + message.Text;
            if (message.Text == "/t" || message.Text == "/t@MUTHUR6000Bot")
            {
                Redis.Message m = redisClient.ReadLast();
                AbstractMessage am = AbstractMessage.Create(m);
                text = am.ToString();
            }

            await botClient.SendTextMessageAsync(
                chatId: message.Chat,
                text,
                ParseMode.Html
            );
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
