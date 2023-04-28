using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathMenu : Singleton<DeathMenu>
{
    public void OnContinueClicked()
    {
        SceneManager.LoadScene(DataPersistenceManager.instance.gameData.areaInMap);
        Time.timeScale = 1f;
    }
}
