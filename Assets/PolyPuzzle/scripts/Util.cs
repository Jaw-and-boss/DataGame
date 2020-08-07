using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hitcode_tangram
{
    public class Util : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public static int GetIntersection(Vector3 a, Vector3 b, Vector3 c, Vector3 d, out Vector3 contractPoint)
        {
            contractPoint = new Vector3(0, 0);

            if (Mathf.Abs(b.z - a.z) + Mathf.Abs(b.x - a.x) + Mathf.Abs(d.z - c.z)
                    + Mathf.Abs(d.x - c.x) == 0)
            {
                if ((c.x - a.x) + (c.z - a.z) == 0)
                {
                    //Debug.Log("ABCD是同一个点！");
                }
                else
                {
                    //Debug.Log("AB是一个点，CD是一个点，且AC不同！");
                }
                return 0;
            }

            if (Mathf.Abs(b.z - a.z) + Mathf.Abs(b.x - a.x) == 0)
            {
                if ((a.x - d.x) * (c.z - d.z) - (a.z - d.z) * (c.x - d.x) == 0)
                {
                    //Debug.Log("A、B是一个点，且在CD线段上！");
                }
                else
                {
                    //Debug.Log("A、B是一个点，且不在CD线段上！");
                }
                return 0;
            }
            if (Mathf.Abs(d.z - c.z) + Mathf.Abs(d.x - c.x) == 0)
            {
                if ((d.x - b.x) * (a.z - b.z) - (d.z - b.z) * (a.x - b.x) == 0)
                {
                    //Debug.Log("C、D是一个点，且在AB线段上！");
                }
                else
                {
                    //Debug.Log("C、D是一个点，且不在AB线段上！");
                }
                return 0;
            }

            if ((b.z - a.z) * (c.x - d.x) - (b.x - a.x) * (c.z - d.z) == 0)
            {
                //Debug.Log("线段平行，无交点！");
                return 0;
            }

            contractPoint.x = ((b.x - a.x) * (c.x - d.x) * (c.z - a.z) -
                    c.x * (b.x - a.x) * (c.z - d.z) + a.x * (b.z - a.z) * (c.x - d.x)) /
                    ((b.z - a.z) * (c.x - d.x) - (b.x - a.x) * (c.z - d.z));
            contractPoint.z = ((b.z - a.z) * (c.z - d.z) * (c.x - a.x) - c.z
                    * (b.z - a.z) * (c.x - d.x) + a.z * (b.x - a.x) * (c.z - d.z))
                    / ((b.x - a.x) * (c.z - d.z) - (b.z - a.z) * (c.x - d.x));

            if ((contractPoint.x - a.x) * (contractPoint.x - b.x) <= 0
                    && (contractPoint.x - c.x) * (contractPoint.x - d.x) <= 0
                    && (contractPoint.z - a.z) * (contractPoint.z - b.z) <= 0
                    && (contractPoint.z - c.z) * (contractPoint.z - d.z) <= 0)
            {

                //Debug.Log("线段相交于点(" + contractPoint.x + "," + contractPoint.z + ")！");
                return 1; // ‘相交  
            }
            else
            {
                //Debug.Log("线段相交于虚交点(" + contractPoint.x + "," + contractPoint.z + ")！");
                return -1; // ‘相交但不在线段上  
            }
        }
    }
}