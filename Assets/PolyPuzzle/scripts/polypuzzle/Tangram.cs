using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
namespace Hitcode_tangram
{
    public class Tangram : MonoBehaviour
    {

        // Use this for initialization
        GameObject crispTemplate;
        void Start()
        {
            GameManager.getInstance().init();
            GameData.getInstance().resetData();

            crispTemplate = GameObject.Find("crisp");

            StartCoroutine("nextframe");
            //initGame();
        }

        IEnumerator nextframe()
        {
            yield return new WaitForEndOfFrame();

        }
        [HideInInspector]
        public List<GameObject> polygons;
        List<Vector3> polygonPosition;
        GameObject placerect;
        GameObject crispContainer;

        public void initSingleMode()
        {
            GameData.getInstance().resetData();
            crispTemplate = GameObject.Find("crisp");
            StartCoroutine("nextframe");
            initGame();
        }

       
        public void initGame()
        {

            GameData.instance.init();
            GameObject btnTip = GameObject.Find("btnTip");
            if (btnTip) 
            {
                btnTip.GetComponent<Button>().interactable = true;
            }

            placerect = GameObject.Find("placeRect");
            // Create Vector2 vertices
            GameObject chessboard = GameObject.Find("chessboard");



            Vector3 tmin = Vector3.zero; 
            Vector3 tmax = Vector3.zero;
           
            //game mode
            if (chessboard.GetComponent<Image>()!=null)
            {
                tmin = chessboard.GetComponent<Image>().rectTransform.rect.min;
                tmax = chessboard.GetComponent<Image>().rectTransform.rect.max;
                GameData.getInstance().frameWidth = tmax.x - tmin.x;
                GameData.getInstance().frameHeight = tmax.y - tmin.y;
            }
           


          
            polygons = new List<GameObject>();

            polygonPosition = new List<Vector3>();
         
            crispContainer = GameObject.Find("crispContainer");
            GameObject pContainer = GameObject.Find("pContainer");
            //Camera UICam = GameObject.Find("Back_UI_Cam").GetComponent<Camera>();

           
    

            Vector3[] corners = { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };
            chessboard.GetComponent<RectTransform>().GetWorldCorners(corners);

            float tw = Mathf.Abs((corners[0] - corners[3]).x);
            float th = Mathf.Abs((corners[0] - corners[2]).y);

            GameData.instance.cheesBoardCorners = corners;

            GameData.instance.cheesBoardSize = new Vector2(tw, th);

            //get a random texture

            int tTextureNo = Random.Range(0, 6);//got 5 textures in materials folder total
            //tTextureNo = 6; GameData.instance.uvZoom = 1;//test
            int maxCol = 7;//how many pieces your want on a row mostly
            int row = Mathf.CeilToInt(GameData.instance.tangramPoints.Count / (float)maxCol);
            int col = row == 1 ? GameData.instance.tangramPoints.Count : maxCol;

            //print(GameData.instance.tangramPoints.Count+"  "+row+"  "+col);
            for (int i = 0; i < GameData.instance.tangramPoints.Count; i++)
            {
                List<Vector2> tpolys = GameData.instance.tangramPoints[i];

                GameObject tpolygon = Instantiate(crispTemplate) as GameObject;
                tpolygon.name = i.ToString();
                tpolygon.transform.parent = crispContainer.transform;
                tpolygon.transform.position = Vector3.zero;
                tpolygon.transform.localScale = Vector3.one;

                tpolygon.GetComponent<PolygonGenerator>().genMesh(tpolys, GameData.instance.tangramPositions[i]);

                

                List<Vector2> tgrids = GameData.instance.tangramPoints[i];
                int tgridminX = 0, tgridmaxX = 0, tgridminY = 0, tgridmaxY = 0;

                for (int ii = 0; ii < tgrids.Count; ii++)
                {
                    if (tgridminX >= tgrids[ii].x)
                    {
                        tgridminX = (int)tgrids[ii].x;
                    }

                    if (tgridminY >= tgrids[ii].y)
                    {
                        tgridminY = (int)tgrids[ii].y;
                    }

                    if (tgridmaxX <= tgrids[ii].x)
                    {
                        tgridmaxX = (int)tgrids[ii].x;
                    }

                    if (tgridmaxY <= tgrids[ii].y)
                    {
                        tgridmaxY = (int)tgrids[ii].y;
                    }
                }
                tpolygon.GetComponent<TouchPoly>().myId = i ;//give the id order of current piece

                tpolygon.GetComponent<TouchPoly>().myGrids = new List<Vector2>();
                tpolygon.GetComponent<TouchPoly>().myGrids.Add(new Vector2(tgridminX, tgridminY));
                tpolygon.GetComponent<TouchPoly>().myGrids.Add(new Vector2(tgridmaxX, tgridmaxY));
                //print("grids" + (tpolygon.GetComponent<TouchPoly>().myGrids[1]- tpolygon.GetComponent<TouchPoly>().myGrids[0]));

               
               


                tw = GameData.instance.cheesBoardSize.x;
                th = GameData.instance.cheesBoardSize.y;
                //

                //the  des postion
                float toffsetX = GameData.instance.tangramPositions[i].x * tw / GameData.instance.gridSizeX;
                float toffsetY = GameData.instance.tangramPositions[i].y * th / GameData.instance.gridSizeY;

                Vector3 toffset = new Vector3(toffsetX, toffsetY, 0);
                //Vector3 toffset = GameData.instance.tangramPositions[i];

                


               //set dest postion
               TouchPoly tpoly = tpolygon.GetComponent<TouchPoly>();
                tpoly.desPos = toffset - new Vector3(tw / 2, th / 2) + chessboard.transform.position;
                tpoly.desPos = new Vector3(tpolygon.transform.position.x, tpolygon.transform.position.y, 0);
                tpoly.desGrid = GameData.instance.tangramPositions[i];




                pContainer.GetComponent<RectTransform>().GetWorldCorners(corners);
                float pw = Mathf.Abs((corners[0] - corners[3]).x);
                float ph = Mathf.Abs((corners[0] - corners[2]).y);
                //float tgap = .7f;
                //start positon
                

                float xgap = .2f;
                
                float tsize = (pw - xgap * ((float)col+1f))/(float)(col);

                float tShrink = 3f;

                GameData.shrink = tsize / tShrink;

                tpolygon.transform.localScale = new Vector3(GameData.shrink, GameData.shrink, 1f);

                float tpolyWidth = tpolygon.GetComponent<MeshRenderer>().bounds.size.x;
                float tpolyHeight = tpolygon.GetComponent<MeshRenderer>().bounds.size.y;


                float ygap = (float)ph / (row + 1f);
                tpolygon.transform.position = pContainer.transform.position - new Vector3(pw / 2f, ph / 2f, 0)  + new Vector3(xgap, 0, 0) + new Vector3(tsize/2,0); ;
                int myXIndex = (i % col);
                int myYIndex = Mathf.FloorToInt(i / col);


                tpolygon.transform.position += new Vector3(myXIndex * xgap + myXIndex * tsize - tpolyWidth / 2f, + (row - myYIndex) * ygap - tpolyHeight / 2f, 0);



                //set a  larger collider for drag
                
                //tpoly.mysize = tpoly.GetComponent<MeshCollider>().bounds.size;
                BoxCollider tbox = tpolygon.AddComponent<BoxCollider>();
                tbox.enabled = false;
                float tboxMax = Mathf.Max(tbox.size.x, tbox.size.y);
                //print("mysize" + tpoly.mysize);




                //record startPostion
                polygonPosition.Add(tpolygon.transform.position);
                tpoly.oriPos = tpolygon.transform.position;

                Renderer renderer = tpolygon.GetComponent<MeshRenderer>().GetComponent<Renderer>();

                
                renderer.material = Resources.Load<Material>("Tangram/Materials/tex"+tTextureNo);


                renderer.material.shader = Shader.Find("Ciconia Studio/Double Sided/Emissive/Diffuse");
                if (GameData.instance.uvZoom == 1)//picture puzzle mode
                {
                    renderer.material.SetColor("_Color", Color.white);
                }
                else//color puzzle mode
                {
                    renderer.material.SetColor("_Color", Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f));
                }

                //renderer.material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
                //renderer.material.mainTexture = Resources.Load("tex01") as Texture;
                //renderer.material.shader = Shader.Find("Mobile/Particles/Alpha Blended");
                polygons.Add(tpolygon);

                tpolygon.GetComponent<BoxCollider>().size = new Vector3(tboxMax, tboxMax, 1);


                //=================set game time datas====================
                //store bound information for each polygon one by one
                GameData.instance.polyBounds.Add(new Vector2(tgridmaxX - tgridminX, tgridmaxY - tgridminY));
                //init polyon postion to another list,would change value by play the game.
                GameData.instance.currentStartPoses.Add(new Vector2(-1,-1));//init an illeage value as they are not on board yet
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }


