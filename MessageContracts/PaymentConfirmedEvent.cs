using System;

namespace MessageContracts
{
    public interface PaymentConfirmedEvent
    {
        Guid MessageId { get; set; }
    }
}
