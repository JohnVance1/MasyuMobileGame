using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public GameObject KeyPanel;
    public GameObject LevelName;
    public GameObject LevelCompletePanel;

    public HexagonGrid hexGrid;

    public void BackButton()
    {
        GameManager.Instance.SavesEdges();
        LevelTransferScript.Instance.currentLevel = GameManager.Instance.grid.LevelInfo;
        SceneManager.LoadScene("LevelSelectScene");
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

    private void Start()
    {
        LevelName.GetComponent<TextMeshProUGUI>().text = "Level " + LevelTransferScript.Instance.LevelNum;
    }

    private void Update()
    {
        if(hexGrid.PathFound)
        {
            StartCoroutine(GotoLevelSelect());
        }
    }

    public IEnumerator GotoLevelSelect()
    {

        yield return new WaitForSeconds(0.5f);
        LevelCompletePanel.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        BackButton();

    }



}