        bool canFade = false;
        int n = 0;
        /// <summary>
        /// as reset game data clear the game instancely.
        /// Sometimes,do clear in another way(for example do some animation)
        /// </summary>
        public void clear(bool restart = false)
        {
            if (crispContainer == null) return;
            canFade = true;
            GameData.instance.isLock = true;
            n = crispContainer.transform.childCount;
            foreach (Transform tobj in crispContainer.transform)
            {
                tobj.DOScale(Vector2.zero, .4f).SetUpdate(true).OnComplete(() =>
                {
                    Destroy(tobj.gameObject);
                    n--;
                    if (n <= 0)
                    {

                        placerect.transform.position = new Vector2(0, -1000);
                        placerect.GetComponent<SpriteRenderer>().enabled = false;
                        GameData.instance.resetData();
                        GameData.instance.isLock = false;
                        if (restart)
                        {
                            initGame();
                        }
                    }

                });

            }

        }

        public void onlyClear()
        {
            if (crispContainer != null)
            {
                foreach (Transform tobj in crispContainer.transform)
                {
                    if(tobj !=null)
                    Destroy(tobj.gameObject);
                }
            }
        }


        public bool checkWin()
        {
            bool isWin = false;
            int nCorrect = 0;
            for (int i = 0; i < GameData.instance.tangramPositions.Count; i++)
            {
                if (GameData.instance.currentStartPoses[i] == GameData.instance.tangramPositions[i])
                {
                    nCorrect++;
                }
            }
            if (nCorrect == GameData.instance.tangramPositions.Count)
            {
                isWin = true;
            }

            if (isWin)
            {
                foreach(Transform tg in crispContainer.transform)
                {
                    //tg.gameObject.GetComponent<MeshCollider>().enabled = false;
                    tg.gameObject.transform.Find("Colliders").gameObject.SetActive(false);
                }
                if (GameData.instance.main != null)//game mode
                {
                    GameData.instance.main.gameWin();
                }
                else//widget mode
                {
                    GameObject.Find("3Dscene").BroadcastMessage("polyPuzzleWin");
                }
            }

            return isWin;
        }

    }
}
