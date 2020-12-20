using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn;
using Yarn.Unity;

namespace KXI 
{
    public class ScarfDialogueUI : DialogueViewBase
    {
        private bool isRunningLine;
        private bool isRunningOption;
        private Action dialogueLineFinishAction;
        private Action<int> optionSelectedAction;
        private ScarfManager manager;

        private void Awake()
        {
            manager = GetComponent<ScarfManager>();
        }

        public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
            DialogueInfo info = new DialogueInfo
            {
                Speaker = dialogueLine.CharacterName,
                Dialogue = dialogueLine.TextWithoutCharacterName.Text,
                RawText = dialogueLine.Text.Text,
                isDirection = dialogueLine.CharacterName == "SYS"
            };

            isRunningLine = true;
            dialogueLineFinishAction = onDialogueLineFinished;

            manager.DisplayStory(info);
        }

        public override void RunOptions(DialogueOption[] dialogueOptions, Action<int> onOptionSelected)
        {
            OptionInfo[] options = new OptionInfo[dialogueOptions.Length];

            for (int i = 0; i < dialogueOptions.Length; i++)
            {
                options[i].id = dialogueOptions[i].DialogueOptionID;
                options[i].text = dialogueOptions[i].Line.Text.Text;
            }

            isRunningOption = true;
            optionSelectedAction = onOptionSelected;

            manager.DisplayOptions(options);
        }

        public override void DismissLine(Action onDismissalComplete)
        {
            onDismissalComplete();
        }

        public override void OnLineStatusChanged(LocalizedLine dialogueLine)
        {
            // whee
        }

        public override void DialogueComplete()
        {
            manager.EndStory();
        }

        public void FinishLine()
        {
            if (!isRunningLine)
            {
                Debug.LogError("Finish Line has been requested when no line is running");
                return;
            }

            isRunningLine = false;
            dialogueLineFinishAction?.Invoke();
            //MarkLineComplete();
        }

        public void SelectOption(int optionID)
        {
            if (!isRunningOption)
            {
                Debug.LogError("Select Option has fired when no options are available: #" + optionID);
                return;
            }

            isRunningOption = false;
            optionSelectedAction?.Invoke(optionID);
        }
    }
}
