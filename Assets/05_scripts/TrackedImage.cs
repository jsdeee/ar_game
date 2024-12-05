using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class _TrackedImage : MonoBehaviour
{
    public ARTrackedImageManager ARTrackedImageManager;
    public GameObject[] ARObject; //AR顯示物件
    private int TrackedImageCount;//辨識圖掃過計數

    public Text infotext;

    void Start()
    {
        infotext.text = "掃描物件";
    }

    void OnEnable() => ARTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    void OnDisable() => ARTrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var newImage in eventArgs.added) //當此辨識圖第一次被偵測到時，執行方法(執行一次)
        {
            TrackedImageCount++;
           
        }

        foreach (var updatedImage in eventArgs.updated)
        {
            if (updatedImage.trackingState == TrackingState.Tracking)
            {
                for (int i = 0; i < ARTrackedImageManager.referenceLibrary.count; i++)
                {
                    int n = i;
                    if (updatedImage.referenceImage.name == ARTrackedImageManager.referenceLibrary[n].name)
                    {
                        infotext.text = "已掃描到物件";
                        ARObject[n].transform.position = updatedImage.transform.position;           //AR顯示物件的座標與錨點一致
                        ARObject[n].transform.rotation = updatedImage.transform.localRotation;      //AR顯示物件的旋轉軸與錨點一致
                        ARObject[n].SetActive(true);

                    }
                }
            }
            else
            {
                for (int i = 0; i < ARTrackedImageManager.referenceLibrary.count; i++)
                {
                    int n = i;
                    if (updatedImage.referenceImage.name == ARTrackedImageManager.referenceLibrary[n].name)
                    {
                        ARObject[n].SetActive(false);            //AR顯示物件不顯示
                    }
                }
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
}
