using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Android.Bluetooth;
using Android.Bluetooth.LE;
using Android.Content;
using Android.OS;
using Ho.BLE;
using Java.Util;
using Debug = System.Diagnostics.Debug;
using Random = System.Random;

namespace Ho.Droid
{
    public class BleServer
    {

        private readonly string BLEServiceUID = "def472d6-8829-4d1d-a1b9-6cb12d64b4c0";
        private readonly string BLECharacteristicUID_Data = "4c908b39-952f-4d0e-adb1-2b9382902840";
        private readonly string BLEDescriptorUID_DATA = "7f369f3d-8caf-4306-8c5f-ab8be020d63f";
        
        
        private readonly BluetoothManager _bluetoothManager;
        private BluetoothAdapter _bluetoothAdapter;
        private BleGattServerCallback _bluettothServerCallback;
        private BluetoothGattServer _bluetoothServer;
        private BluetoothGattCharacteristic _characteristic;

        public BLEClient.ConnectionStates ConnectionState = BLEClient.ConnectionStates.Waiting;
        
        private int count = 0;
        private BluetoothLeAdvertiser myBluetoothLeAdvertiser;


        [Obsolete("Obsolete")]
        public BleServer(Context ctx, string userProfil)
        {
            
            _bluetoothManager = (BluetoothManager)ctx.GetSystemService(Context.BluetoothService);

            if (_bluetoothManager is null)
            {
                Debug.WriteLine("No bluetooth Manager");
                return;
            }
            
            _bluetoothAdapter = _bluetoothManager.Adapter;

            _bluettothServerCallback = new BleGattServerCallback();
            _bluetoothServer = _bluetoothManager.OpenGattServer(ctx, _bluettothServerCallback);

            var service = new BluetoothGattService(UUID.FromString(BLEServiceUID), GattServiceType.Primary);
            
            
            _characteristic = new BluetoothGattCharacteristic(UUID.FromString(BLECharacteristicUID_Data), GattProperty.Read | GattProperty.Notify | GattProperty.Write, GattPermission.Read | GattPermission.Write);
            _characteristic.AddDescriptor(new BluetoothGattDescriptor(UUID.FromString(BLEDescriptorUID_DATA),GattDescriptorPermission.Read | GattDescriptorPermission.Write));
            _characteristic.SetValue(userProfil + '\n');
            service.AddCharacteristic(_characteristic);


            if (_bluetoothServer != null) _bluetoothServer.AddService(service);
            Debug.WriteLine($"Server created! {_characteristic.GetStringValue(1)}");
            

            _bluettothServerCallback.CharacteristicReadRequest += _bluettothServerCallback_CharacteristicReadRequest;

            if (_bluetoothAdapter != null)
            {
                myBluetoothLeAdvertiser = _bluetoothAdapter.BluetoothLeAdvertiser;

            
                var builder = new AdvertiseSettings.Builder();
                builder.SetAdvertiseMode(AdvertiseMode.LowLatency);
                builder.SetConnectable(true);
                builder.SetTimeout(0);
                builder.SetTxPowerLevel(AdvertiseTx.PowerHigh);
                AdvertiseData.Builder dataBuilder = new AdvertiseData.Builder();
                dataBuilder.SetIncludeDeviceName(false);
                dataBuilder.AddServiceUuid(ParcelUuid.FromString(BLEServiceUID));
                dataBuilder.SetIncludeTxPowerLevel(true);


                if (myBluetoothLeAdvertiser != null)
                {
                    myBluetoothLeAdvertiser.StopAdvertising(new BleAdvertiseCallback());


                    ConnectionState = BLEClient.ConnectionStates.Waiting;
                    count = 0;
                    myBluetoothLeAdvertiser.StartAdvertising(builder.Build(), dataBuilder.Build(),
                        new BleAdvertiseCallback());
                }
            }
        }
        
        List<List<byte>> Split(byte[] source, int length)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / length)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }
        void _bluettothServerCallback_CharacteristicReadRequest(object sender, BleEventArgs e)
        {
            ConnectionState = BLEClient.ConnectionStates.Writing;
            count++;
            var t = Split(e.Characteristic.GetValue(), 20);
            var ti = new byte[]{};
            try
            {
                ti = t[count - 1].ToArray();
            }
            catch (Exception exception)
            {
                ti = new byte[]{Convert.ToByte('\n')};
                
            }
            _bluetoothServer.SendResponse(e.Device, e.RequestId, GattStatus.Success, e.Offset, ti);
            Debug.WriteLine(Encoding.UTF8.GetString(ti));
            if (count >= t.Count)
            {
                ConnectionState = BLEClient.ConnectionStates.End;
                if (myBluetoothLeAdvertiser != null)
                {
                    myBluetoothLeAdvertiser.StopAdvertising(new BleAdvertiseCallback());
                    count = 0;
                }
                
            }
            return;
        }

    }

    public class BleAdvertiseCallback : AdvertiseCallback
    {
        
        public override void OnStartFailure(AdvertiseFailure errorCode)
        {
            Debug.WriteLine("Adevertise start failure {0}", errorCode);
            base.OnStartFailure(errorCode);
        }

        public override void OnStartSuccess(AdvertiseSettings settingsInEffect)
        {
            Debug.WriteLine("Adevertise start success {0}", settingsInEffect.Mode);
            base.OnStartSuccess(settingsInEffect);
        }
    }
}