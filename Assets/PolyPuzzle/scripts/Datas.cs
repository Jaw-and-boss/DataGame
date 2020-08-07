using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hitcode_tangram
{
    public class Datas : ScriptableObject
    {

        private TextAsset datas;
        private Dictionary<string, Dictionary<string, string>> data;
        void Start()
        {

        }

        public string[] getData(string dataName)
        {



            datas = Resources.Load<TextAsset>(dataName + "/" + GameData.difficulty);
            string[] lines = new string[0];
            data = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, string> loc = new Dictionary<string, string>();
            lines = datas.text.Split('\n');

            return lines;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}