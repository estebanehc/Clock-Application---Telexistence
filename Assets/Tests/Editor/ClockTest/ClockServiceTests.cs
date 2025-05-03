using System;
using NUnit.Framework;

public class ClockServiceTests
{
    private ClockService clockService;

        [SetUp]
        public void SetUp()
        {
            clockService = new ClockService();
        }

        [Test]
        public void GetCurrentTime_ReturnsValidDateTime()
        {
            var systemNow = DateTime.Now;
            var currentTime = clockService.CurrentTime;
            Assert.IsTrue(currentTime >= systemNow.AddSeconds(-1) && currentTime <= systemNow.AddSeconds(1), "Current time should be within 1 second of system time.");
        }
}
