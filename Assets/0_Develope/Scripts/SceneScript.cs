using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneScript : MonoBehaviour
{
    public GameObject Canvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadPuzzle()
    {
        Canvas.SetActive(false);
        SceneManager.LoadScene("Puzzle", LoadSceneMode.Additive);
    }
}
