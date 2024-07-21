using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectButton : MonoBehaviour
{
    public int levelNumber;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonPress()
    {
        LevelTransferScript.Instance.LevelNum = levelNumber;
        SceneManager.LoadScene("LevelScene");

    }
}
