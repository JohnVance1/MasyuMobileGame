using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
    public GameObject KeyPanel;

    public void BackButton()
    {
        LevelTransferScript.Instance.currentLevel = GameManager.Instance.grid.LevelInfo;
        SceneManager.LoadScene(0);
    }

    public void OpenKeyPanel()
    {
        if(KeyPanel.activeSelf)
        {
            KeyPanel.SetActive(false);
        }
        else
        {
            KeyPanel.SetActive(true);
        }
    }



}
