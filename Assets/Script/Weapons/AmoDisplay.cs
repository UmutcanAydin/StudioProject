using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmoDisplay : MonoBehaviour
{
    public GameObject amoTextUI;
    public int pistolCount;


    // Update is called once per frame
    void Update()
    {
        amoTextUI.GetComponent<Text>().text = "AMO:" + pistolCount;
        
    }
}
