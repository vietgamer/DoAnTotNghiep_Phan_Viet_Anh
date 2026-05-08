using UnityEngine;

public class FlagSpawner : MonoBehaviour
{
    [Header("Cấu hình Cờ")]
    public GameObject flagPrefab; // Kéo Prefab cái cờ vào đây

    [Header("Cấu hình Vị trí")]
    public int columns = 5;       // Số hàng ngang khi xếp cờ
    public float spacing = 0.2f;  // Khoảng cách giữa các cái cờ

    void Start()
    {
        // Đợi một chút để BoardManager khởi tạo xong số lượng bom
        Invoke("SpawnFlags", 0.5f);
    }

    public void SpawnFlags()
    {
        // Lấy số lượng bom từ BoardManager (Dùng Instance đã tạo ở bước trước)
        int totalFlags = BoardManager.Instance.mineCount;

        for (int i = 0; i < totalFlags; i++)
        {
            // Tính toán vị trí xếp cờ theo hàng lối cho đẹp
            float xPos = (i % columns) * spacing;
            float zPos = (i / columns) * spacing;
            Vector3 spawnPos = transform.position + new Vector3(xPos, 0, zPos);

            // Tạo cờ
            GameObject newFlag = Instantiate(flagPrefab, spawnPos, transform.rotation);

            // Đặt túi/khay làm cha để dễ quản lý trong Hierarchy
            newFlag.transform.SetParent(this.transform);
        }
    }
}