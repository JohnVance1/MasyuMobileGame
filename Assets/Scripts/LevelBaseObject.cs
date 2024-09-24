using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu]
public class LevelBaseObject : SerializedScriptableObject
{
    public int width;
    public int height;
    public Node[,] map;
    public List<Edge> Edges;
    public List<Vector2Int> straightNodeLocations;
    public List<Vector2Int> sharpTurnNodeLocations;
    public List<Vector2Int> wideTurnNodeLocations;

    public int levelID;

    public bool IsComplete;

    private void Awake()
    {
        Edges = new List<Edge>();
        map = new Node[width, height];
    }
    public void OnEnable()
    {
        //map = new Node[width, height];
    }

 

}
