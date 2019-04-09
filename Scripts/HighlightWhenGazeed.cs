using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;

public class HighlightWhenGazeed : MonoBehaviour,IFocusable
{
    


    void start()
    {
        this.gameObject.GetComponentInChildren<TextMesh>().fontSize = 48;
    }


    public void OnFocusEnter()
    {
        this.gameObject.GetComponentInChildren<TextMesh>().fontSize = 70;
    }



    public void OnFocusExit()
    {
        this.gameObject.GetComponentInChildren<TextMesh>().fontSize = 48;
    }



 

}
