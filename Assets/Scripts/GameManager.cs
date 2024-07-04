using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public TouchManager touchManager;
    public HexagonGrid grid;
    public List<LevelBaseObject> levels;

    private void Awake()
    {
        grid.LevelInfo = levels[0];
    }

    private void OnEnable()
    {
        TouchManager.addEdge += grid.AddAnEdge;
    }

    private void OnDisable()
    {
        TouchManager.addEdge -= grid.AddAnEdge;

    }
}
