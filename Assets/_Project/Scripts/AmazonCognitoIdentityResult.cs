#region Assembly AWSSDK.CognitoIdentity, Version=3.3.0.0, Culture=neutral, PublicKeyToken=null
// D:\Dhaval\Projects\Unity\bitszer-unity-client-master\Assets\Plugins\AWSSDK.CognitoIdentity.dll
// Decompiled with ICSharpCode.Decompiler 6.1.0.5902
#endregion

using System;

namespace Amazon.CognitoIdentity
{
    public class AmazonCognitoIdentityResult<TResponse>
    {
        public TResponse Response
        {
            get;
            internal set;
        }

        public Exception Exception
        {
            get;
            internal set;
        }

        public object State
        {
            get;
            internal set;
        }

        public AmazonCognitoIdentityResult(object state)
        {
            State = state;
        }

        public AmazonCognitoIdentityResult(TResponse response, Exception exception, object state)
        {
            Response = response;
            Exception = exception;
            State = state;
        }
    }
}
#if false // Decompilation log
'226' items in cache
------------------
Resolve: 'mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
WARN: Version mismatch. Expected: '2.0.0.0', Got: '4.0.0.0'
Load from: 'C:\Program Files\Unity\Hub\Editor\2020.3.26f1\Editor\Data\NetStandard\compat\2.0.0\shims\netfx\mscorlib.dll'
------------------
Resolve: 'AWSSDK.Core, Version=3.3.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'AWSSDK.Core, Version=3.3.0.0, Culture=neutral, PublicKeyToken=885c28607f98e604'
Load from: 'D:\Dhaval\Projects\Unity\bitszer-unity-client-master\Assets\Plugins\AWSSDK.Core.dll'
------------------
Resolve: 'AWSSDK.SecurityToken, Version=3.3.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'AWSSDK.SecurityToken, Version=3.3.0.0, Culture=neutral, PublicKeyToken=885c28607f98e604'
Load from: 'D:\Dhaval\Projects\Unity\bitszer-unity-client-master\Assets\Plugins\AWSSDK.SecurityToken.dll'
------------------
Resolve: 'System.Core, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
WARN: Version mismatch. Expected: '3.5.0.0', Got: '4.0.0.0'
Load from: 'C:\Program Files\Unity\Hub\Editor\2020.3.26f1\Editor\Data\NetStandard\compat\2.0.0\shims\netfx\System.Core.dll'
------------------
Resolve: 'System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
WARN: Version mismatch. Expected: '2.0.0.0', Got: '4.0.0.0'
Load from: 'C:\Program Files\Unity\Hub\Editor\2020.3.26f1\Editor\Data\NetStandard\compat\2.0.0\shims\netfx\System.dll'
------------------
Resolve: 'Microsoft.Win32.Registry, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Could not find by name: 'Microsoft.Win32.Registry, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
------------------
Resolve: 'netstandard, Version=2.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Found single assembly: 'netstandard, Version=2.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Load from: 'C:\Program Files\Unity\Hub\Editor\2020.3.26f1\Editor\Data\NetStandard\ref\2.0.0\netstandard.dll'
------------------
Resolve: 'System.Security.Principal.Windows, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Could not find by name: 'System.Security.Principal.Windows, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
------------------
Resolve: 'System.Security.AccessControl, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Could not find by name: 'System.Security.AccessControl, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
------------------
Resolve: 'System.CodeDom, Version=4.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Could not find by name: 'System.CodeDom, Version=4.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
#endif
