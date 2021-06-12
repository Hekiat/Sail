using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace sail
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "CharacterConfiguration", menuName = "Custom/CharacterConfiguration", order = 1)]
    public class CharacterConfiguration : ScriptableObject
    {
        public CharacterID ID = CharacterID.Invalid;

        public LocalizedString Name = null;
    }
}