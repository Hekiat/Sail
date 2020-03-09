using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class TimelineWidget : MonoBehaviour
    {
        public GameObject TimelineCharacterPrefab = null;

        public float MaxTimer = 1000f;
        public List<TimelineCharacterWidget> characters = new List<TimelineCharacterWidget>();

        void Start()
        {
            for (int i = 0; i < 3; i++)
            {
                var go = Instantiate(TimelineCharacterPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
                go.transform.SetParent(transform);

                var character = go.GetComponent<TimelineCharacterWidget>();
                character.currentTimer = i * 100f;
                characters.Add(character);

                var rectTrans = character.GetComponent<RectTransform>();
                rectTrans.localPosition = Vector3.zero;
                rectTrans.anchorMin = Vector2.zero;
                rectTrans.anchorMax = Vector2.zero;
                rectTrans.pivot = new Vector2(0.5f, 1f);
            }
        }

        // Update is called once per frame
        void Update()
        {
            updateCharacters();
        }

        bool a = false;

        void updateCharacters()
        {
            //if (a == true)
            //{
            //    return;
            //}
            //
            var rootRectTrans = GetComponent<RectTransform>();

            foreach (var character in characters)
            {
                var rectTrans = character.GetComponent<RectTransform>();
                var newPos = rectTrans.anchoredPosition;
                newPos.x = character.currentTimer / MaxTimer * rootRectTrans.rect.width;
                rectTrans.anchoredPosition = newPos;
            }
            a = true;
        }
    }
}