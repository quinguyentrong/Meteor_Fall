using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TouchController: MonoBehaviour
{
    public static TouchController Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        //Input.simulateMouseWithTouches = true;
        HalfHeight = Screen.height * 0.5f;
    }

    private void Start()
    {
        CustomEventManager.Instance.OnNewGame += EnableTouch;
        CustomEventManager.Instance.OnGameOver += DisableTouch;
    }
    private void OnDestroy()
    {
        CustomEventManager.Instance.OnNewGame -= EnableTouch;
        CustomEventManager.Instance.OnGameOver -= DisableTouch;
    }
    private bool IsTouchEnable = false;
    private void EnableTouch()
    {
        IsTouchEnable = true;
    }

    private void DisableTouch(bool isRedWin)
    {
        IsTouchEnable = false;
    }


    private float HalfHeight;
    Touch RedSideTouch;
    Touch BlueSideTouch;
    private void Update()
    {
        if (IsTouchEnable == false) return;
        bool isRedSideGetTouch = false;
        bool isBlueSideGetTouch = false;
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (Input.GetTouch(i).position.y > HalfHeight)
                {
                    if (isBlueSideGetTouch == false)
                    {
                        BlueSideTouch = Input.GetTouch(i);
                        isBlueSideGetTouch = true;
                        ManageTouch_BlueSide();
                    }
                }
                else
                {
                    if (isRedSideGetTouch == false)
                    {
                        RedSideTouch = Input.GetTouch(i);
                        isRedSideGetTouch = true;
                        ManageTouch_RedSide();
                    }
                }
            }
        }
    }

    public Action<Vector2> OnStartTouching_RedSide;
    public Action<Vector2> OnTouching_RedSide;
    public Action OnStopTouching_RedSide;
    private void ManageTouch_RedSide()
    {
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(RedSideTouch.position);
        touchPosition.z = 0;

        switch (RedSideTouch.phase)
        {
            case TouchPhase.Began:
                if (OnStartTouching_RedSide != null)
                {
                    OnStartTouching_RedSide(touchPosition);
                }
                break;
            case TouchPhase.Moved:
                if (OnTouching_RedSide != null)
                {
                    OnTouching_RedSide(touchPosition);
                }
                break;
            case TouchPhase.Ended:
                if (OnStopTouching_RedSide != null)
                {
                    OnStopTouching_RedSide();
                }
                break;
        }
    }

    public Action<Vector2> OnStartTouching_BlueSide;
    public Action<Vector2> OnTouching_BlueSide;
    public Action OnStopTouching_BlueSide;
    private void ManageTouch_BlueSide()
    {
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(BlueSideTouch.position);
        touchPosition.z = 0;

        switch (BlueSideTouch.phase)
        {
            case TouchPhase.Began:
                if (OnStartTouching_BlueSide != null)
                {
                    OnStartTouching_BlueSide(touchPosition);
                }
                break;
            case TouchPhase.Moved:
                if (OnTouching_BlueSide != null)
                {
                    OnTouching_BlueSide(touchPosition);
                }
                break;
            case TouchPhase.Ended:
                if (OnStopTouching_BlueSide != null)
                {
                    OnStopTouching_BlueSide();
                }
                break;
        }
    }
}
