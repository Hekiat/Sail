using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Assertions;

namespace sail
{
    public class DialogueData
    {
        public DialogueData(LocalizedString characterName, List<LocalizedString> dialogues)
        {
            CharacterName = characterName;
            Dialogues = new Queue<LocalizedString>(dialogues);
        }

        public LocalizedString CharacterName { get; set; } = null;
        public Queue<LocalizedString> Dialogues { get; set; } = new Queue<LocalizedString>();
    }

    public class DialogueWidget : MonoBehaviour
    {
        public Image _Portrait = null;

        public CanvasGroup _MainPanel = null;

        private TextMeshProUGUI _Text = null;
        private LocalizeStringEvent _LocalizedAsset = null;
        
        private DialogueData _Data = null;

        private bool _InstantRoll = false;

        private bool _Rolling = false;

        public void setLocalizedString(LocalizedString str)
        {
            _LocalizedAsset.StringReference = str;
        }

        void Start()
        {
            _Text = GetComponentInChildren<TextMeshProUGUI>();
            _LocalizedAsset = GetComponentInChildren<LocalizeStringEvent>();

            Assert.IsNotNull(_Text, "Component not found.");
            Assert.IsNotNull(_LocalizedAsset, "Component not found.");

            GlobalManagers.dialogueManager.registerWidget(this);
            gameObject.SetActive(false);
            hide();
        }

        void Update()
        {
            if(_Data == null)
            {
                return;
            }

            bool accepted = Input.GetKeyDown(KeyCode.Return);

            if (_Rolling == false && accepted)
            {
                if (_Data.Dialogues.Count == 0)
                {
                    hide();
                }
                else
                {
                    next();
                }
            }
            else if(accepted)
            {
                _InstantRoll = true;
            }
        }

        public void request(LocalizedString characterName, List<LocalizedString> dialogues)
        {
            _Data = new DialogueData(characterName, dialogues);

            show();

            next();
        }

        void next()
        {
            var text = _Data.Dialogues.Dequeue();
            StartCoroutine(displayText(text));
        }

        void show()
        {
            gameObject.SetActive(true);

            var rectTransform = _MainPanel.transform as RectTransform;

            LeanTween.moveX(rectTransform, 0, 0.5f).setDelay(0.2f).setEaseOutBack();
            LeanTween.alphaCanvas(_MainPanel, 1f, 0.4f).setDelay(0.2f);

            LeanTween.moveX(_Portrait.rectTransform, 0, 0.5f).setEaseOutBack();
            LeanTween.alpha(_Portrait.rectTransform, 1f, 0.4f);
        }

        void hide()
        {
            var rectTransform = _MainPanel.transform as RectTransform;
            LeanTween.moveX(rectTransform, 1000, 0.5f).setDelay(0.4f).setEaseInBack().setOnComplete(() => gameObject.SetActive(false));
            LeanTween.alphaCanvas(_MainPanel, 0f, 0.4f).setDelay(0.4f);

            LeanTween.moveX(_Portrait.rectTransform, -300f, 0.5f).setEaseInBack();
            LeanTween.alpha(_Portrait.rectTransform, 0f, 0.4f);
        }

        IEnumerator displayText(LocalizedString text)
        {
            var localizedText = text.GetLocalizedString();

            _Rolling = true;

            _Text.text = "";
            foreach (var letter in localizedText)
            {
                if (_InstantRoll == false)
                {
                    _Text.text += letter;
                    yield return null;
                }
                else
                {
                    _Text.text = localizedText;
                    _InstantRoll = false;
                    _Rolling = false;
                    break;
                }
            }

            _Rolling = false;
        }
    }

}
