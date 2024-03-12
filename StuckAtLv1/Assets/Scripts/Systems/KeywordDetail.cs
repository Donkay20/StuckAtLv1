using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeywordDetail : MonoBehaviour
{
    [SerializeField] private Button end;
    [SerializeField] private TextMeshProUGUI keywordTitle, keywordDetail;
    [Space]
    [SerializeField] private string[] titlePool = new string[2];
    [TextArea(10,10)]
    [SerializeField] private string[] detailPool = new string[2]; 
    private void Awake() {
        InitializeButton();
    }

    private void ClickOut() {
        gameObject.SetActive(false);
    }

    public void Enable(string keyword) {
        
        gameObject.SetActive(true);
        keywordTitle.text = "";
        keywordDetail.text = "";
        Debug.Log("Tooltip: " + keyword);

        switch(keyword) {
            case "overheal":
                keywordTitle.text = titlePool[0];
                keywordDetail.text = detailPool[0];
                break;
            case "anemia":
                keywordTitle.text = titlePool[1];
                keywordDetail.text = detailPool[1];
                break;
            case "afterimage":
                keywordTitle.text = titlePool[2];
                keywordDetail.text = detailPool[2];
                break;
            case "anemicshock":
                keywordTitle.text = titlePool[3];
                keywordDetail.text = detailPool[3];
                break;
            case "bloodsucker":
                keywordTitle.text = titlePool[4];
                keywordDetail.text = detailPool[4];
                break;
            case "bulwark":
                keywordTitle.text = titlePool[5];
                keywordDetail.text = detailPool[5];
                break;
            case "penetration":
                keywordTitle.text = titlePool[6];
                keywordDetail.text = detailPool[6];
                break;
            case "avarice":
                keywordTitle.text = titlePool[7];
                keywordDetail.text = detailPool[7];
                break;
        }
    }

    private void InitializeButton() {
        end.onClick.AddListener(() => ClickOut());
    }
}
