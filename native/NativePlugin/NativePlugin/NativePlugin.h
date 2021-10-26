//
//  NativePlugin.h
//  NativePlugin macOS
//
//  Created by Dmitry Victorov on 26.10.2021.
//

#ifndef NativePlugin_h_9fd5b930adb74cfc957720c9cc6f0f23
#define NativePlugin_h_9fd5b930adb74cfc957720c9cc6f0f23

#include <inttypes.h>


typedef struct Argument
{
    uint32_t argId;
    float floats[2];
} Argument;


typedef void (* __cdecl Callback)(Argument arg);

extern char * __cdecl CreateStringFromArg(Argument arg);
extern void __cdecl SetCallback(Callback callback);
extern void __cdecl RunCallback(uint32_t argId, float float1, float float2);


#endif // #ifndef NativePlugin_h_9fd5b930adb74cfc957720c9cc6f0f23
