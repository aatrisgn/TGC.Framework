using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Producer;

namespace TGC.Emulators.Tests;

public sealed class EventHubEmulatorTests : IAsyncLifetime
{
    // 127.0.0.2, not 127.0.0.1: the Event Hubs emulator's AMQP endpoint is bound to a
    // separate loopback IP so it doesn't collide with the Service Bus emulator on port
    // 5672 — see src/Emulators/docker/README.md for why. SAS_KEY_VALUE is Microsoft's
    // documented emulator placeholder, not a real secret.
    private const string ConnectionString =
        "Endpoint=sb://127.0.0.2;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;EntityPath=eh1";
    private const string ConsumerGroup = "cg1";

    private EventHubProducerClient _producer = null!;

    public async Task InitializeAsync()
    {
        _producer = new EventHubProducerClient(ConnectionString);
        await EmulatorRetry.UntilSuccessAsync(() => _producer.GetEventHubPropertiesAsync());
    }

    public async Task DisposeAsync() => await _producer.DisposeAsync();

    [Fact]
    public async Task PushAndReadMessage_FromEventHub()
    {
        var body = $"event-{Guid.NewGuid()}";

        await using var consumer = new EventHubConsumerClient(ConsumerGroup, ConnectionString);
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));

        // Start reading from "now" (not earliest) so this doesn't have to wade through
        // events left over from previous test runs against the same running emulator.
        var receiveTask = Task.Run(async () =>
        {
            try
            {
                await foreach (var partitionEvent in consumer.ReadEventsAsync(startReadingAtEarliestEvent: false, cancellationToken: cts.Token))
                {
                    var candidate = partitionEvent.Data?.EventBody.ToString();
                    if (candidate == body)
                    {
                        return candidate;
                    }
                }
            }
            catch (OperationCanceledException)
            {
            }

            return null;
        });

        // Give the consumer a moment to attach to every partition before we publish,
        // otherwise the event could be sent before the read loop starts listening.
        await Task.Delay(TimeSpan.FromSeconds(2));

        using var eventBatch = await _producer.CreateBatchAsync();
        Assert.True(eventBatch.TryAdd(new EventData(body)));
        await _producer.SendAsync(eventBatch);

        var receivedBody = await receiveTask;
        Assert.Equal(body, receivedBody);
    }
}
