using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;
public enum NodeType
{
    None,
    Straight,
    SharpTurn,
    WideTurn,
    Required
}


public class Node : SerializedMonoBehaviour
{
    public NodeType type;

    [SerializeField]
    public List<Node> Edges { get; set; }

    public bool IsSatisfied { get; private set; }

    public bool IsError { get; private set; }

    public int x;
    public int y;


    private void Awake()
    {
        Edges = new List<Node>();
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if(Edges.Count > 0 )
        {
            foreach (var edge in Edges)
            {
                Gizmos.DrawLine(transform.position, edge.transform.position);

            }
        }
        
    }


    /// <summary>
    /// Checks to see if the player has correctly placed their lines 
    /// Mainly checks special nodes to see if their conditions are met
    /// </summary>
    public void CheckNodeBehavior()
    {
        if (Edges.Count == 2)
        {
            if (type == NodeType.SharpTurn)
            {
                if (!CheckEdgesSharpTurn(Edges[0], Edges[1]))
                {
                    ErrorDisplay();
                    IsSatisfied = false;
                }
                else
                {
                    IsSatisfied = true;

                }
            }
            else if (type == NodeType.Straight)
            {
                if (!CheckEdgesSraight(Edges[0], Edges[1]))
                {
                    ErrorDisplay();
                    IsSatisfied = false;
                }
                else
                {
                    IsSatisfied = true;

                }

            }
            else if(type == NodeType.WideTurn)
            {

            }
            else if (type == NodeType.None)
            {
                ErrorDisplay();

            }
        }
        else if (Edges.Count > 2)
        {
            ErrorDisplay();
        }
        else
        {
            //Icon.color = Color.white;

        }
    }

    /// <summary>
    /// Checks to see if the 2 edges attached to a node are parallell
    /// </summary>
    /// <param name="s1"></param>
    /// <param name="s2"></param>
    /// <returns></returns>
    public bool CheckEdgesSraight(Node s1, Node s2)
    {
        if (Vector3.Dot(s1.transform.position - transform.position, transform.position - s2.transform.position) == 1)
        {
            return true;
        }
        return false;

    }

    /// <summary>
    /// Checks to see if the 2 edges attached to a node are perpendicular
    /// </summary>
    /// <param name="s1"></param>
    /// <param name="s2"></param>
    /// <returns></returns>
    public bool CheckEdgesWideTurn(Node s1, Node s2)
    {
        if (Vector3.Dot(s1.transform.position - transform.position, transform.position - s2.transform.position) > 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Checks to see if the 2 edges attached to a node are perpendicular
    /// </summary>
    /// <param name="s1"></param>
    /// <param name="s2"></param>
    /// <returns></returns>
    public bool CheckEdgesSharpTurn(Node s1, Node s2)
    {
        if (Vector3.Dot(s1.transform.position - transform.position, transform.position - s2.transform.position) < 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Placeholder for showing when a node is not used correctly
    /// </summary>
    public void ErrorDisplay()
    {
        UnityEngine.Debug.Log("Error with space X: " + x + " Y: " + y);
        //Icon.color = Color.red;
    }


}
