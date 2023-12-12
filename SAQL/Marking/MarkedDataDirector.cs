using Microsoft.AspNetCore.Mvc;
using SAQL.Contexts;
using SAQL.DTO;
using SAQL.Entities;

namespace SAQL.Marking
{
    public class MarkedDataDirector
    {
        public SAQLContext Context { get; set; }

        public string JSON { get; set; }
        public void makeJSON(JSONBuilder builder, long PatientID, long DoctorID, long DeviceID, PhysiologicalData PD)
        {
            JSONMarkedData newMarkedData = new JSONMarkedData();
            builder.resetMarkedData(newMarkedData);
            
            Patient patient = Context.Patients.Find(PatientID);
            builder.setPatient(patient);

            Device device = Context.Devices.Find(DeviceID);
            builder.setDevice(device);

            Doctor doctor = Context.Doctors.Find(DoctorID);
            builder.setDoctor(doctor);

            builder.setPhysiologicalData(PD);

            JSON = builder.GetData();
        }
    }
}
