using System;

namespace nostromo_muthur.Domain.Messages
{
    class Ds18b20Message : AbstractMessage
    {
        public float Temperature;

        public override string ToString()
        {
            return $"{base.ToString()}\r\n<b>Temperature: {(Math.Floor(Temperature * 10) / 10)}</b>";
        }
    }
}
