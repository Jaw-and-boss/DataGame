using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Remain : MonoBehaviour
{
    public UnityEvent triggerEnter;
    public Button InteractButton;

    int remainid;
    string questType = "puzzle";

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Entered");
            triggerEnter.Invoke();
            InteractButton.interactable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Exited");
            InteractButton.gameObject.SetActive(false);
            InteractButton.interactable = false;
        }

    }
}
