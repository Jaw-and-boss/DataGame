using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hitcode_tangram
{
    namespace polypuzlle
    {
        public class flashRed : MonoBehaviour
        {

            // Use this for initialization

            SpriteRenderer sp;
            void Start()
            {
                sp = GetComponent<SpriteRenderer>();
            }


            // Update is called once per frame
            float spd = -.01f;
            void FixedUpdate()
            {
                sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, sp.color.a + spd);
                if (sp.color.a <= .3 || sp.color.a > .7f)
                {
                    spd *= -1f;
                }
            }
        }
    }
}