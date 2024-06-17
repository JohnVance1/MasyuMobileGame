using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEngine.Tilemaps;

public class HexagonGrid : MonoBehaviour
{
    public const int width = 6;  // Width should always be 1 more than the Height
    public const int height = 5;
    List<List<Node>> hexagons = new List<List<Node>>();

    public Tilemap tileMap;
    public Tile defaultTile;

    public List<Vector3> availablePlaces;

     

    public void Start()
    {
        if(tileMap == null)
        {
            Debug.LogWarning("No Tilemap is assigned to: " + gameObject.name);
            return;
        }

        availablePlaces = new List<Vector3>();

        tileMap.SetTile(new Vector3Int(-2, -4, 0), defaultTile);

        #region Take All Tiles On Map and Convert them into a Vector3 List
        //for (int n = tileMap.cellBounds.xMin; n < tileMap.cellBounds.xMax; n++)
        //{
        //    for (int p = tileMap.cellBounds.yMin; p < tileMap.cellBounds.yMax; p++)
        //    {
        //        Vector3Int localPlace = new Vector3Int(n, p, (int)tileMap.transform.position.y);
        //        Vector3 place = tileMap.CellToWorld(localPlace);
        //        if (tileMap.HasTile(localPlace))
        //        {
        //            //Tile at "place"
        //            availablePlaces.Add(place);
        //        }
        //        else
        //        {
        //            //No tile at "place"
        //        }
        //    }
        //}
        #endregion

    }

    public Node[,] grid = new Node[width, height];

    public List<Node> AdjacencyList { get; set; }


    /// <summary>
    /// Sets up the grid verticies and the adjacency matrix
    /// </summary>
    public void PopulateGrid()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                grid[i, j].x = i;
                grid[i, j].y = j;
                AddVertex(grid[i, j]);
            }
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




}
