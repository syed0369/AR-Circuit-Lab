using System.Collections.Generic;
using UnityEngine;

public class BreadboardGraph : MonoBehaviour
{
    public static BreadboardGraph Instance;

    private Dictionary<string, HashSet<string>> graph = new();

    void Awake()
    {
        Instance = this;
    }

    public void Connect(BPinID a, BPinID b)
    {
        AddEdge(a.nodeId, b.nodeId);
        Debug.Log($"Connected {a.nodeId} <-> {b.nodeId}");
    }

    void AddEdge(string a, string b)
    {
        if (!graph.ContainsKey(a)) graph[a] = new();
        if (!graph.ContainsKey(b)) graph[b] = new();

        graph[a].Add(b);
        graph[b].Add(a);
    }

    public bool AreConnected(string a, string b)
    {
        var visited = new HashSet<string>();
        return DFS(a, b, visited);
    }

    bool DFS(string cur, string target, HashSet<string> visited)
    {
        if (cur == target) return true;
        visited.Add(cur);

        if (!graph.ContainsKey(cur)) return false;

        foreach (var n in graph[cur])
            if (!visited.Contains(n) && DFS(n, target, visited))
                return true;

        return false;
    }
}
