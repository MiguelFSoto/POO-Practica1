using System;
using System.Collections.Generic;
using Practica1.Validators;
using Practica1.Rules;
using Practica1.Entities;

namespace Practica1.Facades
{
    class Boundary
    {
        public Interactor controller;
        public List<Robot> robotList;
        public List<Plot> plotList;
        public Dictionary<Crop, int[]> cropList; //Crop: Stored, Seeds
        public Plot argPlot;
        public Robot argRobot;
        public Crop argCrop;
        public Validator stringValidate;

        public Boundary()
        {
            controller = new Interactor();

            robotList = new List<Robot>();
            robotList.Add(new Robot("r1"));
            robotList.Add(new Robot("r2"));
            robotList.Add(new Robot("r3"));

            plotList = new List<Plot>();
            plotList.Add(new Plot("p1"));
            plotList.Add(new Plot("p2"));
            plotList.Add(new Plot("p3"));
            plotList.Add(new Plot("p4"));

            //cropList.Add(new Crop("c1", 3), new Tuple<int, int>(0, 10));
            //cropList.Add(new Crop("c2", 5), new Tuple<int, int>(0, 5));

            cropList = new Dictionary<Crop, int[]>();
            cropList.Add(new Crop("c1", 3), new int[2]{0, 10});
            cropList.Add(new Crop("c2", 5), new int[2]{0, 5});

            stringValidate = new Validator();
            stringValidate.RuleList.Add(new StrRule());
            stringValidate.RuleList.Add(new LengthRule());
        }

        public void update()
        {
            robotList.ForEach(newRobot =>
            {
                if (!newRobot.enabled)
                {
                    if (newRobot.battery != newRobot.maxBattery)
                    {
                        newRobot.battery++;
                    }
                }
                if (newRobot.enabled && newRobot.battery < 1)
                {
                    Console.WriteLine("El robot " + newRobot.id + "se quedo sin bateria y fue desactivado automaticamente");
                    newRobot.enabled = false;
                }
            });

            plotList.ForEach(newPlot =>
            {
                var rand = new Random();
                bool rngCheck1 = rand.Next(101) < 10 ? true : false;
                bool rngCheck2 = rand.Next(101) < 10 ? true : false;
                bool rngCheck3 = rand.Next(101) < 40 ? true : false;

                if (newPlot.planted != null)
                {
                    if (newPlot.waterOn)
                    {
                        newPlot.timePassed += newPlot.fertilized > 0 ? 2 : 1;
                    }
                    if (newPlot.timePassed > newPlot.planted.timeNeeded * 2 && rngCheck3)
                    {
                        Console.WriteLine("El cultivo de la parcela " + newPlot.id + " no fue recolectado a tiempo y se perdio");
                        newPlot.planted = null;
                        newPlot.timePassed = 0;
                    }
                    if (newPlot.tempProblem && rngCheck3)
                    {
                        Console.WriteLine("El cultivo de la parcela " + newPlot.id + "se perdio debido a un problema de temperatura");
                        newPlot.planted = null;
                        newPlot.timePassed = 0;
                    }
                    if (newPlot.plague && rngCheck3)
                    {
                        Console.WriteLine("El cultivo de la parcela " + newPlot.id + "se perdio debido a una plaga");
                        newPlot.planted = null;
                        newPlot.timePassed = 0;
                    }

                    if (rngCheck1)
                    {
                        Console.WriteLine("Hay un problema de temperatura en la parcela " + newPlot.id);
                        newPlot.tempProblem = true;
                    }
                    if (rngCheck2)
                    {
                        Console.WriteLine("Hay una plaga de temperatura en la parcela " + newPlot.id);
                        newPlot.plague = true;
                    }
                }

                if (newPlot.fertilized > 0)
                {
                    newPlot.fertilized--;
                }
            });
        }

        public void menu()
        {
            string option = "";
            while (option != "0")
            {
                Console.WriteLine("=================");
                Console.WriteLine("Seleccione la accion:");
                Console.WriteLine("1) Ver Inventario");
                Console.WriteLine("2) Ver Estado de una Parcela");
                Console.WriteLine("3) Activar/Desactivar Robot");
                Console.WriteLine("4) Plantar Semillas");
                Console.WriteLine("5) Fertilizar una Parcela");
                Console.WriteLine("6) Recolectar una Parcela");
                Console.WriteLine("7) Activar/Desactivar Agua para una Parcela");
                Console.WriteLine("8) Arreglar un problema de una Parcela");
                Console.WriteLine("0) Salir");
                option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        printInventory();
                        break;

                    case "2":
                        if (statusData())
                            controller.plotStatus(argPlot, argRobot);
                        break;

                    case "3":
                        if (toggleData())
                            controller.robotToggle(argRobot);
                        break;

                    case "4":
                        if (plantData())
                            controller.plantCrop(argPlot, argCrop, argRobot);
                        break;

                    case "5":
                        //Both functions need the same arguments and have no extra conditions, so the same data can be used
                        if (statusData())
                            controller.fertilizePlot(argPlot, argRobot);
                        break;

                    case "6":
                        if (gatherData())
                            controller.gatherPlot(argPlot, argRobot);
                        break;

                    case "7":
                        if (waterData())
                            controller.waterToggle(argPlot);
                        break;


                    case "8":
                        if (fixData())
                            controller.fixPlot(argPlot, argRobot);
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Opcion Invalida");
                        break;
                }
                update();
            }
        }
    
