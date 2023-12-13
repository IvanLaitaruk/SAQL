using Microsoft.EntityFrameworkCore;
using SAQL.Contexts;
using SAQL.DeviceData;
using SAQL.Entities;
using SAQL.Marking;

namespace SAQL
{
    public class DataProcessing
    {
        private SAQLContext _context;

        private DeviceManager _deviceManager = new DeviceManager();
        private MarkedDataDirector _markedDataDirector = new MarkedDataDirector();

        public void setContext(SAQLContext context)
        {
            _context = context;
            _markedDataDirector.Context = context;
        }
        public PhysiologicalData processData(string rawData, long deviceID, long patientID, long doctorID)
        {
            Device device = _context.Devices.Find(deviceID);
            if(device != null)
            {
                _deviceManager.connectToWatch(deviceID);
                IReadDataStrategy readDataStrategy;
                switch (device.OS)
                {
                    case OperationSystem.WatchOS:
                        readDataStrategy = new IOSReadDataStrategy(_context);

                        break;
                    case OperationSystem.WearOS:
                        readDataStrategy = new AndroidReadDataStrategy(_context);
                        break;
                    default:
                        readDataStrategy= new AndroidReadDataStrategy(_context);
                        break;
                }
                _deviceManager.setReadStrategy(readDataStrategy);
                PhysiologicalData PD = _deviceManager.readData(rawData);
                Patient patient = _context.Patients.Find(patientID);
                
                PD.Patient = patient;
                PD.PatientId = patientID;
              
                _deviceManager.disconnectFromWatch();

                JSONBuilder jSONBuilder = new JSONBuilder();
                _markedDataDirector.makeJSON(jSONBuilder, patientID, doctorID, deviceID, PD);

                return PD;
            }
            return new PhysiologicalData();
        }

        public string GetLastJSON()
        {
            return _markedDataDirector.JSON;
        }
    }
}
