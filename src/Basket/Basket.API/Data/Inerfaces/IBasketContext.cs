using StackExchange.Redis;

namespace Basket.API.Data.Inerfaces
{
    public interface IBasketContext
    {
        IDatabase Redis { get; }
    }
}
