using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;


public class OnInputclickedTest : MonoBehaviour,IInputClickHandler {

    //public GameObject Prefab;

    // Use this for initialization
    void Start ()
    {
        //GameObject InputTest = Instantiate(Prefab) as GameObject;
        //InputManager.Instance.PushModalInputHandler(this.gameObject);
        Debug.Log("Color Change!");
	}

    public void OnInputClicked(InputClickedEventData eventData)
    {
        this.gameObject.GetComponent<MeshRenderer>().material.color = RandomColor();

    }

    public Color RandomColor()
    {
        float r = UnityEngine.Random.Range(0.0f,1.0f);
        float g = UnityEngine.Random.Range(0.0f, 1.0f);
        float b = UnityEngine.Random.Range(0.0f, 1.0f);
        Color color = new Color(r, g, b);
        return color;

    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}
