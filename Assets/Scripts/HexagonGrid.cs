using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using static TouchManager;


public struct Edge
{
    public Vector2Int A;
    public Vector2Int B;
}

public class HexagonGrid : MonoBehaviour
{
    public int width;  // Width should always be 1 more than the Height
    public int height;

    public Node nodePrefab;

    public float nodeWidth { get; private set; }
    public float nodeHeight { get; private set; }

    public Node[,] grid;
    public LevelBaseObject LevelInfo;

    public List<Node> AdjacencyList;

    public List<Node> SpecialNodes;

    private List<Node> NodesWithEdges;

    public List<Edge> Edges;

    //private List<Node> visited; 
    private List<Node> path;

    public bool PathFound;

    public Camera cam;


    public Vector3[] compass = { Vector3.right,
        new Vector3(.5f, 0.86602540378f, 0),
        new Vector3(-.5f, 0.86602540378f, 0),
        Vector3.left,
        new Vector3(-.5f, -0.86602540378f, 0),
        new Vector3(.5f, -0.86602540378f, 0) };



    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        nodeWidth = nodePrefab.transform.GetChild(0).GetComponent<SpriteRenderer>().bounds.size.x;
        nodeHeight = nodePrefab.transform.GetChild(0).GetComponent<SpriteRenderer>().bounds.size.y;
        
        Debug.Log("Node Height: " + nodeHeight);
        Debug.Log("Node Width: " + nodeWidth);

        AdjacencyList = new List<Node>();
        SpecialNodes = new List<Node>();
        NodesWithEdges = new List<Node>();
        path = new List<Node>();

        PathFound = false;

