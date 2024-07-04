using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEngine.Tilemaps;
using System.IO;
using UnityEditor.Experimental.GraphView;
using System;

public class HexagonGrid : MonoBehaviour
{
    public int width;  // Width should always be 1 more than the Height
    public int height;

    public Node nodePrefab;

    private float nodeWidth;
    private float nodeHeight;

    public Node[,] grid;
    public LevelBaseObject LevelInfo;

    public List<Node> AdjacencyList;

    public List<Node> SpecialNodes;

    public bool PathFound;

    public void Start()
    {
        AdjacencyList = new List<Node>();
        SpecialNodes = new List<Node>();
        nodeWidth = nodePrefab.GetComponentInChildren<SpriteRenderer>().bounds.size.x;
        nodeHeight = nodePrefab.GetComponentInChildren<SpriteRenderer>().bounds.size.y;
        //nodeWidth = transform.localScale.x;
        //nodeHeight = transform.localScale.y;
        PathFound = false;

        Debug.Log("Node Height: " + nodeHeight);
        Debug.Log("Node Width: " + nodeWidth);

        grid = LevelInfo.map;
        width = LevelInfo.width;
        height = LevelInfo.height;

        PopulateGrid();       

    }  

    private void Update()
    {
        foreach (Node node in AdjacencyList)
        {
            node.CheckNodeBehavior();
        }

        if (isCyclic())
        {
            Debug.Log("Graph contains cycle");
        }
        else
        {
            Debug.Log("Graph doesn't contain cycle");
        }
    }

