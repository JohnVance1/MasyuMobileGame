using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TouchManager touchManager;
    public HexagonGrid grid;
    public List<LevelBaseObject> levels;

    private void Awake()
    {
        grid.LevelInfo = levels[LevelTransferScript.Instance.LevelNum - 1];
    }

    

    private void OnEnable()
    {
        TouchManager.addEdge += grid.AddAnEdge;
        TouchManager.removeEdge += grid.RemoveAnEdge;

    }

    private void OnDisable()
    {
        TouchManager.addEdge -= grid.AddAnEdge;
        TouchManager.removeEdge -= grid.RemoveAnEdge;

    }
}
