namespace TinyKV.Server;

public class SignalRConfig
{
    const long DefaultMaximumReceiveMessageSize = 1024 * 32; // 32KB default
    public long MaximumReceiveMessageSize { get; set; } = DefaultMaximumReceiveMessageSize;
}

public class StoreConfig
{
    const int DefaultStoreInitialCapacity = 2000;
    const int DefaultStoreConcurrencyLevel = 1; // one per processing core
    public int ConcurrencyLevel { get; set; } = DefaultStoreConcurrencyLevel;
    public int InitialCapacity { get; set; } = DefaultStoreInitialCapacity;
}
