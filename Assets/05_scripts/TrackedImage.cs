using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.SceneManagement; // 引入場景管理命名空間

public class _TrackedImage : MonoBehaviour
{
    public ARTrackedImageManager ARTrackedImageManager;
    public GameObject[] ARObject; //AR顯示物件
    private int TrackedImageCount;//辨識圖掃過計數

    public Text infotext;

    private GameObject currentObject = null; // 當前顯示的物件
    public int trackedImageCount = -1; // 紀錄掃描到的物件編號
    private bool isObjectDisplayed = false; // 物件是否已顯示

    public GameObject confirmbtn;
    public GameObject cancelbtn;
    public GameObject canvascurrentlyobject;

    private Vector2 rotationVelocity; // 旋轉速度
    private float rotationDamping = 0.55f; // 旋轉阻尼，控制減速效果

    void Start()
    {
        canvascurrentlyobject.SetActive(false);
        confirmbtn.SetActive(false); 
        cancelbtn.SetActive(false);
        infotext.text = "掃描物件";
    }

    void OnEnable() => ARTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    void OnDisable() => ARTrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;

    void Update()
    {
        if (isObjectDisplayed && currentObject != null)
        {
            // 更新物件位置到螢幕中心點
            Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 1.0f);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenCenter);
            currentObject.transform.position = worldPosition;

            // 處理旋轉功能
            HandleObjectRotation();
            ApplyRotationInertia();
        }
    }


    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var newImage in eventArgs.added) //當此辨識圖第一次被偵測到時，執行方法(執行一次)
        {
            for (int i = 0; i < ARTrackedImageManager.referenceLibrary.count; i++)
            {
                int n = i;
                if (newImage.referenceImage.name == ARTrackedImageManager.referenceLibrary[n].name)
                {
                    trackedImageCount = i;

                    // 顯示掃描物件到螢幕中間
                    if (currentObject == null)
                    {
                        canvascurrentlyobject.SetActive(true);
                        currentObject = Instantiate(ARObject[trackedImageCount]);
                        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 1.0f);
                        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenCenter);
                        currentObject.transform.position = worldPosition;
                        currentObject.transform.rotation = Quaternion.identity;
                        
                        infotext.text = "已掃描到物件，請旋轉物件！";
                        confirmbtn.SetActive(true);
                        cancelbtn.SetActive(true);

                        isObjectDisplayed = true; // 開啟旋轉功能
                    }
                }
            }
           
        }

        //foreach (var updatedImage in eventArgs.updated)
        //{
        //    if (updatedImage.trackingState == TrackingState.Tracking)
        //    {
        //        for (int i = 0; i < ARTrackedImageManager.referenceLibrary.count; i++)
        //        {
        //            int n = i;
        //            if (updatedImage.referenceImage.name == ARTrackedImageManager.referenceLibrary[n].name)
        //            {
        //                infotext.text = "已掃描到物件";
        //                ARObject[n].transform.position = updatedImage.transform.position;           //AR顯示物件的座標與錨點一致
        //                ARObject[n].transform.rotation = updatedImage.transform.localRotation;      //AR顯示物件的旋轉軸與錨點一致
        //                ARObject[n].SetActive(true);

        //            }
        //        }
        //    }
        //    else
        //    {
        //        for (int i = 0; i < ARTrackedImageManager.referenceLibrary.count; i++)
        //        {
        //            int n = i;
        //            if (updatedImage.referenceImage.name == ARTrackedImageManager.referenceLibrary[n].name)
        //            {
        //                ARObject[n].SetActive(false);            //AR顯示物件不顯示
        //            }
        //        }
        //    }
        //}

      
    }

    private void HandleObjectRotation()
    {
        // 检查是否有触控操作
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // 检测手指滑动
            if (touch.phase == TouchPhase.Moved)
            {
                float rotationSpeed = 1.5f; // 旋转速度系数
                float deltaX = touch.deltaPosition.x * rotationSpeed; // X轴滑动量
                // float deltaY = touch.deltaPosition.y * rotationSpeed; // Y轴滑动量

                // 设置旋转速度，加速效果
                rotationVelocity.x += deltaX; // 根据X方向滑动增量更新X轴旋转速度
                // rotationVelocity.y += deltaY; // 根据Y方向滑动增量更新Y轴旋转速度
            }
        }

        // 持续旋转物件
        if (currentObject != null)
        {
            // 仅绕物件本身的 Y 轴旋转
            currentObject.transform.Rotate(Vector3.up, -rotationVelocity.x, Space.Self);
            // 仅绕物件本身的 X 轴旋转
            currentObject.transform.Rotate(Vector3.right, rotationVelocity.y, Space.Self);

            // 实现减速（阻尼效果）
            rotationVelocity.x *= 0.95f; // 减少X轴速度以模拟摩擦阻力
            rotationVelocity.y *= 0.95f; // 减少Y轴速度以模拟摩擦阻力

            // 当旋转速度非常小时停止旋转
            if (Mathf.Abs(rotationVelocity.x) < 0.01f)
            {
                rotationVelocity.x = 0f;
            }
            if (Mathf.Abs(rotationVelocity.y) < 0.01f)
            {
                rotationVelocity.y = 0f;
            }
        }
    }

    private void ApplyRotationInertia()
    {
        if (currentObject != null)
        {
            // 持續應用旋轉速度
            currentObject.transform.Rotate(Vector3.up, -rotationVelocity.x, Space.Self);
            currentObject.transform.Rotate(Vector3.right, rotationVelocity.y, Space.Self);

            // 減少旋轉速度
            rotationVelocity *= rotationDamping;

            // 當速度非常小時，停止旋轉
            if (rotationVelocity.magnitude < 0.01f)
            {
                rotationVelocity = Vector2.zero;
            }
        }
    }



    public void _StopTracked()
    {
        ARTrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;

        for (int i = 0; i < ARObject.Length; i++)
        {
            int n = i;
            ARObject[n].SetActive(false);
        }
    }

    public void _RestartTracked()
    {
        ARTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;

        for (int i = 0; i < ARObject.Length; i++)
        {
            int n = i;
            ARObject[n].SetActive(false);
        }
    }
    public void confirmbtn_fuc()
    {
        ARTrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
        SceneManager.LoadScene("ar_page02");
    }

    public void cancelbtn_fuc()
    {
        // 销毁当前显示的物件
        if (currentObject != null)
        {
            Destroy(currentObject);
        }

        // 重置状态
        currentObject = null;
        trackedImageCount = -1;
        isObjectDisplayed = false;

        // 隐藏 UI 和物件容器
        canvascurrentlyobject.SetActive(false);
        cancelbtn.SetActive(false);
        confirmbtn.SetActive(false);

        // 确保 ARTrackedImageManager 正确订阅事件
        if (ARTrackedImageManager != null)
        {
            // 确保 ARTrackedImageManager 启用
            if (!ARTrackedImageManager.enabled)
            {
                ARTrackedImageManager.enabled = true;
            }

            // 确保重新订阅事件
            ARTrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
            ARTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
        }

        // 更新信息
        infotext.text = "請繼續掃描物件..";
    }





}
