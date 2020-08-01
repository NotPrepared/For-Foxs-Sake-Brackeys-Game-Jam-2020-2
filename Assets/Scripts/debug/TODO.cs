using System;
using UnityEngine;

namespace debug
{
    public static class TODO
    {
        public static void asException(string msg) => throw new TODO_Exception(msg);
        public static void asLogWarning(string msg) => Debug.LogWarning(msg);

        private class TODO_Exception : Exception
        {
            public TODO_Exception(string msg) : base(msg)
            { /*Ignored*/ }
        }
    }
}