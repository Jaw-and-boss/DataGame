using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
namespace Hitcode_tangram
{
    public class musicScript : MonoBehaviour
    {

        // Use this for initialization
        public static string m_publisherId_ios, m_publisherId_android;
        bool NOADS = false;
        public static bool loaded = false;

        void Start()
        {
            if (loaded)
                return;
            DontDestroyOnLoad(gameObject);
            SceneManager.LoadSceneAsync("MainMenu");
            float tradio = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;




            loaded = true;
            asgroups = new List<AudioSource>();
            StartCoroutine("recycle");

            StartCoroutine("tick");
        }

        bool canRecycle = false;
        List<AudioSource> asgroups;
        IEnumerator recycle()
        {
            while (true)
            {
                yield return new WaitForSeconds(.1f);

                if (asgroups.Count > 30)
                {
                    for (int i = 0; i < 15; i++)
                    {

                        Destroy(asgroups[0]);
                        asgroups.RemoveAt(0);
                    }
                }
            }
        }

        public static int nTick = 0;
        IEnumerator tick()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                if (nTick > 0)
                {
                    nTick--;
                }
            }

        }

        void OnApplicationPause(bool paused)
        {
#if UNITY_ANDROID
            // Manage Chartboost plugin lifecycle
            //		CBBinding.pause(paused);
#endif
        }




        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = 1;
                //			GameData.getInstance().init();
                Debug.Log(Application.loadedLevelName);
                if (Application.loadedLevelName == "Game")
                {
                    GameManager.getInstance().stopAllSFX();
                    SceneManager.LoadScene("LevelMenu");
                }
                else if (Application.loadedLevelName == "LevelMenu")
                {
                    GameManager.getInstance().stopAllSFX();
                    SceneManager.LoadScene("MainMenu");
                }
                else if (Application.loadedLevelName == "MainMenu")
                {

                    GameObject.Find("PanelFade").SendMessage("exit");
                }




            }
        }



        public AudioSource PlayAudioClip(AudioClip clip, bool isloop = false)
        {
            if (clip == null) return null;


            //		AudioSource source = (AudioSource)gameObject.GetComponent("AudioSource");
            //		if (source == null)

            AudioSource source;

            if (isloop)
            {
                bool tExist = false;
                AudioSource[] as1 = GetComponentsInChildren<AudioSource>();
                foreach (AudioSource tas in as1)
                {
                    if (tas && tas.clip)
                    {
                        string clipname = (tas.clip.name);
                        if (clipname == clip.name)
                        {
                            source = tas;
                            tExist = true;
                            source.Play();
                            return source;
                            break;
                        }
                    }
                }
            }

            source = (AudioSource)gameObject.AddComponent<AudioSource>();


            //		if (!tExist) {
            //			source = (AudioSource)gameObject.AddComponent<AudioSource>();
            //		}



            source.clip = clip; source.minDistance = 1.0f; source.maxDistance = 50; source.rolloffMode = AudioRolloffMode.Linear;
            source.transform.position = transform.position;
            source.loop = isloop;
            source.Play();
            if (!isloop)
            {//not bg
                asgroups.Add(source);
            }
            return source;
        }

    }
}