using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMap : MonoBehaviour
{

    #region Definition
    class Cloud
    {
        public GameObject Instance { get; set; } = null;

        //Vector3 Position { get; set; } = Vector3.zero;
        public float Cycle { get; set; } = 0f;

        public float Speed { get; set; } = 0f;

        public float Height { get; set; } = 1.2f;

        public Vector3 MovementNormal {
            get
            {
                return _MovementNormal;
            }
            set
            {
                _MovementNormal = value.normalized;

                var secondary = Vector3.up;

                if(Mathf.Abs(Vector3.Dot(_MovementNormal, secondary)) > 0.98f)
                {
                    secondary = Vector3.right;
                }

                _Up = Vector3.Cross(_MovementNormal, secondary).normalized;
                _Side = Vector3.Cross(_MovementNormal, _Up).normalized;
            }
        }


        private Vector3 _MovementNormal = Vector3.forward;
        private Vector3 _Up = Vector3.up;
        private Vector3 _Side = Vector3.right;

        public void updateTransform()
        {
            Cycle += Speed;

            Vector3 pos = Height * Mathf.Cos(Cycle) * _Up + Height * Mathf.Sin(Cycle) * _Side;
            Instance.transform.localPosition = pos;

            var target = Instance.transform.position - Instance.transform.parent.position;
            Instance.transform.up = target.normalized;
        }
    }
    #endregion

    // Prefabs
    public GameObject CloudPrefab;
    //public GameObject MapEventLinkWidgetPrefab;

    public GameObject Camera = null;

    public float CameraOffset = 3f;

    public GameObject Flag = null;

    private List<Cloud> _Clouds = new List<Cloud>();

    private List<WorldMapItem> Items = new List<WorldMapItem>();

    private int _ItemIndex = 0;

    void Start()
    {
        // GetComponent< Renderer>().bounds.center

        //transform.position -= 2f * Vector3.right;
        //transform.position -= 2f * Vector3.forward;


        for(int i=0; i<8; ++i)
        {
            var scale = UnityEngine.Random.Range(0.05f, 0.2f);
            var normal = new Vector3(Random.value, Random.value, Random.value);
            var speed = UnityEngine.Random.Range(0.0001f, 0.001f);
            var height = UnityEngine.Random.Range(1.1f, 1.25f);

            addCloud(Vector3.one * scale, normal, speed, height);
        }

        Items = new List<WorldMapItem>(GetComponentsInChildren<WorldMapItem>());
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(Vector3.up, 0.1f);

        foreach (var c in _Clouds)
        {
            c.updateTransform();
        }

        Flag.transform.position = Items[_ItemIndex].Position;

        Flag.transform.up = (Flag.transform.position - transform.position).normalized;

        var camPos = Camera.transform.position;
        var camRot = Camera.transform.rotation;

        Camera.transform.position = Flag.transform.position + CameraOffset * Flag.transform.up;
        Camera.transform.LookAt(Flag.transform);

        var localPos = Camera.transform.worldToLocalMatrix.MultiplyPoint(transform.position);
        var localRot = Quaternion.Inverse(Camera.transform.rotation) * transform.rotation;

        Camera.transform.position = camPos;
        Camera.transform.rotation = camRot;

        //transform.position = Vector3.Lerp(transform.position, Camera.transform.localToWorldMatrix.MultiplyPoint(localPos), 0.01f);
        transform.rotation = Quaternion.Lerp(transform.rotation, Camera.transform.rotation * localRot, 0.01f);
    }

    void addCloud(Vector3 scale, Vector3 movementNormal, float speed, float height)
    {
        var cloud = new Cloud();
        
        cloud.Instance = Instantiate(CloudPrefab, transform);
        cloud.Instance.layer = LayerMask.NameToLayer("UI");

        cloud.Cycle = Random.value * Mathf.PI * 2f;
        cloud.Instance.transform.localScale = scale;
        cloud.Speed = speed;
        cloud.MovementNormal = movementNormal;
        cloud.Height = height;

        _Clouds.Add(cloud);
    }

    public void next()
    {
        Items[_ItemIndex].GetComponent<MeshRenderer>().material.SetInt("Highlight", 0);

        _ItemIndex = (_ItemIndex + 1) % Items.Count;

        Items[_ItemIndex].GetComponent<MeshRenderer>().material.SetInt("Highlight", 1);
        //Flag.transform.position = Items[_ItemIndex].Position;
    }
}
