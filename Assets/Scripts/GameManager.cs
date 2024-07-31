using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using Unity.Mathematics;

public class GameManager : MonoBehaviour
{
    public HexagonGrid grid;
    public List<LevelBaseObject> levels;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(Instance);
        }
        DontDestroyOnLoad(Instance);
    }

    private void OnLevelWasLoaded(int level)
    {
        grid = GameObject.FindObjectOfType<HexagonGrid>();
        if(grid != null)
        {
            grid.LevelInfo = levels[LevelTransferScript.Instance.LevelNum - 1];
        }
    }
    
}
