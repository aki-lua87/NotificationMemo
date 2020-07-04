using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Prism;
using Prism.Ioc;
using Android.Provider;
using System;

namespace memo2.Droid
{
    [Activity(Label = "通知メモ", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App(new AndroidInitializer()));

            if (!Settings.CanDrawOverlays(this))
            {
                var dlg = new AlertDialog.Builder(this);
                dlg.SetTitle("確認");
                dlg.SetMessage("他のアプリの上に重ねて表示する許可をしてください。");
                dlg.SetPositiveButton("はい", (sender, arg) =>
                {
                    // 権限確認
                    var intent = new Intent(Settings.ActionManageOverlayPermission, Android.Net.Uri.Parse("package:" + Android.App.Application.Context.PackageName));
                    StartActivity(intent);
                });
                dlg.Create().Show();
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
        }
    }
}

