using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace sail
{
    public class SelectedCharacterWidget : MonoBehaviour
    {
        // Input
        public GameObject StatusWidgetPrefab;

        // UI Info
        public TextMeshProUGUI CharacterNameTxt = null;
        public Image HealthBarForeground = null;
        public TextMeshProUGUI HealthTxt = null;
        public GameObject StatusListWidget = null;

        private List<StatusWidget> StatusList = new List<StatusWidget>();

        void Start()
        {
        }

        void Update()
        {
            updateUI();
            updateStatus();
        }

        void updateUI()
        {
            var inst = BattleFSM.Instance;
            if (inst.SelectedEnemy == null)
            {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);

            CharacterNameTxt.text = inst.SelectedEnemy.UnitName;

            var health = inst.SelectedEnemy.Health;
            var maxHealth = inst.SelectedEnemy.MaxHealth;

            HealthTxt.text = $"{health} / {maxHealth}";

            var healthRect = HealthBarForeground.GetComponent<RectTransform>();
            healthRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)health / maxHealth * 190f);
        }

        void updateStatus()
        {
            var statusList = BattleFSM.Instance.SelectedEnemy.Status;
            foreach (var status in statusList)
            {
                var widget = StatusList.Find((s) => s.Status == status);
                if (widget == null)
                {
                    var statusGO = Instantiate(StatusWidgetPrefab, StatusListWidget.transform);
                    widget = statusGO.GetComponent<StatusWidget>();
                    widget.Status = status;
                    StatusList.Add(widget);
                }
            }
        }
    }
}