using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    [SerializeField] private Button titleButton;
    void Start() {
        InitializeButtons();
    }

    private void InitializeButtons() {
        titleButton.onClick.AddListener(() => TitleScreen());
    }

    private void TitleScreen() {
        SceneManager.LoadScene("TitleScreen");
    }
}
