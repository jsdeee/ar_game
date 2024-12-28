using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MyAnimatorController : MonoBehaviour
{
    public Animator animator;
    private VirtualJoystick virtualJoystick; // �����n�� (�p��)

    // private bool isRun = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (virtualJoystick == null) return; // �p�G�S�������n��A���L
        //// ���o�n�쪺��X��V
        //Vector2 direction = virtualJoystick.inputDirection;

        //if (direction != Vector2.zero)
        //{
        //    ShowRun(); // ����]�B�ʵe
        //}
        //else
        //{
        //    ShowIdle(); // �^��ݾ��ʵe
        //}

        // �˴��O�_���U�ť���
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShowAttack();
        }
        // �ˬd�O�_���U W, A, S, D ����
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            ShowRun();
        }
        else
        {
            ShowIdle();
        }
    }
    //�s�W��k��ReticleBehaviour�b�ͦ�����ɡA�i�H�q�~���]�w�����n��
    public void SetVirtualJoystick(VirtualJoystick joystick)
    {
        this.virtualJoystick = joystick; // �]�m�����n��
    }

    public void ShowAttack()
    {
        animator.SetTrigger("T_isAttack");
    }

    private void ShowRun()
    {
        // �]�w�ʵe���]�B
        animator.SetBool("isRun", true);
    }

    private void ShowIdle()
    {
        // �]�w�ʵe���R��
        animator.SetBool("isRun", false);
    }


}
