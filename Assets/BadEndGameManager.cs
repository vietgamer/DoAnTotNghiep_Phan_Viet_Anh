using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class BadEndGameManager : MonoBehaviour
{
    public AudioSource radioSource;
    public AudioClip winVoiceClip; // File ghi âm tiếng radio báo thành công
    public GameObject uiPanel;    // Panel chứa các nút và text
    public TextMeshProUGUI statusText;


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
        statusText.text = "Qua màn thất bại!";


    }
}