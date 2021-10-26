using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;


namespace Fiftytwo
{
    public class NativePluginController : MonoBehaviour
    {
        [SerializeField] private Button _registerCallbackButton;
        [SerializeField] private Button _unregisterCallbackButton;
        [SerializeField] private Button _runCallbackButton;

        [SerializeField] private InputField _argIdInput;
        [SerializeField] private InputField _float1Input;
        [SerializeField] private InputField _float2Input;

        [SerializeField] private Text _callbackOut;


#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX

        private void OnEnable()
        {
            _registerCallbackButton.onClick.AddListener(RegisterCallbackButtonOnClick);
            _unregisterCallbackButton.onClick.AddListener(UnregisterCallbackButtonOnClick);
            _runCallbackButton.onClick.AddListener(RunCallbackButtonOnClick);
            
            NativePlugin.Event += NativePluginOnEvent;
        }

        private void OnDisable()
        {
            _registerCallbackButton.onClick.RemoveListener(RegisterCallbackButtonOnClick);
            _unregisterCallbackButton.onClick.RemoveListener(UnregisterCallbackButtonOnClick);
            _runCallbackButton.onClick.RemoveListener(RunCallbackButtonOnClick);
            
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

        private void NativePluginOnEvent(NativePlugin.Argument arg)
        {
            _callbackOut.text = $"Callback Out: {arg}";
        }

#endif
    }
}
