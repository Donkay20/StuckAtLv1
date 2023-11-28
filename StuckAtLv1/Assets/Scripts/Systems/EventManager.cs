using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    //temporary, todo
    [SerializeField] private Button confirm;
    [SerializeField] private GameManager manager;
    void Awake()
    {
        InitializeButtons();
    }

    private void Exit() {
        manager.ReceiveCommand("map");
    }

    private void InitializeButtons() {
        confirm.onClick.AddListener(() => {Exit();});
    }
}
