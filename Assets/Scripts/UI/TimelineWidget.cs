using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace sail
{
    public class TimelineWidget : MonoBehaviour
    {
        public GameObject TimelineCharacterPrefab = null;
        public GameObject RangeWidget = null;

        public int TimeMax = 20;
        public List<TimelineCharacterWidget> characters = new List<TimelineCharacterWidget>();

        private RectTransform RectTrans = null;

        private void Awake()
        {
            RectTrans = GetComponent<RectTransform>();
            RangeWidget.SetActive(false);
        }

        void Start()
        {
            //for (int i = 0; i < 3; i++)
            foreach (var unit in BattleFSM.Instance.units)
            {
                var go = Instantiate(TimelineCharacterPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
                go.transform.SetParent(transform);

                var character = go.GetComponent<TimelineCharacterWidget>();
                character.unit = unit;
                character.currentTimer = unit.Cooldown;
                characters.Add(character);

                var rectTrans = character.GetComponent<RectTransform>();
                rectTrans.localPosition = Vector3.zero;
                rectTrans.anchorMin = Vector2.zero;
                rectTrans.anchorMax = Vector2.zero;
                rectTrans.pivot = new Vector2(0.5f, 1f);
            }

            foreach (var character in characters)
            {
                var pos = character.RectTrans.anchoredPosition;
                pos.x = character.currentTimer / TimeMax * RectTrans.rect.width;
                character.RectTrans.anchoredPosition = pos;
            }
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void updateCharacters()
        {
            foreach (var character in characters)
            {
                StartCoroutine(moveToTransition(character, character.unit.Cooldown));
            }
        }

        IEnumerator moveToTransition(TimelineCharacterWidget character, float timer)
        {
            character.currentTimer = timer;

            int duration = 20;
            var initOffset = character.RectTrans.anchoredPosition.x;
            var targetOffset = character.currentTimer / TimeMax * RectTrans.rect.width;

            var delta = targetOffset - initOffset;
            var deltaSample = delta / duration;

            for (int i = 0; i < duration; i++)
            {
                character.RectTrans.anchoredPosition += Vector2.right * deltaSample;
                yield return null;
            }
        }

        public void moveTo(TimelineCharacterWidget character, float timer)
        {
            StartCoroutine(moveToTransition(character, timer));
        }
    }
    [CustomEditor(typeof(TimelineWidget))]
    public class TimelineWidgetEditor : Editor
    {
        float timer = 0f;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            timer = EditorGUILayout.FloatField("New Timer:", timer);

            var tw = (TimelineWidget)target;
            if (GUILayout.Button("Switch"))
            {
                tw.moveTo(tw.characters[0], timer);
            }
        }
    }
}
