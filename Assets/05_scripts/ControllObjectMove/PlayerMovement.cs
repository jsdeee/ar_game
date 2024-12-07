using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public VirtualJoystick virtualJoystick; // �����n�쪺�ޥ�
    private CharacterController controller;
    public float Speed = 0.01f;
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

    //��L�ޱ�
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
        // �q�����n�������J��V
        Vector2 joystickInput = virtualJoystick.inputDirection;

        // �ˬd�O�_����J��V
        if (joystickInput.sqrMagnitude > 0.001f) // �קK�B�z���p��J�A�����ݰ�
        {
            // �p�Ⲿ�ʤ�V�M�t��
            Vector3 moveDirection = new Vector3(joystickInput.x, 0, joystickInput.y).normalized;
            var move = moveDirection * Speed * Time.deltaTime * Speedfactor;
            controller.Move(move);
            Debug.Log("move: " + move);

            // �p�⪫�����¦V����V
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotateSpeed * Time.deltaTime);
            transform.rotation = targetRotation;
        }
    }
}
