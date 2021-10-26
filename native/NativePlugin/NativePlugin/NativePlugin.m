//
//  NativePlugin.m
//  NativePlugin macOS
//
//  Created by Dmitry Victorov on 26.10.2021.
//

#import "NativePlugin.h"


static Callback _callback = NULL;


void __cdecl SetCallback(Callback callback)
{
    _callback = callback;
}

void __cdecl RunCallback(uint32_t argId, float float1, float float2)
{
    Argument arg =
    {
        argId,
        { float1, float2 }
    };

    if (_callback)
        _callback(arg);
}
