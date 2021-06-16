using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace sail
{
    public class CharacterInfoManager : MonoBehaviour
    {
        public CharacterInfo Infos = null;

        private void Awake()
        {
            GlobalManagers.characterInfoManager = this;
        }

        public new LocalizedString name(CharacterID id)
        {
            var item = Infos.Datas.Find((e) => e.CharacterID == id);

            if(item == null)
            {
                return new LocalizedString();
            }

            return item.Name;
        }
    }
}