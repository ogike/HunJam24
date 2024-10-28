using System;
using System.Collections;
using System.Collections.Generic;
using Dialogue;
using TMPro;
using UnityEngine;
using Ink.Runtime;
using Player;

public class GameEnd : MonoBehaviour
{
    [Header("Ink JSON")]
            public TextAsset inkJSON;
            
            [Header("UI")]
            public GameObject visualCue;

            public GameObject endPanel;
            public TextMeshProUGUI endText;

            private bool playerInRange;
            private bool dialogueIsPlaying;
            
            private Story currentStory;
            
            private Coroutine displayLineCoroutine;

            private bool _canContinueToNextLine = false;

            private Transform playerTrans;
            private Vector3 playerStartPos;

            private void Awake() 
            {
                playerInRange = false;
                dialogueIsPlaying = false;
                visualCue.SetActive(false);
                endPanel.SetActive(false);
            }

            private void Start()
            {
                playerTrans = PlayerController.Instance.transform;
                playerStartPos = playerTrans.position;
            }

            private void Update() 
            {
                if (playerInRange && !dialogueIsPlaying) 
                {
                    visualCue.SetActive(true);
                    if (UserInput.Instance.InteractButtonPressedThisFrame)
                    {
                        StartDialogue();
                        dialogueIsPlaying = true;
                        SfxManager.Instance.PlayAudio(SfxManager.Instance.interactSound);
                    }
                }
                
                if(dialogueIsPlaying)
                {
                    visualCue.SetActive(false);
                    
                    if (_canContinueToNextLine
                        && UserInput.Instance.InteractButtonPressedThisFrame)
                    {
                        ContinueStory();
                    }
                }
            }

            private void StartDialogue()
            {
                Debug.Log("End reached");
                currentStory = new Story(inkJSON.text);
                dialogueIsPlaying = true;
                endPanel.SetActive(true);
                ContinueStory();
            }

            private void OnTriggerEnter2D(Collider2D collider) 
            {
                if (collider.gameObject.tag == "Player")
                {
                    playerInRange = true;
                }
            }
    
            private void OnTriggerExit2D(Collider2D collider) 
            {
                if (collider.gameObject.tag == "Player")
                {
                    playerInRange = false;
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

                displayLineCoroutine = StartCoroutine(DisplayLine(currentLine));
            }
            else 
            {
                ExitDialogueMode();
            }
        }

        //TODO: seperate this into the two bubbles
        private IEnumerator DisplayLine(string line)
        {
            Debug.Log(line);
            if (line.Trim().Length == 0)
            {
                ContinueStory();
                yield break;
            }
            
            
            // set the text to the full line, but set the visible characters to 0
            endText.text = line;
            endText.maxVisibleCharacters = 0;

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
                    endText.maxVisibleCharacters = line.Length;
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
                    endText.maxVisibleCharacters++;
                    yield return new WaitForSeconds(DialogueManager.Instance.typingSpeed);
                }
            }
            
            //no choices
            
            _canContinueToNextLine = true;
        }

        public void ExitDialogueMode()
        {
            endPanel.SetActive(false);
            dialogueIsPlaying = false;
            playerTrans.position = playerStartPos;
        }
}
