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

        public TextMeshProUGUI _MainText = null;
        public LocalizeStringEvent _MainLocalizedAsset = null;

        public LocalizeStringEvent _CharacterLocalizedAsset = null;

        private DialogueData _Data = null;

        private bool _InstantRoll = false;

        private bool _Rolling = false;

        void Start()
        {
            //_MainText = GetComponentInChildren<TextMeshProUGUI>();
            //_MainLocalizedAsset = GetComponentInChildren<LocalizeStringEvent>();

            Assert.IsNotNull(_MainText, "Component not found.");
            Assert.IsNotNull(_MainLocalizedAsset, "Component not found.");

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

        public void request(CharacterID character, List<LocalizedString> dialogues)
        {
            _Data = new DialogueData(GlobalManagers.characterInfoManager.name(character), dialogues);
            
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

            _CharacterLocalizedAsset.StringReference = _Data.CharacterName;

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

            _MainText.text = "";
            foreach (var letter in localizedText)
            {
                if (_InstantRoll == false)
                {
                    _MainText.text += letter;
                    yield return null;
                }
                else
                {
                    _MainText.text = localizedText;
                    _InstantRoll = false;
                    _Rolling = false;
                    break;
                }
            }

            _Rolling = false;
        }
    }

}
