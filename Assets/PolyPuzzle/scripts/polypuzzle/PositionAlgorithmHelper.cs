using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hitcode_tangram
{
    public class PositionAlgorithmHelper : MonoBehaviour
    {



        public static bool PositionPnpoly(int nvert, List<Vector2> vert, float testx, float testy)
        {
            int i, j, c = 0;
            for (i = 0, j = nvert - 1; i < nvert; j = i++)
            {
                if (((vert[i].y > testy) != (vert[j].y > testy)) && (testx < (vert[j].x - vert[i].x) * (testy - vert[i].y) / (vert[j].y - vert[i].y) + vert[i].x))
                {
                    c = 1 + c;
                }
            }
            if (c % 2 == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }




        /// <summary>  
        /// 判断点是否在多边形内.  
        /// ----------原理----------  
        /// 注意到如果从P作水平向左的射线的话，如果P在多边形内部，那么这条射线与多边形的交点必为奇数，  
        /// 如果P在多边形外部，则交点个数必为偶数(0也在内)。  
        /// </summary>  
        /// <param name="checkPoint">要判断的点</param>  
        /// <param name="polygonPoints">多边形的顶点</param>  
        /// <returns></returns>  
        public static bool IsInPolygon(Vector2 checkPoint, List<Vector2> polygonPoints)
        {
            bool inside = false;
            int pointCount = polygonPoints.Count;
            Vector2 p1, p2;
            for (int i = 0, j = pointCount - 1; i < pointCount; j = i, i++)//第一个点和最后一个点作为第一条线，之后是第一个点和第二个点作为第二条线，之后是第二个点与第三个点，第三个点与第四个点...  
            {
                p1 = polygonPoints[i];
                p2 = polygonPoints[j];
                if (checkPoint.y < p2.y)
                {//p2在射线之上  
                    if (p1.y <= checkPoint.y)
                    {//p1正好在射线中或者射线下方  
                        if ((checkPoint.y - p1.y) * (p2.x - p1.x) > (checkPoint.x - p1.x) * (p2.y - p1.y))//斜率判断,在P1和P2之间且在P1P2右侧  
                        {
                            //射线与多边形交点为奇数时则在多边形之内，若为偶数个交点时则在多边形之外。  
                            //由于inside初始值为false，即交点数为零。所以当有第一个交点时，则必为奇数，则在内部，此时为inside=(!inside)  
                            //所以当有第二个交点时，则必为偶数，则在外部，此时为inside=(!inside)  
                            inside = (!inside);
                        }
                    }
                }
                else if (checkPoint.y < p1.y)
                {
                    //p2正好在射线中或者在射线下方，p1在射线上  
                    if ((checkPoint.y - p1.y) * (p2.x - p1.x) < (checkPoint.x - p1.x) * (p2.y - p1.y))//斜率判断,在P1和P2之间且在P1P2右侧  
                    {
                        inside = (!inside);
                    }
                }
            }
            return inside;
        }


        /// <summary>  
        /// 判断点是否在多边形内.  
        /// ----------原理----------  
        /// 注意到如果从P作水平向左的射线的话，如果P在多边形内部，那么这条射线与多边形的交点必为奇数，  
        /// 如果P在多边形外部，则交点个数必为偶数(0也在内)。  
        /// 所以，我们可以顺序考虑多边形的每条边，求出交点的总个数。还有一些特殊情况要考虑。假如考虑边(P1,P2)，  
        /// 1)如果射线正好穿过P1或者P2,那么这个交点会被算作2次，处理办法是如果P的从坐标与P1,P2中较小的纵坐标相同，则直接忽略这种情况  
        /// 2)如果射线水平，则射线要么与其无交点，要么有无数个，这种情况也直接忽略。  
        /// 3)如果射线竖直，而P0的横坐标小于P1,P2的横坐标，则必然相交。  
        /// 4)再判断相交之前，先判断P是否在边(P1,P2)的上面，如果在，则直接得出结论：P再多边形内部。  
        /// </summary>  
        /// <param name="checkPoint">要判断的点</param>  
        /// <param name="polygonPoints">多边形的顶点</param>  
        /// <returns></returns>  
        public static bool IsInPolygon2(Vector2 checkPoint, List<Vector2> polygonPoints)
        {
            int counter = 0;
            int i;
            double xinters;
            Vector2 p1, p2;
            int pointCount = polygonPoints.Count;
            p1 = polygonPoints[0];
            for (i = 1; i <= pointCount; i++)
            {
                p2 = polygonPoints[i % pointCount];
                if (checkPoint.y > Mathf.Min(p1.y, p2.y)//校验点的Y大于线段端点的最小Y  
                    && checkPoint.y <= Mathf.Max(p1.y, p2.y))//校验点的Y小于线段端点的最大Y  
                {
                    if (checkPoint.x <= Mathf.Max(p1.x, p2.x))//校验点的X小于等线段端点的最大X(使用校验点的左射线判断).  
                    {
                        if (p1.y != p2.y)//线段不平行于X轴  
                        {
                            xinters = (checkPoint.y - p1.y) * (p2.x - p1.x) / (p2.y - p1.y) + p1.x;
                            if (p1.x == p2.x || checkPoint.x <= xinters)
                            //if(checkPoint.x <= xinters && p1.x!=p2.x)
                            {
                                counter++;

                            }
                        }
                    }

                }
                p1 = p2;
            }

            if (counter % 2 == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        public static bool IsPointInPolygon(Vector2 point, List<Vector2> polygon)
        {
            int polygonLength = polygon.Count, i = 0;
            bool inside = false;
            // x, y for tested point.
            float pointX = point.x, pointY = point.y;
            // start / end point for the current polygon segment.
            float startX, startY, endX, endY;
            Vector2 endPoint = polygon[polygonLength - 1];
            endX = endPoint.x;
            endY = endPoint.y;
            while (i < polygonLength)
            {
                startX = endX; startY = endY;
                endPoint = polygon[i++];
                endX = endPoint.x; endY = endPoint.y;
                //
                inside ^= (endY > pointY ^ startY > pointY) /* ? pointY inside [startY;endY] segment ? */
                          && /* if so, test if it is under the segment */
                          ((pointX - endX) < (pointY - endY) * (startX - endX) / (startY - endY));
            }
            return inside;
        }





        //Line segment-line segment intersection in 2d space by using the dot product
        //p1 and p2 belongs to line 1, and p3 and p4 belongs to line 2 
        public static bool AreLineSegmentsIntersectingDotProduct(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
        {
            bool isIntersecting = false;

            if (IsPointsOnDifferentSides(p1, p2, p3, p4) && IsPointsOnDifferentSides(p3, p4, p1, p2))
            {
                isIntersecting = true;
            }

            return isIntersecting;
        }

        //Are the points on different sides of a line?
        private static bool IsPointsOnDifferentSides(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
        {
            bool isOnDifferentSides = false;

            //The direction of the line
            Vector3 lineDir = p2 - p1;

            //The normal to a line is just flipping x and z and making z negative
            Vector3 lineNormal = new Vector3(-lineDir.z, lineDir.y, lineDir.x);

            //Now we need to take the dot product between the normal and the points on the other line
            float dot1 = Vector3.Dot(lineNormal, p3 - p1);
            float dot2 = Vector3.Dot(lineNormal, p4 - p1);

            //If you multiply them and get a negative value then p3 and p4 are on different sides of the line
            if (dot1 * dot2 < 0f)
            {
                isOnDifferentSides = true;
            }

            return isOnDifferentSides;
        }





        /// <summary>
        /// 判断两条线是否相交
        /// </summary>
        /// <param name="a">线段1起点坐标</param>
        /// <param name="b">线段1终点坐标</param>
        /// <param name="c">线段2起点坐标</param>
        /// <param name="d">线段2终点坐标</param>
        /// <param name="intersection">相交点坐标</param>
        /// <returns>是否相交 0:两线平行  -1:不平行且未相交  1:两线相交</returns>

        public static int GetIntersection(Vector2 a, Vector2 b, Vector2 c, Vector2 d, ref Vector2 intersection)
        {

            if (Mathf.Abs(b.x - a.y) + Mathf.Abs(b.x - a.x) + Mathf.Abs(d.y - c.y) + Mathf.Abs(d.x - c.x) == 0)
            {
                if (c.x - a.x == 0)
                {
                    //Debug.Log("ABCD not same！");
                }
                else
                {
                    //Debug.Log("AB same，CDsame，AC different！");
                }
                return 0;
            }

            if (Mathf.Abs(b.y - a.y) + Mathf.Abs(b.x - a.x) == 0)
            {
                if ((a.x - d.x) * (c.y - d.y) - (a.y - d.y) * (c.x - d.x) == 0)
                {
                    //Debug.Log("A、Bsame,on CD！");
                }
                else
                {
                    //Debug.Log("A、Bsame not On CD");
                }
                return 0;
            }

            if (Mathf.Abs(d.y - c.y) + Mathf.Abs(d.x - c.x) == 0)
            {
                if ((d.x - b.x) * (a.y - b.y) - (d.y - b.y) * (a.x - b.x) == 0)
                {
                    //Debug.Log("C、Dsame,on AB");
                }
                else
                {
                    //Debug.Log("C、Dsame not on AB");
                }
            }


            if ((b.y - a.y) * (c.x - d.x) - (b.x - a.x) * (c.y - d.y) == 0)
            {
                //Debug.Log("parrall");
                return 0;
            }

            intersection.x = ((b.x - a.x) * (c.x - d.x) * (c.y - a.y) - c.x * (b.x - a.x) * (c.y - d.y) + a.x * (b.y - a.y) * (c.x - d.x)) / ((b.y - a.y) * (c.x - d.x) - (b.x - a.x) * (c.y - d.y));
            intersection.y = ((b.y - a.y) * (c.y - d.y) * (c.x - a.x) - c.y * (b.y - a.y) * (c.x - d.x) + a.y * (b.x - a.x) * (c.y - d.y)) / ((b.x - a.x) * (c.y - d.y) - (b.y - a.y) * (c.x - d.x));


            if ((intersection.x - a.x) * (intersection.x - b.x) <= 0 && (intersection.x - c.x) * (intersection.x - d.x) <= 0 && (intersection.y - a.y) * (intersection.y - b.y) <= 0 && (intersection.y - c.y) * (intersection.y - d.y) <= 0)
            {
                if (intersection == a || intersection == b)
                {
                    //Debug.Log("I Intersect at" + intersection.x + "," + intersection.y + ")！");
                    return 2; //'intersect with my point(a or b)
                }
                else if (intersection != a && intersection != b && intersection != c && intersection != d)
                {
                    return 1;//intersect not point,polygon intersect directly!
                }
                else
                {
                    //Debug.Log("I been Intersect at(" + intersection.x + "," + intersection.y + ")！");
                    return 3;
                }

            }
            else
            {
                //Debug.Log("on extended place(" + intersection.x + "," + intersection.y + ")！");
                //print("A:" + a);
                //print("B:" + b);
                //print("C:" + c);
                //print("D:" + d);
                return -1; //'intersect but not on the segment
            }
        }

        public static Vector2 CalculatePoint(Vector2 a, Vector2 b, float distance)
        {
            Vector3 A = a;
            Vector3 B = b;
            Vector3 P = distance * Vector3.Normalize(B - A) + A;

            //Vector3 P = Vector3.Lerp(A, B, distance / (A - B).magnitude);



            return new Vector2(P.x, P.y);

        }


        public static bool pointOnBoundary(Vector2 p, List<Vector2> polygon)
        {
            bool onBoundary = false;
            for (int i = 0; i < polygon.Count - 1; i++)
            {
                Vector2 p1 = polygon[i];
                Vector2 p2 = polygon[i + 1];

                onBoundary = GetPointIsInLine(p, p1, p2, .1f);
                if (onBoundary)
                {
                    break;
                }
            }
            return onBoundary;
        }


        //判断点是否在直线上

        public static bool GetPointIsInLine(Vector2 pf, Vector2 p1, Vector2 p2, float range)
        {

            //range 判断的的误差，不需要误差则赋值0



            //点在线段首尾两端之外则return false
            //print("pf" + pf+"p1"+p1+"p2"+p2);
            if (pf.Equals(p1) || pf.Equals(p2))
            {
                //print("pfeeeeequal" + pf);
                return true;
            }
            float cross = (p2.x - p1.x) * (pf.x - p1.x) + (p2.y - p1.y) * (pf.y - p1.y);
            if (cross < 0) return false;
            float d2 = (p2.x - p1.x) * (p2.x - p1.x) + (p2.y - p1.y) * (p2.y - p1.y);
            if (cross > d2) return false;





            float r = cross / d2;
            float px = p1.x + (p2.x - p1.x) * r;
            float py = p1.y + (p2.y - p1.y) * r;

            //判断距离是否小于误差
            return Mathf.Sqrt((pf.x - px) * (pf.x - px) + (py - pf.y) * (py - pf.y)) <= range;
        }
    }



}