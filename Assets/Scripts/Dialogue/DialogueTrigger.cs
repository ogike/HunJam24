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
                    SfxManager.Instance.PlayAudio(SfxManager.Instance.interactSound);
                    DialogueManager.Instance.EnterDialogueMode(inkJSON, speechBubblePosition);
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