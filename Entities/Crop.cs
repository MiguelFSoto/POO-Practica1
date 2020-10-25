namespace Practica1.Entities
{
    public class Crop
    {
        public string name;
        public int timeNeeded;

        public Crop(string argName, int argTime)
        {
            name = argName;
            timeNeeded = argTime;
        }
    }
}