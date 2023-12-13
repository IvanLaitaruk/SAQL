using SAQL.Contexts;
using SAQL.Entities;
using System.Globalization;

namespace SAQL.DeviceData
{
    public class IOSReadDataStrategy : IReadDataStrategy
    {
        public SAQLContext Context { get; set; }
        public PhysiologicalData GetData(string rawData, long watchID)
        {
            var PD = new PhysiologicalData();
            NumberFormatInfo formatWithComma = new NumberFormatInfo();
            formatWithComma.NumberDecimalSeparator = ".";
            double newVersion = 0;

            string[] splits = rawData.Split('!');
            if (splits.Length >= 6)
            {
                var parsed = double.TryParse(splits[4], NumberStyles.Any, formatWithComma, out newVersion);

                PD.SystolicPressure = int.Parse(splits[0]);
                PD.DiastolicPressure = int.Parse(splits[1]);
                PD.Pulse = int.Parse(splits[2]);
                PD.StepsAmount = int.Parse(splits[3]);
                PD.Temperature = parsed ? newVersion : 0.0;
                PD.Oxygen = int.Parse(splits[5]);
            }
            PD.LastUpdate = DateTime.Now;

            return PD;
        }
        public IOSReadDataStrategy(SAQLContext context)
        {
            Context = context;
        }
    }
}
