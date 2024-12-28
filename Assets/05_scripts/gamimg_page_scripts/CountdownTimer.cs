using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // �p�G�ϥ� TextMeshPro�A�אּ using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public Text countdownText; // �p�G�ϥ� TextMeshPro�A�אּ public TMP_Text countdownText;
    public float countdownTime = 30f; // �˼ƪ����
    private float currentTime;

    private bool isCounting = false;

    void Start()
    {
        currentTime = countdownTime; // ��l�ƭ˼Ʈɶ�
        UpdateCountdownDisplay();
        StartCountdown();
    }

    void Update()
    {
        if (isCounting && currentTime > 0)
        {
            currentTime -= Time.deltaTime; // �C�V��֮ɶ�
            UpdateCountdownDisplay();
            if (currentTime <= 0)
            {
                currentTime = 0;
                isCounting = false; // ����˼�
                OnCountdownEnd();
            }

        }
    }

    private void UpdateCountdownDisplay()
    {
        // ��s�˼Ʈɶ�����r�榡�A��ܬ� "����:���"
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
        countdownText.text = "�˼Ƶ���!";
        // �b�o�̲K�[�˼Ƶ����᪺�欰�A��p��ܹC�������e����Ĳ�o�ƥ�C
    }
}
