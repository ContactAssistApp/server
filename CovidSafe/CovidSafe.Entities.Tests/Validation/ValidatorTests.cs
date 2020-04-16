using System;

using CovidSafe.Entities.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CovidSafe.Entities.Tests.Validation
{
    /// <summary>
    /// <see cref="Validator"/> Unit Tests
    /// </summary>
    [TestClass]
    public class ValidatorTests
    {
        /// <summary>
        /// <see cref="Validator.ValidateGuid(string, string)"/> accepts 
        /// valid <see cref="Guid"/> strings
        /// </summary>
        [TestMethod]
        public void ValidateGuid_AcceptsValidInput()
        {
            // Arrange
            string input1 = "00000000-0000-0000-0000-000000000000";
            string input2 = "00000000000000000000000000000000";

            // Act
            RequestValidationResult result1 = Validator.ValidateGuid(input1);
            RequestValidationResult result2 = Validator.ValidateGuid(input2);

            // Assert
            Assert.IsTrue(result1.Passed);
            Assert.IsTrue(result2.Passed);
        }

        /// <summary>
        /// <see cref="Validator.ValidateGuid(string, string)"/> flags 
        /// invalid <see cref="Guid"/> strings
        /// </summary>
        [TestMethod]
        public void ValidateGuid_FailsOnInvalidInputs()
        {
            // Arrange
            string input1 = "00000000-0000-0000-0000-00000000000"; // Too short
            string input2 = "0000000000000000000000000000000"; // Too short
            string input3 = "Not a guid!"; // Invalid format

            // Act
            RequestValidationResult result1 = Validator.ValidateGuid(input1);
            RequestValidationResult result2 = Validator.ValidateGuid(input2);
            RequestValidationResult result3 = Validator.ValidateGuid(input3);

            // Assert
            Assert.IsFalse(result1.Passed);
            Assert.IsFalse(result2.Passed);
            Assert.IsFalse(result3.Passed);
        }

        /// <summary>
        /// <see cref="Validator.ValidateLatitude(double, string)"/> accepts 
        /// valid latitude <see cref="double"/> objects
        /// </summary>
        [TestMethod]
        public void ValidateLatitude_AcceptsValidInput()
        {
            // Arrange
            long input1 = 0;
            long input2 = 90;
            long input3 = -90;

            // Act
            RequestValidationResult result1 = Validator.ValidateLatitude(input1);
            RequestValidationResult result2 = Validator.ValidateLatitude(input2);
            RequestValidationResult result3 = Validator.ValidateLatitude(input3);

            // Assert
            Assert.IsTrue(result1.Passed);
            Assert.IsTrue(result2.Passed);
            Assert.IsTrue(result3.Passed);
        }

        /// <summary>
        /// <see cref="Validator.ValidateLatitude(double, string)"/> flags 
        /// invalid latitude <see cref="double"/> objects
        /// </summary>
        [TestMethod]
        public void ValidateLatitude_FailsOnInvalidInputs()
        {
            // Arrange
            long input1 = 91; // Too high
            long input2 = -91; // Too low

            // Act
            RequestValidationResult result1 = Validator.ValidateLatitude(input1);
            RequestValidationResult result2 = Validator.ValidateLatitude(input2);

            // Assert
            Assert.IsFalse(result1.Passed);
            Assert.IsFalse(result2.Passed);
        }

        /// <summary>
        /// <see cref="Validator.ValidateLongitude(double, string)"/> accepts 
        /// valid longitude <see cref="double"/> objects
        /// </summary>
        [TestMethod]
        public void ValidateLongitude_AcceptsValidInput()
        {
            // Arrange
            long input1 = 0;
            long input2 = 180;
            long input3 = -180;

            // Act
            RequestValidationResult result1 = Validator.ValidateLongitude(input1);
            RequestValidationResult result2 = Validator.ValidateLongitude(input2);
            RequestValidationResult result3 = Validator.ValidateLongitude(input3);

            // Assert
            Assert.IsTrue(result1.Passed);
            Assert.IsTrue(result2.Passed);
            Assert.IsTrue(result3.Passed);
        }

        /// <summary>
        /// <see cref="Validator.ValidateLongitude(double, string)"/> flags 
        /// invalid longitude <see cref="double"/> objects
        /// </summary>
        [TestMethod]
        public void ValidateLongitude_FailsOnInvalidInputs()
        {
            // Arrange
            long input1 = 181; // Too high
            long input2 = -181; // Too low

            // Act
            RequestValidationResult result1 = Validator.ValidateLongitude(input1);
            RequestValidationResult result2 = Validator.ValidateLongitude(input2);

            // Assert
            Assert.IsFalse(result1.Passed);
            Assert.IsFalse(result2.Passed);
        }

        /// <summary>
        /// <see cref="Validator.ValidateSeed(string, string)"/> accepts 
        /// valid <see cref="Guid"/> strings
        /// </summary>
        [TestMethod]
        public void ValidateSeed_AcceptsValidInput()
        {
            // Arrange
            string input1 = "00000000-0000-0000-0000-000000000000";
            string input2 = "00000000000000000000000000000000";

            // Act
            RequestValidationResult result1 = Validator.ValidateGuid(input1);
            RequestValidationResult result2 = Validator.ValidateGuid(input2);

            // Assert
            Assert.IsTrue(result1.Passed);
            Assert.IsTrue(result2.Passed);
        }

        /// <summary>
        /// <see cref="Validator.ValidateSeed(string, string)"/> accepts 
        /// valid <see cref="Guid"/> strings
        /// </summary>
        [TestMethod]
        public void ValidateSeed_FailsOnInvalidInputs()
        {
            // Arrange
            string input1 = "00000000-0000-0000-0000-00000000000"; // Too short
            string input2 = "0000000000000000000000000000000"; // Too short
            string input3 = "Not a guid!"; // Invalid format

            // Act
            RequestValidationResult result1 = Validator.ValidateGuid(input1);
            RequestValidationResult result2 = Validator.ValidateGuid(input2);
            RequestValidationResult result3 = Validator.ValidateGuid(input3);

            // Assert
            Assert.IsFalse(result1.Passed);
            Assert.IsFalse(result2.Passed);
            Assert.IsFalse(result3.Passed);
        }

        /// <summary>
        /// <see cref="Validator.ValidateTimeRange(long, long)"/> accepts 
        /// valid time ranges
        /// </summary>
        [TestMethod]
        public void ValidateTimeRange_AcceptsValidInput()
        {
            // Arrange
            long startTime1 = 0;
            long endTime1 = 1;

            long startTime2 = 1;
            long endTime2 = 1;

            // Act
            RequestValidationResult result1 = Validator.ValidateTimeRange(startTime1, endTime1);
            RequestValidationResult result2 = Validator.ValidateTimeRange(startTime2, endTime2);

            // Assert
            Assert.IsTrue(result1.Passed);
            Assert.IsTrue(result2.Passed);
        }

        /// <summary>
        /// <see cref="Validator.ValidateTimeRange(long, long)"/> flags 
        /// invalid time ranges
        /// </summary>
        [TestMethod]
        public void ValidateTimeRange_FailsOnInvalidInputs()
        {
            // Arrange
            long startTime1 = 1;
            long endTime1 = 0;

            long startTime2 = Int64.MaxValue;
            long endTime2 = Int64.MinValue;

            // Act
            RequestValidationResult result1 = Validator.ValidateTimeRange(startTime1, endTime1);
            RequestValidationResult result2 = Validator.ValidateTimeRange(startTime2, endTime2);

            // Assert
            Assert.IsFalse(result1.Passed);
            Assert.IsFalse(result2.Passed);
        }

        /// <summary>
        /// <see cref="Validator.ValidateTimestamp(long, string)"/> accepts 
        /// valid timestamps
        /// </summary>
        [TestMethod]
        public void ValidateTimestamp_AcceptsValidInput()
        {
            // Arrange
            long timestamp1 = 0;
            long timestamp2 = Int64.MaxValue;

            // Act
            RequestValidationResult result1 = Validator.ValidateTimestamp(timestamp1);
            RequestValidationResult result2 = Validator.ValidateTimestamp(timestamp2);

            // Assert
            Assert.IsTrue(result1.Passed);
            Assert.IsTrue(result2.Passed);
        }

        /// <summary>
        /// <see cref="Validator.ValidateTimestamp(long, string)"/> flags 
        /// negative timestamps
        /// </summary>
        [TestMethod]
        public void ValidateTimestamp_FailsOnNegativeTimestamp()
        {
            // Arrange
            long timestamp1 = -1;
            long timestamp2 = Int64.MinValue;

            // Act
            RequestValidationResult result1 = Validator.ValidateTimestamp(timestamp1);
            RequestValidationResult result2 = Validator.ValidateTimestamp(timestamp2);

            // Assert
            Assert.IsFalse(result1.Passed);
            Assert.IsFalse(result2.Passed);
        }
    }
}
