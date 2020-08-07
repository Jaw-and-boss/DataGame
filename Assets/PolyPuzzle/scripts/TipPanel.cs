using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

namespace Hitcode_tangram
{
    public class TipPanel : MonoBehaviour
    {

        // Use this for initialization
        bool canTick = true;
        Transform bg;
        void Start()
        {
            bg = transform.Find("bg");
        }

        private void OnEnable()
        {
            lb_notip = transform.Find("bg").Find("lb_notip").GetComponentInChildren<Text>();
            canTick = true;

            GameObject.Find("tipTitle").GetComponentInChildren<Text>().text = Localization.Instance.GetString("askTip");
            GameObject.Find("btnYes").GetComponentInChildren<Text>().text = Localization.Instance.GetString("btnyes");
            GameObject.Find("btnNo").GetComponentInChildren<Text>().text = Localization.Instance.GetString("btnno");
        }

        // Update is called once per frame
        int n = 0;

        void Update()
        {
            if (n < 20)
            {
                n++;
                return;
            }
            else
            {
                n = 0;
            }

            if (!canTick)
                return;


            if (GameData.instance.tipRemain > 0)
            {
                lb_notip.text = Localization.Instance.GetString("tipRemain") + GameData.instance.tipRemain;
            }
            else
            {
                lb_notip.text = Localization.Instance.GetString("buyTip");
            }

            return;
            if (GameData.getInstance().tipRemain == 0)
            {

                //			DateTime tnow = System.DateTime.Now;
                TimeSpan ts = new TimeSpan(50, 0, 0, 0);
                DateTime dt2 = DateTime.Now.Subtract(ts);
                long tcTime = dt2.Ticks / 10000000;



                int tTimeLasts = (int)(tcTime - long.Parse(GameData.getInstance().tickStartTime));


                int secondRemain = 300 - tTimeLasts;
                if (secondRemain <= 0)
                {
                    secondRemain = 0;
                    //count of;
                    PlayerPrefs.SetInt("tipRemain", 1);
                    PlayerPrefs.SetString("tipStart", "0");
                    GameData.getInstance().tipRemain = 1;
                    GameData.getInstance().tickStartTime = "0";
                    GameData.getInstance().main.refreshView();
                    checkUI();
                    print("startrefresh");
                }

                //lb_notip.text = Localization.Instance.GetString("nextTip") + (secondRemain).ToString() + " seconds";


            }
        }
        //	public delegate void PanelChangedEventHandler();
        //	public event PanelChangedEventHandler showPanel;
        bool isShowed;
        bool canShow = true;
        public void showTipPanel()
        {
            GameManager.getInstance().playSfx("click");

            if (GameData.instance.isLock) return;

            GameData.getInstance().isUILock = true;
            showOrHideTipPanel();
            //GetComponent<Image>().raycastTarget = true;
        }

        bool isOpenStore;
        bool notUnlock = false;//if open shop ,not unlock UI
        public void yesHandler()
        {
           
            if (!isShowed)
                return;
            notUnlock = false;
            GameData.getInstance().isUILock = false;
            GameManager.getInstance().playSfx("click");
            showOrHideTipPanel();

            if (GameData.instance.tipRemain > 0)
            {

                showTip();
            }
            else
            {
                //buy tip
                print("open store");
                isOpenStore = true;
                notUnlock = true;
                GameData.getInstance().isUILock = true;
                
            }
           
        }

        public void buyNow()
        {
            if (!isShowed)
                return;
            notUnlock = false;
            GameManager.getInstance().playSfx("click");
            showOrHideTipPanel();
            //buy tip
            print("open store");
            notUnlock = true;
            isOpenStore = true;
   
        }

        public void noHandler()
        {
            GameData.getInstance().isLock = true;
            GameManager.getInstance().playSfx("click");

            notUnlock = false;

            showOrHideTipPanel();
        }





        public void OnShowCompleted()
        {
            // Add event handler code here
            //		print ("showOver");
            isShowed = true;
            canShow = true;
        }

        public void OnHideCompleted()
        {
            //		print ("hideOver");	
            isShowed = false;
            canShow = true;
            
            GameObject.Find("btnRetry").GetComponent<Button>().interactable = true;

            //GetComponent<Image>().raycastTarget = false;
            if (isOpenStore)
            {
                GameData.instance.main.panelBuyCoinC.SetActive(true);
                GameData.instance.isLock = true;
                isOpenStore = false;
            }
            StartCoroutine("waitaframe");
        }

        IEnumerator waitaframe()
        {
            yield return new WaitForEndOfFrame();
            GameData.getInstance().isLock = false;
            if (!notUnlock)//unlockUI when not using shop
            {
                GameData.getInstance().isUILock = false;
            }
        }

        Text lb_notip;
        Button btnYes, btnNo;
        void checkUI()
        {

            btnYes = bg.Find("btnYes").GetComponent<Button>();
            btnNo = bg.Find("btnNo").GetComponent<Button>();
            //		print (GameData.getInstance ().tipRemain + "remain");
            //if (GameData.getInstance().tipRemain == 0)
            //{

            //    lb_notip.enabled = true;
            //    btnYes.interactable = false;

            //}
            //else
            //{
            //    btnYes.interactable = true;
            //    lb_notip.enabled = false;
            //}
            //if (GameData.getInstance().isLock)
            //   Gameo.Find("btnRetryB").GetComponent<Button>().interactable = false;
        }
        float startX;
        public void showOrHideTipPanel()
        {
            if (!canShow)
                return;
            gameObject.SetActive(true);
            GameData.getInstance().tickStartTime = PlayerPrefs.GetString("tipStart", "0");
            // Add event handler code here
            if (!isShowed)
            {

                bg.DOMoveX(0, .2f).SetEase(Ease.Linear).OnComplete(OnShowCompleted);
                //						
                startX = bg.transform.position.x;

                canShow = false;
                GameData.getInstance().isLock = true;
                //disable some UI;
                checkUI();

            }
            else
            {

                canShow = false;
               
                transform.Find("bg").DOMoveX(startX, .2f).SetEase(Ease.Linear).OnComplete(OnHideCompleted);

            }


        }

