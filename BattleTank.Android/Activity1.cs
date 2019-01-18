using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using BattleTank.Core;

namespace BattleTank.Android
{
    [Activity(Label = "BattleTank.Android"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = global::Android.Content.PM.LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.Landscape
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize)]
    public class Activity1 : Microsoft.Xna.Framework.AndroidGameActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
       
            base.OnCreate(savedInstanceState);
            this.Window.ClearFlags(WindowManagerFlags.Fullscreen);
            this.Window.AddFlags(WindowManagerFlags.Fullscreen); // hide the status bar

            int uiOptions = (int)Window.DecorView.SystemUiVisibility;

            uiOptions |= (int)SystemUiFlags.LowProfile;
            uiOptions |= (int)SystemUiFlags.Fullscreen;
            uiOptions |= (int)SystemUiFlags.LayoutFullscreen;
            uiOptions |= (int)SystemUiFlags.HideNavigation;
            uiOptions |= (int)SystemUiFlags.LayoutHideNavigation;
            uiOptions |= (int)SystemUiFlags.Immersive;
            uiOptions |= (int)SystemUiFlags.ImmersiveSticky;


            Window.DecorView.SystemUiVisibility = (StatusBarVisibility)uiOptions;





            var g = new Game1();
            SetContentView((View)g.Services.GetService(typeof(View)));
            g.Run();
           

        }


    }
}

