#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX

using System.Runtime.InteropServices;


namespace Fiftytwo
{
    public static class NativePlugin
    {
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct Argument
        {
            public uint argId;
            public fixed float floats[2];
        
            public override string ToString()
            {
                return $"{argId} : ({floats[0]}; {floats[1]})";
            }
        }

        // This binding version does not crash
        // [StructLayout(LayoutKind.Sequential)]
        // public struct Argument
        // {
        //     public uint argId;
        //     public float float1;
        //     public float float2;
        //
        //     public override string ToString()
        //     {
        //         return $"{argId} : ({float1}; {float2})";
        //     }
        // }


        public delegate void Callback(Argument arg);

        private const string DllName = "NativePlugin";

        public static event Callback Event;

        private static Callback _callback;


        public static void RegisterCallback()
        {
            _callback = OnCallback;
            SetCallback(_callback);
        }

        public static void UnregisterCallback()
        {
            SetCallback(null);
            _callback = null;
        }

        [DllImport( DllName, CallingConvention = CallingConvention.Cdecl )]
        private static extern void SetCallback(Callback callback);

        [DllImport( DllName, CallingConvention = CallingConvention.Cdecl )]
        public static extern void RunCallback(uint arg1, float arg2, float arg3);


        [AOT.MonoPInvokeCallback(typeof(Callback))]
        private static void OnCallback(Argument arg)
        {
            Event?.Invoke(arg);
        }
    }
}

#endif
