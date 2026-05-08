using UnityEngine;

public class KeepCoilParallel : MonoBehaviour
{
    // Chúng ta sẽ tham chiếu đến thân máy (cha)
    public Transform mainShaftTransform;

    // Góc xoay mong muốn của vòng dò so với mặt đất (thường là 0,0,0)
    private Quaternion targetRotation = Quaternion.identity;

    void Start()
    {
        // Nếu chưa gắn thân máy trong Inspector, tự tìm cha
        if (mainShaftTransform == null)
        {
            mainShaftTransform = transform.parent;
        }

        // Khởi tạo góc xoay mục tiêu là "thẳng đứng/song song mặt đất"
        // Target rotation is often just identity (0,0,0) for parallel to ground
        targetRotation = Quaternion.identity;
    }

    void LateUpdate() // Dùng LateUpdate để đảm bảo thân máy đã di chuyển xong
    {
        if (mainShaftTransform == null) return;

        // --- CÁCH 1: GIỮ SONG SONG CỐ ĐỊNH (WORLD SPACE) ---
        // Cách này ép vòng dò luôn có góc xoay (0,0,0) trong thế giới
        // Bất kể thân máy xoay thế nào, vòng dò vẫn song song mặt đất.

        //transform.rotation = targetRotation; // Chỉ dùng cái này nếu không cần giới hạn trục

        // --- CÁCH 2: GIỮ SONG SONG VÀ GIỚI HẠN CHỈ XOAY 1 CHIỀU (Khớp Xoay) ---
        // Để vòng dò chỉ xoay 1 trục (ví dụ trục X của khớp), 
        // ta cần tính toán góc bù trừ local.

        // 1. Lấy góc xoay hiện tại của Thân máy trong World space
        Quaternion shaftRotation = mainShaftTransform.rotation;

        // 2. Tính toán góc xoay cần thiết cho khớp con để bù trừ lại góc xoay của cha
        // Mục tiêu: WorldRotation(Con) = WorldRotation(Cha) * LocalRotation(Con) = TargetWorldRotation
        // => LocalRotation(Con) = WorldRotation(Cha)^-1 * TargetWorldRotation

        Quaternion neededLocalRotation = Quaternion.Inverse(shaftRotation) * targetRotation;

        // 3. Tách lấy góc Euler để dễ giới hạn trục
        Vector3 localEuler = neededLocalRotation.eulerAngles;

        // 4. GIỚI HẠN CHỈ XOAY 1 TRỤC (Ví dụ trục X - gật lên xuống)
        // Ta giữ nguyên góc bù trừ ở trục X, và ép Y và Z về 0 (theo local)
        // Điều này có nghĩa khớp nối chỉ cho phép vòng dò 'gật' lên xuống.

        float lockedLocalX = localEuler.x;

        // Áp dụng góc xoay local mới chỉ trên trục X
        transform.localRotation = Quaternion.Euler(lockedLocalX, 0f, 0f);
    }
}