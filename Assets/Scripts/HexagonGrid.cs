using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEngine.Tilemaps;

public class HexagonGrid : MonoBehaviour
{
    public const int width = 6;  // Width should always be 1 more than the Height
    public const int height = 6;

    public Node nodePrefab;

    private float nodeWidth;
    private float nodeHeight;

    public Node[,] grid = new Node[width, height];

    public List<Node> AdjacencyList { get; set; }


    public void Start()
    {
        AdjacencyList = new List<Node>();
        nodeWidth = nodePrefab.GetComponentInChildren<SpriteRenderer>().bounds.size.x;
        nodeHeight = nodePrefab.GetComponentInChildren<SpriteRenderer>().bounds.size.y;
        //nodeWidth = transform.localScale.x;
        //nodeHeight = transform.localScale.y;

        Debug.Log("Node Height: " + nodeHeight);
        Debug.Log("Node Width: " + nodeWidth);

        PopulateGrid();
        

       

    }  

    private void Update()
    {
        foreach (Node node in AdjacencyList)
        {
            node.CheckNodeBehavior();
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

        grid[2, 2].type = NodeType.SharpTurn;
        grid[2, 2].GetComponentInChildren<SpriteRenderer>().color = Color.red;
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




}
