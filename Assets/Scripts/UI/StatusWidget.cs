using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace sail
{
    public class StatusWidget : MonoBehaviour
    {
        public Image Image;
        public Text Text;

        public StatusBase Status { get { return _Status; } set { _Status = value; init(); } }
        public Sprite Sprite { get { return Image.sprite; } set { Image.sprite = value; } }
        public Color Color { get { return Image.color; } set { Image.color = value; } }
        public Color TextColor { get { return Text.color; } set { Text.color = value; } }

        public List<Sprite> StatusSprites;

        private StatusBase _Status;

        void Start()
        {
        }

        void Update()
        {
            if (Status == null)
            {
                Destroy(this);
                return;
            }

            Text.text = Status.Value.ToString();
        }

        void init()
        {
            if (Status is ShieldStatus)
            {
                Color = new Color(0f, 0.5f, 1f);
                Sprite = StatusSprites[0]; // tmp
            }
            else if (Status is FireStatus)
            {
                Color = new Color(1f, 0.5f, 0f);
                Sprite = StatusSprites[1]; // tmp
            }
        }
    }
}
