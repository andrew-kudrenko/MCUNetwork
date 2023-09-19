﻿namespace MCUNetwork.Models
{
    internal class Microcontroller
    {
        public readonly MicrocontrollerMemory Memory;

        public Microcontroller(double memoryVolume, double thresholdRatio)
        {
            Memory = new (memoryVolume, thresholdRatio);
        }
    }
}