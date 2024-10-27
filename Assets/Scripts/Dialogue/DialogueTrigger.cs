using TMPro;
using UnityEngine;

namespace Dialogue
{
    /// <summary>
    /// Holds everything needed for a dialogue from NPC side
    /// </summary>
    public class DialogueTrigger : MonoBehaviour
    {
        [Header("Ink JSON")]
        public TextAsset inkJSON;
        
        [Header("UI")]
        public GameObject visualCue;

        public Transform speechBubblePosition;
        public Transform playerTalkingPosition;

        private bool playerInRange;

        private void Awake() 
        {
            playerInRange = false;
            visualCue.SetActive(false);
        }

        private void Update() 
        {
            if (playerInRange && !DialogueManager.Instance.dialogueIsPlaying) 
            {
                visualCue.SetActive(true);
                if (UserInput.Instance.InteractButtonPressedThisFrame)
                {
                    Debug.Log(inkJSON.name);
                    Debug.Log(speechBubblePosition.position);
                    Debug.Log(DialogueManager.Instance.name);
                    DialogueManager.Instance.EnterDialogueMode(inkJSON, speechBubblePosition, playerTalkingPosition.position);
                }
            }
            else 
            {
                visualCue.SetActive(false);
            }
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
    }
}