//using System.Collections.Concurrent;

//namespace FlowAISystem.Application.AI.Memory;

//public class AIConversationMemory
//{
//    private readonly ConcurrentDictionary<string, object> _memory =
//        new();

//    // ==========================================
//    // Save
//    // ==========================================

//    public void Set<T>(
//        string key,
//        T value)
//    {
//        _memory[key] = value!;
//    }

//    // ==========================================
//    // Read
//    // ==========================================

//    public T? Get<T>(
//        string key)
//    {
//        if (_memory.TryGetValue(key, out var value))
//        {
//            return (T)value;
//        }

//        return default;
//    }

//    // ==========================================
//    // Exists
//    // ==========================================

//    public bool Contains(
//        string key)
//    {
//        return _memory.ContainsKey(key);
//    }

//    // ==========================================
//    // Remove
//    // ==========================================

//    public void Remove(
//        string key)
//    {
//        _memory.TryRemove(key, out _);
//    }

//    // ==========================================
//    // Clear
//    // ==========================================

//    public void Clear()
//    {
//        _memory.Clear();
//    }
//}

using System.Collections.Concurrent;

namespace FlowAISystem.Application.AI.Memory;

public class AIConversationMemory
{
    private readonly ConcurrentDictionary<string, object> _memory = new();


    public void Set(
        string key,
        object value)
    {
        _memory[key] = value;
    }


    public T? Get<T>(
        string key)
    {
        if (_memory.TryGetValue(key, out var value))
        {
            return (T)value;
        }

        return default;
    }


    public bool Contains(
        string key)
    {
        return _memory.ContainsKey(key);
    }


    public void Remove(
        string key)
    {
        _memory.TryRemove(key, out _);
    }


    public void Clear()
    {
        _memory.Clear();
    }
}