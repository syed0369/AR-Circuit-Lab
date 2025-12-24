// using System.Collections.Generic;
// using UnityEngine;

// public class GraphCircuit : MonoBehaviour
// {
//     public static GraphCircuit Instance;

//     Dictionary<string, HashSet<string>> graph =
//         new Dictionary<string, HashSet<string>>();

//     void Awake()
//     {
//         Instance = this;
//     }

//     public void Connect(string a, string b)
//     {
//         if (!graph.ContainsKey(a)) graph[a] = new HashSet<string>();
//         if (!graph.ContainsKey(b)) graph[b] = new HashSet<string>();

//         graph[a].Add(b);
//         graph[b].Add(a);
//     }

//     public bool IsConnected(string a, string b)
//     {
//         HashSet<string> visited = new HashSet<string>();
//         return DFS(a, b, visited);
//     }

//     bool DFS(string current, string target, HashSet<string> visited)
//     {
//         if (current == target) return true;
//         if (!graph.ContainsKey(current)) return false;

//         visited.Add(current);

//         foreach (var n in graph[current])
//         {
//             if (!visited.Contains(n))
//             {
//                 if (DFS(n, target, visited)) return true;
//             }
//         }
//         return false;
//     }
// }
