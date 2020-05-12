using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace sail
{
    public class TileMesh : MonoBehaviour
    {
        public float Radius = 0.5f;
        public float Height = 1f;

        // Private
        private MeshFilter MeshFilter = null;
        private MeshCollider MeshCollider = null;

        private Vector3[] Vertices = null;
        private Vector3[] HexVertices = null;
        private Vector3[] CubeVertices = null;

        private Vector3[] Normals = null;

        public TileType Type = TileType.Cube;

        public void Start()
        {
            MeshFilter = GetComponent<MeshFilter>();
            MeshCollider = GetComponent<MeshCollider>();

            var mesh = new Mesh();
            MeshFilter.mesh = mesh;

            mesh.name = "CustomMesh";

            float r = Radius;
            float hr = r / 2f; // Half radius
            float w = Mathf.Sqrt(3) * hr;
            float h = Height;

            HexVertices = new Vector3[]{
            // Top Face
            new Vector3 ( 0, h,  0),  //  0 Center   Center
            new Vector3 ( 0, h,  r),  //  1 Center   Up
            new Vector3 ( w, h,  hr), //  2 Right   HUp
            new Vector3 ( w, h, -hr), //  3 Right   HDown
            new Vector3 ( 0, h, -r),  //  4 Center   Down
            new Vector3 (-w, h, -hr), //  5 Left    HDown
            new Vector3 (-w, h,  hr), //  6 Left     Up

            // Bottom Face
            new Vector3 ( 0, 0,  0),  //  7 Center   Center
            new Vector3 ( 0, 0,  r),  //  8 Center   Up
            new Vector3 ( w, 0,  hr), //  9 Right   HUp
            new Vector3 ( w, 0, -hr), // 10 Right   HDown
            new Vector3 ( 0, 0, -r),  // 11 Center   Down
            new Vector3 (-w, 0, -hr), // 12 Left    HDown
            new Vector3 (-w, 0,  hr), // 13 Left    HUp

            // Side Faces
            // Right Up
            new Vector3 ( 0, h,  r),  // 14 Center   Up
            new Vector3 ( 0, 0,  r),  // 15 Center   Up
            new Vector3 ( w, h,  hr), // 16 Right   HUp
            new Vector3 ( w, 0,  hr), // 17 Right   HUp

            // Right
            new Vector3 ( w, h,  hr), // 18 Right   HUp
            new Vector3 ( w, 0,  hr), // 19 Right   HUp
            new Vector3 ( w, h, -hr), // 20 Right   HDown
            new Vector3 ( w, 0, -hr), // 21 Right   HDown

            // Right Down
            new Vector3 ( w, h, -hr), // 22 Right   HDown
            new Vector3 ( w, 0, -hr), // 23 Right   HDown
            new Vector3 ( 0, h, -r),  // 24 Center   Down
            new Vector3 ( 0, 0, -r),  // 25 Center   Down

            // Left Down
            new Vector3 ( 0, h, -r),  // 26 Center   Down
            new Vector3 ( 0, 0, -r),  // 27 Center   Down
            new Vector3 (-w, h, -hr), // 28 Left    HDown
            new Vector3 (-w, 0, -hr), // 29 Left    HDown

            // Left
            new Vector3 (-w, h, -hr), // 30 Left    HDown
            new Vector3 (-w, 0, -hr), // 31 Left    HDown
            new Vector3 (-w, h,  hr), // 32 Left    HUp
            new Vector3 (-w, 0,  hr), // 33 Left    HUp

            // Left Up
            new Vector3 (-w, h,  hr), // 34 Left    HUp
            new Vector3 (-w, 0,  hr), // 35 Left    HUp
            new Vector3 ( 0, h,  r),  // 36 Center   Up
            new Vector3 ( 0, 0,  r),  // 37 Center   Up
        };

            CubeVertices = new Vector3[]{
            // Top Face
            new Vector3 ( 0, h,  0f), // 0  Center Center
            new Vector3 ( 0, h,  r),  // 1  Center Up
            new Vector3 ( r, h,  r),  // 2  Right  Up
            new Vector3 ( r, h, -r),  // 3  Right  Down
            new Vector3 ( 0, h, -r),  // 4  Center Down
            new Vector3 (-r, h, -r),  // 5  Left   Down
            new Vector3 (-r, h,  r),  // 6  Left   Up

            // Bottom Face
            new Vector3 ( 0, 0,  0f), // 7  Center Center
            new Vector3 ( 0, 0,  r),  // 8  Center Up
            new Vector3 ( r, 0,  r),  // 9  Right  Up
            new Vector3 ( r, 0, -r),  // 10 Right  Down
            new Vector3 ( 0, 0, -r),  // 11 Center Down
            new Vector3 (-r, 0, -r),  // 12 Left   Down
            new Vector3 (-r, 0,  r),  // 13 Left   Up

            // Side Faces
            // Right Up
            new Vector3 ( 0, h,  r),  // 14 Center Up
            new Vector3 ( 0, 0,  r),  // 15 Center Up
            new Vector3 ( r, h,  r),  // 16 Right  Up
            new Vector3 ( r, 0,  r),  // 17 Right  Up

            // Right
            new Vector3 ( r, h,  r), // 18 Right   Up
            new Vector3 ( r, 0,  r), // 19 Right   Up
            new Vector3 ( r, h, -r), // 20 Right   Down
            new Vector3 ( r, 0, -r), // 21 Right   Down

            // Right Down
            new Vector3 ( r, h, -r), // 22 Right   Down
            new Vector3 ( r, 0, -r), // 23 Right   Down
            new Vector3 ( 0, h, -r), // 24 Center  Down
            new Vector3 ( 0, 0, -r), // 25 Center  Down

            // Left Down
            new Vector3 ( 0, h, -r), // 26 Center  Down
            new Vector3 ( 0, 0, -r), // 27 Center  Down
            new Vector3 (-r, h, -r), // 28 Left    Down
            new Vector3 (-r, 0, -r), // 29 Left    Down

            // Left
            new Vector3 (-r, h, -r), // 30 Left    Down
            new Vector3 (-r, 0, -r), // 31 Left    Down
            new Vector3 (-r, h,  r), // 32 Left    Up
            new Vector3 (-r, 0,  r), // 33 Left    Up

            // Left Up
            new Vector3 (-r, h,  r), // 34 Left   Up
            new Vector3 (-r, 0,  r), // 35 Left   Up
            new Vector3 ( 0, h,  r),  // 36 Center Up
            new Vector3 ( 0, 0,  r),  // 37 Center Up
        };

            mesh.vertices = (Vector3[])CubeVertices.Clone();
            Vertices = mesh.vertices;

            int[] triangles = {
            /// Top
            0, 1, 2,
            0, 2, 3,
            0, 3, 4,
            0, 4, 5,
            0, 5, 6,
            0, 6, 1,

            /// Bottom
            7, 9, 8,
            7, 10, 9,
            7, 11, 10,
            7, 12, 11,
            7, 13, 12,
            7, 8, 13,

            /// Side
            // R U
            14, 15, 16,
            15, 17, 16,
            // R
            18, 19, 20,
            19, 21, 20,
            // R D
            22, 23, 24,
            23, 25, 24,
            // L D
            26, 27, 28,
            27, 29, 28,
            // L
            30, 31, 32,
            31, 33, 32,
            // L U
            34, 35, 36,
            35, 37, 36
        };

            mesh.triangles = triangles;

            Normals = new Vector3[mesh.triangles.Length];
            MeshFilter.mesh.RecalculateNormals();

            updateCollider();

            //Vector2[] uvs = {
            //    new Vector2(0, 0.66f),
            //    new Vector2(0.25f, 0.66f),
            //    new Vector2(0, 0.33f),
            //    new Vector2(0.25f, 0.33f),
            //
            //    new Vector2(0.5f, 0.66f),
            //    new Vector2(0.5f, 0.33f),
            //    new Vector2(0.75f, 0.66f),
            //    new Vector2(0.75f, 0.33f),
            //
            //    new Vector2(1, 0.66f),
            //    new Vector2(1, 0.33f),
            //
            //    new Vector2(0.25f, 1),
            //    new Vector2(0.5f, 1),
            //
            //    new Vector2(0.25f, 0),
            //    new Vector2(0.5f, 0),
            //};
            //mesh.uv = uv;
        }

        public void Update()
        {
        }

        public void morphTo(float blendDuration, TileType ToType)
        {
            StartCoroutine(startMorphTo(blendDuration, ToType));
        }

        private IEnumerator startMorphTo(float blendDuration, TileType ToType)
        {
            var initialVertices = Type == TileType.Cube ? CubeVertices : HexVertices;
            Type = ToType;
            var targetVertices = Type == TileType.Cube ? CubeVertices : HexVertices;

            var verticeCount = MeshFilter.mesh.vertices.Length;

            var timer = 0f;
            while (timer < blendDuration)
            {
                timer = Mathf.Min(timer + Time.deltaTime, blendDuration);

                for (int i = 0; i < verticeCount; ++i)
                {
                    Vertices[i] = Vector3.Lerp(initialVertices[i], targetVertices[i], timer / blendDuration);
                }

                MeshFilter.mesh.vertices = Vertices;
                MeshFilter.mesh.RecalculateNormals();

                yield return null;
            }

            updateCollider();
        }

        void updateCollider()
        {
            if (MeshCollider == null)
            {
                return;
            }

            MeshCollider.sharedMesh = null;
            MeshCollider.sharedMesh = MeshFilter.mesh;
        }
    }


    #if UNITY_EDITOR
    [CustomEditor(typeof(TileMesh))]
    public class TileMeshEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            TileMesh mesh = (TileMesh)target;
            if (GUILayout.Button("Switch"))
            {
                var type = (TileType)(((int)mesh.Type + 1) % (int)TileType.Count);
                mesh.morphTo(2f, type);
            }
        }
    }
    #endif
}