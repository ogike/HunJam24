using TMPro;
using UnityEngine;

namespace SoundTrigger
{
    public class SoundTrigger : MonoBehaviour
    {
        public AudioSource _audioSource;
        private bool playerInRange;
        private bool soundIsPlaying;

        private void Awake() 
        {
            playerInRange = false;
        }

        private void Update() 
        {
            if (playerInRange) {
                if(!_audioSource.isPlaying)  {
                    Debug.Log("Playing sound");
                    _audioSource.Play();
                }
            } else {
                if(_audioSource.isPlaying) {
                    Debug.Log("Stopping sound");
                    _audioSource.Stop();
                }
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