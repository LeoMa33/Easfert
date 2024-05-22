using System;
using System.Collections.Generic;
using Android;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Ho.BLE;
using Ho.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Application = Android.App.Application;

[assembly: Dependency(typeof(Ho.Droid.BLETest))]
namespace Ho.Droid
{
    [Activity(Label = "Easfert", Icon = "@mipmap/ic_launcher",Theme = "@style/MainTheme", MainLauncher = false,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        
        //private BleServer _bleServer;
        
        private const int REQUEST_PERMISSION_CODE = 1001;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
            initPermission();
        }
        private void initPermission()
        {
            List<String> mPermissionList = new List<string>();
            // When the Android version is 12 or greater, apply for new Bluetooth permissions
            if (Build.VERSION.SdkInt >= BuildVersionCodes.S)
            {
                mPermissionList.Add(Manifest.Permission.BluetoothScan);
                mPermissionList.Add(Manifest.Permission.BluetoothAdvertise);
                mPermissionList.Add(Manifest.Permission.BluetoothConnect);
                //Request for location permissions based on your actual needs
                //mPermissionList.add(Manifest.permission.ACCESS_COARSE_LOCATION);
                //mPermissionList.add(Manifest.permission.ACCESS_FINE_LOCATION);
            }
            else
            {
                mPermissionList.Add(Manifest.Permission.AccessCoarseLocation);
                mPermissionList.Add(Manifest.Permission.AccessFineLocation);
            }

            ActivityCompat.RequestPermissions(this, mPermissionList.ToArray(), REQUEST_PERMISSION_CODE);
            //_bleServer = new BleServer(this.ApplicationContext);
        }
    }

    public class BLETest:IBLEServer
    {
        private BleServer _bleServer;

        public BLEClient.ConnectionStates ConnectionState => _bleServer.ConnectionState;

        public void StartAdvert(string userProfil)
        {
            _bleServer = new BleServer(Application.Context, userProfil);
        }
    }
}