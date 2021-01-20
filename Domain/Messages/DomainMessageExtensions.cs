using System;
using System.Collections.Generic;

namespace Domain.Messages
{
    public static class DomainMessageExtensions
    {
        public static List<DomainMessage> ToDomainMessages(this Exception ex)
        {
            var messages = new List<DomainMessage>();

            if (ex is null) return messages;

            messages.Add(new DomainMessage(ex.GetType().Name, ex.Message));
            messages.AddRange(ToDomainMessages(ex.InnerException));

            return messages;
        }
    }
}
