using System;
using System.Linq;
using StackExchange.Redis;
namespace nostromo_muthur.Redis
{
    class Client
    {
        private static ConnectionMultiplexer redis;

        private readonly IDatabase db;


        public Client(string host)
        {
            string server = host;
            string port = "6379";
            string cnstring = $"{server}:{port}";
            // string cnstring = $"{server}:{port},abortConnect=false";

            if (redis == null)
            {
                redis = ConnectionMultiplexer.Connect(cnstring);
            }
            db = redis.GetDatabase();

        }

        public Message ReadLast()
        {
            StreamEntry[] entries = db.StreamRange("nostromo-brett", maxId: "+", count: 1, messageOrder: Order.Descending);
            var id = entries[0].Id;
            var timestring = id.ToString().Split('-')[0];
            long timestamp = Convert.ToInt64(timestring);
            var datetime = DateTime.UnixEpoch.AddMilliseconds(timestamp).ToLocalTime();

            Message m = new Message(datetime);

            foreach (var pair in entries[0].Values)
            {
                m.Add(pair.Name, pair.Value);
            }

            return m;
        }
    }
}
