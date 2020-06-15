using AutoMapper;
using CovidSafe.API.v20200611;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CovidSafe.API.Tests.v20200611
{
    /// <summary>
    /// Unit Tests for AutoMapper <see cref="MappingProfiles"/>
    /// </summary>
    [TestClass]
    public class MappingProfilesTests
    {
        /// <summary>
        /// <see cref="MappingProfiles"/> pass their internal validation using 
        /// AutoMapper
        /// </summary>
        [TestMethod]
        public void MappingProfiles_PassValidation()
        {
            // Assemble
            var mapperConfig = new MapperConfiguration(
                opts => opts.AddProfile<MappingProfiles>()
            );

            // Act
            mapperConfig.AssertConfigurationIsValid();

            // Assert
            // Exception thrown on invalid mappings
        }
    }
}