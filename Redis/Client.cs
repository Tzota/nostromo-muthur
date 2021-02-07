using StackExchange.Redis;
using System.Linq;

namespace nostromo_muthur.Redis
{
    class Client
    {
        private static ConnectionMultiplexer redis;

        IDatabase db;

        public Client()
        {
            string server = "127.0.0.1";
            string port = "6379";
            string cnstring = $"{server}:{port}";

            if (redis == null)
            {
                redis = ConnectionMultiplexer.Connect(cnstring);
            }
            db = redis.GetDatabase();
            // var redisOptions = new RedisCacheOptions
            // {
            //     ConfigurationOptions = new ConfigurationOptions()
            // };
            // redisOptions.ConfigurationOptions.EndPoints.Add(cnstring);
            // var opts = Options.Create<RedisCacheOptions>(redisOptions);

            // IDistributedCache cache = new Microsoft.Extensions.Caching.StackExchangeRedis.RedisCache(opts);
        }

        public string ReadOne()
        {
            StreamEntry[] entries = db.StreamRange("nostromo-brett", maxId: "+", count: 1, messageOrder: Order.Descending);
            return string.Join(';', entries[0].Values.Select(x => x.Name + " " + x.Value).ToArray());
        }
    }
}
