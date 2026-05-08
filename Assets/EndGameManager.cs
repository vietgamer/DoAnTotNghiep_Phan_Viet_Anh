using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class EndGameManager : MonoBehaviour
{
    public AudioSource radioSource;
    public AudioClip winVoiceClip; // File ghi âm tiếng radio báo thành công
    public GameObject uiPanel;    // Panel chứa các nút và text
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI timeText;

    int minutes = Mathf.FloorToInt((600f - dongho.timeRemaining) / 60);
    int seconds = Mathf.FloorToInt((600f - dongho.timeRemaining) % 60);


    void Start()
    {
        uiPanel.SetActive(false); // Ẩn UI khi mới vào scene
        PlayEndGameSequence();


    }

    void PlayEndGameSequence()
    {
        // 1. Phát tiếng radio
        radioSource.clip = winVoiceClip;
        radioSource.Play();

        // 2. Đợi radio nói xong thì hiện UI (ví dụ radio dài 5 giây)
        Invoke("ShowResults", winVoiceClip.length);
    }

    void ShowResults()
    {
        uiPanel.SetActive(true);
        statusText.text = "Qua màn thành công";

        // Lấy thời gian từ màn chơi trước (giả sử lưu trong PlayerPrefs hoặc Static variable)
        float totalSeconds = PlayerPrefs.GetFloat("FinishTime", 0);
        TimeSpan t = TimeSpan.FromSeconds(totalSeconds);
        timeText.text = string.Format("Thời gian hoàn thành: {0:00}:{1:00}", minutes, seconds);
    }
}