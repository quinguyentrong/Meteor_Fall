using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomEventManager: MonoBehaviour
{
    public static CustomEventManager Instance;
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
    }
    /// <summary>
    /// Event đầu tiên khi khởi tạo game mới
    /// </summary>
    public Action OnNewGame;
    /// <summary>
    /// Event khi có thay đổi score (dùng cả đầu game)
    /// <br>Còn nếu tỉ số là 2-1 nghiêng về đỏ chẳng hạn thì là new Vector2Int(2,1)</br>
    /// </summary>
    public Action<Vector2Int> OnSetScore;
    /// <summary>
    /// Event khi phân định thắng thua
    /// <br>TRUE nếu đỏ thắng, FALSE khi xanh thắng</br>
    /// </summary>
    public Action<bool> OnGameOver;
}
