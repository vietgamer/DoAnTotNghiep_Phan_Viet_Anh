using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/*[RequireComponent(typeof(XRBaseInteractable))]*/
[RequireComponent(typeof(MeshRenderer))]
public class Tile : MonoBehaviour
{
    public int x, y;
    public bool isMine = false;
    public int adjacentMines = 0;
    public bool isRevealed = false;
    public bool isFlagged = false;
    private bool isHoveredByDetector = false;

    [HideInInspector] public BoardManager board;

    private MeshRenderer meshRenderer;
    private TextMesh numberText;

    private float targetAlpha = 0f;
    public float hoverAlpha = 0.4f;
    public float revealedAlpha = 1.0f;

    private const string SHADER_ALPHA = "_Alpha";
    private const string SHADER_HOVER = "_Hover_Intensity";

    private Color originalColor;
    public Color hoverColor = Color.yellow;

    public enum InteractionMode { Reveal, Flag }
    public static InteractionMode currentMode = InteractionMode.Reveal;

    private GameObject plantedFlag; // Lưu trữ cái cờ đã cắm trên ô này



    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        numberText = GetComponentInChildren<TextMesh>();
        originalColor = meshRenderer.material.color;

        meshRenderer = GetComponent<MeshRenderer>();
        // Reset về trong suốt và tắt hiệu ứng hover lúc đầu
        meshRenderer.material.SetFloat(SHADER_ALPHA, 0f);
        meshRenderer.material.SetFloat(SHADER_HOVER, 0f);

        /*
        XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();
        interactable.selectEntered.AddListener(OnSelected);
        interactable.selectExited.AddListener(OnSelectExited);
        interactable.hoverEntered.AddListener(OnHoverEntered);
        interactable.hoverExited.AddListener(OnHoverExited);
        */
    }

    public void Init(int _x, int _y, BoardManager _board)
    {
        x = _x;
        y = _y;
        board = _board;
        isMine = false;
        adjacentMines = 0;
        isRevealed = false;
        isFlagged = false;

        meshRenderer.material.color = originalColor;
        if (numberText != null)
            numberText.text = "";
    }

    // Trong Tile.cs
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("detector"))
        {
            isHoveredByDetector = true;
            meshRenderer.material.color = hoverColor;

            // BÁO CHO MÁY DÒ BIẾT ĐANG ĐỨNG TRÊN Ô NÀY
            MineDetector detector = other.GetComponent<MineDetector>();
            if (detector != null)
            {
                detector.SetCurrentTile(this);
            }

            // Bật hiệu ứng nhiễu sóng và đặt độ mờ 40%
            meshRenderer.material.SetFloat(SHADER_ALPHA, hoverAlpha);
            meshRenderer.material.SetFloat(SHADER_HOVER, 1.0f); // Hiện vạch


        }

        if (other.CompareTag("Flag") && !isRevealed && plantedFlag == null)
        {
            PlantFlag(other.gameObject);

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("detector"))
        {
            isHoveredByDetector = false;
            if (!isRevealed) meshRenderer.material.color = originalColor;

            // BÁO CHO MÁY DÒ LÀ ĐÃ RỜI ĐI
            MineDetector detector = other.GetComponent<MineDetector>();
            if (detector != null)
            {
                detector.ClearCurrentTile(this);
            }

            // Tắt hết
            meshRenderer.material.SetFloat(SHADER_ALPHA, 0f);
            meshRenderer.material.SetFloat(SHADER_HOVER, 0f);
        }
    }

    /*
    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        if (!isRevealed)
            meshRenderer.material.color = hoverColor;
    }

    private void OnHoverExited(HoverExitEventArgs args)
    {
        if (!isRevealed)
            meshRenderer.material.color = originalColor;
    }

  

    private void OnSelected(SelectEnterEventArgs args)
    {
        /*
        if (!isRevealed && !isFlagged)
        {
       
        if (currentMode == InteractionMode.Reveal)
        {
            // CHẾ ĐỘ LẬT Ô
            if (!isFlagged) // Không cho lật nếu đang có cờ
            {
                Reveal();
            }
        }
        else
        {
            // CHẾ ĐỘ CẮM CỜ
            ToggleFlag();
        }
        
        if (!isMine && adjacentMines == 0)
        {
            board.FloodReveal(x, y);
        }

    }
        
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        UnReveal();
    }

*/

   

    public void OnDetectorAction()
    {
        if (isHoveredByDetector && !isRevealed && !isFlagged)
        {
            if (currentMode == InteractionMode.Reveal)
            {
                // CHẾ ĐỘ LẬT Ô
                if (!isFlagged) // Không cho lật nếu đang có cờ
                {
                    Reveal();
                }
            }
            else
            {
                // CHẾ ĐỘ CẮM CỜ
                ToggleFlag();
            }
        }
    }

    public void UnReveal()
    {
        if (!isRevealed) return;
        isRevealed = false;

        meshRenderer.material.color = originalColor;
        if (numberText != null)
            numberText.text = "";
    }    

    public void Reveal()
    {
        /*
        meshRenderer.material.SetFloat(SHADER_ALPHA, revealedAlpha);
        meshRenderer.material.SetFloat(SHADER_HOVER, 1.0f);
        */

        if (isRevealed) return;
        isRevealed = true;

        if (isMine)
        {
            /*
            meshRenderer.material.color = Color.red;
            if (numberText != null)
                numberText.text = "M";
            Explosion explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity)
                            .GetComponent<Explosion>();
            explosion.TriggerExplosion();
            */
            meshRenderer.material.color = Color.white;
            if (adjacentMines > 0 && numberText != null)
                numberText.text = adjacentMines.ToString();
        }
        else
        {
            meshRenderer.material.color = Color.white;
            if (adjacentMines > 0 && numberText != null)
                numberText.text = adjacentMines.ToString();
        }
    }

    public void ToggleFlag()
    {
        /*
        if (isRevealed) return;
        isFlagged = !isFlagged;
        if (numberText != null)
            numberText.text = isFlagged ? "F" : "";
        */

        if (isRevealed) return;
        isRevealed = true;

        if (isMine)
        {
            meshRenderer.material.color = Color.white;
            if (numberText != null)
                adjacentMines = 1;
                numberText.text = adjacentMines.ToString();
        }    

    }

    public static void ToggleMode()
    {
        if (currentMode == InteractionMode.Reveal)
            currentMode = InteractionMode.Flag;
        else
            currentMode = InteractionMode.Reveal;

        Debug.Log("Chế độ hiện tại: " + currentMode);
    }

    void PlantFlag(GameObject flag)
    {


        // 2. Chỉnh vị trí cờ về chính giữa ô gạch
        flag.transform.SetParent(this.transform);
        flag.transform.localPosition = new Vector3(0, 0.5f, 0); // Chỉnh độ cao tùy model
        flag.transform.localRotation = Quaternion.identity;

        // 3. Đánh dấu trạng thái
        isFlagged = true;
        plantedFlag = flag;

        // 4. Cập nhật số lượng cờ đúng trên đồng hồ
        if (isMine) BoardManager.Instance.CorrectFlagsCount++;
    }
}


