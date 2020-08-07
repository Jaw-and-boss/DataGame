using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hitcode_tangram
{
    public class TestGame : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            GameManager.getInstance().init();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnMouseDown()
        {
            GetComponent<MeshRenderer>().GetComponent<Renderer>().material.color = Color.white;

            //init game
            Tangram tg = GameObject.Find("Tangram").GetComponent<Tangram>();
            tg.onlyClear();

            //set difficulty and level
            GameData.difficulty = 0;
            GameData.instance.cLevel = Random.Range(0, GameData.totalLevel[GameData.difficulty]);

            //start game;
            tg.initSingleMode();

        }

        void polyPuzzleWin()
        {
            GetComponent<MeshRenderer>().GetComponent<Renderer>().material.color = Color.red;
        }
    }
}