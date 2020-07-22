using System.Windows.Media;

namespace BusinessLogic
{
    public class Car
    {
        public double Speed { get; set; }
        public string Driver { get; set; }

        public Color Color { get; set; }
    }

    public class Human
    {
        public string Name { get; set; }
        public bool HasDrivingLicense { get; set; }
    }
}
