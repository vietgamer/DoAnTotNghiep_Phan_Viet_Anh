using UnityEngine;
using System.Collections;

public class MiniTerrainController : MonoBehaviour
{
    public Transform terrainAnchor;
    public Vector3 hiddenPos;
    public Vector3 visiblePos;
    public float moveSpeed = 2f;
    public AudioSource sfx;

    private bool isShown = false;

    void Start()
    {
        // Đảm bảo lúc mới vào game, sa bàn nằm đúng vị trí ẩn
        if (terrainAnchor != null)
        {
            terrainAnchor.localPosition = hiddenPos;
        }
        isShown = false; // Đảm bảo biến này luôn là false khi bắt đầu
    }

    public void ToggleTerrain()
    {
        // Đảo ngược trạng thái trước khi chạy logic di chuyển
        isShown = !isShown;


        StopAllCoroutines();

        // Chọn mục tiêu dựa trên trạng thái mới
        Vector3 target = isShown ? visiblePos : hiddenPos;
        StartCoroutine(MoveTerrain(target));

        if (sfx != null) sfx.Play();

        Debug.Log("Sa bàn đang di chuyển tới: " + (isShown ? "Hiện" : "Ẩn"));
    }

    IEnumerator MoveTerrain(Vector3 target)
    {
        // Sử dụng vòng lặp While với độ sai số nhỏ
        while (Vector3.Distance(terrainAnchor.localPosition, target) > 0.001f)
        {
            terrainAnchor.localPosition = Vector3.MoveTowards(
                terrainAnchor.localPosition,
                target,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }
        // Đảm bảo khớp vị trí hoàn toàn khi xong
        terrainAnchor.localPosition = target;
    }
}