using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LinkTextHandler : MonoBehaviour, IPointerClickHandler
{
    private TMP_Text linkedText;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Camera cameraToUse;
    [SerializeField] private KeywordDetail keywordDetail;

    private void Awake() {
        linkedText = GetComponent<TMP_Text>();
    }

    private void OnEnable() {
        linkedText = GetComponent<TMP_Text>();
        linkedText.ForceMeshUpdate();
    }

    public void OnPointerClick(PointerEventData eventData) {
        Vector3 mousePosition = new Vector3(eventData.position.x, eventData.position.y, z:0);
        var linkTaggedText = TMP_TextUtilities.FindIntersectingLink(linkedText, mousePosition, cameraToUse);
        if (linkTaggedText != -1) {
            Debug.Log(linkTaggedText);
            TMP_LinkInfo linkInfo = linkedText.textInfo.linkInfo[linkTaggedText];
            keywordDetail.Enable(linkInfo.GetLinkID());
        }
    }
}
