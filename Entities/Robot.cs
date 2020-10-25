namespace Practica1.Entities
{
    public class Robot
    {
        public string id;
        public bool enabled;
        public int maxBattery;
        public int battery;

        public Robot(string argId)
        {
            id = argId;
            enabled = false;
            maxBattery = 10;
            battery = maxBattery;
        }
    }
}