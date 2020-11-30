using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TutorialManager : MonoBehaviour
{
    public bool t1_playermove = false;
    public bool t2_playerswipe = false;
    public bool t3_arrive = false;
    public bool t4_interact = false;

    public GameObject t1;
    public GameObject t2;
    public GameObject t3;
    public GameObject t4;

    public Joystick playerMovement;
    public Joystick playerSwipe;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(T1());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void T3End()
    {
        t3_arrive = true;
    }

    private IEnumerator T1()
    {
        t1.SetActive(true);
        while (true)
        {
            if (playerMovement.Horizontal != 0f || playerMovement.Vertical != 0f)
            {
                t1_playermove = true;
                t1.SetActive(false);
                break;
            }
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine(T2());
        yield return new WaitForSeconds(1f);
    }

    private IEnumerator T2()
    {
        t2.SetActive(true);
        while (true)
        {
            if (playerSwipe.Horizontal != 0f || playerSwipe.Vertical != 0f)
            {
                t2_playerswipe = true;
                t2.SetActive(false);
                break;
            }
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine(T3());
        yield return new WaitForSeconds(1f);
    }

    private IEnumerator T3()
    {
        t3.SetActive(true);
        while (true)
        {
            if (t3_arrive)
            {
                t3.SetActive(false);
                break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(T4());
        yield return new WaitForSeconds(1f);
    }

    private IEnumerator T4()
    {
        t4.SetActive(true);
        while (true)
        {
            if (t4_interact)
            {
                t4.SetActive(false);
                break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(1f);
    }
}
