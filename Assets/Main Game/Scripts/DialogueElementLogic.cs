using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class DialogueElementLogic : MonoBehaviour
{
    public float startAnimTime = 0.25f;
    public Sprite ControlSprite;

    public TMP_Text DialogueText;
    public CanvasGroup canvas;

    private RectTransform rTransform;
    private Image DisplayImage;
    private GameManager manager;

    private void Awake()
    {
        manager = GetComponentInParent<GameManager>();
        rTransform = GetComponent<RectTransform>();
        DisplayImage = GetComponent<Image>();
        canvas = GetComponent<CanvasGroup>();
        canvas.alpha = 0f;
    }

    public void SetupElement(string speaker, string text, bool isControl = false)
    {
        if (isControl)
        {
            DisplayImage.sprite = ControlSprite;
            speaker = "";
        }

        string output;

        if (speaker == "" || speaker == null)
            output = text;
        else
            output = string.Format("<font=\"OpenSans-ExtraBold SDF\">{0}</font>\n{1}", speaker, text);

        canvas.alpha = 1f;
        DialogueText.SetText(output);
        DialogueText.alpha = 0f;

        Vector3 newSize = rTransform.sizeDelta;
        rTransform.sizeDelta = new Vector3(newSize.x, 0f, newSize.z);

        Sequence StartSequence = DOTween.Sequence();
        StartSequence
            .Append(rTransform.DOSizeDelta(newSize, startAnimTime))
            .Append(DialogueText.DOFade(1f, startAnimTime))
            .OnComplete(() => {
                manager.FinishDisplayingDialogue();
            });
    }

    public void TakeDownElement() 
    {
        Destroy(this.gameObject);
    }
}