        public void printPlots()
        {
            plotList.ForEach(newPlot =>
            {
                Console.WriteLine("Parcela: " + newPlot.id);
                Console.WriteLine("=================");
            });
        }

        public void printRobots()
        {
            robotList.ForEach(newRobot =>
            {
                Console.WriteLine("Robot: " + newRobot.id);
                Console.WriteLine("Estado: " + (newRobot.enabled ? "Activado" : "Desactivado"));
                Console.WriteLine("Bateria: " + newRobot.battery + "/" + newRobot.maxBattery);
                Console.WriteLine("=================");
            });
        }

        public void printInventory()
        {
            foreach(KeyValuePair<Crop, int[]> kvp in cropList)
            {
                Console.WriteLine(kvp.Key.name);
                Console.WriteLine("Almacenado: " + kvp.Value[0]);
                Console.WriteLine("Semillas: " + kvp.Value[1]);
                Console.WriteLine("=================");
            }
        }

        public bool statusData()
        {
            bool result;
            bool found = false;
            string input;
            Console.WriteLine("Seleccione la parcela: ");
            printPlots();
            input = Console.ReadLine();
            stringValidate.Value = input;
            result = stringValidate.ValidateField();

            plotList.ForEach(newPlot =>
                {
                    if (!(found))
                    {
                        found = newPlot.id == input;
                        if (found)
                        {
                            argPlot = newPlot;
                        }
                    }
                });

            if (result && found)
            {
                found = false;
                Console.WriteLine("Seleccione el robot: ");
                printRobots();
                input = Console.ReadLine();
                stringValidate.Value = input;
                result = result && stringValidate.ValidateField();

                robotList.ForEach(newRobot =>
                {
                    if (!(found))
                    {
                        found = newRobot.id == input;
                        if (found)
                        {
                            argRobot = newRobot;
                        }
                    }
                });

                if (result && found)
                {
                    if (!argRobot.enabled)
                    {
                        Console.WriteLine("Este robot no esta habilitado");
                        result = false;
                    }
                    else if (argRobot.battery < 1)
                    {
                        Console.WriteLine("Este robot no tiene bateria");
                        result = false;
                    }
                }
                else
                {
                    Console.WriteLine("Robot no encontrado");
                    result = false;
                }
            }
            else
            {
                Console.WriteLine("Parcela no encontrada");
                result = false;
            }
            return result;
        }

        public bool toggleData()
        {
            bool result;
            bool found = false;
            string input;
            Console.WriteLine("Seleccione el robot: ");
            printRobots();
            input = Console.ReadLine();
            stringValidate.Value = input;
            result = stringValidate.ValidateField();

            robotList.ForEach(newRobot =>
               {
                   if (!(found))
                   {
                       found = newRobot.id == input;
                       if (found)
                       {
                           argRobot = newRobot;
                       }
                   }
               });

            if (!found)
            {
                Console.WriteLine("Robot no encontrado");
                result = false;
            }
            return result;
        }

