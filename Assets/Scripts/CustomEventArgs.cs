using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace sail
{
    public class CustomEventArgs<T> : EventArgs
    {
        public T value;

        public CustomEventArgs()
        {
            value = default(T);
        }

        public CustomEventArgs(T value)
        {
            this.value = value;
        }
    }
}