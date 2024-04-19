using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] private Button title, retry;

    private void Start() {
        InitializeButtons();
        Time.timeScale = 0;
    }

    private void ClickedButton(int option) {
        switch(option) {
            case 0:
                Time.timeScale = 1;
                SceneManager.LoadScene("MainGame");
                break;
            case 1:
                Time.timeScale = 1;
                SceneManager.LoadScene("TitleScreen");
                break;
        }
    }

    private void InitializeButtons() {
        title.onClick.AddListener(() => ClickedButton(1));
        retry.onClick.AddListener(() => ClickedButton(0));
    }
}