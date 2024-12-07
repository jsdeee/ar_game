using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MyAnimatorController : MonoBehaviour
{
    public Animator animator;
    public VirtualJoystick virtualJoystick; // 虛擬搖桿

    // private bool isRun = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // 取得搖桿的輸出方向
        Vector2 direction = virtualJoystick.inputDirection;

        if (direction != Vector2.zero)
        {
            ShowRun(); // 播放跑步動畫
        }
        else
        {
            ShowIdle(); // 回到待機動畫
        }

        // 檢測是否按下空白鍵
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShowAttack();
        }
        //// 檢查是否按下 W, A, S, D 按鍵
        //if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        //{
        //    ShowRun();
        //}
        //else
        //{
        //    ShowIdle();
        //}
    }

    private void ShowAttack()
    {
        animator.SetTrigger("T_isAttack");
    }

    private void ShowRun()
    {
        // 設定動畫為跑步
        animator.SetBool("isRun", true);
    }

    private void ShowIdle()
    {
        // 設定動畫為靜止
        animator.SetBool("isRun", false);
    }
}
