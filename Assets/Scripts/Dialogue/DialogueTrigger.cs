using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

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
        public Transform cameraPosition; 

        private bool playerInRange;
        private bool isTalking;

        public Animator animator;

        private void Awake() 
        {
            playerInRange = false;
            isTalking = false;
            visualCue.SetActive(false);

            animator.SetBool("isTalking", false);
        }

        private void Update() 
        {
            if (playerInRange && !DialogueManager.Instance.dialogueIsPlaying) 
            {
                visualCue.SetActive(true);
                if (UserInput.Instance.InteractButtonPressedThisFrame)
                {
                    DialogueManager.Instance.EnterDialogueMode(inkJSON, speechBubblePosition,
                        playerTalkingPosition.position, this);
                    
                    CameraFollow.Instance.SetDialoguePosition(cameraPosition.position);
                    
                    isTalking = true;
                    animator.SetBool("isTalking", true);
                    SfxManager.Instance.PlayAudio(SfxManager.Instance.interactSound);
                }
            }
            else 
            {
                visualCue.SetActive(false);
            }
        }

        public void StopTalking()
        {
            animator.SetBool("isTalking", false);
            animator.SetTrigger("Hide");
            isTalking = false;
        }

        private void OnTriggerEnter2D(Collider2D collider) 
        {
            if (collider.gameObject.tag == "Player")
            {
                playerInRange = true;
                
                if(!isTalking)
                    animator.SetTrigger("Appear");
            }
        }

        private void OnTriggerExit2D(Collider2D collider) 
        {
            if (collider.gameObject.tag == "Player")
            {
                playerInRange = false;
                
                if(!isTalking)
                    animator.SetTrigger("Hide");
            }
        }
    }
}