    /// <summary>
    /// Sets up the grid verticies and the adjacency matrix
    /// </summary>
    public void PopulateGrid()
    {
        Vector3 location;

        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                location = new Vector3(i, 0, 0) * nodeWidth +
                    new Vector3(0, j, 0) * nodeHeight * .75f +
                    ((j % 2) == 0 ? Vector3.zero : new Vector3(1, 0, 0) * nodeWidth * .5f);

                grid[i, j] = Instantiate(nodePrefab, location, Quaternion.identity, transform).GetComponent<Node>();
                grid[i, j].x = i;
                grid[i, j].y = j;

                Node gridNode = grid[i, j].GetComponent<Node>();

                AddVertex(gridNode);
            }
        }
        SetSpecialNodes();



        //grid[2, 2].type = NodeType.SharpTurn;

        foreach (Node node in AdjacencyList)
        {
            if (node.type != NodeType.None)
            {
                SpecialNodes.Add(node);
                node.UpdateColor();
            }
        }

        //grid[2, 2].GetComponentInChildren<SpriteRenderer>().color = Color.red;
    }


    public void SetSpecialNodes()
    {
        foreach (Vector2Int loc in LevelInfo.straightNodeLocations)
        {
            grid[loc.x, loc.y].type = NodeType.Straight;
        }

        foreach (Vector2Int loc in LevelInfo.sharpTurnNodeLocations)
        {
            grid[loc.x, loc.y].type = NodeType.SharpTurn;
        }

        foreach (Vector2Int loc in LevelInfo.wideTurnNodeLocations)
        {
            grid[loc.x, loc.y].type = NodeType.WideTurn;
        }
    }


    /// <summary>
    /// Removes all of the Edge's on each Space
    /// </summary>
    public void RemoveAllEdges()
    {
        foreach (Node sp in AdjacencyList)
        {
            sp.Edges.Clear();
        }
    }

    /// <summary>
    /// Adds a new vertex to the graph
    /// </summary>
    /// <param name="newVertex">Name of the new vertex</param>
    /// <returns>Returns the success of the operation</returns>
    public bool AddVertex(Node newVertex)
    {
        // Ignore duplicate vertices.
        if (AdjacencyList.Find(v => v == newVertex) != null) return true;

        // Add vertex to the graph
        AdjacencyList.Add(newVertex);
        return true;
    }

    /// <summary>
    /// Adds a new edge between two given vertices in the graph
    /// </summary>
    /// <param name="v1">Name of the first vertex</param>
    /// <param name="v2">Name of the second vertex</param>
    /// <returns>Returns the success of the operation</returns>
    public bool AddAnEdge(Node v1, Node v2)
    {
        // Add vertex v2 to the edges of vertex v1
        AdjacencyList.Find(v => v == v1).Edges.Add(v2);

        // Add vertex v1 to the edges of vertex v2
        AdjacencyList.Find(v => v == v2).Edges.Add(v1);

        return true;
    }

    /// <summary>
    /// Removes an edge between two given vertices in the graph
    /// </summary>
    /// <param name="v1">Name of the first vertex</param>
    /// <param name="v2">Name of the second vertex</param>
    /// <returns>Returns the success of the operation</returns>
    public bool RemoveAnEdge(Node v1, Node v2)
    {
        // We assume all edges are valid and already exist.

        // Remove vertex v2 to the edges of vertex v1
        AdjacencyList.Find(v => v == v1).Edges.Remove(v2);

        // Remove vertex v1 to the edges of vertex v2
        AdjacencyList.Find(v => v == v2).Edges.Remove(v1);

        return true;
    }

    /// <summary>
    /// Returns whether or not there is an edge between the two Spaces
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <returns></returns>
    public bool DoesEdgeExist(Node v1, Node v2)
    {
        if (AdjacencyList.Find(v => v == v1).Edges.Contains(v2) ||
        AdjacencyList.Find(v => v == v2).Edges.Contains(v1))
        {
            return true;
        }
        return false;

    }

    // A recursive function that uses visited[]
    // and parent to detect cycle in subgraph
    // reachable from vertex v.
    bool isCyclicUtil(Node v, List<Node> visited, Node parent)
    {
        // Mark the current node as visited
        visited.Find(temp => temp == v).Visited = true;

        // Recur for all the vertices
        // adjacent to this vertex
        foreach (Node node in AdjacencyList.Find(temp => temp == v).Edges)
        {
            // If an adjacent is not visited,
            // then recur for that adjacent
            if (!visited.Find(temp => temp == node).Visited)
            {
                if (isCyclicUtil(node, visited, v))
                    return true;
            }

            // If an adjacent is visited and
            // not parent of current vertex,
            // then there is a cycle.
            else if (node != parent)
                return true;
        }
        return false;
    }

    // Returns true if the graph contains
    // a cycle, else false.
    private bool isCyclic()
    {
        // Mark all the vertices as not visited
        // and not part of recursion stack
        int count = AdjacencyList.Count;

        List<Node> visited = new List<Node>(AdjacencyList);
        for (int i = 0; i < count; i++)
        {
            visited[i].Visited = false;
        }
        // Call the recursive helper function
        // to detect cycle in different DFS trees
        for (int u = 0; u < count; u++)
        {
            // Don't recur for u if already visited
            if (!visited[u].Visited)
            { 
                if (isCyclicUtil(visited[u], visited, null))
                    return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Checks to see if there is a completed loop that combines all of the special nodes
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    public void CheckCompleteLoop(Node start, Node end)
    {
        Dictionary<Node, bool> visited = new Dictionary<Node, bool>();
        Dictionary<Node, Node> path = new Dictionary<Node, Node>();

        Queue<Node> worklist = new Queue<Node>();

        visited.Add(start, false);

        worklist.Enqueue(start);

        while (worklist.Count != 0)
        {
            Node node = worklist.Dequeue();

            foreach (Node neighbor in node.Edges)
            {
                if (!visited.ContainsKey(neighbor))
                {
                    visited.Add(neighbor, true);
                    path.Add(neighbor, node);
                    worklist.Enqueue(neighbor);
                }
            }
        }

        if (path.ContainsKey(end) && CheckSpecialNodes())
        {
            Node startEnd = end;
            while (end != start)
            {
                //Debug.Log(startEnd + ": " + end);
                Debug.Log("Path Found!!!");

                end = path[end];
            }
            PathFound = true;
        }
    }

    /// <summary>
    /// Checks each of the Straight, SharpTurn, and WideTurn Nodes to see if their conditions are met
    /// </summary>
    /// <returns></returns>
    public bool CheckSpecialNodes()
    {
        foreach(Node node in SpecialNodes)
        {
            if(!node.IsSatisfied)
            {
                return false;
            }
        }
        return true;
    }

    //public bool DetectLoop(Node h)
    //{
    //    HashSet<Node> s = new HashSet<Node>();

        


    //    while (h != null)
    //    {
    //        // If we have already has this node
    //        // in hashmap it means there is a cycle
    //        // (Because you we encountering the
    //        // node second time).
    //        if (s.Contains(h))
    //            return true;

    //        // If we are seeing the node for
    //        // the first time, insert it in hash
    //        s.Add(h);

    //        h = h.next;
    //    }

    //    return false;
    //}





}
