using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // �ޤJ�����޲z�R�W�Ŷ�

public class turnpage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void turnto_ar_page() 
    {
        SceneManager.LoadScene("ar_page");
    }
}
