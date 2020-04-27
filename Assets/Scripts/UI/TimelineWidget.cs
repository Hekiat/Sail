﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using sail.animation;

namespace sail
{
    public class TimelineWidget : MonoBehaviour
    {
        public GameObject TimelineCharacterPrefab = null;
        public GameObject ActionPreviewWidget = null;

        public int TimeMax = 10;
        public List<TimelineCharacterWidget> characters = new List<TimelineCharacterWidget>();

        private RectTransform RectTrans = null;

        private void Awake()
        {
            RectTrans = GetComponent<RectTransform>();
            ActionPreviewWidget.SetActive(false);
        }

        void Start()
        {
            foreach (var unit in BattleFSM.Instance.enemies)
            {
                var go = Instantiate(TimelineCharacterPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
                go.transform.SetParent(transform);

                var character = go.GetComponent<TimelineCharacterWidget>();
                character.Unit = unit;
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

        void Update()
        {
            var action = BattleFSM.Instance.ActionController.Action;
            if (action == null)
            {
                ActionPreviewWidget.SetActive(false);
                return;
            }

            ActionPreviewWidget.SetActive(true);
            var cost = action.Cost;
            var width = (float)action.Cost / TimeMax * RectTrans.rect.width;
            var rectTransform = ActionPreviewWidget.GetComponent<RectTransform>();

            rectTransform.sizeDelta = new Vector2(width, rectTransform.sizeDelta.y);
        }

        public void updateCharacters()
        {
            foreach (var character in characters)
            {
                character.currentTimer = character.Unit.Cooldown;

                var targetOffset = character.currentTimer / TimeMax * RectTrans.rect.width;
                var target = new Vector2(targetOffset, character.RectTrans.anchoredPosition.y);
                character.RectTrans.MoveAnchoredPositionTo(target);
            }
        }
    }
}
