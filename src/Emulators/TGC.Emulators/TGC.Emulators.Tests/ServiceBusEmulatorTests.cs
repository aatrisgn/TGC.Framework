using Azure.Messaging.ServiceBus;

namespace TGC.Emulators.Tests;

public sealed class ServiceBusEmulatorTests : IAsyncLifetime
{
    // 127.0.0.1: the Service Bus emulator's AMQP endpoint. SAS_KEY_VALUE is Microsoft's
    // documented emulator placeholder, not a real secret.
    private const string ConnectionString =
        "Endpoint=sb://127.0.0.1;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
    private const string QueueName = "queue.1";
    private const string TopicName = "topic.1";
    private const string SubscriptionName = "subscription.1";

    private ServiceBusClient _client = null!;

    public async Task InitializeAsync()
    {
        _client = new ServiceBusClient(ConnectionString);

        await EmulatorRetry.UntilSuccessAsync(async () =>
        {
            await using var receiver = _client.CreateReceiver(QueueName);
            await receiver.PeekMessageAsync();
            return true;
        });
    }

    public async Task DisposeAsync() => await _client.DisposeAsync();

    [Fact]
    public async Task PushAndReadMessage_UsingQueue()
    {
        var body = $"queue-message-{Guid.NewGuid()}";

        await using var sender = _client.CreateSender(QueueName);
        await sender.SendMessageAsync(new ServiceBusMessage(body));

        await using var receiver = _client.CreateReceiver(QueueName);
        var received = await receiver.ReceiveMessageAsync(TimeSpan.FromSeconds(30));

        Assert.NotNull(received);
        Assert.Equal(body, received!.Body.ToString());

        await receiver.CompleteMessageAsync(received);
    }

    [Fact]
    public async Task PushAndReadMessage_UsingTopicSubscription()
    {
        var body = $"topic-message-{Guid.NewGuid()}";

        await using var sender = _client.CreateSender(TopicName);
        await sender.SendMessageAsync(new ServiceBusMessage(body));

        await using var receiver = _client.CreateReceiver(TopicName, SubscriptionName);
        var received = await receiver.ReceiveMessageAsync(TimeSpan.FromSeconds(30));

        Assert.NotNull(received);
        Assert.Equal(body, received!.Body.ToString());

        await receiver.CompleteMessageAsync(received);
    }
}
