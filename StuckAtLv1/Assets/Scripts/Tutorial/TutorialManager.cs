using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{   
    [TextArea(3,3)]
    [SerializeField] private string[] tutorialText;
    [SerializeField] private Sprite[] tutorialImage;
    [SerializeField] private TextMeshProUGUI tutorialUIText;
    [SerializeField] private Image tutorialUIImage;
    [SerializeField] private Button yesButton, noButton;
    [SerializeField] private GameObject tutorialQuestion;
    [Space]
    [SerializeField] private GameObject player;
    [SerializeField] private Enemy tutorialEnemy1;
    [SerializeField] private Enemy tutorialEnemy2;
    [SerializeField] private GameObject blockingWall;
    private int triggerCount;
    void Start() {
        InitializeButtons();
        
        tutorialUIText.text = tutorialText[0];
        tutorialUIImage.sprite = tutorialImage[0];

        tutorialEnemy1.SetTarget(player); tutorialEnemy2.SetTarget(player);
    }

    void Update() {
        if ((tutorialEnemy1.GetHealth() < tutorialEnemy1.maxHP) || (tutorialEnemy2.GetHealth() < tutorialEnemy2.maxHP)){
            if (triggerCount == 3) {
                NextMessage(4);
            }
        }

        if (Input.GetMouseButtonDown(1)) {
            if (triggerCount == 4) {
                NextMessage(5);
            }
        }

        if (!tutorialEnemy1.isActiveAndEnabled && !tutorialEnemy2.isActiveAndEnabled) {
            Destroy(blockingWall);
            NextMessage(6);
        }
    }

    private void BeginTutorial() {
        tutorialQuestion.SetActive(false);
    }

    public void NextMessage(int trigger) {
        if (trigger == 7) {
            ExitTutorial();
        } else {
            triggerCount = trigger;
            tutorialUIText.text = tutorialText[trigger];
            tutorialUIImage.sprite = tutorialImage[trigger];

            if (trigger == 3) {
                tutorialEnemy1.SetSpeed(1); tutorialEnemy2.SetSpeed(1);
            }
        }
    }

    private void InitializeButtons() {
        yesButton.onClick.AddListener(() => BeginTutorial());
        noButton.onClick.AddListener(() => ExitTutorial());
    }

    public void ExitTutorial() {
        SceneManager.LoadScene("RuinsIntro");
    }
}
