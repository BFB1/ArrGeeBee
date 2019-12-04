using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenHelper : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI scoreTextField;
    
    private void Awake()
    {
        scoreTextField.text = $"You survived for {Score.Seconds} seconds and shot {Score.Kills} blobs";
        Score.Kills = Score.Seconds = 0;
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
