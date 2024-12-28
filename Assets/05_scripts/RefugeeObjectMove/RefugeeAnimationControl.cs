using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RefugeeAnimationControl : MonoBehaviour
{
    public Animator Refugeeanimator;
    // Start is called before the first frame update
    void Start()
    {
        Refugeeanimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerDeathAnimation()
    {
        Refugeeanimator.SetTrigger("Die"); // 觸發死亡動畫
    }
}
