using SAQL.Contexts;
using SAQL.Entities;

namespace SAQL.DeviceData
{
    public class AndroidReadDataStrategy : IReadDataStrategy
    {
        public SAQLContext Context { get; set; }
        public PhysiologicalData GetData(string rawData, long watchID)
        {
            var PD = new PhysiologicalData();
           
            string[] splits = rawData.Split('!');
            if (splits.Length >= 6)
            {
                PD.SystolicPressure = int.Parse(splits[0]);
                PD.DiastolicPressure = int.Parse(splits[1]);
                PD.Pulse = int.Parse(splits[2]);
                PD.StepsAmount = int.Parse(splits[3]);
                PD.Temperature = double.Parse(splits[4]);
                PD.Oxygen = int.Parse(splits[5]);
            }
            PD.LastUpdate = DateTime.Now;

            return PD;
        }
        public AndroidReadDataStrategy(SAQLContext context)
        {
            Context = context;
        }
    }
}
