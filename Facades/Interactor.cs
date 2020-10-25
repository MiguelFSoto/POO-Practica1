using System;
using Practica1.Entities;

namespace Practica1.Facades
{
    public class Interactor
    {
        public void plotStatus(Plot argPlot, Robot argRobot)
        {
            Console.WriteLine("Id:" + argRobot.id);
            Console.Write("Plantado: ");
            if (argPlot.planted != null)
            {
                Console.WriteLine(argPlot.planted.name);
                Console.WriteLine("Tiempo transcurrido: " + argPlot.timePassed);
            }
            else
            {
                Console.WriteLine("Nada");
            }
            Console.WriteLine("Fertilizado por: " + argPlot.fertilized);
            Console.WriteLine("Agua: " + (argPlot.waterOn ? "Prendida" : "Apagada"));
            
            if (argPlot.plague)
            {
                Console.WriteLine("Se ha detectado una plaga");
            }
            if (argPlot.tempProblem)
            {
                Console.WriteLine("Se ha detectado un problema de temperatura");
            }

            argRobot.battery--;
        }

        public void robotToggle(Robot argRobot)
        {
            argRobot.enabled = !argRobot.enabled;
        }

        public void plantCrop(Plot argPlot, Crop argCrop, Robot argRobot)
        {
            argPlot.planted = argCrop;
            argPlot.timePassed = 0;

            argRobot.battery--;
        }

        public void fertilizePlot(Plot argPlot, Robot argRobot)
        {
            argPlot.fertilized += 5;

            argRobot.battery--;
        }

        public void gatherPlot(Plot argPlot, Robot argRobot)
        {
            argPlot.planted = null;
            argPlot.timePassed = 0;

            argRobot.battery--;
        }

        public void waterToggle(Plot argPlot)
        {
            argPlot.waterOn = !argPlot.waterOn;
        }

        public void fixPlot(Plot argPlot, Robot argRobot)
        {
            if (argPlot.plague)
            {
                argPlot.plague = false;
                argRobot.battery--;
            }
            if (argPlot.tempProblem)
            {
                argPlot.tempProblem = false;
                argRobot.battery--;
            }
        }
    }
}