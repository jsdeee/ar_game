using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ReticleBehaviour : MonoBehaviour
{
    public GameObject PreviewObject; // 預覽的 3D 物件
    public GameObject SpawnedObject; // 真正生成的物件
    private bool isObjectCreated = false; // 確認是否已真正生成的物件
    // public GameObject Child;
    public DrivingSurfaceManager DrivingSurfaceManager;
    public ARPlane CurrentPlane;
    public GameObject[] ARObject; //AR顯示物件


    private int trackedImageIndex;
    public GameObject SpawnButton; // 按鈕用來建立物件
    public GameObject JoystickObject;  // 存放在Canvas中JoystickObject的物件
    public VirtualJoystick joystick; // 把joystick在 Canvas 中的虛擬搖桿腳本拉過來，之後會動態生成傳給生成物件

    // AR遊戲的場景
    public GameObject ARplanePrefab; // 在 Inspector 中手動拖入的平面預置物件

    private void Start()
    {
        // 設置按鈕初始狀態為隱藏
        SpawnButton.SetActive(false);
        JoystickObject.SetActive(false);

        if (PlayerPrefs.HasKey("TrackedImageIndex"))
        {
            trackedImageIndex = PlayerPrefs.GetInt("TrackedImageIndex");
            if (trackedImageIndex >= 0 && trackedImageIndex < ARObject.Length)
            {
                PreviewObject = Instantiate(ARObject[trackedImageIndex]);
                PreviewObject.SetActive(false); // 預設為隱藏

                // Child = ARObject[trackedImageIndex];
                Debug.Log($"成功生成物件，索引：{trackedImageIndex}");
            }           
        }
        else
        {
            PreviewObject = null;
            Debug.LogWarning("無效的 AR 物件索引！");
        }
        // Child = transform.GetChild(0).gameObject;

    }

    private void Update()
    {
        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        DrivingSurfaceManager.RaycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinBounds);
        CurrentPlane = null;
        ARRaycastHit? hit = null;

        if (hits.Count > 0)
        {
            var lockedPlane = DrivingSurfaceManager.LockedPlane;
            hit = lockedPlane == null
            ? hits[0]
            : hits.SingleOrDefault(x => x.trackableId == lockedPlane.trackableId);
        }

        if (hit.HasValue)
        {
            CurrentPlane = DrivingSurfaceManager.PlaneManager.GetPlane(hit.Value.trackableId);

            // 將準心移動到平面位置
            transform.position = hit.Value.pose.position;

            // 設置物件的方向與相機一致
            Quaternion cameraRotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);


            // 顯示預覽物件
            if (PreviewObject != null && isObjectCreated != true)
            {
                PreviewObject.SetActive(true);
                PreviewObject.transform.position = transform.position;
                PreviewObject.transform.rotation = cameraRotation; // 使用相機方向的旋轉
                // 顯示建立按鈕
                SpawnButton.SetActive(true);
            }

            // 顯示建立按鈕
            // SpawnButton.SetActive(true);
        }
        else
        {
            // 如果沒有檢測到平面，隱藏預覽物件和按鈕
            if (PreviewObject != null) PreviewObject.SetActive(false);
            SpawnButton.SetActive(false);
        }

        // Child.SetActive(CurrentPlane != null);
    }


    public void createObject()
    {
        JoystickObject.SetActive(true);
        if (CurrentPlane != null && PreviewObject != null)
        {
            if (joystick == null)
            {
                Debug.LogError("Prefab 或 Joystick 未設置！");
                return;
            }

            // 生成一個空物件
            GameObject parentObject = new GameObject("ARParentObject");
            parentObject.transform.position = PreviewObject.transform.position; // 設置位置與 PreviewObject 相同
            parentObject.transform.rotation = PreviewObject.transform.rotation; // 設置方向與 PreviewObject 相同

            // 在空物件下生成 SpawnedObject
            SpawnedObject = Instantiate(PreviewObject); // 從 PreviewObject 生成新的物件
            SpawnedObject.transform.SetParent(parentObject.transform); // 設置父物件為剛生成的空物件

            // 將 SpawnedObject 的本地位置設置為 (0, 0, 0)
            SpawnedObject.transform.localPosition = Vector3.zero;
            SpawnedObject.transform.localRotation = Quaternion.identity; // 重置本地旋轉
            SpawnedObject.SetActive(true);

            // 直接生成平面
            if (ARplanePrefab != null) // 確保已經在 Inspector 設置了平面預置物件
            {
                GameObject plane = Instantiate(ARplanePrefab, parentObject.transform); // 生成平面，並設置父物件
                plane.transform.localPosition = Vector3.zero; // 與 parentObject 對齊
                plane.transform.localRotation = Quaternion.identity; // 重置旋轉
            }
            else
            {
                Debug.LogError("Plane Prefab 未在 Inspector 設置！");
            }


            DrivingSurfaceManager.LockPlane(CurrentPlane); // 鎖定當前平面

            // 動態設定虛擬搖桿
            PlayerMovement playerMovement = SpawnedObject.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.SetVirtualJoystick(joystick); // 將 joystick 傳遞到物件
            }

            // 設置第一個子物件的虛擬搖桿
            if (SpawnedObject.transform.childCount > 0)
            {
                Transform firstChild = SpawnedObject.transform.GetChild(0);
                MyAnimatorController animatorController = firstChild.GetComponent<MyAnimatorController>();
                if (animatorController != null)
                {
                    animatorController.SetVirtualJoystick(joystick);
                    Debug.Log("已將虛擬搖桿傳遞給第一個子物件的 MyAnimatorController");
                }
            }

            isObjectCreated = true;

            // 隱藏預覽物件和按鈕
            PreviewObject.SetActive(false);
            SpawnButton.SetActive(false);
        }
    }

}
