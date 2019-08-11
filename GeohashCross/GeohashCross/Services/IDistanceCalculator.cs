using System;
using GeohashCross.Models;

namespace GeohashCross.Services
{
    public interface IDistanceCalculator
    {
        double CalculateDistance(Location start, Location end);
    }
}
