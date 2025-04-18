using Xunit;
using QuoridorLib.Models;

namespace QuoridorTest;
public class PlayerTest
{

        [Fact]
        public void GetName_Should_Return_Gabin()
        {
            // Arrange
            var Gabin = new Player("Gabin");

            // Act
            Gabin.GetName();

            // Assert
            Assert.Equal("Gabin", Gabin.Name);
        }
    }
}
