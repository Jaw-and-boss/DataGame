using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
namespace Hitcode_tangram
{
    public class PolygonGenerator: MonoBehaviour
    {
        void Start()
        {




        }


        
        public void genMesh(List<Vector2> vecs, Vector3 startPos_)
        {



            float gridWidth = GameData.instance.frameWidth / GameData.instance.gridSizeX;
            float gridHeight = GameData.instance.frameHeight / GameData.instance.gridSizeY;

            float maxX = 0, maxY = 0;
            List<Vector2> vertices2D = new List<Vector2>();
            for (int i = 0; i < vecs.Count - 1; i++)
            {
                Vector2 tvec = new Vector2(vecs[i].x * gridWidth, vecs[i].y * gridHeight);

                vertices2D.Add(tvec);

                if (maxX < tvec.x) maxX = tvec.x;
                if (maxY < tvec.y) maxY = tvec.y;


            }
            Vector3 initP = vertices2D[0];

            // Use the triangulator to get indices for creating triangles
            Triangulator tr = new Triangulator(vertices2D);
            int[] indices = tr.Triangulate();

            // Create the Vector3 vertices
            Vector3[] vertices = new Vector3[vertices2D.Count];
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new Vector3(vertices2D[i].x, vertices2D[i].y, 0);
            }

            // Create the mesh
            Mesh msh = new Mesh();
            msh.vertices = vertices;
            msh.triangles = indices;
            msh.RecalculateNormals();
            msh.RecalculateBounds();


            Vector2[] uvs = new Vector2[vertices.Length];

            for (int i = 0; i < uvs.Length; i++)
            {
                uvs[i] = (new Vector2(vecs[i].x / GameData.instance.gridSizeX, vecs[i].y / GameData.instance.gridSizeY) + new Vector2(startPos_.x / GameData.instance.gridSizeX, startPos_.y / GameData.instance.gridSizeY)) * GameData.instance.uvZoom;
            }
            msh.uv = uvs;


            // Set up game object with mesh;
            //gameObject.AddComponent(typeof(MeshRenderer));
            MeshFilter filter = gameObject.GetComponent<MeshFilter>();
            filter.mesh = msh;

            gameObject.AddComponent<NewMeshCollider>();
            gameObject.GetComponent<NewMeshCollider>().Calculate();

            //gameObject.GetComponent<MeshCollider>().convex = true;
            //gameObject.AddComponent(typeof(cakeslice.Outline));
            //gameObject.GetComponent<cakeslice.Outline>().color = Random.Range(0, 3);

        }
    }
}