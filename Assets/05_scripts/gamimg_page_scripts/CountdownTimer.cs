using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 如果使用 TextMeshPro，改為 using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public Text countdownText; // 如果使用 TextMeshPro，改為 public TMP_Text countdownText;
    public float countdownTime = 30f; // 倒數的秒數
    private float currentTime;

    private bool isCounting = false;

    void Start()
    {
        currentTime = countdownTime; // 初始化倒數時間
        UpdateCountdownDisplay();
        StartCountdown();
    }

    void Update()
    {
        if (isCounting && currentTime > 0)
        {
            currentTime -= Time.deltaTime; // 每幀減少時間
            UpdateCountdownDisplay();
            if (currentTime <= 0)
            {
                currentTime = 0;
                isCounting = false; // 停止倒數
                OnCountdownEnd();
            }

        }
    }

    private void UpdateCountdownDisplay()
    {
        // 更新倒數時間的文字格式，顯示為 "分鐘:秒數"
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        countdownText.text = $"{minutes:00}:{seconds:00}";
    }

    public void StartCountdown()
    {
        isCounting = true;
    }

    public void StopCountdown()
    {
        isCounting = false;
    }

    private void OnCountdownEnd()
    {
        countdownText.text = "倒數結束!";
        // 在這裡添加倒數結束後的行為，比如顯示遊戲結束畫面或觸發事件。
    }
}
