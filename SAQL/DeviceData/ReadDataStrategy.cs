using SAQL.Entities;

namespace SAQL.DeviceData
{
    public interface IReadDataStrategy
    {
        public PhysiologicalData GetData(string rawData, long watchID);
    }
}
