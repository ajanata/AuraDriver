Simple .NET wrapper and test program for the Asus Aura SDK.

The SDK DLL is only available in 32-bit form, so this has to do silly stuff with COM to be able to use it in a 64-bit client application. Some manual steps seem to be required to trigger the COM surrogate process; I followed http://blogs.microsoft.co.il/sasha/2014/01/07/hosting-a-net-dll-as-an-out-of-process-com-server/ and got it working. I believe the only important part that needs done is step 4.

The demo application assumes that your LED controllers all have 7 LEDs. Adjust startingColors if this is not the case; if they do not have the same number of LEDs then it will require more work to function.
