using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static HomeworkClass.Program;
using static System.Collections.Specialized.BitVector32;

namespace HomeworkClass
{
    internal class Program
    {
        enum GarbageTypes
        {
            Пластик,
            Стекло,
            Железо
        }
        static void Main(string[] args)
        {
            Random rand = new Random();
            string path = @"C:\Users\Семья\SOURCE\repos\HomeworkClass\HomeworkClass\Data.csv";

            List<Employee> employees = new List<Employee>();

            var f = Params.ReadFromFile(path);
            foreach (var item in f)
            {
                string[] info = item.Split(';');
                string position = info[1];
                Params.ids.Add(uint.Parse(info[0]));
                switch (position)
                {
                    case "Водитель мусоровоза":
                        employees.Add(new Driver(uint.Parse(info[0]), info[2], info[3], info[4], int.Parse(info[5]), int.Parse(info[6]), info[7], info[8], int.Parse(info[0])));
                        break;
                    case "Лаборант":
                        employees.Add(new LabAssistant(uint.Parse(info[0]), info[2], info[3], info[4], int.Parse(info[5]), info[6], info[7], int.Parse(info[8])));
                        break;
                    case "Сортировщик мусора":
                        employees.Add(new GarbageSorter(uint.Parse(info[0]), info[2], info[3], info[4], int.Parse(info[5]), info[6], int.Parse(info[7])));
                        break;
                    default:
                        break;
                }

            }

            foreach (Employee employee in employees)
            {
                if (employee is Driver driver)
                {
                    Console.WriteLine(driver.GetFullInfo());
                    Console.WriteLine(driver.Enter());
                    driver.RemoveHours(1);
                    driver.TakeCarFromGarage();
                    driver.GetToSector();
                    driver.LoadGarbage();
                    driver.UnloadGarbage();
                    driver.PutCarInGarage();
                    driver.AddHours(10);

                }
                else if (employee is LabAssistant labAssistant)
                {
                    Console.WriteLine(labAssistant.GetFullInfo());
                    labAssistant.PutCarInParkingLot();
                    labAssistant.RemoveHours(1);
                    Console.WriteLine(labAssistant.Enter());
                    for (int i = 0; i < 3; i++)
                    {
                        Console.Write($"Введите пароль (осталось попыток: {3 - i}): ");
                        string password = Console.ReadLine();
                        if (labAssistant.GetEnterToLab(password))
                        {
                            Console.WriteLine("Пароль верный. Вход в лабораторию открыт!");
                            break;
                        }
                        if (i == 2)
                        {

                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.Clear();
                            Console.WriteLine("Вы ввели неверный пароль три раза. Конец программы!");
                            return;
                        }
                    }
                    labAssistant.SoilContaminationAssessment();
                    labAssistant.TakeBrake();
                    labAssistant.AddHours(10);
                    labAssistant.TakeCarParkingLot();

                }
                else if (employee is GarbageSorter garbageSorter)
                {
                    Console.WriteLine(garbageSorter.GetFullInfo());
                    garbageSorter.PutCarInParkingLot();
                    Console.WriteLine(garbageSorter.Enter());
                    garbageSorter.RemoveHours(1);
                    for (int i = 0; i < 3; i++)
                    {
                        Console.Write($"Введите пароль (осталось попыток: {3 - i}): ");
                        string password = Console.ReadLine();
                        if (garbageSorter.GetOpenLocker(password))
                        {
                            Console.WriteLine("Пароль верный. Шкафчик открыт!");
                            break;
                        }
                        if (i == 2)
                        {

                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.Clear();
                            Console.WriteLine("Вы ввели неверный пароль три раза. Конец программы!");
                            return;
                        }
                    }
                    string current = garbageSorter.StartSort();
                    Console.WriteLine($"Какой это типа мусора (пластик, стекло или бумага)?: {current}");
                    string type = Console.ReadLine().ToLower();
                    garbageSorter.StopSort(type);
                    garbageSorter.AddHours(10);

                }
            }
            Console.WriteLine("Все хорошо поработали!");
            Console.ReadKey();


        }
    }
        public static class Params
        {
            public static List<uint> ids = new List<uint>();
            public static List<string> ReadFromFile(string path)
            {
                List<string> employees = new List<string>();
                StreamReader reader = new StreamReader(path);
                while (!reader.EndOfStream)
                {
                    employees.Add(reader.ReadLine());
                }
                return employees;
            }
        }

        public abstract class Employee
        {
            public uint ID { get; } // ID присваивается один раз и больше никогда не меняется
            public string Surname { get; set; }
            public string Name { get; set; }
            public string Patronymic { get; set; }
            private int _hours = 0;

