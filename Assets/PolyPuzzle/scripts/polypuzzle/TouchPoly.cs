using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//[RequireComponent(typeof(MeshCollider))]

namespace Hitcode_tangram
{
    public class TouchPoly : MonoBehaviour
    {

        private Vector3 screenPoint;
        private Vector3 offset;
        private Camera cam;

        Tangram tangram;
        public Vector2 oriPos;
        public Vector2 desPos;
        public Vector2 desGrid;
        public Vector2 mysize = Vector2.zero;
        public List<Vector2> myGrids;//grids occupied
        public int myId;

        GameObject placerect;
        Bounds placeRectBound;
        GameObject chessBoard;
        private void Start()
        {
            tangram = GameObject.Find("Tangram").GetComponent<Tangram>();
            placerect = GameObject.Find("placeRect");
            chessBoard = GameObject.Find("chessboard");

           


            StartCoroutine("waitaframe");
        }

        IEnumerator waitaframe()
        {
            yield return new WaitForEndOfFrame();
            placeRectBound = placerect.GetComponent<SpriteRenderer>().sprite.bounds;

        }
        void OnMouseDown()
        {
          
            if (GameData.instance.isUILock) return;
            cam = GameObject.Find("gameCam").GetComponent<Camera>();
            //cam = GameObject.Find("Back_UI_Cam").GetComponent<Camera>();
            screenPoint = cam.WorldToScreenPoint(gameObject.transform.position);

            offset = gameObject.transform.position - cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
            Vector3 toffset2 = Vector3.zero;
            if (transform.localScale.x < 1f)
            {
                transform.localScale = Vector3.one;
                toffset2 = new Vector3(-mysize.x / (GameData.shrink) / 2, 0, 0);

                offset += toffset2;

                
            }


            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -.02f);
            gameObject.transform.Translate(toffset2);



            getCurrentGrid(gameObject);

        }

        void OnMouseDrag()
        {
            if (GameData.instance.isUILock) return;
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

            Vector3 curPosition = cam.ScreenToWorldPoint(curScreenPoint) + offset;
            transform.position = new Vector3(curPosition.x, curPosition.y, -.02f);

            getCurrentGrid(gameObject);
        }

        private void OnMouseUp()
        {

            if (GameData.instance.isUILock) return;
            //placerect.SetActive(false);
            placerect.GetComponent<SpriteRenderer>().enabled = false;

            if (!canplace)
            {
                transform.localScale = new Vector3(GameData.shrink, GameData.shrink, 1);
                transform.position = oriPos;
                GetComponent<BoxCollider>().enabled = true;
                placed = false;

                //reset poly positon to illeague postion(bottom)
                GameData.instance.currentStartPoses[myId] = new Vector2(-1, -1);
            }
            else
            {



                bool tisOccupied = false;

                tisOccupied = checkIntersection();//the most complicate algorithm for polygon intersection dectection

                
                //setGrid(myId);


                //can place down
                if (!tisOccupied)
                {
                    transform.position = placerect.transform.position;
                    if(transform.GetComponent<BoxCollider>() !=null)
                    GetComponent<BoxCollider>().enabled = false;
                    placed = true;

                    GameData.instance.currentStartPoses[myId] = cGridPos;



                    GameObject.Find("Tangram").GetComponent<Tangram>().checkWin();
                }
                else//rect interactived,but does not mean can not place here,need further more accurate check
                {
                    transform.localScale = new Vector3(GameData.shrink, GameData.shrink, 1);
                    //disable its current postion
                    GameData.instance.currentStartPoses[myId] = new Vector2(-1,-1);
                    transform.position = oriPos;
                    GetComponent<BoxCollider>().enabled = true;
                    placed = false;

                    
                }

    



            }


        }


