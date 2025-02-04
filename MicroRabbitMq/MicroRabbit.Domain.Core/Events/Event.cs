namespace MicroRabbit.Domain.Core.Events;
public class Event
{
    public DateTime TimeStamp { get; protected set; }

    protected Event()
    {
        TimeStamp = DateTime.Now;
    }
}
