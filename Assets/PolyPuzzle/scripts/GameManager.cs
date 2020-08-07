using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
using System.Collections.Generic;
using UnityEngine.Advertisements;
//using ChartboostSDK;
//using AppAdvisory.Ads;
/// <summary>
/// The main controller singleton class
/// </summary>

namespace Hitcode_tangram
{
    public class GameManager
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        /// <summary>
        /// not used yet.
        /// </summary>
        /// <returns>The object by name.</returns>
        /// <param name="objname">Objname.</param>
        public GameObject getObjectByName(string objname)
        {
            GameObject rtnObj = null;
            foreach (GameObject obj in Object.FindObjectsOfType(typeof(GameObject)))
            {
                if (obj.name == objname)
                {
                    rtnObj = obj;
                }
            }
            return rtnObj;
        }



        public static GameManager instance;
        public static GameManager getInstance()
        {
            if (instance == null)
            {
                instance = new GameManager();
                instance.music = GameObject.Find("music");
                //PlayerPrefs.DeleteAll ();//uncomment this if you want to reset saved data
            }
            return instance;
        }



        GameObject music;//a instance for play music
                         /// <summary>
                         /// Plaies the music.
                         /// </summary>
                         /// <param name="str">String.</param>
                         /// <param name="isforce">If set to <c>true</c> isforce.</param>
        public void playMusic(string str, bool isforce = false)
        {

            //do not play the same music again
            if (!isforce)
            {
                if (bgMusic != null && musicName == str)
                {
                    return;
                }
            }


            if (!music)
                return;


            AudioSource tmusic = null;

            AudioClip clip = (AudioClip)Resources.Load("sound/" + str, typeof(AudioClip));

            if (GameData.getInstance().isSoundOn == 0)
            {
                if (bgMusic)
                    bgMusic.Stop();
                tmusic = music.GetComponent<musicScript>().PlayAudioClip(clip, true);
                if (str.Substring(0, 2) == "bg")
                {
                    musicName = str;
                    bgMusic = tmusic;

                }
            }

        }







        List<AudioSource> currentSFX = new List<AudioSource>();//sound fx currently playing
        Dictionary<string, int> sfxdic = new Dictionary<string, int>();//check and scan existing sound fx.
                                                                       /// <summary>
                                                                       /// Play the music effects
                                                                       /// </summary>
                                                                       /// <returns>The sfx.</returns>
                                                                       /// <param name="str">String.</param>
        public AudioSource playSfx(string str)
        {
            AudioSource sfxSound = null;

            if (!music)
                return null;
            AudioClip clip = (AudioClip)Resources.Load("sound/" + str, typeof(AudioClip));
            if (GameData.getInstance().isSfxOn == 0)
            {
                sfxSound = music.GetComponent<musicScript>().PlayAudioClip(clip);
                if (sfxSound != null)
                {
                    if (sfxdic.ContainsKey(str) == false || sfxdic[str] != 1)
                    {
                        currentSFX.Add(sfxSound);
                        sfxdic[str] = 1;
                    }
                }
            }

            return sfxSound;


        }


        AudioSource bgMusic = new AudioSource();//the bgmusic instance(always only one)
        public string musicName = "";
        /// <summary>
        /// Stops the background music.
        /// </summary>
        public void stopBGMusic()
        {
            if (bgMusic)
            {
                bgMusic.Stop();
                musicName = "";
            }
        }

        /// <summary>
        /// Stops all sound effects.
        /// </summary>
        public void stopAllSFX()
        {
            foreach (AudioSource taudio in currentSFX)
            {
                if (taudio != null) taudio.Stop();
            }
            currentSFX.Clear();
            sfxdic.Clear();
        }


