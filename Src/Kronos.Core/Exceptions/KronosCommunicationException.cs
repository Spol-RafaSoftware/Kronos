﻿using System;

namespace Kronos.Core.Exceptions
{
    public class KronosCommunicationException : KronosException
    {
        public KronosCommunicationException()
        {
        }

        public KronosCommunicationException(string message)
            : base(message)
        {
        }

        public KronosCommunicationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
