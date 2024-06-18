using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum NodeType
{
    None,
    Straight,
    SharpTurn,
    WideTurn,
    Required
}


public class Node : MonoBehaviour
{
    public NodeType type;
    public List<Node> Edges { get; set; }

    public bool IsSatisfied { get; private set; }

    public bool IsError { get; private set; }

    public int x;
    public int y;



    private void OnDrawGizmos()
    {
        
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
                if (!CheckEdgesTurn(Edges[0], Edges[1]))
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
        if (((s1.x + 1 == s2.x - 1 || s2.x + 1 == s1.x - 1) && (s1.y == s2.y)) ||
            ((s1.y + 1 == s2.y - 1 || s2.y + 1 == s1.y - 1) && (s1.x == s2.x)))
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
    public bool CheckEdgesTurn(Node s1, Node s2)
    {
        if ((s1.x + 1 == s2.x && s1.y + 1 == s2.y) ||
            (s1.x - 1 == s2.x && s1.y - 1 == s2.y) ||
            (s1.x + 1 == s2.x && s1.y - 1 == s2.y) ||
            (s1.x - 1 == s2.x && s1.y + 1 == s2.y) ||
            (s1.x == s2.y && s1.y == s2.x))
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
