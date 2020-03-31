using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TraceDefense.API.Mappings;

namespace TraceDefense.API.Test.Mappings
{
    /// <summary>
    /// Tests for the <see cref="MappingProfiles"/> class
    /// </summary>
    [TestClass]
    public class MappingProfilesShould
    {
        /// <summary>
        /// AutoMapper should be able to successfully map target objefct types
        /// </summary>
        [TestMethod]
        public void PassConfigurationValidation()
        {
            // Assemble
            var mapperConfig = new MapperConfiguration(
                opts => opts.AddProfile<MappingProfiles>()
            );

            // Act
            mapperConfig.AssertConfigurationIsValid();

            // Exception thrown on invalid mapping profile
        }
    }
}
