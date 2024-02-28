using Meliora.Services.CookiesKristen;

namespace Meliora.Tests
{
    public class MelioraServiceTests
    {
        /// <summary>
        /// Tests the GetDivisibilityString method of MelioraService.
        /// It verifies that the method returns the expected divisibility string for a given value.
        /// </summary>
        [Fact]
        public async Task GetDivisibilityString_ShouldReturnDivisibilityString()
        {
            // Arrange
            const int value = 15;
            const string expectedDivisibilityString = "Nursing";
            var melioraService = new MelioraService();

            // Act
            var divisibilityString = await melioraService.GetDivisibilityString(value);

            // Assert
            Assert.Equal(expectedDivisibilityString, divisibilityString);
        }

        /// <summary>
        /// Tests the CountAsync method of MelioraService.
        /// It verifies that the method invokes the provided callback for all numbers in the specified range.
        /// </summary>
        [Fact]
        public async Task CountAsync_ShouldInvokeOnNumberGeneratedForAllNumbers()
        {
            // Arrange
            const int maxValue = 10;
            const int startFrom = 1;
            var numbersGenerated = 0;
            var melioraService = new MelioraService();

            // Act
            await melioraService.CountAsync(maxValue, startFrom, (_, _) =>
            {
                numbersGenerated++;
            }, CancellationToken.None);

            // Assert
            Assert.Equal(maxValue - startFrom + 1, numbersGenerated);
        }

        /// <summary>
        /// Tests the CountAsync method of MelioraService.
        /// It verifies that the method stops counting when cancellation is requested.
        /// </summary>
        [Fact]
        public async Task CountAsync_ShouldStopCountingWhenCancellationRequested()
        {
            // Arrange
            const int maxValue = 10;
            const int startFrom = 1;
            var numbersGenerated = 0;
            var cancellationTokenSource = new CancellationTokenSource();
            var melioraService = new MelioraService();

            // Act
            try
            {
                var countingTask = melioraService.CountAsync(maxValue, startFrom, (_, number) =>
                {
                    numbersGenerated++;
                    if (number == 5)
                    {
                        cancellationTokenSource.Cancel();
                    }
                }, cancellationTokenSource.Token);

                await countingTask;
            }
            catch (Exception e)
            {
                // Assert
                Assert.Equal(5, numbersGenerated);
                Assert.IsType<TaskCanceledException>(e);
            }
        }
    }
}
