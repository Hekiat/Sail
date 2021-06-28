using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace sail
{
    public class CharacterData : ScriptableObject
    {
        public CharacterID CharacterID = CharacterID.Invalid;
        public LocalizedString Name;
    }

    [CreateAssetMenu(fileName = "CharacterInfo", menuName = "Custom/CharacterInfos", order = 1)]
    public class CharacterInfo : ScriptableObject
    {

        [NaughtyAttributes.Expandable]
        public List<CharacterData> Datas = new List<CharacterData>();
    }
}
