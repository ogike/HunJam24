using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Instance { get; private set; }
    public Camera Camera { get; private set; }
    
    [Header("Follow player")]
    public float followSpeed;
    private Vector3 _offsetToPlayerPos; //set in beginning

    [Header("Dialogue zooming")]
    public float regularFov;
    public float zoomedInFov;
    public float zoomSpeed;

    private Vector3 dialogueZoomedPosition;

    private bool _zoomedIn;
    private float _zoomVelocity;
    
    private Transform _playerTrans;
    private Transform _myTrans;


    private Vector3 _basePosVelocity;
    private Vector3 targetPos;
    
    

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one CameraFollow instance in scene!");
            return;
        }

        Camera = GetComponent<Camera>();
        Instance = this;
    }

    private void Start()
    {
        _playerTrans = PlayerController.Instance.transform;
        _myTrans = transform;
        _offsetToPlayerPos = _myTrans.position - _playerTrans.position;

        SetZoomNormal();
    }

    void Update()
    {
        UpdatePosition();
        UpdateZoom();
    }

    public void SetDialoguePosition(Vector3 position)
    {
        dialogueZoomedPosition = position;
    }

    private void UpdatePosition()
    {
        Vector3 myPos = _myTrans.position;
        Vector3 playerPos = _playerTrans.position;
        
        if (_zoomedIn)
        {
            targetPos = dialogueZoomedPosition;
        }
        else
        {
            targetPos = playerPos + _offsetToPlayerPos;
        }
        _myTrans.position = Vector3.SmoothDamp(myPos, targetPos, ref _basePosVelocity, followSpeed);
    }

    private void UpdateZoom()
    {
        float targetZoom = _zoomedIn ? zoomedInFov : regularFov;
        Camera.fieldOfView = Mathf.SmoothDamp(Camera.fieldOfView, targetZoom, ref _zoomVelocity, zoomSpeed);
    }

    public void SetZoomNormal()
    {
        _zoomedIn = false;
    }

    public void SetZoomDialogue()
    {
        _zoomedIn = true;
    } 
}
