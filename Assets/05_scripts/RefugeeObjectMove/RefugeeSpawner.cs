using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefugeeSpawner : MonoBehaviour
{
    public GameObject refugeePrefab; // �������w�m����
    public Transform plane; // �����A�Ψӭ���ͦ��d��
    public int numberOfRefugees = 10; // �n�ͦ��������ƶq

    private List<GameObject> refugees = new List<GameObject>();

    private void Start()
    {
        SpawnRefugees();
    }

    private void SpawnRefugees()
    {
        // ������������
        MeshRenderer planeRenderer = plane.GetComponent<MeshRenderer>();
        Bounds planeBounds = planeRenderer.bounds;

        for (int i = 0; i < numberOfRefugees; i++)
        {
            // �H���ͦ��@�Ӧ�m
            Vector3 randomPosition = new Vector3(
                Random.Range(planeBounds.min.x, planeBounds.max.x),
                planeBounds.center.y,
                Random.Range(planeBounds.min.z, planeBounds.max.z)
            );

            // �ͦ�����
            GameObject refugee = Instantiate(refugeePrefab, randomPosition, Quaternion.identity);

            // ��l�����������ʱ���
            RefugeeControllor refugeeController = refugee.GetComponent<RefugeeControllor>();
            if (refugeeController != null)
            {
                refugeeController.Initialize(planeBounds); // �]�w�������
            }

            refugees.Add(refugee);
            //Debug.Log("planeBounds: " + planeBounds);
        }
    }
}