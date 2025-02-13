using MediatR;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbitMq.Banking.Domain.Commands;
using MicroRabbitMq.Banking.Domain.Events;

namespace MicroRabbitMq.Banking.Domain.CommandHandlers;

public class TransferCommandHandler : IRequestHandler<CreateTransferCommand, bool>
{
    private readonly IEventBus _bus;

    public TransferCommandHandler(IEventBus bus)
    {
        _bus = bus;
    }

    public async Task<bool> Handle(CreateTransferCommand request, CancellationToken cancellationToken)
    {
        //publish event to RabbitMQ
        await _bus.Publish(new TransferCreatedEvent(request.From, request.To, request.Amount));
        return await Task.FromResult(true);
    }
}