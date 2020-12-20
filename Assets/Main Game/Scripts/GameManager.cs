using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using KXI;

public class GameManager : MonoBehaviour
{
    public DialogueElementLogic DialogueElementPrefab;
    public bool isAuto = false;
    public bool tempAuto = false;
    public List<DialogueElementLogic> SpawnedElements;

    [Space]

    public Button ScrollDownButton;
    public Button AutoButton;
    public ScrollRect Scrollview;
    public CanvasGroup SVCanvasGroup;
    public CanvasGroup TitleCanvasGroup;
    public GameObject ExtraControlsArea;
    public GameObject ControlsGroup;
    public GameObject StoryRoot;

    [Space]

    public DialogueControlsLogic DialogueControls;
    public ScarfManager DialogueManager;
    public ParticleManager Particles;
    public NoiseMaker MainSFX;

    private void OnEnable()
    {
        // subscribing to story events
        DialogueManager.OnStoryStart.AddListener(StartDialogue);
        DialogueManager.OnDisplayStory.AddListener(DisplayDialogue);
        DialogueManager.OnDisplayOption.AddListener(DisplayOptions);
        DialogueManager.OnStoryEnd.AddListener(EndDialogue);
    }

    private void OnDisable()
    {
        // unsubbing to story events
        DialogueManager.OnStoryStart.RemoveListener(StartDialogue);
        DialogueManager.OnDisplayStory.RemoveListener(DisplayDialogue);
        DialogueManager.OnDisplayOption.RemoveListener(DisplayOptions);
        DialogueManager.OnStoryEnd.RemoveListener(EndDialogue);
    }

    private void Awake()
    {
        SVCanvasGroup.alpha = 0f;
        SVCanvasGroup.gameObject.SetActive(false);
        TitleCanvasGroup.gameObject.SetActive(true);
        TitleCanvasGroup.alpha = 1f;
        ExtraControlsArea.SetActive(false);
    }

    // quicky just adding story functions
    private void Start()
    {
        DialogueManager.dialogueRunner.AddCommandHandler("PulseSim", () => {
            Particles.SpeedAndSlow();
        });

        DialogueManager.dialogueRunner.AddCommandHandler("SlowSim", () => {
            Particles.SlowDown();
        });

        DialogueManager.dialogueRunner.AddCommandHandler("SpeedSim", () => {
            Particles.SpeedUp();
        });
    }

    #region StoryControlFunctions

    public void ContinueDialogue()
    {
        DialogueControls.CloseContinue();
        DialogueManager.ProgressStory();
    }

    public void SelectOption(int optionID)
    {
        DialogueManager.SelectOption(optionID);
    }

    public void FinishDisplayingDialogue()
    {
        DialogueManager.FinishDisplayStory();

        if (tempAuto)
        {
            ContinueDialogue();
            tempAuto = false;
            return;
        }

        if (isAuto)
            ContinueDialogue();
        else
            DialogueControls.DisplayContinue();
    }

    #endregion StoryControlFunctions

    #region StoryEventFunctions

    private void StartDialogue()
    {
        Particles.SlowDown();
    }

    private void DisplayDialogue(DialogueInfo info)
    {
        DialogueElementLogic element = Instantiate(DialogueElementPrefab, StoryRoot.transform);
        element.SetupElement(info.Speaker, info.Dialogue, info.isDirection);

        ControlsGroup.transform.SetAsLastSibling();

        SpawnedElements.Add(element);
    }

    private void DisplayOptions(OptionInfo[] info)
    {
        string[] options = new string[info.Length];

        foreach (OptionInfo item in info)
        {
            options[item.id] = item.text;
        }

        DialogueControls.DisplayOptions(options);
    }

    private void EndDialogue()
    {
        ResetSequence();
    }

    #endregion StoryEventFunctions

    public void OnHitPlay() 
    {
        Sequence TitleSequence = DOTween.Sequence();
        TitleSequence
            .AppendCallback(() => {
                Particles.SpeedUp();
            })
            .Append(TitleCanvasGroup.DOFade(0f, 1f))
            .AppendInterval(0.5f)
            .AppendCallback(() => {
                TitleCanvasGroup.gameObject.SetActive(false);
                SVCanvasGroup.gameObject.SetActive(true);
                ExtraControlsArea.SetActive(true);
            })
            .Append(SVCanvasGroup.DOFade(1f, 1f))
            .AppendCallback(() => {
                DialogueManager.StartStory();
            });
    }

    public void ToggleAuto() 
    {
        isAuto = !isAuto;
    }

    public void ScrollToBottom() 
    {
        Scrollview.DOVerticalNormalizedPos(0f, 0.25f);
    }

    private void ResetSequence() 
    {
        Sequence ResetSeq = DOTween.Sequence();
        ResetSeq
            .AppendCallback(() => {
                MainSFX.PlayNoise(1);
                Particles.SpeedUp();
            })
            .Append(SVCanvasGroup.DOFade(0f, 1f))
            .AppendCallback(() => {
                ClearSpawnedElements();
            })
            .AppendInterval(1f)
            .Append(SVCanvasGroup.DOFade(1f, 1f))
            .AppendCallback(() => {
                DialogueManager.StartStory();
            });
    }

    private void ClearSpawnedElements()
    {
        if (SpawnedElements.Count == 0)
            return;

        foreach (var item in SpawnedElements)
        {
            item.TakeDownElement();
        }

        SpawnedElements.Clear();
    }
}