        if (LevelInfo.Edges != null)
        {
            Edges = LevelInfo.Edges;
        }
        else
        {
            Edges = new List<Edge>();
        }

        
    }


    public void Start()
    {
        grid = LevelInfo.map;
        width = LevelInfo.width;
        height = LevelInfo.height;
        PopulateGrid();
        SetCameraPositions();




    }

    
    public void SetCameraPositions()
    {
        float localWidth, localHeight;

        localWidth = (((width * 2) - 1) * (nodeWidth / 2)) / 2;
        localHeight = ((height - 1) * nodeHeight * .75f) / 2;

        cam.transform.position = new Vector3(localWidth, localHeight, -10);

    }

    private void Update()
    {        
        foreach (Node node in AdjacencyList)
        {
            node.CheckNodeBehavior();
        }

        if (IsCyclic())
        {
            Debug.Log("Graph contains cycle");
            LevelInfo.IsComplete = true;
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
                grid[i, j].Location = new Vector2Int(i, j);
                
                Node gridNode = grid[i, j];

                AddVertex(gridNode);
            }
        }
        SetSpecialNodes();
        //visited = new List<Node>(AdjacencyList);


        //grid[2, 2].type = NodeType.SharpTurn;

        foreach (Node node in AdjacencyList)
        {
            if (node.type != NodeType.None)
            {
                SpecialNodes.Add(node);
                node.UpdateColor();
            }
        }
         
        if(Edges.Count > 0)
        {
            foreach(Edge edge in Edges)
            {
                OnStartEdges(grid[edge.A.x, edge.A.y], grid[edge.B.x, edge.B.y]);
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

    #region Edge Methods

    /// <summary>
    /// Removes all of the Edge's on each Space
    /// </summary>
    public void RemoveAllEdges()
    {
        foreach (Node sp in AdjacencyList)
        {
            sp.Edges.Clear();
        }
        Edges.Clear();
        NodesWithEdges.Clear();
    }

    

    public Vector3 ClosestDirection(Vector3 v)
    {
        float maxDot = -Mathf.Infinity;
        Vector3 ret = Vector3.zero;

        foreach (Vector3 dir in compass)
        {
            float t = Vector3.Dot(v, dir);
            if (t > maxDot)
            {
                ret = dir;
                maxDot = t;
            }
        }

        return ret;
    }

    public void DrawLine(Node n1, Node n2)
    {
        Vector3 lineDirection = (n2.transform.position - n1.transform.position).normalized;

        lineDirection = ClosestDirection(lineDirection);
        GameObject edge;

        if (lineDirection == compass[0])
        {
            edge = n1.Lines[0];
            edge.SetActive(true);
            edge = n2.Lines[3];
            edge.SetActive(true);

        }
        else if (lineDirection == compass[1])
        {
            edge = n1.Lines[1];
            edge.SetActive(true);
            edge = n2.Lines[4];
            edge.SetActive(true);

        }
        else if (lineDirection == compass[2])
        {
            edge = n1.Lines[2];
            edge.SetActive(true);
            edge = n2.Lines[5];
            edge.SetActive(true);

        }
        else if (lineDirection == compass[3])
        {
            edge = n1.Lines[3];
            edge.SetActive(true);
            edge = n2.Lines[0];
            edge.SetActive(true);

        }
        else if (lineDirection == compass[4])
        {
            edge = n1.Lines[4];
            edge.SetActive(true);
            edge = n2.Lines[1];
            edge.SetActive(true);

        }
        else if (lineDirection == compass[5])
        {
            edge = n1.Lines[5];
            edge.SetActive(true);
            edge = n2.Lines[2];
            edge.SetActive(true);

        }
        
    }
    public void EraseLine(Node n1, Node n2)
    {
        Vector3 lineDirection = (n2.transform.position - n1.transform.position).normalized;

        lineDirection = ClosestDirection(lineDirection);
        GameObject edge;

        if (lineDirection == compass[0])
        {
            edge = n1.Lines[0];
            edge.SetActive(false);
            edge = n2.Lines[3];
            edge.SetActive(false);

        }
        else if (lineDirection == compass[1])
        {
            edge = n1.Lines[1];
            edge.SetActive(false);
            edge = n2.Lines[4];
            edge.SetActive(false);

        }
        else if (lineDirection == compass[2])
        {
            edge = n1.Lines[2];
            edge.SetActive(false);
            edge = n2.Lines[5];
            edge.SetActive(false);

        }
        else if (lineDirection == compass[3])
        {
            edge = n1.Lines[3];
            edge.SetActive(false);
            edge = n2.Lines[0];
            edge.SetActive(false);

        }
        else if (lineDirection == compass[4])
        {
            edge = n1.Lines[4];
            edge.SetActive(false);
            edge = n2.Lines[1];
            edge.SetActive(false);

        }
        else if (lineDirection == compass[5])
        {
            edge = n1.Lines[5];
            edge.SetActive(false);
            edge = n2.Lines[2];
            edge.SetActive(false);

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

        Edge NewEdge;
        NewEdge.A = v1.Location;
        NewEdge.B = v2.Location;
        Edges.Add(NewEdge);
        NodesWithEdges.Add(v1);
        NodesWithEdges.Add(v2);
        DrawLine(v1, v2);

        return true;
    }

    /// <summary>
    /// Adds a new edge between two given vertices in the graph
    /// </summary>
    /// <param name="v1">Name of the first vertex</param>
    /// <param name="v2">Name of the second vertex</param>
    /// <returns>Returns the success of the operation</returns>
    public bool OnStartEdges(Node v1, Node v2)
    {
        // Add vertex v2 to the edges of vertex v1
        AdjacencyList.Find(v => v == v1).Edges.Add(v2);

        // Add vertex v1 to the edges of vertex v2
        AdjacencyList.Find(v => v == v2).Edges.Add(v1);

        Edge NewEdge;
        NewEdge.A = v1.Location;
        NewEdge.B = v2.Location;
        NodesWithEdges.Add(v1);
        NodesWithEdges.Add(v2);
        DrawLine(v1, v2);

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

        NodesWithEdges.Remove(v1);
        NodesWithEdges.Remove(v2);
        RemoveLevelOBJEdges(v1.Location, v2.Location);
        EraseLine(v1, v2);

        return true;
    }

    public void RemoveLevelOBJEdges(Vector2Int v1, Vector2Int v2)
    {
        Edge temp = Edges.Find(N => N.A == v1 && N.B == v2);
        Edges.Remove(temp);
        temp = Edges.Find(N => N.A == v2 && N.B == v1);
        Edges.Remove(temp);

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

    #endregion


    // A recursive function that uses visited
    // and parent to detect cycle in subgraph
    // reachable from vertex v.
    bool IsCyclicUtil(Node v, List<Node> NWE, List<Node> path, Node parent)
    {
        // Mark the current node as visited
        NWE.Find(temp => temp == v).Visited = true;

        // Recur for all the vertices
        // adjacent to this vertex
        foreach (Node node in AdjacencyList.Find(temp => temp == v).Edges)
        {
            // If an adjacent is not visited,
            // then recur for that adjacent
            if (!NWE.Find(temp => temp == node).Visited)
            {
                if (IsCyclicUtil(node, NWE, path, v))
                {
                    path.Add(node);
                    return true;
                }
            }

            // If an adjacent is visited and
            // not parent of current vertex,
            // then there is a cycle.
            else if (node != parent)
            {
                path.Add(node);
                return true;
            }
        }
        if (path.Count > 0)
        {
            path.Clear();
        }
        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns> If there is a path that loops and has all of the special Nodes included </returns>
    private bool IsCyclic()
    {
        // Mark all the vertices as not visited
        // and not part of recursion stack
        int count = NodesWithEdges.Count;

        for (int i = 0; i < count; i++)
        {
            NodesWithEdges[i].Visited = false;
        }
        // Call the recursive helper function
        // to detect cycle in different DFS trees
        for (int u = 0; u < count; u++)
        {
            // Don't recur for u if already visited
            if (!NodesWithEdges[u].Visited)
            { 
                if (IsCyclicUtil(NodesWithEdges[u], NodesWithEdges, path, null))
                {
                    if (DoesLoopContainSpecialNodes() && AreNodesSatisfied())
                    {
                        PathFound = true;
                        return true;
                    }                    

                    return false;
                }   
                
            }
        }
        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"> Takes in the Loop from IsCyclic() </param>
    /// <returns> Checks if all of the Special Nodes are included in the loop </returns>
    public bool DoesLoopContainSpecialNodes()
    {
        foreach(Node node in SpecialNodes)
        {
            if(!path.Contains(node))
            {
                return false;
            }
        }
        return true;
    }

    public bool AreNodesSatisfied()
    {
        foreach (Node node in NodesWithEdges)
        {
            if (!path.Contains(node))
            {
                path.Clear();
                return false;

            }
        }

        foreach (Node node in path)
        {
            
            if (!node.IsSatisfied)
            {
                path.Clear();
                return false;
            }
        }
        
        return true;
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

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    foreach (Node node in path)
    //    {
    //        Gizmos.DrawSphere(node.transform.position, .2f);
    //    }
    //}





}
