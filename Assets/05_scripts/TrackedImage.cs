using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class _TrackedImage : MonoBehaviour
{
    public ARTrackedImageManager ARTrackedImageManager;
    public GameObject[] ARObject; //AR��ܪ���
    private int TrackedImageCount;//���ѹϱ��L�p��

    public Text infotext;

    void Start()
    {
        infotext.text = "���y����";
    }

    void OnEnable() => ARTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    void OnDisable() => ARTrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var newImage in eventArgs.added) //�����ѹϲĤ@���Q������ɡA�����k(����@��)
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
                        infotext.text = "�w���y�쪫��";
                        ARObject[n].transform.position = updatedImage.transform.position;           //AR��ܪ��󪺮y�лP���I�@�P
                        ARObject[n].transform.rotation = updatedImage.transform.localRotation;      //AR��ܪ��󪺱���b�P���I�@�P
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
                        ARObject[n].SetActive(false);            //AR��ܪ������
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
