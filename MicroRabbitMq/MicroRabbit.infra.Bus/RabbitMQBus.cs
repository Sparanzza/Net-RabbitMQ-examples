using System.Text;
using MediatR;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.Commands;
using MicroRabbit.Domain.Core.Events;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MicroRabbit.infra.Bus;

public class RabbitMqBus : IEventBus
{
    private readonly IMediator _mediator;
    private readonly Dictionary<string, List<Type>> _handlers;
    private readonly List<Type> _eventTypes;

    public RabbitMqBus(IMediator mediator)
    {
        _mediator = mediator;
        _handlers = new Dictionary<string, List<Type>>();
        _eventTypes = new List<Type>();
    }

    public Task SendCommand<T>(T command) where T : Command
    {
        return _mediator.Send(command);
    }

    public async Task Publish<T>(T @event) where T : Event
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();

        var eventName = @event.GetType().Name;
        await channel.QueueDeclareAsync(queue: eventName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
        var message = JsonConvert.SerializeObject(@event);
        var body = Encoding.UTF8.GetBytes(message);
        await channel.BasicPublishAsync(exchange: string.Empty, routingKey: eventName, body: body);
        Console.WriteLine($" [x] Sent {message}");
    }

    public async Task Subscribe<T, TH>() where T : Event where TH : IEventHandler<T>
    {
        var eventName = typeof(T).Name;
        var handlerType = typeof(TH);

        if (!_eventTypes.Contains(typeof(T)))
        {
            _eventTypes.Add(typeof(T));
        }

        if (!_handlers.ContainsKey(eventName))
        {
            _handlers.Add(eventName, new List<Type>());
        }

        if (_handlers[eventName].Any(s => s.GetType() == handlerType))
        {
            throw new ArgumentException($"Handler Type {handlerType.Name} already is registered for '{eventName}'",
                nameof(handlerType));
        }

        _handlers[eventName].Add(handlerType);

        await StartBasicConsume<T>();
    }

    private async Task StartBasicConsume<T>() where T : Event
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();
        var eventName = typeof(T).Name;
        
        await channel.QueueDeclareAsync(eventName, false, false, false, null);
        var consumer = new AsyncEventingBasicConsumer(channel);
        
        consumer.ReceivedAsync += Consumer_Received;
        await channel.BasicConsumeAsync(eventName, true, consumer);
    }

    private Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
    {
        var eventName = @event.RoutingKey;
        var message = Encoding.UTF8.GetString(@event.Body.ToArray());
        Console.WriteLine($" [x] Received {message}");

        try
        {
            _ = ProccessEvent(eventName, message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return Task.CompletedTask;
    }

    private async Task ProccessEvent(string eventName, string message)
    {
        if (_handlers.ContainsKey(eventName))
        {
            var subscriptions = _handlers[eventName];
            foreach (var subscription in subscriptions)
            {
                var handler = Activator.CreateInstance(subscription);
                if (handler == null) continue;
                var eventType = _eventTypes.SingleOrDefault(t => t.Name == eventName);
                if (eventType != null)
                {
                    var @event = JsonConvert.DeserializeObject(message, eventType);
                    var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);
                    await (Task) concreteType.GetMethod("Handle").Invoke(handler, [@event]);
                }
            }
        }
    }
}