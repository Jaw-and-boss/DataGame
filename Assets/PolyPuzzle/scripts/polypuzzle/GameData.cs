using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hitcode_tangram
{
    public class GameData : ScriptableObject
    {// Singleton<GameData> {

        // Use this for initialization

        public int nLink = 0; //check in game.When nlink = 0.All the lines linked,so win.
        public int levelPassed = 0;//how much level you passed
        public int cLevel = 0;//currect level
        public int bestScore = 0;//bestscore for level
        public int isSoundOn = 0;//whether game music is on
        public int isSfxOn = 0;//whether the game sound effect is on
        public static bool isTrial;//not used
        public static string lastWindow = "";//not used

        public int currentScene = 0;

        public int tipRemain = 0;//how much tip you remain
        public static bool isAds;

        public MainScript main;//the mainscript instance of the game
        public static int[] totalLevel = { 100, 100, 100, 50, 50 };//total levels,currently,we make things easier,only use 50 levels the same for each difficulty
        public List<List<int>> levelStates;
        public static int difficulty = 0;//easy,
        public int mode = 0;

        public List<int> levelPass;


        public int tipUsed = 0;

        public static GameData instance;
        public static GameData getInstance()
        {
            if (instance == null)
            {
                instance = ScriptableObject.CreateInstance<GameData>();
            }
            return instance;
        }

        public bool isWin = false;//check if win
        public bool isLock = false;//check if game ui can touch or lock and wait
        public bool isUILock = false;
        public string tickStartTime = "0";//game count down.
        public List<int> lvStar = new List<int>();//level stars you got for each level
        public bool isfail = false;//whether the game failed

        /// <summary>
        /// Always uses for initial or reset to start a new level.
        /// </summary>
        public void resetData()
        {
            isLock = false;
            isWin = false;
            isfail = false;
            isUILock = false;
            //		levelPassed = PlayerPrefs.GetInt ("levelPass", 0);
            //		Debug.Log ("levelpassed=" + levelPassed);
            tipRemain = PlayerPrefs.GetInt("tipRemain", 3);
            tickStartTime = PlayerPrefs.GetString("tipStart", "0");


            //game
            currentStartPoses = new List<Vector2>();
            polyBounds = new List<Vector2>();
            polyPosArr = new List<Vector2>();

            tipUsed = 0;
        }


        /// <summary>
        /// Gets the system laguage.
        /// </summary>
        /// <returns>The system laguage.</returns>
        public int GetSystemLaguage()
        {
            int returnValue = 0;
            switch (Application.systemLanguage)
            {
                case SystemLanguage.Chinese:
                    returnValue = 1;
                    break;
                case SystemLanguage.ChineseSimplified:
                    returnValue = 1;
                    break;
                case SystemLanguage.ChineseTraditional:
                    returnValue = 1;
                    break;
                default:
                    returnValue = 0;
                    break;

            }
            returnValue = 0;//test
            return returnValue;
        }















        JSONNode levelData;

        public void init()
        {
            //cLevel = 2;//test,set a level number if you want to test directly
            string tData = Datas.CreateInstance<Datas>().getData("Tangram")[cLevel];//level
            levelData = JSONArray.Parse(tData);

            //Debug.Log(tData);

            //get gridSize first;
            gridSizeX = levelData["b"][2]["x"];
            gridSizeY = levelData["b"][2]["y"];


            occupyiedGrids = new int[GameData.instance.gridSizeX, GameData.instance.gridSizeY];

            JSONNode cripsArr = (levelData["p"]);
            tangramPoints = new List<List<Vector2>>();
            tangramPositions = new List<Vector2>();
            for (int i = 0; i < cripsArr.Count; i++)
            {
                //geometory
                JSONNode tPolyNodes = cripsArr[i]["v"];//one poly points;
                                                                  //Debug.Log(tPolyNodes);
                List<Vector2> tPolyVecs = new List<Vector2>();
                for (int j = 0; j < tPolyNodes.Count; j++)
                {
                    Vector2 tp;
                    tp.x = tPolyNodes[j]["x"];
                    tp.y = tPolyNodes[j]["y"];


                    //fix some data error
                    if (j > 0)
                    {
                        if (!tp.Equals(tPolyVecs[tPolyVecs.Count - 1]))
                        {
                            tPolyVecs.Add(tp);
                        }
                    }
                    else
                    {
                        tPolyVecs.Add(tp);
                    }


                }

                tangramPoints.Add(tPolyVecs);
                polyPosArr.Add(new Vector2(-1, -1));//init grid pos;

                //position
                JSONNode tPolyPose = cripsArr[i]["o"];//one poly startpostion;



                Vector2 tp2;
                tp2.x = tPolyPose["x"];
                tp2.y = tPolyPose["y"];

                tangramPositions.Add(tp2);




            }
            return;





        }


        public string getLevel(int no)
        {

            return levelData[1]["levels"][no];
        }


        public int gridSizeX = 10;
        public int gridSizeY = 10;
        public List<List<Vector2>> tangramPoints;
        public List<Vector2> tangramPositions;
        public Vector2 cheesBoardSize;
        public Vector3[] cheesBoardCorners;
        public float frameWidth;//gameframe width;
        public float frameHeight;//gameframe height;
        public List<Vector2> polyPosArr;//current grid place of each polygon
        public int uvZoom = 3;//zoom uv to make it smaller and repeat;set it to 1 if you want the puzzle result exactly the same of the texture.

        public List<Vector2> polyBounds;//record bound for each polygon
        public List<Vector2> currentStartPoses;//polygon's current start positions
        public int[,] occupyiedGrids;


        public static float shrink = .15f;
    }


}



    