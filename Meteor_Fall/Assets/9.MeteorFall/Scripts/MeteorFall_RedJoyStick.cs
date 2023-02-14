using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeteorFall_RedJoyStick : MonoBehaviour
{
    [SerializeField] private GameObject JoyStick;
    [SerializeField] private RectTransform JoyStickRect;
    [SerializeField] private RectTransform JoyStickCenter;
    [SerializeField] private Canvas Canvas;

    private float CanvasWidth;
    private float CanvasRatio;
    private float CanvasHeight;

    private bool IsTouching = false;
    private bool IsCanUseJoyStick = false;

    private Vector2 CurrentTouchPos;
    private Vector2 IntialTouchPos = Vector2.zero;


    private void Start()
    {
        CanvasWidth = Canvas.GetComponent<RectTransform>().rect.width;
        CanvasRatio = CanvasWidth / Screen.width;
        CanvasHeight = Screen.height * CanvasRatio;

        MeteorFall_GameManager.Instance.OnJoyStick += OnJoyStick;
        MeteorFall_GameManager.Instance.OnEndGame += OnEndGame;
    }

    private void OnDestroy()
    {
        MeteorFall_GameManager.Instance.OnJoyStick -= OnJoyStick;
        MeteorFall_GameManager.Instance.OnEndGame -= OnEndGame;
    }

    private void Update()
    {
        if (IsCanUseJoyStick == false) return;
        
        if (Input.GetMouseButtonDown(0))
        {
            if (Input.mousePosition.y > (float)Screen.height / 2) return;
            
            JoyStick.SetActive(true);

            IsTouching = true;

            CurrentTouchPos.x = Input.mousePosition.x * CanvasRatio;
            CurrentTouchPos.y = Input.mousePosition.y * CanvasRatio;

            IntialTouchPos = CurrentTouchPos;

            SetJoyStickPos(IntialTouchPos);
        }

        if (Input.GetMouseButton(0))
        {
            if (IsTouching)
            {
                CurrentTouchPos.x = Input.mousePosition.x * CanvasRatio;
                CurrentTouchPos.y = Input.mousePosition.y * CanvasRatio;

                SetJoyStickState(IntialTouchPos, CurrentTouchPos);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            IsTouching = false;

            JoyStick.SetActive(false);
            ResetJoyStickState();
        }
    }

    public Vector2 NormalizeJoyStickDragVector { get; private set; }

    private void SetJoyStickPos(Vector2 initTouchPos)
    {
        JoyStickRect.anchoredPosition = initTouchPos - new Vector2(CanvasWidth * 0.5f, CanvasHeight * 0.5f);
    }

    private void SetJoyStickState(Vector2 initTouchPos, Vector2 currentTouchPos)
    {
        NormalizeJoyStickDragVector = (currentTouchPos - initTouchPos).normalized;
        if (Vector2.SqrMagnitude(initTouchPos - currentTouchPos) > 18 * 18)
        {
            JoyStickCenter.anchoredPosition = NormalizeJoyStickDragVector * 18;
        }
        else
        {
            JoyStickCenter.anchoredPosition = currentTouchPos - initTouchPos;
        }
    }

    private void ResetJoyStickState()
    {
        JoyStickRect.anchoredPosition = new Vector2(0,-600f);
        JoyStickCenter.anchoredPosition = Vector2.zero;
        NormalizeJoyStickDragVector = new Vector2(0, 0);
    }

    private void OnJoyStick()
    {
        IsCanUseJoyStick = true;
    }

    private void OnEndGame()
    {
        IsCanUseJoyStick = false;
        JoyStick.SetActive(false);
    }
}