            public Employee(uint id)
            {
                ID = id;
            }

            public Employee(uint id, string surname, string name, string patronymic, int hours)
            {
                ID = id;
                Surname = surname;
                Name = name;
                Patronymic = patronymic;
                _hours = hours;
            }

            public string Enter() // пропуск сотрудника на территорию
            {
                if (Params.ids.Contains(ID))
                {
                    return GetDestination();
                }
                return string.Empty;
            }

            public abstract string GetFullInfo();

            public string GetDestination()
            {
                if (GetType() == typeof(Driver))
                {
                    return "Зона выгрузки";
                }
                else if (GetType() == typeof(LabAssistant))
                {
                    return "Лаборатория контроля загрязнений";
                }
                else if (GetType() == typeof(GarbageSorter))
                {
                    return "Производственная зона";
                }
                return string.Empty;
            }

            public void AddHours(int hours)
            {
                _hours += hours;
                Console.WriteLine($"Успешно добавлено {hours} часов");
            }

            public void RemoveHours(int hours)
            {
                if (_hours - hours < 0)
                {
                    Console.WriteLine("Нельзя отнять столько часов!");
                    return;
                }
                _hours -= hours;
                Console.WriteLine($"Успешно удалено {hours} часов");
            }


        }

        public class Driver : Employee
        {
            public int DrivingLicense { get; }
            public string RegMark { get; set; }
            public string Sector { get; set; }
            public int TravelHours { get; set; }
            private bool _isLoaded = false;
            private bool _isCarInGarage = false;
            public Driver(uint id) : base(id)
            {

            }
            public Driver(uint id, string surname, string name, string patronymic, int hours) :
                base(id, surname, name, patronymic, hours)
            {

            }
            public Driver(uint id, string surname, string name, string patronymic, int hours, int dl) :
               base(id, surname, name, patronymic, hours)
            {
                DrivingLicense = dl;
            }
            public Driver(uint id, string surname, string name, string patronymic, int hours, int dl, string regmark, string sector, int travelHours) :
               base(id, surname, name, patronymic, hours)
            {
                DrivingLicense = dl;
                RegMark = regmark;
                Sector = sector;
                TravelHours = travelHours;
            }

            public override string GetFullInfo()
            {
                return $"Водитель {ID}: {Surname} {Name} {Patronymic}, ВУ: {DrivingLicense}, Автомобиль: {RegMark}, Район: {Sector}, Время в пути: {TravelHours}";
            }

            public void LoadGarbage()
            {
                if (_isLoaded)
                {
                    Console.WriteLine("Машина уже загружена");
                }
                else
                {
                    _isLoaded = true;
                    Console.WriteLine("Машина успешно загружена");
                }
            }
            public void UnloadGarbage()
            {
                if (_isLoaded)
                {
                    _isLoaded = true;
                    Console.WriteLine("Машина успешно разгружена");
                }
                else
                {
                    Console.WriteLine("Машина пустая");
                }
            }

            public void GetToSector()
            {
                Console.WriteLine($"Водитель отправился в район {Sector}");
            }

            public void PutCarInGarage()
            {
                if (_isCarInGarage)
                {
                    Console.WriteLine("Машина уже в гараже!");
                }
                else
                {
                    _isCarInGarage = true;
                    Console.WriteLine("Машина успешно поставлена в гараж!");
                }
            }

            public void TakeCarFromGarage()
            {
                if (_isCarInGarage)

                {
                    _isCarInGarage = false;
                    Console.WriteLine("Машина выгнана из гаража");
                }
                else
                {
                    Console.WriteLine("Машина успешно покинула гараж!");
                }
            }




        }

