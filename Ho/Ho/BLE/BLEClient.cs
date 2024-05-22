using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json;
using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BLE.Abstractions.Exceptions;
using Xamarin.Forms;

namespace Ho.BLE
{
    public class BLEClient
    {
        public enum ConnectionStates
        {
            Waiting,
            Connected,
            Reading,
            Writing,
            End
        }
        
        
        private static readonly string BLEServiceUID = "def472d6-8829-4d1d-a1b9-6cb12d64b4c0";
        private readonly string BLECharacteristicUID_Data = "4c908b39-952f-4d0e-adb1-2b9382902840";
        private readonly string BLEDescriptorUID_DATA = "7f369f3d-8caf-4306-8c5f-ab8be020d63f";

        public static ConnectionStates ConnectionState = ConnectionStates.Waiting;
        
        private static IAdapter adapter;
        private static List<IDevice> deviceList;
        
        public static void BLEConnection()
        {
        
            var ble = CrossBluetoothLE.Current;
            adapter = CrossBluetoothLE.Current.Adapter;
            deviceList = new List<IDevice>();
            adapter.ScanTimeout = 5000;

            var state = ble.State;

            ble.StateChanged += (s, e) =>
            {
                Debug.WriteLine($"The bluetooth state changed to {e.NewState}");
            };

            adapter.DeviceDiscovered += OnDeviceDiscovered;
            adapter.DeviceConnected += OnDeviceConnected;
            ScanDevi(adapter);
        }

        private static async void OnDeviceConnected(object sender, DeviceEventArgs args)
        {
            ConnectionState = ConnectionStates.Connected;
            Debug.WriteLine("Connected");
            try
            {
                if (args.Device != null)
                {
                    var service = await args.Device.GetServiceAsync(Guid.Parse(BLEServiceUID));
                    var characteristics = await service.GetCharacteristicsAsync();
                    var characteristic = characteristics[0];

                    ConnectionState = ConnectionStates.Reading;

                    string t = "";
                    while (t == "" || t.Last() != '\n')
                    {
                        var bytes = await characteristic.ReadAsync();
                        t += Encoding.UTF8.GetString(bytes);
                        if (Encoding.UTF8.GetString(bytes) == "")
                        {
                            break;
                        }
                        Debug.WriteLine(Encoding.UTF8.GetString(bytes));
                    }
                    
                    Debug.WriteLine(t);
                    
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        User nU = JsonConvert.DeserializeObject<User>(t);
                        if (nU != null)
                        {
                            Data.AddUserToList(nU);
                            MessagingCenter.Send<BLEClient, User>(new BLEClient(), "new user", nU);
                        }

                    });

                    ConnectionState = ConnectionStates.End;



                }
                else
                {
                    Debug.WriteLine("No Device");
                    ConnectionState = ConnectionStates.End;
                }
            }
            catch (Exception ex)
            {
                ConnectionState = ConnectionStates.End;
                Debug.WriteLine(ex);
            }

            
        }

        private static void OnDeviceDiscovered(object sender, DeviceEventArgs args)
        {
            Debug.WriteLine(args.Device.ToString());
            deviceList.Add(args.Device);
            adapter.StopScanningForDevicesAsync();
        }

        static async private void ScanDevi(IAdapter adapter)
        {
            deviceList.Clear();
            var scanFilterOptions = new ScanFilterOptions();
            scanFilterOptions.ServiceUuids = new [] {Guid.Parse(BLEServiceUID)};
            ConnectionState = ConnectionStates.Waiting;
            await adapter.StartScanningForDevicesAsync(scanFilterOptions);
            
            if(deviceList.Count() != 0) Connect(adapter, deviceList[0]);
            if (adapter.IsScanning)
            {
                await adapter.StopScanningForDevicesAsync();
            }
            
        }

        private static async void Connect(IAdapter adapter, IDevice device)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    var connectParameters = new ConnectParameters(forceBleTransport: true);
                    await adapter.ConnectToDeviceAsync(device, connectParameters);
                }
                catch (DeviceConnectionException ex)
                {
                    Debug.WriteLine(ex);
                }
            });
                
        }
    }
}