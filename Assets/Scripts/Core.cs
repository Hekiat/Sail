using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public static class Layer
    {
        // Unity Default Layers
        public const int Default = 0;
        public const int TransparentFX = 1;
        public const int IgnoreRaycast = 2;
        public const int Undefined0 = 3;
        public const int Water = 4;
        public const int UI = 5;
        public const int Undefined1 = 6;
        public const int Undefined2 = 7;

        // Custom Layers
        public const int Terrain = 10;
        public const int Characters = 11;
    }

    public static class LayerMask
    {
        // Unity Default Layers
        public const int Default = 1 << Layer.Default;
        public const int TransparentFX = 1 << Layer.TransparentFX;
        public const int IgnoreRaycast = 1 << Layer.IgnoreRaycast;
        public const int Undefined0 = 1 << Layer.Undefined0;
        public const int Water = 1 << Layer.Water;
        public const int UI = 1 << Layer.UI;
        public const int Undefined1 = 1 << Layer.Undefined1;
        public const int Undefined2 = 1 << Layer.Undefined2;

        // Custom Layers
        public const int Terrain = 1 << Layer.Terrain;
        public const int Characters = 1 << Layer.Characters;
    }

    public static class MouseButton
    {
        public const int Left = 0;
        public const int Right = 1;
        public const int Middle = 2;
    }

    public class Core
    {

    }
}

//public static class ExtensionMethods
//{
//    public static void ResetTransformation(this Transform trans)
//    {
//        trans.position = Vector3.zero;
//        trans.localRotation = Quaternion.identity;
//        trans.localScale = new Vector3(1, 1, 1);
//    }
//}



