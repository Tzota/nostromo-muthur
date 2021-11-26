using System.ComponentModel;

namespace nostromo_muthur.Domain.Sensors
{
    // Копия есть в nostromo-parker
    [ImmutableObject(true)]
    public class SensorType
    {
        private readonly string code;
        private SensorType(string code)
        {
            this.code = code;
        }

        public string Code => code;

        public override string ToString()
        {
            return this.code;
        }

        private const string UnknownCode = "Unknown";
        public static readonly SensorType Unknown = new SensorType(UnknownCode);


        // Ds18b20 temperature sensor
        private const string Ds18b20Code = "Ds18b20";
        public static readonly SensorType Ds18b20 = new SensorType(Ds18b20Code);

        // Counter is a simple counter beacon
        private const string CounterCode = "counter";
        public static readonly SensorType Counter = new SensorType(CounterCode);

        public static SensorType Create(string code)
        {
            return code switch
            {
                Ds18b20Code => Ds18b20,
                CounterCode => Counter,
                _ => throw new UnknownSensorTypeException(code)
            };
        }
    }
}
