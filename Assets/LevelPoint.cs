using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelPoint : MonoBehaviour
{
    [Header("Level Data")]
    public string levelName;
    public string levelDescription;
    public Sprite levelPreview;
    public string sceneToLoad;
    public string bombcount;
    public string time;

    [Header("UI References")]
    public GameObject infoPanel;
    public GameObject MainUI;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI bombText;
    public Image previewImage;
    public static string selectedScene;

    // Hàm này gọi khi người chơi chọn vào điểm này (Select Entered)
    public void OnPointSelected()
    {
        infoPanel.SetActive(true);
        MainUI.SetActive(false);
        nameText.text = levelName;
        descText.text = levelDescription;
        previewImage.sprite = levelPreview;
        timeText.text = "Thời gian: "+ time;
        bombText.text = "Số bom: " + bombcount;
        selectedScene = sceneToLoad; // Lưu lại scene để nút Play sử dụng
    }
}