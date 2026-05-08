using UnityEngine;
using TMPro;

public class dongho : MonoBehaviour
{
    [SerializeField] private float initialTime = 600;
    public static float timeRemaining; // 10 phút
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;


    public bool timerIsRunning = false;

    void Awake()
    {
        timeRemaining = initialTime; // Gán giá trị từ Unity vào biến static khi game chạy
    }


    void Start()
    {
        timerIsRunning = true;
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;

                // Gọi hàm kết thúc game (Game Over) tại đây
            }
        }

        // 2. Cập nhật số cờ đúng (Lấy từ BoardManager)
        scoreText.text = "Co dung: " + BoardManager.Instance.CorrectFlagsCount;

    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}