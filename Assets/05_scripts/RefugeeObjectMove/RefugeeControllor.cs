using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefugeeControllor : MonoBehaviour
{
    public float moveSpeed = 5f; // ���ʳt��
    public float changeDirectionInterval = 0.1f; // �C�j�X����ܤ�V

    private Vector3 randomDirection; // �H����V
    private Bounds movementBounds; // ���������
    private bool isAlive = true; // �����O�_�s��
    // ��l�Ƥ�k�A�Ω�]�w���ʽd��
    public void Initialize(Bounds bounds)
    {
        // ��l���H����V
        ChangeDirection();
        movementBounds = bounds; // �]�w�������
        Debug.Log("planeBounds: " + movementBounds);
    }

    private void Start()
    {
        // �}�l�H������
        StartCoroutine(ChangeDirectionRoutine());
    }

    private void Update()
    {
        if (!isAlive) return; // �p�G�w���`�A�����

        // �p�G�H����V�����s�A���ફ��¦V���ʤ�V
        if (randomDirection != Vector3.zero)
        {
            // �p��¦V������
            Quaternion targetRotation = Quaternion.LookRotation(randomDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f); // ���Ʊ���
            Vector3 nextPosition = transform.position + transform.forward * moveSpeed * Time.deltaTime;
            if (!IsWithinBounds(nextPosition))
            {
                // ���ܤ�V�]����180�ס^
                randomDirection = -randomDirection; // �N��V�ϦV
                targetRotation = Quaternion.LookRotation(randomDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f); // ���Ʊ���
            }
            else
            {
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime); // �l�תu�۪��󪺥����]Z�b�^����
            }
        }

        // ������u���H����V����
        //transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime); // �l�תu�۪��󪺥����]Z�b�^����

        //Vector3 nextPosition = transform.position + transform.forward * moveSpeed * Time.deltaTime;
        //Debug.Log("nextPosition: " + nextPosition);

    }

    private IEnumerator ChangeDirectionRoutine()
    {
        while (true)
        {
            // �C�j���w�ɶ��ͦ��s���H����V
            yield return new WaitForSeconds(changeDirectionInterval);

            ChangeDirection();
        }
    }
    private void ChangeDirection()
    {
        // �H���ͦ��@�ӷs����V (X, 0, Z)
        randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
    }

        // �ˬd�O�_�b��ɤ�
    private bool IsWithinBounds(Vector3 position)
    {
        return movementBounds.Contains(position);
    }

    // �������`��k
    public void RefugeeDie()
    {
        isAlive = false;

        // ���񦺤`�ʵe�]���]�ʵe������@�Ӧ��`Ĳ�o�� "Die"�^
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Refugee_Die");

            // �T�w�ʵe�W�ٻPĲ�o���@�P
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            float animationLength = stateInfo.length;

            // ����R������
            StartCoroutine(DisappearAfterAnimation(animationLength));
        }
        else
        {
            // �p�G�S���ʵe����A�����R������
            Destroy(gameObject);
        }
    }

    private IEnumerator DisappearAfterAnimation(float delay)
    {
        yield return new WaitForSeconds(delay); // ���ݰʵe���񧹦�
        Destroy(gameObject); // �R������
    }

}
