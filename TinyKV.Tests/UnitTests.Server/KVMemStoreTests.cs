using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using TinyKV.Server;
using Xunit;

namespace TinyKV.Tests.UnitTests.Server;

public class KVMemStoreTests
{
    private readonly KVMemStore _store;

    public KVMemStoreTests()
    {
        var testDictionary = new ConcurrentDictionary<string, string>();
        _store = new KVMemStore(testDictionary);
    }

    [Theory]
    [InlineData("", "value")]
    [InlineData("key1", "value1")]
    [InlineData("key2", "value2")]
    [InlineData("key3", "value3")]
    [InlineData("🚧", "🤣")]
    public async Task Set_And_Get_ShouldStoreAndRetrieveValue(string key, string value)
    {
        await _store.Set(key, value);
        var result = await _store.Get(key);
        Assert.Equal(value, result);
    }

    [Theory]
    [InlineData(null, "value")]
    public async Task Set_NullKey_ShouldThrowException(string key, string value)
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _store.Set(key, value));
    }

    [Theory]
    [InlineData(null)]
    public async Task Get_NullKey_ShouldThrowException(string key)
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _store.Get(key));
    }

    [Fact]
    public async Task Set_ExistingKey_ShouldOverwriteValue()
    {
        await _store.Set("key", "initialValue");
        await _store.Set("key", "newValue");
        var result = await _store.Get("key");
        Assert.Equal("newValue", result);
    }

    [Theory]
    [InlineData("existingKey1")]
    [InlineData("existingKey2")]
    [InlineData("existingKey3")]
    public async Task Has_ExistingKey_ShouldReturnTrue(string key)
    {
        await _store.Set(key, "someValue");
        var result = await _store.Has(key);
        Assert.True(result);
    }

    [Theory]
    [InlineData("nonExistingKey1")]
    [InlineData("nonExistingKey2")]
    [InlineData("nonExistingKey3")]
    public async Task Has_NonExistingKey_ShouldReturnFalse(string key)
    {
        var result = await _store.Has(key);
        Assert.False(result);
    }

    [Theory]
    [InlineData("deleteKey1")]
    [InlineData("deleteKey2")]
    [InlineData("deleteKey3")]
    public async Task Delete_ExistingKey_ShouldRemoveKey(string key)
    {
        await _store.Set(key, "someValue");
        var deleted = await _store.Delete(key);
        var existsAfterDelete = await _store.Has(key);

        Assert.True(deleted);
        Assert.False(existsAfterDelete);
    }

    [Theory]
    [InlineData("nonExistingKey1")]
    [InlineData("nonExistingKey2")]
    [InlineData("nonExistingKey3")]
    public async Task Delete_NonExistingKey_ShouldReturnFalse(string key)
    {
        var deleted = await _store.Delete(key);
        Assert.False(deleted);
    }

    [Fact]
    public async Task Delete_AlreadyDeletedKey_ShouldReturnFalse()
    {
        await _store.Set("key", "value");
        await _store.Delete("key");
        var result = await _store.Delete("key");
        Assert.False(result);
    }

    [Fact]
    public async Task Clear_ShouldRemoveAllKeys()
    {
        await _store.Set("key1", "value1");
        await _store.Set("key2", "value2");
        await _store.Clear();

        var exists1 = await _store.Has("key1");
        var exists2 = await _store.Has("key2");

        Assert.False(exists1);
        Assert.False(exists2);
    }

    [Fact]
    public async Task Clear_AfterLargeInsert_ShouldRemoveAll()
    {
        for (int i = 0; i < 5000; i++)
        {
            await _store.Set($"key{i}", $"value{i}");
        }

        await _store.Clear();

        int remainingKeys = 0;
        for (int i = 0; i < 5000; i++)
        {
            if (await _store.Has($"key{i}"))
                remainingKeys++;
        }

        Assert.Equal(0, remainingKeys);
    }

    [Fact]
    public async Task SetBatch_ShouldStoreMultipleKeyValuePairs()
    {
        var batchData = new Dictionary<string, string>
        {
            { "batchKey1", "batchValue1" },
            { "batchKey2", "batchValue2" },
            { "batchKey3", "batchValue3" }
        };

        await _store.SetBatch(batchData);

        var result1 = await _store.Get("batchKey1");
        var result2 = await _store.Get("batchKey2");
        var result3 = await _store.Get("batchKey3");

        Assert.Equal("batchValue1", result1);
        Assert.Equal("batchValue2", result2);
        Assert.Equal("batchValue3", result3);
    }

    [Fact]
    public async Task GetBatch_ShouldReturnValuesForExistingKeys()
    {
        await _store.Set("batchKey1", "batchValue1");
        await _store.Set("batchKey2", "batchValue2");

        var keys = new List<string> { "batchKey1", "batchKey2", "nonExistingKey" };
        var results = await _store.GetBatch(keys);

        Assert.Equal("batchValue1", results["batchKey1"]);
        Assert.Equal("batchValue2", results["batchKey2"]);
        Assert.False(results.ContainsKey("nonExistingKey"));
    }

    [Fact]
    public async Task HasBatch_ShouldReturnCorrectExistenceStates()
    {
        await _store.Set("batchKey1", "batchValue1");
        var keys = new List<string> { "batchKey1", "batchKey2" };
        var results = await _store.HasBatch(keys);

        Assert.True(results[0]);
        Assert.False(results[1]);
    }

    [Fact]
    public async Task DeleteBatch_ShouldRemoveMultipleKeys()
    {
        await _store.Set("batchKey1", "batchValue1");
        await _store.Set("batchKey2", "batchValue2");
        var keys = new List<string> { "batchKey1", "batchKey2" };

        var deletedCount = await _store.DeleteBatch(keys);
        var exists1 = await _store.Has("batchKey1");
        var exists2 = await _store.Has("batchKey2");

        Assert.Equal(2, deletedCount);
        Assert.False(exists1);
        Assert.False(exists2);
    }

    [Fact]
    public async Task Set_ConcurrentAccess_ShouldMaintainIntegrity()
    {
        var tasks = Enumerable.Range(0, 1000)
            .Select(i => _store.Set($"key{i}", $"value{i}"))
            .ToArray();

        await Task.WhenAll(tasks);

        int existingCount = 0;
        for (int i = 0; i < 1000; i++)
        {
            if (await _store.Has($"key{i}"))
                existingCount++;
        }

        Assert.Equal(1000, existingCount);
    }

    [Fact]
    public async Task SetBatch_LargeDataset_ShouldStoreAll()
    {
        var batch = new Dictionary<string, string>();
        for (int i = 0; i < 10_000; i++)
        {
            batch[$"key{i}"] = $"value{i}";
        }

        await _store.SetBatch(batch);

        var result = await _store.GetBatch([.. batch.Keys]);
        Assert.Equal(10_000, result.Count);
    }
}
