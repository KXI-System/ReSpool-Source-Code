using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yarn;
using Yarn.Unity;

namespace KXI
{
    public class ScarfManager : MonoBehaviour
    {
        public class GenericStoryEvent : UnityEvent { }
        public GenericStoryEvent OnStoryStart = new GenericStoryEvent();
        public GenericStoryEvent OnStoryEnd = new GenericStoryEvent();
        public GenericStoryEvent OnProgressStory = new GenericStoryEvent();
        public GenericStoryEvent OnOptionSelect = new GenericStoryEvent();
        public GenericStoryEvent OnQuitStory = new GenericStoryEvent();

        public class DisplayStoryEvent : UnityEvent<DialogueInfo> { }
        public DisplayStoryEvent OnDisplayStory = new DisplayStoryEvent();

        public class DisplayOptionEvent : UnityEvent<OptionInfo[]> { }
        public DisplayOptionEvent OnDisplayOption = new DisplayOptionEvent();

        private GameManager gManager;
        public DialogueRunner dialogueRunner;
        private ScarfDialogueUI dialogueUI;
        private ScarfVariableStorage variableStorage;

        private void Awake()
        {
            gManager = GetComponentInParent<GameManager>();
            gManager.DialogueManager = this;

            dialogueRunner = GetComponent<DialogueRunner>();
            dialogueUI = GetComponent<ScarfDialogueUI>();
            variableStorage = GetComponent<ScarfVariableStorage>();
        }

        public void StartStory() 
        {
            dialogueRunner.StartDialogue();
            OnStoryStart?.Invoke();
        }

        public void StartStory(string nodeName) 
        {
            dialogueRunner.StartDialogue(nodeName);
            OnStoryStart?.Invoke();
        }

        public void DisplayStory(DialogueInfo info) 
        {
            OnDisplayStory?.Invoke(info);
        }

        public void FinishDisplayStory() 
        {
            dialogueUI.FinishLine();
        }

        public void ProgressStory() 
        {
            dialogueUI.MarkLineComplete();
            OnProgressStory?.Invoke();
        }

        public void EndStory() 
        {
            OnStoryEnd?.Invoke();
        }

        public void DisplayOptions(OptionInfo[] options) 
        {
            OnDisplayOption?.Invoke(options);
        }

        public void SelectOption(int optionID)
        {
            dialogueUI.SelectOption(optionID);
            OnOptionSelect?.Invoke();
        }

        public void QuitStory() 
        {
            OnQuitStory?.Invoke();
        }
    }
}
