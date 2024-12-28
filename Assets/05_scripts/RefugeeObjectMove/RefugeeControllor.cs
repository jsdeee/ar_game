using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefugeeControllor : MonoBehaviour
{
    public float moveSpeed = 5f; // 移動速度
    public float changeDirectionInterval = 0.1f; // 每隔幾秒改變方向

    private Vector3 randomDirection; // 隨機方向
    private Bounds movementBounds; // 平面的邊界
    private bool isAlive = true; // 紀錄是否存活
    // 初始化方法，用於設定移動範圍
    public void Initialize(Bounds bounds)
    {
        // 初始化隨機方向
        ChangeDirection();
        movementBounds = bounds; // 設定平面邊界
        Debug.Log("planeBounds: " + movementBounds);
    }

    private void Start()
    {
        // 開始隨機移動
        StartCoroutine(ChangeDirectionRoutine());
    }

    private void Update()
    {
        if (!isAlive) return; // 如果已死亡，停止移動

        // 如果隨機方向不為零，旋轉物件朝向移動方向
        if (randomDirection != Vector3.zero)
        {
            // 計算朝向的旋轉
            Quaternion targetRotation = Quaternion.LookRotation(randomDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f); // 平滑旋轉
            Vector3 nextPosition = transform.position + transform.forward * moveSpeed * Time.deltaTime;
            if (!IsWithinBounds(nextPosition))
            {
                // 改變方向（旋轉180度）
                randomDirection = -randomDirection; // 將方向反向
                targetRotation = Quaternion.LookRotation(randomDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f); // 平滑旋轉
            }
            else
            {
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime); // 始終沿著物件的正面（Z軸）移動
            }
        }

        // 讓物件沿著隨機方向移動
        //transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime); // 始終沿著物件的正面（Z軸）移動

        //Vector3 nextPosition = transform.position + transform.forward * moveSpeed * Time.deltaTime;
        //Debug.Log("nextPosition: " + nextPosition);

    }

    private IEnumerator ChangeDirectionRoutine()
    {
        while (true)
        {
            // 每隔指定時間生成新的隨機方向
            yield return new WaitForSeconds(changeDirectionInterval);

            ChangeDirection();
        }
    }
    private void ChangeDirection()
    {
        // 隨機生成一個新的方向 (X, 0, Z)
        randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
    }

        // 檢查是否在邊界內
    private bool IsWithinBounds(Vector3 position)
    {
        return movementBounds.Contains(position);
    }

    // 難民死亡方法
    public void RefugeeDie()
    {
        isAlive = false;

        // 播放死亡動畫（假設動畫控制器有一個死亡觸發器 "Die"）
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Refugee_Die");

            // 確定動畫名稱與觸發器一致
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            float animationLength = stateInfo.length;

            // 延遲摧毀物件
            StartCoroutine(DisappearAfterAnimation(animationLength));
        }
        else
        {
            // 如果沒有動畫控制器，直接摧毀物件
            Destroy(gameObject);
        }
    }

    private IEnumerator DisappearAfterAnimation(float delay)
    {
        yield return new WaitForSeconds(delay); // 等待動畫播放完成
        Destroy(gameObject); // 摧毀物件
    }

}