        public class LabAssistant : Employee
        {
            public string LabPassword { get; }
            public string WorkPermit { get; set; }
            public int BreakHours { get; set; }
            private bool _isCarInParkingLot = false;
            public LabAssistant(uint id) : base(id)
            {

            }
            public LabAssistant(uint id, string surname, string name, string patronymic, int hours, string lp) :
                base(id, surname, name, patronymic, hours)
            {
                LabPassword = lp;
            }
            public LabAssistant(uint id, string surname, string name, string patronymic, int hours, string lp, string permit, int breakHours) :
               base(id, surname, name, patronymic, hours)
            {
                LabPassword = lp;
                WorkPermit = permit;
                BreakHours = breakHours;
            }
            public void SoilContaminationAssessment()
            {
                Console.WriteLine("Введите количество содержание свинца в почве (Pb):");
                double pb = double.Parse(Console.ReadLine());
                Console.WriteLine("Введите количество содержание кадмияв почве (Cd):");
                double cd = double.Parse(Console.ReadLine());
                Console.WriteLine("Введите количество содержание цинка в почве (Zn):");
                double zn = double.Parse(Console.ReadLine());
                Console.WriteLine("Введите количество содержание ртути в почве (Hg):");
                double hg = double.Parse(Console.ReadLine());
                double pollution_degree = (pb + cd + zn + hg) / 4;
                if (pollution_degree < 8)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Допустимая степень загрязнения.");
                    Console.ResetColor();
                }
                else if (pollution_degree > 8 && pollution_degree < 16)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Слабая степень загрязнения.");
                    Console.ResetColor();
                }
                else if (pollution_degree > 16 && pollution_degree < 32)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Средняя степень загрязнения.");
                    Console.ResetColor();
                }
                else if (pollution_degree > 32 && pollution_degree < 64)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Сильная степень загрязнения!!!");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("Территория потенциально опасна для пребывания!!!");
                    Console.ResetColor();
                }

            }
            public void PutCarInParkingLot()
            {
                if (_isCarInParkingLot)
                {
                    Console.WriteLine("Машина уже припаркована!");
                }
                else
                {
                    _isCarInParkingLot = true;
                    Console.WriteLine("Машина успешно припаркована!");
                }
            }
            public bool GetEnterToLab(string password)
            {
                return LabPassword == password;
                
            }
            public void TakeCarParkingLot()
            {
                if (_isCarInParkingLot)

                {
                    _isCarInParkingLot = false;
                    Console.WriteLine("Машина уже покинула парковку");
                }
                else
                {
                    Console.WriteLine("Машина успешно покинула парковку!");
                }
            }
            public void TakeBrake()
            {
            Console.WriteLine("Перерыв 1 час");
            }

            public override string GetFullInfo()
            {
                return $"Лаборант {ID}: {Surname} {Name} {Patronymic}, Разрешение на работу: {WorkPermit}, Часы перерыва {BreakHours}";
            }
        }

        public class GarbageSorter : Employee
        {
            private Random rand = new Random();
            private Dictionary<string, string[]> garbage = new Dictionary<string, string[]>
            {
                {"пластик", new string[] { "Пластиковый стакан", "Игрушечная машинка", "Кукла"} },
               {"стекло", new string[] {"Стеклянная бутылка", "Ваза", "Стеклянный шар"} },
               {"бумага", new string[] {"Листовка", "Журнал", "Туалетная бумага"} }
            };
            private string _currentGarbage;
            public string LockerPassword { get; }
            public int BreakHours { get; set; }
            private bool _isCarInParkingLot = false;
            public GarbageSorter(uint id) : base(id)
            {

            }
            public GarbageSorter(uint id, string surname, string name, string patronymic, int hours, string locp) :
            base(id, surname, name, patronymic, hours)
            {
                LockerPassword = locp;
            }
            public GarbageSorter(uint id, string surname, string name, string patronymic, int hours, string locp, int breakHours) :
           base(id, surname, name, patronymic, hours)
            {
                LockerPassword = locp;
                //
                BreakHours = breakHours;
            }
            public string StartSort()
            {
                string[] g = garbage.ElementAt(rand.Next(0, garbage.Count)).Value;
                string current = g[rand.Next(0, g.Length)];
                _currentGarbage = current;
                return current;
                
            }

            public void StopSort(string choice)
            {
                if (garbage.ContainsKey(choice))
                {
                    if (garbage[choice].Contains(_currentGarbage))
                    {
                        Console.WriteLine("Успешно отсортировано");
                    }
                    else
                    {
                        Console.WriteLine("Неправильный выбор!");
                    }
                }
                else
                {
                    Console.WriteLine("Нет такого типа мусора");
                }

              
            }
            public bool GetOpenLocker(string password)
            {
                return LockerPassword == password;
            }
            public void PutCarInParkingLot()
            {
                if (_isCarInParkingLot)
                {
                    Console.WriteLine("Машина уже припаркована!");
                }
                else
                {
                    _isCarInParkingLot = true;
                    Console.WriteLine("Машина успешно припаркована!");
                }
            }
            public void TakeCarParkingLot()
            {
                if (_isCarInParkingLot)

                {
                    _isCarInParkingLot = false;
                    Console.WriteLine("Машина уже покинула парковку");
                }
                else
                {
                    Console.WriteLine("Машина успешно покинула парковку!");
                }
            }

            public override string GetFullInfo()
            {
                return $"Сортировщик мусора {ID}: {Surname} {Name} {Patronymic}, Часы перерыва: {BreakHours}";
            }


        }


  }


