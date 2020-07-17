using System.Threading.Tasks;

namespace Courier
{
    public interface ICourierListener<TEvent>
        where TEvent : class, ICourierEvent
    {
        Task Process(TEvent @event);
    }
}