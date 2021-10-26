using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;


namespace Fiftytwo
{
    public class NativePluginController : MonoBehaviour
    {
        [SerializeField] private Button _registerCallbackButton;
        [SerializeField] private Button _unregisterCallbackButton;
        [SerializeField] private Button _runCallbackButton;
        [SerializeField] private Button _toStringButton;

        [SerializeField] private InputField _argIdInput;
        [SerializeField] private InputField _float1Input;
        [SerializeField] private InputField _float2Input;

        [SerializeField] private Text _callbackOut;
        [SerializeField] private Text _stringOut;


#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX

        private void OnEnable()
        {
            _registerCallbackButton.onClick.AddListener(RegisterCallbackButtonOnClick);
            _unregisterCallbackButton.onClick.AddListener(UnregisterCallbackButtonOnClick);
            _runCallbackButton.onClick.AddListener(RunCallbackButtonOnClick);
            _toStringButton.onClick.AddListener(ToStringButtonOnClick);
            
            NativePlugin.Event += NativePluginOnEvent;
        }

        private void OnDisable()
        {
            _registerCallbackButton.onClick.RemoveListener(RegisterCallbackButtonOnClick);
            _unregisterCallbackButton.onClick.RemoveListener(UnregisterCallbackButtonOnClick);
            _runCallbackButton.onClick.RemoveListener(RunCallbackButtonOnClick);
            _toStringButton.onClick.RemoveListener(ToStringButtonOnClick);

            NativePlugin.Event -= NativePluginOnEvent;
        }

        private void RegisterCallbackButtonOnClick()
        {
            NativePlugin.RegisterCallback();
        }
        
        private void UnregisterCallbackButtonOnClick()
        {
            NativePlugin.UnregisterCallback();
        }

        private void RunCallbackButtonOnClick()
        {
            uint.TryParse(_argIdInput.text, NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingWhite,
                CultureInfo.InvariantCulture, out var argId);
            float.TryParse(_float1Input.text, NumberStyles.Number, CultureInfo.InvariantCulture, out var float1);
            float.TryParse(_float2Input.text, NumberStyles.Number, CultureInfo.InvariantCulture, out var float2);
            NativePlugin.RunCallback(argId, float1, float2);
        }

        private void ToStringButtonOnClick()
        {
            // _stringOut.text = $"String Out: {Marshal.SizeOf<NativePlugin.Argument>()}";
            // return;

            uint.TryParse(_argIdInput.text, NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingWhite,
                CultureInfo.InvariantCulture, out var argId);
            float.TryParse(_float1Input.text, NumberStyles.Number, CultureInfo.InvariantCulture, out var float1);
            float.TryParse(_float2Input.text, NumberStyles.Number, CultureInfo.InvariantCulture, out var float2);

            var arg = NativePlugin.Argument.Create(argId, float1, float2);
            var str = NativePlugin.CreateStringFromArg(arg);
            _stringOut.text = $"String Out: {str}";
        }

        private void NativePluginOnEvent(NativePlugin.Argument arg)
        {
            _callbackOut.text = $"Callback Out: {arg}";
        }

#endif
    }
}
