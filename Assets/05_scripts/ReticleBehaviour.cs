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
    // public GameObject Child;
    public DrivingSurfaceManager DrivingSurfaceManager;
    public ARPlane CurrentPlane;
    public GameObject[] ARObject; //AR顯示物件


    private int trackedImageIndex;
    public GameObject SpawnButton; // 按鈕用來建立物件
    // public GameObject JoystickObject;
    public VirtualJoystick joystick; // 在 Canvas 中的虛擬搖桿

    private void Start()
    {
        // 設置按鈕初始狀態為隱藏
        SpawnButton.SetActive(false);
        //JoystickObject.SetActive(false);

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
            if (PreviewObject != null)
            {
                PreviewObject.SetActive(true);
                PreviewObject.transform.position = transform.position;
                PreviewObject.transform.rotation = cameraRotation; // 使用相機方向的旋轉
            }

            // 顯示建立按鈕
            SpawnButton.SetActive(true);
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
        if (CurrentPlane != null && PreviewObject != null)
        {
            if (joystick == null)
            {
                Debug.LogError("Prefab 或 Joystick 未設置！");
                return;
            }
            // var obj = GameObject.Instantiate(ARObject[trackedImageIndex]);
            // obj.transform.position = transform.position;
            // 真正生成物件
            SpawnedObject = Instantiate(PreviewObject, transform.position, PreviewObject.transform.rotation); // 保留預覽物件的旋轉
            SpawnedObject.SetActive(true);
            DrivingSurfaceManager.LockPlane(CurrentPlane);

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


            // 隱藏預覽物件和按鈕
            PreviewObject.SetActive(false);
            SpawnButton.SetActive(false);
            // JoystickObject.SetActive(true);
        }
    }

}