        bool placed = false;
        public bool canplace = false;
        Vector2 cmygrids;
        Vector2 cGridPos;
        public void getCurrentGrid(GameObject gameObject)
        {
            //add a half grid offset for inaccurate position
            //GameObject oldBoard = GameObject.Find("chessboardN");
            //Vector3[] tVecs = new Vector3[4];
            //oldBoard.GetComponent<RectTransform>().GetLocalCorners(tVecs);

            //placeRect.transform.position = chessBoard.transform.position - new Vector3(GameData.instance.cheesBoardSize.x / 2, GameData.instance.cheesBoardSize.y / 2)
            //       + new Vector3((tpos.x) * GameData.instance.cheesBoardSize.x / GameData.instance.gridSizeX, tpos.y * GameData.instance.cheesBoardSize.y / GameData.instance.gridSizeY);

            Vector3 tpos1 = gameObject.transform.position - chessBoard.transform.position + new Vector3(GameData.instance.cheesBoardSize.x / 2, GameData.instance.cheesBoardSize.y / 2);
            //tpos1.x * GameData
            int tx = Mathf.RoundToInt(tpos1.x * GameData.instance.gridSizeX / GameData.instance.cheesBoardSize.x);
            int ty = Mathf.RoundToInt(tpos1.y * GameData.instance.gridSizeY / GameData.instance.cheesBoardSize.y);
            //int tx = Mathf.FloorToInt(((gameObject.transform.position - GameData.instance.cheesBoardCorners[0]).x) / GameData.instance.cheesBoardSize.x * 100 / GameData.instance.gridSizeX);
            //int ty = Mathf.FloorToInt(((gameObject.transform.position - GameData.instance.cheesBoardCorners[0]).y) / GameData.instance.cheesBoardSize.y * 100 / GameData.instance.gridSizeY);

            cGridPos = new Vector2(tx, ty);
            //print(cGridPos);

            //print(transform.position + "___" + GameData.instance.cheesBoardCorners[0] + "___" + GameData.instance.cheesBoardSize.x);

            cmygrids = myGrids[1] - myGrids[0];
            cmygrids = new Vector2(Mathf.Abs(cmygrids.x), Mathf.Abs(cmygrids.y));
            //print(tmygrids);
            //print(tx+ "__"+ty+"_______"+(tx + tmygrids.x) + "__" + (ty + tmygrids.y));

            Vector2 corner0, corner1;
            corner0 = new Vector2(tx, ty);
            corner1 = new Vector2(tx + cmygrids.x, ty + cmygrids.y);

            if (corner0.x >= 0 && corner0.y >= 0 && corner1.x <= GameData.instance.gridSizeX && corner1.y <= GameData.instance.gridSizeY)
            {
                //placerect.SetActive(true);
                placerect.GetComponent<SpriteRenderer>().enabled = true;
                canplace = true;
            }
            else
            {
                //placerect.SetActive(false);
                placerect.GetComponent<SpriteRenderer>().enabled = false;
                canplace = false;
            }



            float placeRectSizeX = GameData.instance.frameWidth / GameData.instance.gridSizeX * (corner1 - corner0).x;
            float placeRectSizeY = GameData.instance.frameHeight / GameData.instance.gridSizeY * (corner1 - corner0).y;
            placerect.transform.localScale = new Vector3(placeRectSizeX / placeRectBound.size.x, placeRectSizeY / placeRectBound.size.y, -.01f);




            placerect.transform.position = chessBoard.transform.position - new Vector3(GameData.instance.cheesBoardSize.x / 2, GameData.instance.cheesBoardSize.y / 2)
                + new Vector3(tx * GameData.instance.cheesBoardSize.x / GameData.instance.gridSizeX, ty * GameData.instance.cheesBoardSize.y / GameData.instance.gridSizeY);


            placerect.transform.position = new Vector3(placerect.transform.position.x, placerect.transform.position.y, -.01f);





        }




