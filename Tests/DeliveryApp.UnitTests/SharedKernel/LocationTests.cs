using DeliveryApp.Core.Domain.SharedKernel;
using System.Collections.Generic;
using Xunit;

namespace DeliveryApp.UnitTests.SharedKernel
{
    public class LocationTests
    {
        [Fact]
        public void IsSuccess_LocationCreate_ReturnTrue()
        {
            var result = Location.Create(2, 5);

            Assert.True(result.IsSuccess);
        }

        public static IEnumerable<object[]> GetLocations()
        {
            yield return [Location.Create(1, 2).Value, 0];
            yield return [Location.Create(1, 3).Value, 1];
            yield return [Location.Create(2, 1).Value, 2];
            yield return [Location.Create(10, 10).Value, 17];
        }




        [Theory]
        [InlineData(0, 1)]
        [InlineData(0, 0)]
        [InlineData(1, 0)]
        [InlineData(11, 10)]
        [InlineData(11, 11)]
        [InlineData(10, 11)]
        public void IsSuccess_LocationCreate_ReturnFalse(int x, int y)
        {
            var result = Location.Create(x, y);

            Assert.False(result.IsSuccess);
        }

        [Theory]
        [MemberData(nameof(GetLocations))]
        public void LocationDistance_ReturnDistance(Location target, int distance)
        {
            //arrange
            var location = Location.Create(1, 2).Value;

            //act
            var result = location.DistanceTo(target);

            //assert

            Assert.True(result.IsSuccess);
            Assert.Equal(result.Value, distance);

        }
    }

}
