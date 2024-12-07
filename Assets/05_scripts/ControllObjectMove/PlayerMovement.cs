using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public VirtualJoystick virtualJoystick; // 虛擬搖桿的引用
    private CharacterController controller;
    public float Speed = 1f;
    public float Speedfactor = 0.05f;
    public float RotateSpeed = 1.0f;


    // Start is called before the first frame update
    void Start()
    {
        controller = transform.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveLikeWow();
    }

    //鍵盤操控
    //private void MoveLikeWow()
    //{
    //    var horizontal = Input.GetAxis("Horizontal");
    //    var vertical = Input.GetAxis("Vertical");

    //    var move = transform.forward * Speed * vertical * Time.deltaTime;
    //    controller.Move(move);

    //    transform.Rotate(Vector3.up, horizontal * RotateSpeed);

    //    Debug.Log(horizontal);
    //}

    private void MoveLikeWow()
    {
        // 從虛擬搖桿獲取輸入方向
        if (virtualJoystick == null) return; // 確保 joystick 不為空
        // 從虛擬搖桿獲取輸入方向
        Vector2 joystickInput = virtualJoystick.inputDirection;

        // 檢查是否有輸入方向
        if (joystickInput.sqrMagnitude > 0.001f) // 避免處理極小輸入，防止物件抖動
        {
            // 使用CharacterController計算移動方向和速度
            Vector3 moveDirection = new Vector3(joystickInput.x, 0, joystickInput.y).normalized;
            var move = moveDirection * Speed * Time.deltaTime * Speedfactor;
            controller.Move(move);
            Debug.Log("move: " + move);

            // 使用transform移動物件
            //transform.Translate(moveDirection * Speed * Time.deltaTime, Space.World);

            // 計算物件應朝向的方向
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotateSpeed * Time.deltaTime);
            transform.rotation = targetRotation;
        }
    }

    // 新增方法讓外部設定虛擬搖桿
    public void SetVirtualJoystick(VirtualJoystick joystick)
    {
        virtualJoystick = joystick;
    }
}
