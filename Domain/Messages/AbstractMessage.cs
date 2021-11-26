using System.Globalization;
using nostromo_muthur.Domain.Sensors;

namespace nostromo_muthur.Domain.Messages
{
    abstract class AbstractMessage
    {
        public System.DateTime dateTime;

        public SensorType sensorType = SensorType.Unknown;

        private readonly CultureInfo ru = System.Globalization.CultureInfo.CreateSpecificCulture("ru-RU");


        public static AbstractMessage Create(nostromo_muthur.Redis.Message message)
        {
            var st = SensorType.Create(message.Data["sensor type"]); // !!!*** как-то должно пересекаться с протобафом
            if (st == SensorType.Ds18b20)
            {
                var m = new Ds18b20Message();
                m.dateTime = message.DateTime;
                m.sensorType = SensorType.Ds18b20;
                m.Temperature = float.Parse(message.Data["temperature"]); // !!!*** как-то должно пересекаться с протобафом
                return m;
            }

            throw new System.IndexOutOfRangeException(st.Code);
        }

        public override string ToString()
        {
            return string.Join("\r\n", new[] {
                $"date: {dateTime.ToString(ru)}",
                $"sensor type: {sensorType.Code}"
            });
        }
    }
}
