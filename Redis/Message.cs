using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace nostromo_muthur.Redis
{
    public class Message
    {
        private DateTime dt;

        private Dictionary<string, string> data = new Dictionary<string, string>();

        private readonly CultureInfo ru = System.Globalization.CultureInfo.CreateSpecificCulture("ru-RU");

        public Message(DateTime dt)
        {
            this.dt = dt;
        }

        public Dictionary<string, string> Data => new Dictionary<string, string>(data);
        public DateTime DateTime => dt;

        public override string ToString()
        {
            var entities = data.Select(kvp => kvp.Key + " " + kvp.Value).ToArray();

            return $"{dt.ToString(ru)}\r\n{string.Join("\r\n", entities)}";
        }

        internal Message Add(string key, string value)
        {
            this.data.Add(key, value);
            return this;
        }
    }
}
