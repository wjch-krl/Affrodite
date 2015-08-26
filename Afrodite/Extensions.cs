using System;
using Afrodite.Connection;
using Apache.NMS;

namespace Afrodite
{
    public static class Extensions
    {
        public static bool IsAcknowledge(this IMessage message)
        {
            MessageType type;
            if (!Enum.TryParse(message.NMSType, true, out type))
            {
                return false;
            }
            return type == MessageType.Affirmation;
        }
    }
}
