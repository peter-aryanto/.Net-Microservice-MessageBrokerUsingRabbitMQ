using MassTransit;
using DataTransfer;
using System.Collections.Concurrent;

namespace UserServiceB.Consumers
{
  public class DataTransfer01Consumer : IConsumer<DataTransfer01>
  {
    // There is no database, so only this property is updated.
    private static readonly ConcurrentBag<DataTransfer01> _responseBag = new ConcurrentBag<DataTransfer01>();
    public static ConcurrentBag<DataTransfer01> ResponseBag { get { return _responseBag; } }

    public async Task Consume(ConsumeContext<DataTransfer01> context)
    {
      var message = context.Message;

      var newResponse = new DataTransfer01
      {
        Value = message.Value,
      };
      _responseBag.Add(newResponse);
    }
  }
}