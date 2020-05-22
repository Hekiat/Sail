using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class MapWidget : MonoBehaviour
    {
        // Prefabs
        public GameObject MapEventWidgetPrefab;

        // Child Widgets
        public GameObject EventList;

        // Start is called before the first frame update
        void Start()
        {
            generate(5);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void generate(int step)
        {
            var rootRect = EventList.GetComponent<RectTransform>().rect;
            int[] stepInstCount = new int[step];
            Random.InitState(System.DateTime.Now.Second);

            for (int i = 0; i < step; i++)
            {
                stepInstCount[i] = (i == 0 || i == step - 1) ? 1 : Random.Range(2, 8);

                var hOffset = 100f  + rootRect.width / step * i;

                for (int j = 0; j < stepInstCount[i]; j++)
                {
                    var eventWidget = Instantiate(MapEventWidgetPrefab, EventList.transform);
                    var t = eventWidget.GetComponent<RectTransform>();
                    t.anchorMin = new Vector2(0f, 0f);
                    t.anchorMax = new Vector2(0f, 0f);

                    var vOffset = rootRect.height / (stepInstCount[i]+1) * (j+1);
                    t.anchoredPosition = new Vector2(hOffset, vOffset);
                }
            }
        }
    }
}