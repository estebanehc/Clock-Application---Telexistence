using System;

public class ClockService : IClockService
{
    public DateTime CurrentTime => DateTime.Now;
}
