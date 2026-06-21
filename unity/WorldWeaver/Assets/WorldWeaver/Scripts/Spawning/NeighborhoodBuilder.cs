using UnityEngine;
using WorldWeaver.Core;

namespace WorldWeaver.Spawning
{
    /// <summary>
    /// Procedural asphalt roads, sidewalks, and streetlights (Android-friendly low poly).
    /// </summary>
    public class NeighborhoodBuilder : MonoBehaviour
    {
        static readonly Color Asphalt = new Color(0.16f, 0.16f, 0.18f);
        static readonly Color Concrete = new Color(0.55f, 0.56f, 0.58f);
        static readonly Color Grass = new Color(0.24f, 0.36f, 0.23f);

        public Transform districtRoot;

        public void BuildBaseTerrain(Bounds bounds)
        {
            if (districtRoot == null) districtRoot = transform;

            CreateGround(bounds);
            CreateRoads(bounds);
            CreateSidewalks(bounds);
            CreateStreetlights(bounds, 8);
        }

        void CreateGround(Bounds b)
        {
            var plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            plane.name = "Ground";
            plane.transform.SetParent(districtRoot);
            plane.transform.localScale = new Vector3(b.size.x * 0.12f, 1f, b.size.z * 0.12f);
            plane.transform.position = b.center + Vector3.down * 0.02f;
            SetColor(plane, Grass);
        }

        void CreateRoads(Bounds b)
        {
            var main = GameObject.CreatePrimitive(PrimitiveType.Cube);
            main.name = "RoadMain";
            main.transform.SetParent(districtRoot);
            main.transform.localScale = new Vector3(b.size.x, 0.05f, 3.2f);
            main.transform.position = new Vector3(b.center.x, 0.02f, b.center.z);
            SetColor(main, Asphalt);

            var cross = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cross.name = "RoadCross";
            cross.transform.SetParent(districtRoot);
            cross.transform.localScale = new Vector3(3.2f, 0.05f, b.size.z);
            cross.transform.position = new Vector3(b.center.x, 0.025f, b.center.z);
            SetColor(cross, Asphalt);
        }

        void CreateSidewalks(Bounds b)
        {
            foreach (var side in new[] { -1f, 1f })
            {
                var sw = GameObject.CreatePrimitive(PrimitiveType.Cube);
                sw.name = side > 0 ? "SidewalkEast" : "SidewalkWest";
                sw.transform.SetParent(districtRoot);
                sw.transform.localScale = new Vector3(1.4f, 0.08f, b.size.z);
                sw.transform.position = new Vector3(b.center.x + side * 2.8f, 0.05f, b.center.z);
                SetColor(sw, Concrete);
            }
        }

        void CreateStreetlights(Bounds b, int count)
        {
            for (int i = 0; i < count; i++)
            {
                float t = count <= 1 ? 0.5f : i / (float)(count - 1);
                float z = Mathf.Lerp(b.min.z, b.max.z, t);
                foreach (var side in new[] { -1f, 1f })
                {
                    var pole = new GameObject("Streetlight");
                    pole.transform.SetParent(districtRoot);
                    pole.transform.position = new Vector3(b.center.x + side * 3.6f, 0f, z);

                    var post = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    post.transform.SetParent(pole.transform);
                    post.transform.localScale = new Vector3(0.12f, 1.6f, 0.12f);
                    post.transform.localPosition = new Vector3(0f, 1.6f, 0f);
                    SetColor(post, Color.black);

                    var bulb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    bulb.transform.SetParent(pole.transform);
                    bulb.transform.localScale = Vector3.one * 0.35f;
                    bulb.transform.localPosition = new Vector3(0f, 3.1f, 0f);
                    SetColor(bulb, new Color(1f, 0.96f, 0.84f));

                    var light = pole.AddComponent<Light>();
                    light.type = LightType.Point;
                    light.range = 8f;
                    light.intensity = 0.35f;
                    light.color = new Color(1f, 0.91f, 0.72f);
                    light.transform.localPosition = new Vector3(0f, 3f, 0f);
                }
            }
        }

        static void SetColor(GameObject go, Color c)
        {
            var r = go.GetComponent<Renderer>();
            if (r != null)
            {
                var mat = new Material(Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard"));
                mat.color = c;
                r.sharedMaterial = mat;
            }
        }
    }
}
