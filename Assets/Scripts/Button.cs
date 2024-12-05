using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AR按鈕()
    {
        SceneManager.LoadScene("AR_main"); //轉場到AR_main的場
    }

    public void 離開按鈕()
    {
        Application.Quit(); //關閉APP 
    }
}