        /// <summary>
        /// the most complicate algorithm for polygon interection;
        /// check whether is interect,if is,you can not put on the polygon.
        /// </summary>
        bool checkIntersection()
        {

            bool intersected = false ;

            //============================================================


            for (int i = 0; i < GameData.instance.tangramPoints.Count; i++)
            {
                if (i == myId) continue;//skip compare with self
                if (GameData.instance.currentStartPoses[i].x == -1) continue;//this polyon is not on the board,so ignore

                //the points of compare polygon(the polygon you are grabbing)
                List<Vector2> myPoints = GameData.instance.tangramPoints[myId];



                List<Vector2> tmypoint = new List<Vector2>();
                //set compare polygon's current points
                for (int ii = 0; ii < myPoints.Count; ii++)
                {
                    tmypoint.Add(myPoints[ii] + cGridPos);//compare point

                }



                //recreate a copy of the aim polygon(each polygon(except the polygon you are grabbing) on the board)
                List<Vector2> tPolyPoints = new List<Vector2>();
                for (int ii = 0; ii < GameData.instance.tangramPoints[i].Count; ii++)
                {
                    tPolyPoints.Add(GameData.instance.tangramPoints[i][ii]+ GameData.instance.currentStartPoses[i]);
                   
                }


                
                
                for (int ii = 0; ii < tPolyPoints.Count-1; ii ++)
                {
                    for (int jj = 0; jj < tmypoint.Count - 1; jj ++)
                    {
                     

                        Vector2 tvec = Vector2.zero;

                        Vector2 A = tmypoint[jj];
                        Vector2 B = tmypoint[jj + 1];
                        Vector2 C = tPolyPoints[ii];
                        Vector2 D = tPolyPoints[ii + 1];

                        //print("A" + A + "B" + B + "C" + C + "D" + D);


                        //check if my points(grabbing polygon) on aim polygon's boudary
                        bool isOnBoundary = PositionAlgorithmHelper.pointOnBoundary(tmypoint[jj], tPolyPoints);
                        //only check point inside polygon when not on boundary,otherwise would give the job to next steps
                       
                        if (!isOnBoundary)
                        {
                            bool hasPointInsidePolygon = PositionAlgorithmHelper.IsInPolygon(tmypoint[jj], tPolyPoints);
                            if (hasPointInsidePolygon)
                            {
                                intersected = true;
                                print("point inside other polygon and not on boundary.Intersected at once！");
                                return intersected;
                               
                            }
                        }
                        else
                        {
                            //print("onb" + isOnBoundary + "__" + tmypoint[jj]);
                        }

                        //check all line intersection situation
                        int tresult = PositionAlgorithmHelper.GetIntersection(A, B, C, D, ref tvec);
                        //print("result" + tresult);

                        switch (tresult)
                        {
                            case 0://parrall
                                break;
                            case 1://cross
                                {
                                    intersected = true;
                                    print("intersected");
                                   
                                }
                                break;
                            case 2://my point(AorB)collide the aim polygon,this line must inside,outside or on the boundary,cant be crossed
                                //get point from AB to check whether inside the polygon.
                                Vector2 tNewPointOnAB = PositionAlgorithmHelper.CalculatePoint(A, B, 1f); //print(tNewPointOnAB.ToString("F4") + "ppp"+"A"+A+"B"+B);
                                //this point should not on any line of the aim polgyon boundary
            
                                //if inside,must be intersected,means the line is inside the aim polygon,else continue check
                                //bool pIsInside1 = PositionAlgorithmHelper.PositionPnpoly(tPolyPoints.Count, tPolyPoints, tNewPointOnAB.x, tNewPointOnAB.y);
                                bool pIsInside1 = PositionAlgorithmHelper.IsInPolygon(tNewPointOnAB, tPolyPoints);
                                //print(tNewPointOnAB.ToString("F4") + "tpoint");

                                if (pIsInside1)
                                {
                                    //checkdot
                                    bool isOnAimPolygon = PositionAlgorithmHelper.pointOnBoundary(tNewPointOnAB, tPolyPoints);
                                    if (!isOnAimPolygon)
                                    {
                                        //print(tNewPointOnAB.ToString("f4"));
                                        //print("good" + PositionAlgorithmHelper.GetPointIsInLine(new Vector2(5.0447f, 7.0894f), new Vector2(5,7),new Vector2(6,7),.1f));
                                        print("isinside" + pIsInside1);
                                        intersected = true;
                                       
                                    }
                                }
                                   
                                break;
                            case 3://my line be collided with the polygon not on (A or b);means cd is on A-B but not on ab,this line must inside,outside or on the boundary,cant be crossed
                                   //get a point from CD to check whether inside the compare polyon(grabbing polygon)
                                Vector2 tNewPointOnCD = PositionAlgorithmHelper.CalculatePoint(C, D, 1f);
                                //this point should not on any line of the grabbing polgyon boundary
                                //if inside,must be intersected,means the line is inside my polygon(grabbing polygon),else continue check
                                //bool pIsInside2 = PositionAlgorithmHelper.PositionPnpoly(tmypoint.Count, tPolyPoints, tNewPointOnCD.x, tNewPointOnCD.y);
                                bool pIsInside2 = PositionAlgorithmHelper.IsInPolygon(tNewPointOnCD, tmypoint);
                                if (pIsInside2)
                                {
                                    //checkdot
                                    bool isOnMyPolygon = PositionAlgorithmHelper.pointOnBoundary(tNewPointOnCD, tmypoint);
                                    if (!isOnMyPolygon)
                                    { 
                                        print("isinside" + pIsInside2);
                                        intersected = true;
                                       
                                    }
                                }
                                break;
                        }

                        if (intersected) break;
                    }

                }

           

                }
            return intersected;
        }

        

    }




}