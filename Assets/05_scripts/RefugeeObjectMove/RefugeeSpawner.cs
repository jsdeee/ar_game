using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefugeeSpawner : MonoBehaviour
{
    public GameObject refugeePrefab; // 難民的預置物件
    public Transform plane; // 平面，用來限制生成範圍
    public int numberOfRefugees = 10; // 要生成的難民數量

    private List<GameObject> refugees = new List<GameObject>();

    private void Start()
    {
        SpawnRefugees();
    }

    private void SpawnRefugees()
    {
        // 獲取平面的邊界
        MeshRenderer planeRenderer = plane.GetComponent<MeshRenderer>();
        Bounds planeBounds = planeRenderer.bounds;

        for (int i = 0; i < numberOfRefugees; i++)
        {
            // 隨機生成一個位置
            Vector3 randomPosition = new Vector3(
                Random.Range(planeBounds.min.x, planeBounds.max.x),
                planeBounds.center.y,
                Random.Range(planeBounds.min.z, planeBounds.max.z)
            );

            // 生成難民
            GameObject refugee = Instantiate(refugeePrefab, randomPosition, Quaternion.identity);

            // 初始化難民的移動控制
            RefugeeControllor refugeeController = refugee.GetComponent<RefugeeControllor>();
            if (refugeeController != null)
            {
                refugeeController.Initialize(planeBounds); // 設定移動邊界
            }

            refugees.Add(refugee);
            //Debug.Log("planeBounds: " + planeBounds);
        }
    }
}