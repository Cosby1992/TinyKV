using System;
using System.Threading.Tasks;
using TinyKV.Client;

class Program
{
    static async Task Main()
    {
        var client = new KVStoreClient("http://localhost:5009/tinykv");
        await client.StartAsync();

        Console.WriteLine("Setting key 'key:1' to 'value:1'...");
        await client.SetAsync("key:1", "value:1");
        Console.WriteLine("Setting key 'key:2' to 'value:2'...");
        await client.SetAsync("key:2", "value:2");
        Console.WriteLine("Setting key 'key:3' to 'value:3'...");
        await client.SetAsync("key:3", "value:3");

        string value = await client.GetAsync("key:1");
        Console.WriteLine($"Get('key:1') => {value}");
        string value2 = await client.GetAsync("key:2");
        Console.WriteLine($"Get('key:2') => {value2}");
        string value3 = await client.GetAsync("key:3");
        Console.WriteLine($"Get('key:3') => {value3}");
        string noexist = await client.GetAsync("noexist");
        Console.WriteLine($"Get('noexist') => {noexist}");

        await client.DeleteAsync("key:1");
        Console.WriteLine("Deleted 'key:1'");
        await client.DeleteAsync("key:2");
        Console.WriteLine("Deleted 'key:2'");

        string valueVerify = await client.GetAsync("key:1");
        Console.WriteLine($"Get('key:1') => {valueVerify}");
        string valueVerify2 = await client.GetAsync("key:2");
        Console.WriteLine($"Get('key:2') => {valueVerify2}");
        string valueVerify3 = await client.GetAsync("key:3");
        Console.WriteLine($"Get('key:3') => {valueVerify3}");
        string noexistvalueVerify = await client.GetAsync("noexist");
        Console.WriteLine($"Get('noexist') => {noexistvalueVerify}");

        Console.ReadLine();
    }
}
