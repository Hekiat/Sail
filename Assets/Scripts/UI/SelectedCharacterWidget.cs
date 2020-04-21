using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace sail
{
    public class SelectedCharacterWidget : MonoBehaviour
    {
        // Prefab Data
        public Text CharacterNameTxt = null;
        public Image HealthBarForeground = null;
        public Text HealthTxt = null;

        void Start()
        {

        }

        void Update()
        {
            updateUI();
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

            HealthTxt.text = $"{health}/{maxHealth}";

            var healthRect = HealthBarForeground.GetComponent<RectTransform>();
            healthRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)health / maxHealth * 190f);

            // tmp
            var shieldText = GameObject.Find("ShieldText").GetComponent<Text>();
            shieldText.text = $"{inst.SelectedEnemy.Shield}";
        }
    }
}