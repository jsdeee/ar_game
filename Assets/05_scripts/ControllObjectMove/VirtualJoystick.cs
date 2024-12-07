using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public RectTransform joystickBackground; // �n��I��
    public RectTransform joystickHandle; // �n��ޱ���
    public Vector2 inputDirection; // �n���X����V (X, Y)

    private Vector2 joystickCenter; // �n�줤���I

    private void Start()
    {
        // �O���n��I���������I
        joystickCenter = joystickBackground.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // �p�����P�n�줤�ߪ��Z��
        Vector2 direction = eventData.position - joystickCenter;
        float radius = joystickBackground.sizeDelta.x / 2; // �n��I�����b�|

        // ����ޱ���u��b�I����餺����
        inputDirection = Vector2.ClampMagnitude(direction, radius) / radius;

        // ��s�ޱ����m
        joystickHandle.anchoredPosition = inputDirection * radius;

        Debug.Log(inputDirection);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData); // �b���U��Ĳ�o�즲�޿�
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // ���m�n��
        inputDirection = Vector2.zero;
        joystickHandle.anchoredPosition = Vector2.zero;
    }
}
