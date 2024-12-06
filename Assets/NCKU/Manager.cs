using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Manager : MonoBehaviour
{
    // public GameObject CarPrefab;
    public ReticleBehaviour Reticle;
    public DrivingSurfaceManager DrivingSurfaceManager;
    public GameObject[] ARObject; //AR顯示物件

    // public CarBehaviour Car;
    public GameObject spawnedObject;

    public GameObject 物件;

    void Start()
    {
        if (PlayerPrefs.HasKey("TrackedImageIndex"))
        {
            int trackedImageIndex = PlayerPrefs.GetInt("TrackedImageIndex");
            if (trackedImageIndex >= 0 && trackedImageIndex < ARObject.Length)
            {
                // 在空間中生成物件
                spawnedObject = Instantiate(ARObject[trackedImageIndex]);
                spawnedObject.transform.position = Vector3.zero; // 設定生成位置
                spawnedObject.transform.rotation = Quaternion.identity;

                Debug.Log($"成功生成物件，索引：{trackedImageIndex}");
            }
            else
            {
                Debug.LogWarning("無效的 AR 物件索引！");
            }
        }
        else
        {
            Debug.LogWarning("沒有儲存的 AR 物件索引！");
        }
    }

    private void Update()
    {
        //if (Car == null && WasTapped() && Reticle.CurrentPlane != null)
        //{
        //    var obj = GameObject.Instantiate(CarPrefab);
        //    Car = obj.GetComponent<CarBehaviour>();
        //    Car.Reticle = Reticle;
        //    Car.transform.position = Reticle.transform.position;
        //    DrivingSurfaceManager.LockPlane(Reticle.CurrentPlane);
        //}
    }

    private bool WasTapped()
    {
        if (Input.GetMouseButtonDown(0))
        {
            return true;
        }

        if (Input.touchCount == 0)
        {
            return false;
        }

        var touch = Input.GetTouch(0);
        if (touch.phase != TouchPhase.Began)
        {
            return false;
        }

        return true;
    }

    public void 建立物件()
    {
        if(Reticle.CurrentPlane != null)
        { 
            var obj = GameObject.Instantiate(物件);
            obj.transform.position = Reticle.transform.position;
            DrivingSurfaceManager.LockPlane(Reticle.CurrentPlane);
        }
    }
}
