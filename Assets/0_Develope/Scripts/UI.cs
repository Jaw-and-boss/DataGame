using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public GameObject Canvas;
    public bool canvasOn;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void On(GameObject obj)
    {
        obj.SetActive(true);
        Debug.Log(obj.name + " is now on.");
    }
    public void Off(GameObject obj)
    {
        obj.SetActive(false);
        Debug.Log(obj.name + " is now off.");
    }


}
