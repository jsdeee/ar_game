using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Manager : MonoBehaviour
{
    public GameObject CarPrefab;
    public ReticleBehaviour Reticle;
    public DrivingSurfaceManager DrivingSurfaceManager;

    public CarBehaviour Car;

    public GameObject 物件;

    private void Update()
    {
        if (Car == null && WasTapped() && Reticle.CurrentPlane != null)
        {
            var obj = GameObject.Instantiate(CarPrefab);
            Car = obj.GetComponent<CarBehaviour>();
            Car.Reticle = Reticle;
            Car.transform.position = Reticle.transform.position;
            DrivingSurfaceManager.LockPlane(Reticle.CurrentPlane);
        }
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
