using SAQL.Entities;
using System.Text;

namespace SAQL.DeviceData
{
    public enum ConnectionState
    {
        Disconnected = 0,
        Connecting = 1,
        Connected = 2,
    }


    public class DeviceManager
    {
        private IReadDataStrategy _readStrategy;
        public long WatchID { get; set; }
        public ConnectionState ConnectionState { get; set; }

        private string _rawData;

        public PhysiologicalData readData(string rawData)
        {
            _rawData = rawData;
            if (ConnectionState == ConnectionState.Connected)
            {
               return _readStrategy.GetData(_rawData, WatchID);
            }
            return new PhysiologicalData();
        }
        public void setReadStrategy(IReadDataStrategy readDataStrategy)
        {
            _readStrategy = readDataStrategy;
        }
        public void connectToWatch(long watchID)
        {
            WatchID = watchID;
            ConnectionState = ConnectionState.Connected;
        }
        public void disconnectFromWatch()
        {
            WatchID = -1;
            ConnectionState = ConnectionState.Disconnected;
        }
    }
}
