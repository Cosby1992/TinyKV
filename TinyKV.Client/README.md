# 🚀 TinyKV.Client - Lightweight SignalR Client for TinyKV  

**TinyKV.Client** is a lightweight **C# SignalR client** designed to interact with a running **TinyKV Server**. It provides an easy-to-use API for real-time key-value operations over **SignalR**, supporting both **single** and **batch operations**.

---

## 📦 Installation  

To use **TinyKV.Client**, install it via NuGet:

```sh
dotnet add package TinyKV.Client --version 1.0.0
```

Or manually add it to your `.csproj`:

```xml
<PackageReference Include="TinyKV.Client" Version="1.0.0" />
```

### **🔹 Prerequisites**
- **A running TinyKV Server** (Ensure you have access to an active TinyKV instance).
- The server should be accessible via a **SignalR endpoint** at:  
  ```
  http://localhost:5009/tinykv
  ```

---

## ⚙️ Usage  

### **1️⃣ Initialize the Client**
Connect to a **running TinyKV Server**:

```csharp
var client = new TinyKVClient("http://localhost:5009/tinykv");
await client.StartAsync();
```

### **2️⃣ Perform Key-Value Operations**  

#### **Set a Value**  
```csharp
await client.SetAsync("username", "Alice");
```

#### **Get a Value**  
```csharp
string value = await client.GetAsync("username");
Console.WriteLine(value); // Alice
```

#### **Check if a Key Exists**  
```csharp
bool exists = await client.HasAsync("username");
```

#### **Delete a Key**  
```csharp
bool deleted = await client.DeleteAsync("username");
```

#### **Clear All Keys**  
```csharp
await client.ClearAsync();
```

---

## 🔄 Batch Operations  

**Set multiple values:**  
```csharp
await client.SetBatchAsync(new Dictionary<string, string>
{
    { "key1", "value1" },
    { "key2", "value2" }
});
```

**Retrieve multiple values:**  
```csharp
var values = await client.GetBatchAsync(new List<string> { "key1", "key2" });
```

**Check multiple keys:**  
```csharp
bool[] exists = await client.HasBatchAsync(new List<string> { "key1", "key2" });
```

**Delete multiple keys:**  
```csharp
int deletedCount = await client.DeleteBatchAsync(new List<string> { "key1", "key2" });
```

---

## 📜 License  

This project is licensed under the **MIT License**.