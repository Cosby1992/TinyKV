# 🏗️ TinyKV - A Lightweight Key-Value Store over SignalR

**TinyKV** is a lightweight, in-memory key-value store built using **ASP.NET Core** and **SignalR**. It provides both single and batch operations for key-value management, enabling efficient data storage with real-time communication.

## 🚀 Features

- **Real-time Communication**: Uses SignalR for instant key-value operations.
- **In-Memory Storage**: Utilizes `ConcurrentDictionary` for fast and thread-safe access.
- **Batch Operations**: Supports bulk `set`, `get`, `has`, and `delete` operations.
- **Lightweight & Fast**: Optimized for low-latency access with minimal overhead.

---

## 📦 Installation & Setup

### **Prerequisites**
- .NET **7.0+**
- SignalR-compatible client
- Optional: **Docker** for containerized deployment

### **Running the TinyKV Server**
Clone the repository and navigate to the project directory:

```sh
git clone https://github.com/Cosby1992/TinyKV.git
cd TinyKV
```

Run the server:

```sh
dotnet run --project TinyKV.Server
```

The server will start and listen for connections at:
```
http://localhost:5009/tinykv
```

---

## ⚙️ Architecture

TinyKV consists of the following components:

### **Server (`TinyKV.Server`)**
- Built with **ASP.NET Core** and **SignalR**.
- Uses **ConcurrentDictionary** for in-memory storage.
- Supports key-value operations: `Get`, `Set`, `Has`, `Delete`, `Clear`.
- Provides batch operations for handling multiple keys at once.

### **Client (`TinyKV.Client`)**
- Implements a **C# SignalR client** to interact with the TinyKV server.
- Supports both single and batch operations.
- Uses asynchronous operations for efficiency.
- Designed with **dependency injection** for testability.

### **Tests (`TinyKV.Tests`)**
- Includes **unit tests** using **Moq** and **XUnit**.
- Covers both single and batch key-value operations.

Run the tests with:

```sh
dotnet test
```

---

## 🔗 API Reference

### **Single Operations**
| Method         | Description                         |
|---------------|-------------------------------------|
| `SetAsync`    | Store a key-value pair             |
| `GetAsync`    | Retrieve a value for a given key   |
| `HasAsync`    | Check if a key exists              |
| `DeleteAsync` | Remove a key-value pair            |
| `ClearAsync`  | Clears all key-value pairs         |

### **Batch Operations**
| Method          | Description                        |
|----------------|------------------------------------|
| `SetBatchAsync` | Store multiple key-value pairs    |
| `GetBatchAsync` | Retrieve multiple values         |
| `HasBatchAsync` | Check multiple keys existence    |
| `DeleteBatchAsync` | Remove multiple keys          |

---

## 📦 Docker Support

Run TinyKV in a **Docker container**:

1. **Build the Docker image**:
   ```sh
   docker build -t tinykv .
   ```

2. **Run the container**:
   ```sh
   docker run -p 5009:5009 tinykv
   ```

---

## 📜 License

This project is licensed under the **MIT License**.