        public bool plantData()
        {
            bool result;
            bool found = false;
            string input;
            Console.WriteLine("Seleccione la parcela: ");
            printPlots();
            input = Console.ReadLine();
            stringValidate.Value = input;
            result = stringValidate.ValidateField();

            plotList.ForEach(newPlot =>
                {
                    if (!(found))
                    {
                        found = newPlot.id == input;
                        if (found)
                        {
                            argPlot = newPlot;
                        }
                    }
                });

            if (argPlot.planted != null)
            {
                Console.WriteLine("Ya hay algo plantado en esta parcela");
                return false;
            }

            if (result && found)
            {
                found = false;
                Console.WriteLine("Seleccione el robot: ");
                printRobots();
                input = Console.ReadLine();
                stringValidate.Value = input;
                result = result && stringValidate.ValidateField();

                robotList.ForEach(newRobot =>
                {
                    if (!(found))
                    {
                        found = newRobot.id == input;
                        if (found)
                        {
                            argRobot = newRobot;
                        }
                    }
                });

                if (result && found)
                {
                    if (!argRobot.enabled)
                    {
                        Console.WriteLine("Este robot no esta habilitado");
                        result = false;
                    }
                    else if (argRobot.battery < 1)
                    {
                        Console.WriteLine("Este robot no tiene bateria");
                        result = false;
                    }
                    else
                    {
                        found = false;
                        Console.WriteLine("Seleccione el cultivo: ");
                        printInventory();
                        input = Console.ReadLine();
                        stringValidate.Value = input;
                        result = result && stringValidate.ValidateField();

                        foreach (KeyValuePair<Crop, int[]> kvp in cropList)
                        {
                            if (!found)
                            {
                                found = kvp.Key.name == input;
                                if (found)
                                {
                                    argCrop = kvp.Key;
                                    kvp.Value[1]--;
                                }
                            }
                        }

                        if (!found)
                        {
                            Console.WriteLine("Cultivo no encontrado");
                            result = false;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Robot no encontrado");
                    result = false;
                }
            }
            else
            {
                Console.WriteLine("Parcela no encontrada");
                result = false;
            }
            return result;
        }

        public bool gatherData()
        {
            bool result;
            bool found = false;
            string input;
            Console.WriteLine("Seleccione la parcela: ");
            printPlots();
            input = Console.ReadLine();
            stringValidate.Value = input;
            result = stringValidate.ValidateField();

            plotList.ForEach(newPlot =>
                {
                    if (!(found))
                    {
                        found = newPlot.id == input;
                        if (found)
                        {
                            argPlot = newPlot;
                        }
                    }
                });

            if (argPlot.planted == null)
            {
                Console.WriteLine("No hay nada plantado en esta parcela");
                return false;
            }

            if (!(argPlot.timePassed >= argPlot.planted.timeNeeded))
            {
                Console.WriteLine("Esta parcela no esta lista para recolectar");
                result = false;
            }

            if (result && found)
            {
                found = false;
                Console.WriteLine("Seleccione el robot: ");
                printRobots();
                input = Console.ReadLine();
                stringValidate.Value = input;
                result = result && stringValidate.ValidateField();

                robotList.ForEach(newRobot =>
                {
                    if (!(found))
                    {
                        found = newRobot.id == input;
                        if (found)
                        {
                            argRobot = newRobot;
                        }
                    }
                });

                if (result && found)
                {
                    if (!argRobot.enabled)
                    {
                        Console.WriteLine("Este robot no esta habilitado");
                        result = false;
                    }
                    else if (argRobot.battery < 1)
                    {
                        Console.WriteLine("Este robot no tiene bateria");
                        result = false;
                    }
                    else
                    {
                        cropList[argPlot.planted][0]++;
                        cropList[argPlot.planted][1]++;
                    }
                }
                else
                {
                    Console.WriteLine("Robot no encontrado");
                    result = false;
                }
            }
            else
            {
                Console.WriteLine("Parcela no encontrada");
                result = false;
            }
            return result;
        }

        public bool waterData()
        {
            bool result;
            bool found = false;
            string input;
            Console.WriteLine("Seleccione la parcela: ");
            printPlots();
            input = Console.ReadLine();
            stringValidate.Value = input;
            result = stringValidate.ValidateField();

            plotList.ForEach(newPlot =>
               {
                   if (!(found))
                   {
                       found = newPlot.id == input;
                       if (found)
                       {
                           argPlot = newPlot;
                       }
                   }
               });

            if (!found)
            {
                Console.WriteLine("Parcela no encontrada");
                result = false;
            }
            return result;
        }

        public bool fixData()
        {
            bool result;
            bool found = false;
            string input;
            Console.WriteLine("Seleccione la parcela: ");
            printPlots();
            input = Console.ReadLine();
            stringValidate.Value = input;
            result = stringValidate.ValidateField();

            plotList.ForEach(newPlot =>
                {
                    if (!(found))
                    {
                        found = newPlot.id == input;
                        if (found)
                        {
                            argPlot = newPlot;
                        }
                    }
                });

            if (!argPlot.tempProblem && !argPlot.plague)
            {
                Console.WriteLine("No hay ningun problema en esta parcela");
                result = false;
            }

            if (result && found)
            {
                found = false;
                Console.WriteLine("Seleccione el robot: ");
                printRobots();
                input = Console.ReadLine();
                stringValidate.Value = input;
                result = result && stringValidate.ValidateField();

                robotList.ForEach(newRobot =>
                {
                    if (!(found))
                    {
                        found = newRobot.id == input;
                        if (found)
                        {
                            argRobot = newRobot;
                        }
                    }
                });

                if (result && found)
                {
                    int batteryNeeded = (argPlot.plague ? 1 : 0) + (argPlot.tempProblem ? 1 : 0);
                    if (!argRobot.enabled)
                    {
                        Console.WriteLine("Este robot no esta habilitado");
                        result = false;
                    }
                    else if (argRobot.battery < batteryNeeded)
                    {
                        Console.WriteLine("Este robot no tiene bateria");
                        result = false;
                    }
                }
                else
                {
                    Console.WriteLine("Robot no encontrado");
                    result = false;
                }
            }
            else
            {
                Console.WriteLine("Parcela no encontrada");
                result = false;
            }
            return result;
        }
    }
}