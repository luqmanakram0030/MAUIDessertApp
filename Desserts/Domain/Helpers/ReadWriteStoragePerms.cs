using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desserts.Domain.Helpers
{
    public class ReadWriteStoragePerms : Permissions.BasePlatformPermission
    {
#if ANDROID
        public override (string androidPermission, bool isRuntime)[] RequiredPermissions => new List<(string androidPermission, bool isRuntime)>
    {
        (Android.Manifest.Permission.ReadExternalStorage, true),
        (Android.Manifest.Permission.WriteExternalStorage, true)
    }.ToArray();
#endif
    }
}
