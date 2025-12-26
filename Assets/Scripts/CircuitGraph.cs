using System.Collections.Generic;
using UnityEngine;

public class CircuitGraph : MonoBehaviour
{
    public static CircuitGraph Instance;

    // Node -> all nodes directly connected
    private Dictionary<string, HashSet<string>> edges = new();

    void Awake()
    {
        Instance = this;
    }

    // Create a unique graph node for each breadboard or Arduino pin
    public void Connect(string from, string to)
    {
        if (!edges.ContainsKey(from)) edges[from] = new HashSet<string>();
        if (!edges.ContainsKey(to)) edges[to] = new HashSet<string>();

        edges[from].Add(to);
        edges[to].Add(from);

        Debug.Log($"🔗 Connected: {from} <--> {to}");
    }

    // BFS connectivity check
    public bool IsConnected(string start, string target)
    {
        if (!edges.ContainsKey(start) || !edges.ContainsKey(target)) return false;

        HashSet<string> visited = new();
        Queue<string> q = new();
        q.Enqueue(start);

        while (q.Count > 0)
        {
            string n = q.Dequeue();
            if (n == target) return true;
            visited.Add(n);

            foreach (string link in edges[n])
                if (!visited.Contains(link))
                    q.Enqueue(link);
        }
        return false;
    }
    public bool IsNodeConflict(string node, string newPin)
    {
        // If the node doesn't exist yet, no conflict
        if (!edges.ContainsKey(node)) return false;

        foreach (var connectedNode in edges[node])
        {
            // If any connected node is another Arduino pin with a DIFFERENT name
            if (connectedNode.StartsWith("ARD_D") || connectedNode == "ARD_5V" || connectedNode == "ARD_3.3V")
            {
                if (connectedNode != newPin)
                    return true;
            }
        }
        return false;
    }

}
