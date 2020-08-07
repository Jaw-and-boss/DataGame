using UnityEngine;
using System.Collections;
// using SmartLocalization;
using UnityEngine.UI;
using UnityEngine.Purchasing;
//using AppAdvisory.Ads;
namespace Hitcode_tangram
{
    public class PanelBuyCoin : MonoBehaviour
    {

        // Use this for initialization
        GameObject scrollpanel;
        string lang = "";
        public GameObject btnBuyCoinClose;
        public GameObject panelBuyAlert;
        GameObject panel;
        void Start()
        {


            panel = transform.Find("panel").gameObject;
            initView();
            GameData.getInstance().isLock = true; //.lockGame(true);


        }



        // Update is called once per frame
        void Update()
        {

        }

        //public void showMe()
        //{

        //    ATween.MoveTo(panel, ATween.Hash("ignoretimescale", true, "islocal", true, "y", 40, "time", 1f, "easeType", "easeOutExpo", "oncomplete", "OnShowCompleted", "oncompletetarget", this.gameObject));

        //}

     

        void initView()
        {


            panel.transform.Find("title").GetComponent<Text>().text = Localization.Instance.GetString("titleShop");
            panel.transform.Find("btnClose").GetComponentInChildren<Text>().text = Localization.Instance.GetString("btnClose");

            for (int i = 0; i < 3; i++)
            {
                GameObject trow = GameObject.Find("row" + i);
                trow.transform.Find("lbDetail").GetComponent<Text>().text = Localization.Instance.GetString("price" + (i) + "Tip");


                trow.transform.Find("lbPrice").GetComponent<Text>().text = Localization.Instance.GetString("price" + (i));
            }
            GameObject trow3 = GameObject.Find("row3");
            trow3.transform.Find("lbPrice").GetComponent<Text>().text = Localization.Instance.GetString("free");
            trow3.transform.Find("lbDetail").GetComponent<Text>().text = Localization.Instance.GetString("freeCoin");
        }


        IEnumerator hideWait()
        {
            yield return new WaitForSeconds(30);
            panelBuyAlert.SetActive(false);
        }


        public void OnClick2(GameObject g)
        {
            GameManager.getInstance().playSfx("click");
            switch (g.name)
            {
                case "btnBuyCoin":
                    GameManager.getInstance().playSfx("click");

                    int tindex = int.Parse(g.transform.parent.name.Substring(3, 1));

                    if (tindex < 3)
                    {
                        GameManager.getInstance().buy(tindex);
                    }
                    else
                    {
                        GameManager.getInstance().buy(tindex);
                    }
                    break;
                case "btnClose":
                    GameManager.getInstance().playSfx("click");
                    gameObject.SetActive(false);
                    GameData.instance.isLock = false;
                    //panel = transform.Find("panel").gameObject;
                    //ATween.MoveTo(panel.gameObject, ATween.Hash("ignoretimescale", true, "islocal", true, "y", 600, "time", 1f, "easeType", "easeOutExpo", "oncomplete", "OnHideCompleted", "oncompletetarget", this.gameObject, "oncompleteparams", "buyClose"));
                    GameData.getInstance().isUILock = false;
                    break;
            }
        }


        void OnHideCompleted(string str)
        {
            switch (str)
            {
                case "buyClose":
                    gameObject.SetActive(false);
                    GameData.getInstance().isLock = false;//.lockGame(false);
                    break;
            }

        }


    }
}