        /// <summary>
        /// Stops the music.
        /// </summary>
        /// <param name="musicName">Music name.</param>
        public void stopMusic(string musicName = "")
        {
            if (music)
            {
                AudioSource[] as1 = music.GetComponentsInChildren<AudioSource>();
                foreach (AudioSource tas in as1)
                {
                    if (musicName == "")
                    {
                        tas.Stop();
                        break;
                    }
                    else
                    {
                        if (tas && tas.clip)
                        {
                            string clipname = (tas.clip.name);
                            if (clipname == musicName)
                            {
                                tas.Stop();


                                musicName = "";
                                if (sfxdic.ContainsKey(clipname))
                                {
                                    sfxdic[clipname] = 0;
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Submits the score to game center.
        /// </summary>
        public void submitGameCenter()
        {
            if (!isAuthored)
            {
                //Debug.Log("authenticating...");
                //initGameCenter();
            }
            else
            {
                Debug.Log("submitting score...");
                //			int totalScore = getAllScore();
                int bestScore = GameData.getInstance().bestScore;
                ReportScore(Const.LEADER_BOARD_ID, bestScore);
            }

        }


        public static bool inited;//check if inited
                                  /// <summary>
                                  /// Init this controller instance.only once.
                                  /// </summary>
        public void init()
        {
            //get data
            if (inited)
                return;

            initLocalize();

            int allScore = 0;

            //stars not use for this game
            //for(int i = 0;i<GameData.totalLevel[GameData.difficulty];i++){
            //	int tScore = PlayerPrefs.GetInt("levelScore_"+i.ToString(),0);
            //	allScore += tScore;
            //	//save star state to gameobject
            //	int tStar = PlayerPrefs.GetInt("levelStar_"+i.ToString(),0);
            //	GameData.getInstance().lvStar.Add(tStar);
            //}

            GameData.getInstance().levelPass = new List<int>();
            for (int i = 0; i < GameData.totalLevel.Length; i++)
            {
                int tDiffLevelPassed = PlayerPrefs.GetInt("levelPassed" + i);
                GameData.instance.levelPass.Add(tDiffLevelPassed);

            }

            GameData.getInstance().levelStates = new List<List<int>>();
            for (int i = 0; i < GameData.totalLevel.Length; i++)
            {
                GameData.instance.levelStates.Add(new List<int>());
                for (int j = 0; j < GameData.totalLevel[i]; j++)
                {

                    int tState = PlayerPrefs.GetInt("PolyPuzzle_" + i + "_" + j, 0);
                    GameData.instance.levelStates[i].Add(tState);
                    GameData.getInstance().levelStates[i][j] = tState;


                    if (tState == 1)
                    {
                        allScore++;
                    }
                }
            }

            GameData.instance.bestScore = allScore;

            Debug.Log("bestScore is:" + allScore);
            //GameData.getInstance ().levelPassed = PlayerPrefs.GetInt("levelPassed",0);
            //Debug.Log ("current passed level = " + GameData.getInstance ().levelPassed);

            //for continue,set default to lastest level
            //GameData.getInstance ().cLevel = GameData.getInstance ().levelPassed;



            GameData.getInstance().bestScore = allScore;
            GameData.getInstance().isSoundOn = (int)PlayerPrefs.GetInt("sound", 0);
            GameData.getInstance().isSfxOn = (int)PlayerPrefs.GetInt("sfx", 0);


            initAds();
            initPurchase();
            initGameCenter();
            inited = true;

        }
        public bool noToggleSound = false;

        void initLocalize()
        {
            //int localize
            if (Localization.Instance != null)
                Localization.Instance.SetLanguage(GameData.getInstance().GetSystemLaguage());
        }

        public static Purchaser purchaser;

        void initPurchase()
        {

            if (music != null)
            {
                purchaser = music.GetComponent<Purchaser>();
            }
        }

        void initAds()
        {

            //AdsManager.instance.ShowBanner();
        }

        //=================================GameCenter======================================
        public void initGameCenter()
        {
            Social.localUser.Authenticate(HandleAuthenticated);
        }


        private bool isAuthored = false;
        private void HandleAuthenticated(bool success)
        {
            //        Debug.Log("*** HandleAuthenticated: success = " + success);
            if (success)
            {
                Social.localUser.LoadFriends(HandleFriendsLoaded);
                Social.LoadAchievements(HandleAchievementsLoaded);
                Social.LoadAchievementDescriptions(HandleAchievementDescriptionsLoaded);


                isAuthored = true;
                submitGameCenter();

            }



        }

        private void HandleFriendsLoaded(bool success)
        {
            //        Debug.Log("*** HandleFriendsLoaded: success = " + success);
            foreach (IUserProfile friend in Social.localUser.friends)
            {
                //            Debug.Log("*   friend = " + friend.ToString());
            }
        }

        private void HandleAchievementsLoaded(IAchievement[] achievements)
        {
            //        Debug.Log("*** HandleAchievementsLoaded");
            foreach (IAchievement achievement in achievements)
            {
                //            Debug.Log("*   achievement = " + achievement.ToString());
            }
        }

        private void HandleAchievementDescriptionsLoaded(IAchievementDescription[] achievementDescriptions)
        {
            //        Debug.Log("*** HandleAchievementDescriptionsLoaded");
            foreach (IAchievementDescription achievementDescription in achievementDescriptions)
            {
                //            Debug.Log("*   achievementDescription = " + achievementDescription.ToString());
            }
        }

        // achievements

        public void ReportProgress(string achievementId, double progress)
        {
            if (Social.localUser.authenticated)
            {
                Social.ReportProgress(achievementId, progress, HandleProgressReported);
            }
        }

        private void HandleProgressReported(bool success)
        {
            //        Debug.Log("*** HandleProgressReported: success = " + success);
        }

        public void ShowAchievements()
        {
            if (Social.localUser.authenticated)
            {
                Social.ShowAchievementsUI();
            }
        }


        // leaderboard

        public void ReportScore(string leaderboardId, long score)
        {
            Debug.Log("submitting score to GC...");
            if (Social.localUser.authenticated)
            {
#if UNITY_IOS
                Social.ReportScore(score, leaderboardId, HandleScoreReported);
#endif
            }
        }

        public void HandleScoreReported(bool success)
        {
            //        Debug.Log("*** HandleScoreReported: success = " + success);
        }

        public void ShowLeaderboard()
        {
            Debug.Log("showLeaderboard");
            if (Social.localUser.authenticated)
            {
                Social.ShowLeaderboardUI();
            }
        }

        //=============================================GameCenter=========================


        public void showInterestitial()
        {
            if (musicScript.nTick == 0)
            {

                //AdsManager.instance.ShowInterstitial();


                musicScript.nTick = 30;//how many seconds to show a interestitial;
            }
        }


        //================================store=====================================

        //in app
        //		public const string NON_CONSUMABLE0 = "com.xxx.unlockall";//only use this for this version
        public const string CONSUMABLE0 = "20Coin";
        public const string CONSUMABLE1 = "50Coins";
        public const string CONSUMABLE2 = "100Coins";
        public const string CONSUMABLE3 = "notused";//not used
        //only for google store if have one.Otherwise just ignore.
        public const string publishKey = "";
        public bool test = true;//set it to false when you publish to test for real.
                                /// <summary>
                                /// Buy item
                                /// </summary>
                                /// <param name="index">Index.</param>
        public void buy(int index)
        {
            if (test)
            {
                purchansedCallback("pack" + index);
            }
            else
            {
                switch (index)
                {
                    case 0:
                        purchaser.BuyConsumable("pack0");
                        break;
                    case 1:
                        purchaser.BuyConsumable("pack1");
                        break;
                    case 2:
                        purchaser.BuyConsumable("pack2");
                        break;
                    case 3:
                        //this for video reward
                        ShowRewardedAd();
                        break;



                }
            }
        }

        public void restore()
        {
            purchaser.RestorePurchases();
        }

        /// <summary>
        /// This will be called when a purchase completes.
        /// </summary>
        /// 
        public void purchansedCallback(string id)
        {

            bool buyenough = false;
            switch (id)
            {
                case "pack0":
                    buyenough = true;
                    //GameData.getInstance().coin += 120;
                    GameData.getInstance().tipRemain += 20;


                    break;
                case "pack1":
                    buyenough = true;
                    //GameData.getInstance().coin += 300;
                    GameData.getInstance().tipRemain += 50;
                    break;
                case "pack2":
                    buyenough = true;
                    //GameData.getInstance().coin += 500;
                    GameData.getInstance().tipRemain += 100;
                    break;
                case "pack3":
                    //this for video reward
                    ShowRewardedAd();

                    break;
            }

            PlayerPrefs.SetInt("tipRemain", GameData.instance.tipRemain);
            GameData.instance.main.refreshView();
            //PlayerPrefs.SetInt("coin", GameData.getInstance().coin);
            //GameObject txtCoin = GameObject.Find("txtCoin");
            //if (txtCoin != null)
            //{
            //    txtCoin.GetComponent<Text>().text = GameData.getInstance().coin.ToString();
            //}
            if (buyenough && !test)
            {
                GameData.isAds = false;
                PlayerPrefs.SetInt("noAds", 1);
            }


        }

        public string getPrice(int index)
        {
            if (purchaser == null) return null;
            string[] ids = { CONSUMABLE0, CONSUMABLE1, CONSUMABLE2, CONSUMABLE3 };

            string tprice = purchaser.getPrice(ids[index]);
            //Debug.Log(tprice);
            return tprice;
        }

        //=========================================ads==================================
        public void ShowRewardedAd()//for unity ads only;
        {
#if UNITY_IPHONE || UNITY_ANDROID

            //if (Chartboost.hasRewardedVideo(CBLocation.Default))
            //{
            //    Chartboost.showRewardedVideo(CBLocation.Default);
            //}
            //else
            if (Advertisement.IsReady(""))
            {
                var options = new ShowOptions { resultCallback = HandleShowResult };
                Advertisement.Show("", options);
            }
#endif
        }

        //handle unity ads callback
#if UNITY_IPHONE || UNITY_ANDROID
        private void HandleShowResult(ShowResult result)
        {
            switch (result)
            {
                case ShowResult.Finished:
                    Debug.Log("The ad was successfully shown.");
                    //
                    // YOUR CODE TO REWARD THE GAMER
                    // Give coins etc.
                    makeReward();
                    break;
                case ShowResult.Skipped:
                    Debug.Log("The ad was skipped before reaching the end.");
                    break;
                case ShowResult.Failed:
                    Debug.LogError("The ad failed to be shown.");
                    break;
            }
        }
#endif
        //public void CacheVideo()
        //{
        //    Chartboost.cacheRewardedVideo(CBLocation.Default);
        //}

        //bool isfirst = true;//show interestital on start
        //void cbInterestitialReady(CBLocation location)
        //{
        //    if (isfirst)
        //    {
        //        Chartboost.showInterstitial(CBLocation.Default);
        //        isfirst = false;
        //    }
        //}

        //void didFailToLoadRewardedVideo(CBLocation location, CBImpressionError error)
        //{
        //    Debug.Log(string.Format("didFailToLoadRewardedVideo: {0} at location {1}", error, location));
        //}

        //void didDismissRewardedVideo(CBLocation location)
        //{
        //    Debug.Log("didDismissRewardedVideo: " + location);
        //}

        //void didCloseRewardedVideo(CBLocation location)
        //{
        //    Debug.Log("didCloseRewardedVideo: " + location);
        //}

        //void didClickRewardedVideo(CBLocation location)
        //{
        //    Debug.Log("didClickRewardedVideo: " + location);
        //}
        //public bool isCachedVideo = false;
        //void didCacheRewardedVideo(CBLocation location)
        //{
        //    Debug.Log("didCacheRewardedVideo: " + location);
        //    isCachedVideo = true;
        //}

        //bool shouldDisplayRewardedVideo(CBLocation location)
        //{
        //    Debug.Log("shouldDisplayRewardedVideo: " + location);
        //    return true;
        //}

        //void didCompleteRewardedVideo(CBLocation location, int reward)
        //{
        //    Debug.Log(string.Format("didCompleteRewardedVideo: reward {0} at location {1}", reward, location));


        //    makeReward();

        //}

        //void didDisplayRewardedVideo(CBLocation location)
        //{
        //    Debug.Log("didDisplayRewardedVideo: " + location);
        //}

        //free tip
        void makeReward()
        {


            GameData.getInstance().tipRemain += 2;
            PlayerPrefs.SetInt("tipRemain", GameData.instance.tipRemain);
            GameData.instance.main.refreshView();

            PlayerPrefs.Save();


        }


    }
}