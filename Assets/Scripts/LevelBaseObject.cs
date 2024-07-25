using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LevelBaseObject : ScriptableObject
{
    public int width;
    public int height;
    public Node[,] map;
    public List<Vector2Int> straightNodeLocations;
    public List<Vector2Int> sharpTurnNodeLocations;
    public List<Vector2Int> wideTurnNodeLocations;

    public int levelID;

    public bool IsComplete;

    public void OnEnable()
    {
        map = new Node[width, height];

    }

 

}
