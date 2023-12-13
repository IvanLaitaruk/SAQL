using Microsoft.EntityFrameworkCore;
using SAQL.Contexts;
using SAQL.Controllers;
using SAQL.DeviceData;
using SAQL.Marking;
using System.Text.Json;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAQL.Entities;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;

namespace SAQLTest
{
    public class Tests
    {
        private SAQLContext context;
        private AndroidReadDataStrategy strategy;
        private DataController _dataController;
        private Mock<SAQLContext> _mockContext;
        private IHttpClientFactory _httpClientFactory;
        [SetUp]
        public void Setup()
        {
             context = new SAQLContext();
            strategy = new AndroidReadDataStrategy(context);
            // Set up the mock SAQLContext
            _mockContext = new Mock<SAQLContext>();

            // Set up the mock HttpClient
            var services = new ServiceCollection();
            _httpClientFactory = Mock.Of<IHttpClientFactory>();
            services.AddSingleton(_httpClientFactory);

            //_dataController = new DataController(_mockContext.Object)
            //{
            //    ControllerContext = new ControllerContext
            //    {
            //        HttpContext = new DefaultHttpContext()
            //    }
            //};

        }
        
        [Test]
        public void ReturnsSystolicPressure()
        {
            string rawData = "120!80!75!10000!98.6!98";

            // Act
            var result = strategy.GetData(rawData, 1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(120, result.SystolicPressure);
        }
        [Test]
        public void ReturnsDiastolicPressure()
        {
            // Arrange
            string rawData = "120!80!75!10000!98.6!98";

            // Act
            var result = strategy.GetData(rawData, 1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(80, result.DiastolicPressure);
        }

        [Test]
        public void ReturnsPulse()
        {
            // Arrange
            string rawData = "120!80!75!10000!98.6!98";

            // Act
            var result = strategy.GetData(rawData, 1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(75, result.Pulse);
        }

        [Test]
        public void ReturnsStepsAmount()
        {
            // Arrange
            string rawData = "120!80!75!10000!98.6!98";

            // Act
            var result = strategy.GetData(rawData, 1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(10000, result.StepsAmount);
        }

        [Test]
        public void ReturnsTemperature()
        {
            // Arrange
            string rawData = "120!80!75!10000!98.6!98";

            // Act
            var result = strategy.GetData(rawData, 1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(98.6, result.Temperature, 0.01);
        }

        [Test]
        public void ReturnsDataWithOxygen()
        {
            // Arrange
            string rawData = "120!80!75!10000!98.6!98";

            // Act
            var result = strategy.GetData(rawData, 1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(98, result.Oxygen);
        }
        [Test]
        public void ReturnsCorrectJSON()
        {
            // Arrange
            // Replace this with a valid patient ID from your database
            int patientId = 23;

            // Retrieve existing records from the database based on the patient ID
            var patient = context.Patients
                .Include(p => p.Doctor)
                .Include(p => p.Device)
                .FirstOrDefault(p => p.Id == patientId);

            var physiologicalData = context.PhysiologicalData
                .FirstOrDefault(p => p.PatientId == patientId);

            if (patient == null || physiologicalData == null)
            {
                Assert.Inconclusive("Could not find required data in the database for testing.");
                return;
            }

            var jsonMarkedData = new JSONMarkedData
            {
                PhysiologicalData = physiologicalData,
                Patient = patient,
                Doctor = patient.Doctor,
                Device = patient.Device
            };

            // Act
            var result = jsonMarkedData.getJSON();

            // Assert
            Assert.IsNotNull(result);

            var jsonDocument = JsonDocument.Parse(result);
            var root = jsonDocument.RootElement;

            // Example assertions based on the expected JSON structure
            Assert.AreEqual(patient.Name, root.GetProperty("PatientName").GetString());
            Assert.AreEqual(patient.Surname, root.GetProperty("PatientSurname").GetString());
            Assert.AreEqual(patient.Doctor?.Name, root.GetProperty("DoctorName").GetString());
            Assert.AreEqual(patient.Doctor?.Surname, root.GetProperty("DoctorSurname").GetString());
            Assert.AreEqual(patient.Device?.Model, root.GetProperty("DeviceModel").GetString());
            Assert.AreEqual(patient.Device?.Brand, root.GetProperty("DeviceBrand").GetString());

            var pressure = root.GetProperty("Pressure");
            Assert.AreEqual(physiologicalData.SystolicPressure, pressure.GetProperty("Systolic").GetInt32());
            Assert.AreEqual(physiologicalData.DiastolicPressure, pressure.GetProperty("Diastolic").GetInt32());
            Assert.AreEqual(physiologicalData.Pulse, root.GetProperty("Pulse").GetInt32());
            Assert.AreEqual(physiologicalData.StepsAmount, root.GetProperty("StepsAmount").GetInt32());
            Assert.AreEqual(physiologicalData.Temperature, root.GetProperty("Temperature").GetDouble());
            Assert.AreEqual(physiologicalData.Oxygen, root.GetProperty("Oxygen").GetInt32());
        }

    }
}