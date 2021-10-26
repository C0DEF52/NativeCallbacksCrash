#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX

using System.Runtime.InteropServices;


namespace Fiftytwo
{
    public static class NativePlugin
    {
        // Not good!
        // https://docs.microsoft.com/en-US/dotnet/standard/native-interop/best-practices#fixed-buffers
        // https://docs.microsoft.com/ru-ru/dotnet/standard/native-interop/best-practices#fixed-buffers
        // However, there are some gotchas with fixed buffers. Fixed buffers of non-blittable types won't be correctly
        // marshaled, so the in-place array needs to be expanded out to multiple individual fields. Additionally, in
        // .NET Framework and .NET Core before 3.0, if a struct containing a fixed buffer field is nested within a
        // non-blittable struct, the fixed buffer field won't be correctly marshaled to native code.
        //
        // [StructLayout(LayoutKind.Sequential)]
        // public unsafe struct Argument
        // {
        //     public uint argId;
        //     public fixed float floats[2];
        //     
        //     public static Argument Create( uint argId, float float1, float float2 )
        //     {
        //         var arg = new Argument();
        //         arg.argId = argId;
        //         arg.floats[0] = float1;
        //         arg.floats[1] = float2;
        //         return arg;
        //     }
        //
        //     public override string ToString()
        //     {
        //         return $"{argId} : ({floats[0]}; {floats[1]})";
        //     }
        // }

        //This binding version does not crash
        [StructLayout(LayoutKind.Sequential)]
        public struct Argument
        {
            public uint argId;
            public float float1;
            public float float2;
        
            public static Argument Create( uint argId, float float1, float float2 )
            {
                return new Argument()
                {
                    argId = argId,
                    float1 = float1,
                    float2 = float2
                };
            }
        
            public override string ToString()
            {
                return $"{argId} : ({float1}; {float2})";
            }
        }


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
        public static extern string CreateStringFromArg(Argument arg);

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
