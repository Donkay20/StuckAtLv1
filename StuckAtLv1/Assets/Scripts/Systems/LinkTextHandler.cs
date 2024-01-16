using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LinkTextHandler : MonoBehaviour, IPointerClickHandler
{
    private TMP_Text linkedText;
    [SerializeField] private Canvas canvas;
    private Camera cameraToUse;
    [SerializeField] private KeywordDetail keywordDetail;



    private void Awake() {
        linkedText = GetComponent<TMP_Text>();
        //canvas = GetComponentInParent<Canvas>();
        cameraToUse = null; //because we're using screen space overlay
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector3 mousePosition = new Vector3(eventData.position.x, eventData.position.y, z:0);
        Debug.Log(mousePosition);
        var linkTaggedText = TMP_TextUtilities.FindNearestLink(linkedText, mousePosition, cameraToUse);
        Debug.Log(linkTaggedText);
        if (linkTaggedText != -1) {
            TMP_LinkInfo linkInfo = linkedText.textInfo.linkInfo[linkTaggedText];
            keywordDetail.Enable(linkInfo.GetLinkID());
            Debug.Log(linkInfo.GetLinkID());
            Debug.Log(linkInfo.GetLinkText());
        }
        //throw new System.NotImplementedException();
    }
}
