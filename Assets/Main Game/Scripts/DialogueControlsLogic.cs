using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class DialogueControlsLogic : MonoBehaviour
{
    [System.Serializable]
    public struct OptionGroup
    {
        public Button button;
        public TMP_Text text;
    }

    public float ControlAnimTime = 0.25f;
    public float ContinueSize = 100f;
    public float OptionButtonSize = 100f;

    [Space]

    public Button ContinueButton;
    public CanvasGroup ContinueCanvas;

    [Space]

    public CanvasGroup OptionsGroup;
    public OptionGroup[] OptionButtons;

    private GameManager manager;
    private RectTransform rTransform;
    private CanvasGroup canvas;
    
    private void Awake()
    {
        manager = GetComponentInParent<GameManager>();
        rTransform = GetComponent<RectTransform>();
        canvas = GetComponent<CanvasGroup>();

        ContinueButton.onClick.AddListener(manager.ContinueDialogue);
        ContinueButton.interactable = false;

        for (int i = 0; i < OptionButtons.Length; i++)
        {
            int id = i;

            OptionButtons[i].button.onClick.AddListener(() => {
                ChooseOption(id);
            });
        }

        CloseOptions();
    }

    public void DisplayOptions(string[] choices)
    {
        Vector3 prevSize = rTransform.sizeDelta;
        Vector2 newSize = new Vector3(prevSize.x, OptionButtonSize * choices.Length, prevSize.z);

        OptionsGroup.alpha = 0f;
        OptionsGroup.gameObject.SetActive(true);

        for (int i = 0; i < choices.Length; i++)
        {
            OptionButtons[i].text.SetText(choices[i]);
        }

        Sequence DisplayOptionsSequence = DOTween.Sequence();
        DisplayOptionsSequence
            .AppendInterval(ControlAnimTime)
            .Append(rTransform.DOSizeDelta(newSize, ControlAnimTime))
            .Append(OptionsGroup.DOFade(1f, ControlAnimTime))
            .AppendCallback(() => {
                for (int i = 0; i < choices.Length; i++)
                {
                    OptionButtons[i].button.interactable = true;
                }
            });
    }

    public void ChooseOption(int id)
    {
        CloseOptions();

        Vector3 prevSize = rTransform.sizeDelta;
        Vector2 newSize = new Vector3(prevSize.x, ContinueSize, prevSize.z);

        Sequence CloseOptionSequence = DOTween.Sequence();
        CloseOptionSequence
            .AppendCallback(() => {
                for (int i = 0; i < OptionButtons.Length; i++)
                {
                    OptionButtons[i].button.interactable = false;
                }
            })
            .Append(OptionsGroup.DOFade(0f, ControlAnimTime))
            .Append(rTransform.DOSizeDelta(newSize, ControlAnimTime))
            .AppendCallback(() => {
                CloseOptions();
                manager.SelectOption(id);
            });
    }

    public void CloseOptions()
    {
        OptionsGroup.alpha = 0;
        OptionsGroup.gameObject.SetActive(false);

        for (int i = 0; i < OptionButtons.Length; i++)
        {
            OptionButtons[i].button.interactable = false;
            OptionButtons[i].text.SetText("");
        }
    }

    public void DisplayContinue() 
    {
        ContinueCanvas.DOFade(1f, ControlAnimTime)
            .OnComplete(() => {
                ContinueButton.interactable = true;
            });
    }

    public void CloseContinue() 
    {
        ContinueButton.interactable = false;
        ContinueCanvas.DOFade(0f, ControlAnimTime);
    }
}
