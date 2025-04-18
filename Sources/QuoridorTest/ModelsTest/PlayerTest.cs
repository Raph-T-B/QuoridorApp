namespace QuoridorTest;


public class PlayerTest
{

        [Fact]
        public void GetName_Should_Return_Gabin()
        {
            // Arrange
            Player Gabin = new Player("Raphael");

            // Act
            Gabin.setName("Gabin");

            // Assert
            Assert.Equal("Gabin", Gabin.Name);
        }
    }
}
