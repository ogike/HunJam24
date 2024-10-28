// All of the code in this folder is copied/derived from Github user shapedbyrainstudios
// Repo: https://github.com/shapedbyrainstudios/ink-dialogue-system

using System;
using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using Player;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance { get; private set; }

        [Header("Params")]
        public float typingSpeed = 0.04f;

        [Header("Load Globals JSON")]
        [SerializeField] private TextAsset loadGlobalsJSON;

        [Header("NPC Dialogue UI")]
        [SerializeField] private GameObject npcDialoguePanel;
        [SerializeField] private GameObject npcDialogueContinueIcon;
        [SerializeField] private TextMeshProUGUI npcDialogueText;
        private Transform npcDialoguePosition; //passed from DialogueTrigger
        private Transform npcDialoguePanelTransform;
        private Vector3 playerTalkingPos;
        [SerializeField] private float playerGoToTalkingPosSpeed;
        private Vector3 playerTalkingPosVelocity;
        private DialogueTrigger npcTriggerer;

            [Header("Player Dialogue UI")]
        [SerializeField] private GameObject playerDialoguePanel;
        [SerializeField] private GameObject playerDialogueChoicesIcon;
        [SerializeField] private TextMeshProUGUI playerDialogueText;
        [SerializeField] private Transform playerDialoguePosition;
        private Transform playerDialoguePanelTransform;


        private Story currentStory;
        public bool dialogueIsPlaying { get; private set; }

        private bool _canContinueToNextLine = false;
        private int _currentChoiceIndex = 0;
        private bool npcTalking;
        private bool _switchedChoiceAlready;

        private Coroutine displayLineCoroutine;

        private const string PLAYER_STRING_TAG = "player";
        private const string MUSHROOM_SPEAKING_TAG = "mushroom";

        private DialogueVariables dialogueVariables;

        private const float _floatingPointTolerance = 0.01f;

        private void Awake() 
        {
            if (Instance != null)
            {
                Debug.LogWarning("Found more than one Dialogue Manager in the scene");
            }
            Instance = this;

            dialogueVariables = new DialogueVariables(loadGlobalsJSON);
            npcDialoguePanelTransform = npcDialoguePanel.transform;
            playerDialoguePanelTransform = playerDialoguePanel.transform;
        }

        private void Start() 
        {
            dialogueIsPlaying = false;
            npcDialoguePanel.SetActive(false);
            playerDialoguePanel.SetActive(false);
            npcDialogueContinueIcon.SetActive(false);
        }

        private void Update() 
        {
            // return right away if dialogue isn't playing
            if (!dialogueIsPlaying) 
            {
                return;
            }
            

            // handle continuing to the next line in the dialogue when submit is pressed
            if (_canContinueToNextLine
                && UserInput.Instance.InteractButtonPressedThisFrame)
            {
                if(currentStory.currentChoices.Count == 0)
                    ContinueStory();
                else
                    MakeChoice();
            }

            if (currentStory.currentChoices.Count > 0)
            {
                float xInput = UserInput.Instance.MoveInput.x;
                if (_switchedChoiceAlready && xInput == 0)
                {
                    _switchedChoiceAlready = false;
                }
                
                if (!_switchedChoiceAlready)
                {
                    if (Math.Abs(xInput - 1) < _floatingPointTolerance)
                    {
                        NextChoice();
                        _switchedChoiceAlready = true;
                    }
                    else if (Math.Abs(xInput - (-1)) < _floatingPointTolerance)
                    {
                        PreviousChoice();
                        _switchedChoiceAlready = true;
                    }
                }
            }
        }

        public void EnterDialogueMode(TextAsset inkJSON, Transform npcDialoguePosition, 
            Vector3 playerTalkingPosition, DialogueTrigger triggerer) 
        {
            currentStory = new Story(inkJSON.text);
            dialogueIsPlaying = true;
            // npcDialoguePanel.SetActive(true);
            // playerDialoguePanel.SetActive(true);
            
            dialogueVariables.StartListening(currentStory);

            npcTriggerer = triggerer;
            
            this.npcDialoguePosition = npcDialoguePosition;
            npcDialoguePanelTransform.position = npcDialoguePosition.position;
            playerTalkingPos = playerTalkingPosition;

            StartCoroutine(StartStory());
        }
        
        /// <summary>
        /// Player go to position, zoom, starts story once done
        /// </summary>
        private IEnumerator StartStory()
        {
            CameraFollow.Instance.SetZoomDialogue();
            
            Transform playerTrans = PlayerController.Instance.transform;
            playerTalkingPos.z = playerTrans.position.z; //Dont affect Z axis
            float curTime = 0;

            while (curTime < playerGoToTalkingPosSpeed + 0.1f)
            {
                playerTrans.position = Vector3.SmoothDamp(playerTrans.position, playerTalkingPos, ref playerTalkingPosVelocity, playerGoToTalkingPosSpeed);
                playerDialoguePanelTransform.position = playerDialoguePosition.position;
                curTime += Time.deltaTime; 
                yield return new WaitForSeconds(0);
            }
            
            ContinueStory();
        }

        private IEnumerator ExitDialogueMode() 
        {
            yield return new WaitForSeconds(0.2f);

            dialogueVariables.StopListening(currentStory);

            dialogueIsPlaying = false;
            npcDialoguePanel.SetActive(false);
            npcDialogueText.text = "";
            playerDialoguePanel.SetActive(false);
            playerDialogueText.text = "";
            
            CameraFollow.Instance.SetZoomNormal();

            if (npcTriggerer)
            {
                npcTriggerer.StopTalking();
                npcTriggerer = null;
            }
        }

        private void ContinueStory() 
        {
            if (currentStory.canContinue) 
            {
                // set text for the current dialogue line
                if (displayLineCoroutine != null) 
                {
                    StopCoroutine(displayLineCoroutine);
                }

                string currentLine = currentStory.Continue();
                HandleTags(currentStory.currentTags);
                
                displayLineCoroutine = StartCoroutine(DisplayLine(currentLine));
            }
            else 
            {
                StartCoroutine(ExitDialogueMode());
            }
        }

        //TODO: seperate this into the two bubbles
        private IEnumerator DisplayLine(string line)
        {
            if (line.Trim().Length == 0)
            {
                ContinueStory();
                yield break;
            }
            
            TextMeshProUGUI currentDialogueText = npcTalking ? npcDialogueText : playerDialogueText;

            if (npcTalking)
            {
                playerDialoguePanel.SetActive(false);
                npcDialoguePanel.SetActive(true);
            }
            else
            {
                playerDialoguePanel.SetActive(true);
                npcDialoguePanel.SetActive(false);
            }
            
            // set the text to the full line, but set the visible characters to 0
            currentDialogueText.text = line;
            currentDialogueText.maxVisibleCharacters = 0;
            
            // hide items while text is typing
            npcDialogueContinueIcon.SetActive(false);
            playerDialogueChoicesIcon.SetActive(false);

            _canContinueToNextLine = false;

            bool isAddingRichTextTag = false;

            // wait to reset frame input
            yield return new WaitForSeconds(0);
            
            // display each letter one at a time
            foreach (char letter in line.ToCharArray())
            {
                
                // if the submit button is pressed, finish up displaying the line right away
                if (UserInput.Instance.InteractButtonPressedThisFrame) 
                {
                    Debug.Log("Skipping this line.");
                    currentDialogueText.maxVisibleCharacters = line.Length;
                    break;
                }

                // check for rich text tag, if found, add it without waiting
                if (letter == '<' || isAddingRichTextTag) 
                {
                    isAddingRichTextTag = true;
                    if (letter == '>')
                    {
                        isAddingRichTextTag = false;
                    }
                }
                // if not rich text, add the next letter and wait a small time
                else 
                {
                    currentDialogueText.maxVisibleCharacters++;
                    yield return new WaitForSeconds(typingSpeed);
                }
            }

            // actions to take after the entire line has finished displaying
            npcDialogueContinueIcon.SetActive(true);
            
            // Handle possible choices
            if (currentStory.currentChoices.Count > 0)
            {
                ShowChoicePanel();
                DisplayChoice(0);
            }

            _canContinueToNextLine = true;
        }

        private void HandleTags(List<string> currentTags)
        {
            npcTalking = false; //if there are no tags, assume player is talking
            // loop through each tag and handle it accordingly
            foreach (string tag in currentTags) 
            {
                switch (tag.Trim())
                {
                    case MUSHROOM_SPEAKING_TAG:
                        npcTalking = true;
                        break;
                    case PLAYER_STRING_TAG:
                        npcTalking = false;
                        break;
                    default:
                        Debug.LogWarning("Tag could not be appropriately parsed: " + tag);
                        npcTalking = true;
                        break;
                }
            }
        }
        
        public void MakeChoice()
        {
            if (_canContinueToNextLine) 
            {
                currentStory.ChooseChoiceIndex(_currentChoiceIndex);
                ContinueStory();
            }
        }

        private void ShowChoicePanel()
        {
            _currentChoiceIndex = 0;
            playerDialoguePanel.SetActive(true);
            playerDialogueChoicesIcon.SetActive(true);
        }
        
        private void DisplayChoice(int index)
        {
            playerDialogueText.text = currentStory.currentChoices[index].text;
        }

        public void NextChoice()
        {
            if(currentStory.currentChoices.Count == 0) return;

            _currentChoiceIndex++;
            if (_currentChoiceIndex >= currentStory.currentChoices.Count)
                _currentChoiceIndex = 0;
            
            DisplayChoice(_currentChoiceIndex);
        }

        public void PreviousChoice()
        {
            if(currentStory.currentChoices.Count == 0) return;

            _currentChoiceIndex--;
            if (_currentChoiceIndex < 0)
                _currentChoiceIndex = currentStory.currentChoices.Count-1;
            
            DisplayChoice(_currentChoiceIndex);
        }

        public Ink.Runtime.Object GetVariableState(string variableName) 
        {
            Ink.Runtime.Object variableValue = null;
            dialogueVariables.variables.TryGetValue(variableName, out variableValue);
            if (variableValue == null) 
            {
                Debug.LogWarning("Ink Variable was found to be null: " + variableName);
            }
            return variableValue;
        }

        // This method will get called anytime the application exits.
        // Depending on your game, you may want to save variable state in other places.
        public void OnApplicationQuit() 
        {
            dialogueVariables.SaveVariables();
        }

    }
}
