using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public RectTransform joystickBackground; // 搖桿背景
    public RectTransform joystickHandle; // 搖桿操控桿
    public Vector2 inputDirection; // 搖桿輸出的方向 (X, Y)

    private Vector2 joystickCenter; // 搖桿中心點

    private void Start()
    {
        // 記錄搖桿背景的中心點
        joystickCenter = joystickBackground.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 計算手指與搖桿中心的距離
        Vector2 direction = eventData.position - joystickCenter;
        float radius = joystickBackground.sizeDelta.x / 2; // 搖桿背景的半徑

        // 限制操控桿只能在背景圓圈內移動
        inputDirection = Vector2.ClampMagnitude(direction, radius) / radius;

        // 更新操控桿位置
        joystickHandle.anchoredPosition = inputDirection * radius;

        Debug.Log(inputDirection);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData); // 在按下時觸發拖曳邏輯
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // 重置搖桿
        inputDirection = Vector2.zero;
        joystickHandle.anchoredPosition = Vector2.zero;
    }
}
