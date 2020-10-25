namespace Practica1.Entities
{
    public class Plot
    {
        public string id;
        public Crop planted;
        public int fertilized;
        public bool waterOn;
        public bool plague;
        public bool tempProblem;
        public int timePassed;

        public Plot(string argId)
        {
            id = argId;
            planted = null;
            fertilized = 0;
            waterOn = false;
            plague = false;
            tempProblem = false;
            timePassed = 0;
        }
    }
}