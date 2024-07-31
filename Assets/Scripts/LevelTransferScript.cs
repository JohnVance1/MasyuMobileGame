using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransferScript : MonoBehaviour
{
    public int LevelNum;

    public LevelBaseObject currentLevel;

    public static LevelTransferScript Instance
    {
        get; private set;
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(Instance);
        }
        DontDestroyOnLoad(Instance);
        
    }



}
