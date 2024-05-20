using DeliveryApp.Core.Domain.SharedKernel;
using Xunit;

namespace DeliveryApp.UnitTests.SharedKernel
{
    public partial class WeightTests
    {

        [Fact]
        public void IsSuccess_WeightCreate_ReturnTrue()
        {
            var result = Weight.Create(5);

            Assert.True(result.IsSuccess);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public void IsSuccess_WeightCreate_ReturnFalse(int value)
        {
            var result = Weight.Create(value);

            Assert.False(result.IsSuccess);
        }
       
    }
}
