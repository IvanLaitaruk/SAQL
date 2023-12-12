using Microsoft.EntityFrameworkCore;
using SAQL.Entities;

namespace SAQL.Marking
{
    public class MarkedDataBuilder
    {
        protected MarkedData _markedData;
        public void resetMarkedData(MarkedData newMarkedData)
        {
            _markedData = newMarkedData;
        }
        public void setPatient(Patient patient)
        {
            _markedData.Patient = patient;
        }
        public void setDoctor(Doctor doctor)
        {
            _markedData.Doctor = doctor;
        }
        public void setPhysiologicalData(PhysiologicalData physiologicalData)
        {
            _markedData.PhysiologicalData = physiologicalData;
        }
        public void setDevice(Device device)
        {
            _markedData.Device = device;
        }
        public MarkedData getResult()
        {
            return _markedData;
        }
    }
}
