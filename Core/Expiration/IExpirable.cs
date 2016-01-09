using System;

namespace NetDotNet.Core.Expiration
{
    internal interface IExpirable
    {
        void Expire(bool early);
        DateTime? GetExpiration();
    }
}