        void showTip()
        {


            if (GameData.getInstance().tipRemain > 0)
            {
                GameData.getInstance().tipRemain--;
                PlayerPrefs.SetInt("tipRemain", GameData.getInstance().tipRemain);
                GameData.getInstance().main.refreshView();

                //have not give a tip
                //GameData.getInstance().tickStartTime = PlayerPrefs.GetString("tipStart", "0");
                //if (GameData.getInstance().tickStartTime == "0")
                //{
                //    canTick = false;
                //    //				long tcTime = System.DateTime.Now.Ticks;

                //    TimeSpan ts = new TimeSpan(50, 0, 0, 0);
                //    DateTime dt2 = DateTime.Now.Subtract(ts);
                //    //				print (dt2.Ticks/10000000/3600);
                //    long tcTime = dt2.Ticks / 10000000;

                //    PlayerPrefs.SetString("tipStart", tcTime.ToString());
                //    GameData.getInstance().tickStartTime = tcTime.ToString();
                //    //				print (tcTime+"tctime11");
                //    canTick = true;
                //}
            }

            if (GameData.getInstance().tipRemain == 0)
            {
                canTick = true;
            }
            else
            {
                canTick = false;
            }




            GameObject tContainer = GameObject.Find("crispContainer");
            GameObject chessBoard = GameObject.Find("chessboard");
            GameObject placeRect = GameObject.Find("placeRect");

            //reset all first,and record which one is current,skip to tip the correct block

            //record already correct;
            List<int> tCorrectBlockIds = new List<int>();

            for (int i = 0; i < GameData.instance.tangramPositions.Count; i++)
            {
                if (GameData.instance.currentStartPoses[i] == GameData.instance.tangramPositions[i])
                {
                    tCorrectBlockIds.Add(i);
                }
            }
            string tc = "";
            for(int a = 0; a < tCorrectBlockIds.Count; a++)
            {
                tc += tCorrectBlockIds[a];
            }

            foreach (Transform tblock in tContainer.transform)
            {

                string tblockName = tblock.name;

                bool ignoreCurrentBlock = false;
                for (int i = 0; i < tCorrectBlockIds.Count; i++)
                {
                    if (int.Parse(tblockName) == tCorrectBlockIds[i])
                    {

                        ignoreCurrentBlock = true;
                        break;
                    }
                   

                }

                if (ignoreCurrentBlock) continue;
                tblock.GetComponent<TouchPoly>().canplace = false;
                tblock.GetComponent<TouchPoly>().SendMessage("OnMouseUp");
            }



            //for (int i = 0; i <= GameData.instance.tipUsed; i++)
            for (int i = 0; i < tContainer.transform.childCount; i++)
            {
                if (GameData.instance.tipUsed < tContainer.transform.childCount)
                {
                    Transform tTipblock = tContainer.transform.Find(i.ToString());//test
                    string tblockName = tTipblock.name;

                    if (GameData.instance.tipRemain > 0)
                    {
                        //if no tip neccessary anymore.Disable tip button to disable waste.
                        //if no tip left,not disable the button. click tip again would ask for buy
                        if (GameData.instance.tipUsed >= tContainer.transform.childCount - 2)
                        {
                            GameObject.Find("btnTip").GetComponent<Button>().interactable = false;
                        }

                    }

                    bool isPlacedCorrect = false;
                    for (int ii = 0; ii < tCorrectBlockIds.Count; ii++)
                    {
                       
                        if (int.Parse(tblockName) == tCorrectBlockIds[ii])
                        {
                            //print(tblockName + "id");
                            isPlacedCorrect = true;
                            break;
                        }
                    }
                    if (isPlacedCorrect) continue;//ignore this block;
                        
                    int selectedBlockId = int.Parse(tblockName);
                    tCorrectBlockIds.Add(selectedBlockId);

                    tTipblock.transform.localScale = Vector3.one;
                    tTipblock.transform.Find("Colliders").gameObject.SetActive(false);//GetComponent<MeshCollider>().enabled = false;
                    tTipblock.GetComponent<BoxCollider>().enabled = false;
                    Vector2 tpos = GameData.instance.tangramPositions[selectedBlockId];


                    //test
                    placeRect.transform.position = chessBoard.transform.position - new Vector3(GameData.instance.cheesBoardSize.x / 2, GameData.instance.cheesBoardSize.y / 2)
                    + new Vector3((tpos.x) * GameData.instance.cheesBoardSize.x / GameData.instance.gridSizeX, tpos.y * GameData.instance.cheesBoardSize.y / GameData.instance.gridSizeY) ;

                    //print(chessBoard.transform.position);
            

                    //placeRect.transform.position = new Vector2(-99999, 0);
                    tTipblock.transform.position = placeRect.transform.position;
                    tTipblock.GetComponent<TouchPoly>().getCurrentGrid(tTipblock.gameObject);//
                    tTipblock.GetComponent<TouchPoly>().canplace = true; 
                    tTipblock.GetComponent<TouchPoly>().SendMessage("OnMouseUp");
                    //only give one tip
                    break;
                }
            }
            GameData.instance.tipUsed++;

        }

    }
}
