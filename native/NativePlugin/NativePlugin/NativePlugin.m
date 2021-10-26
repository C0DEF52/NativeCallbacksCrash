//
//  NativePlugin.m
//  NativePlugin macOS
//
//  Created by Dmitry Victorov on 26.10.2021.
//

#include <string.h>
#include <stdio.h>
#import "NativePlugin.h"


static Callback _callback = NULL;


char * __cdecl CreateStringFromArg(Argument arg)
{
    char buffer[4096];
    snprintf(buffer, sizeof(buffer), "%d : (%f; %f)", arg.argId, arg.floats[0], arg.floats[1]);

    // Unity manual says it is safe to return string allocated by C malloc()
    // They will be free()'ed by managed code after marshaling. strdup() uses malloc() under the hood.
    return strdup(buffer);
}

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
