using Microsoft.Maui.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
#if IOS
using UIKit;
using Foundation;
#elif ANDROID
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
#endif

namespace Desserts.Helpers
{
    public static class FormHandler
    {
        public static void RemoveBorders()
        {
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("Borderless", (handler, view) =>
            {
#if ANDROID
              
                handler.PlatformView.BackgroundTintList =
                    Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);

#elif IOS
                handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
            handler.PlatformView.Layer.BorderWidth = 0;
            handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
            });
            Microsoft.Maui.Handlers.EditorHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
            {
#if ANDROID
                handler.PlatformView.BackgroundTintList =
                    Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#elif IOS
                handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
            handler.PlatformView.Layer.BorderWidth = 0;
            //handler.PlatformView.B = UIKit.UITextBorderStyle.None;
#endif
            });
            Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping("Borderless", (handler, view) =>
            {
#if ANDROID
                 handler.PlatformView.BackgroundTintList =
                    Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#elif IOS
                handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
            handler.PlatformView.Layer.BorderWidth = 0;
            handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
            });

        }

    }
}
