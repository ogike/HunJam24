using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Player;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public bool Alive { get; private set; }
    public bool Paused { get; private set; }
    
    private Transform _playerTransform;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one GameManager in scene! " + gameObject);
            return;
        }

        Instance = this;
        Alive = true;
        Paused = false;
    }
    
    private void Start()
    {
        _playerTransform = PlayerController.Instance.transform;
    }
    
    public void PlayerDeath()
    {
        Alive = false;
    }
    
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        //TODO: should not be hardcoded and rely on being first scene
        SceneManager.LoadScene(0);
    }

    public void ChangePause(bool paused)
    {
        Paused = paused;
    }
    
    public bool ChangeTime(float newValue)
    {
        if (!Alive) return false;
        
        Time.timeScale = newValue;
        return true;
    }
}
