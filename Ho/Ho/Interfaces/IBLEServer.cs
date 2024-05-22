using Ho.BLE;

namespace Ho.Interfaces
{
    public interface IBLEServer
    {
        BLEClient.ConnectionStates ConnectionState { get; }
        void StartAdvert(string userProfil);
        
    }
}