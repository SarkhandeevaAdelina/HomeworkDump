using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tumakov7
{
    internal class Program
    {

        static void Main(string[] args)
        {
            dz71();
            Console.ReadKey();
        }
        public enum AccountType
        {
            Текущий,
            Сберегательный
        }

        class BankAccount
        {
            private long number;
            private decimal balance;
            private AccountType accountType;
            private static long uniqNum;

            public void FillIn(long number, decimal balance)
            {
                this.number = number;
                this.balance = balance;
            }
            public AccountType Type()
            {
                return accountType;
            }
            public static long UniqNumber()
            {
                return uniqNum++;
            }
            public decimal PutMoney(decimal summa)
            {
                balance += summa;
                return balance;
            }
            public bool WithdrawMoney(decimal summa)
            {
                bool examination = (balance >= summa);
                if (examination)
                {
                    balance -= summa;
                }
                return examination;
            }

            public void PrintValues()
            {
                Console.WriteLine("Номер аккаунта:" + number);
                Console.WriteLine("Баланс:" + balance);
                Console.WriteLine("Тип аккаунта:" + accountType);
            }


        }

        public static void up71()
        {
            BankAccount bankAccount = new BankAccount();
            Console.WriteLine("Введите номер банковского счета:");
            long number;
            while (!long.TryParse(Console.ReadLine(), out number))
            {
                Console.WriteLine("Неверный ввод, попробуйте еще раз");
            }

            Console.WriteLine("Введите баланс банковского счета:");
            decimal balance;
            while (!decimal.TryParse(Console.ReadLine(), out balance))
            {
                Console.WriteLine("Неверный ввод, попробуйте еще раз");
            }
            bankAccount.FillIn(number, balance);
            bankAccount.Type();
            bankAccount.PrintValues();
        }

        public static void up72()
        {
            BankAccount bankAccount = new BankAccount();
            long number = BankAccount.UniqNumber();
            Console.WriteLine("Введите баланс банковского счета:");
            decimal balance;
            while (!decimal.TryParse(Console.ReadLine(), out balance))
            {
                Console.WriteLine("Неверный ввод, попробуйте еще раз");
            }
            bankAccount.FillIn(number, balance);
            bankAccount.Type();
            bankAccount.PrintValues();
        }
        private static void TestPutMoney(BankAccount acc)
        {
            Console.WriteLine("Введите сумму");
            decimal sum;
            while (!decimal.TryParse(Console.ReadLine(), out sum))
            {
                Console.WriteLine("Неверный ввод, попробуйте еще раз");
            }
            acc.PutMoney(sum);
        }
        private static void TestWithdrawMoney(BankAccount acc)
        {
            Console.WriteLine("Введите сумму");
            decimal sum;
            while (!decimal.TryParse(Console.ReadLine(), out sum))
            {
                Console.WriteLine("Неверный ввод, попробуйте еще раз");
            }
            if (!acc.WithdrawMoney(sum))
            {
                Console.WriteLine("Невозможно снять данную сумму денег");
            }
        }
        public static void up73()
        {
            Console.Write("Введите операцию:");
            string operation = Console.ReadLine().ToLower();
            BankAccount bankAccount = new BankAccount();
            long number = BankAccount.UniqNumber();
            Console.WriteLine("Введите баланс банковского счета:");
            decimal balance;
            while (!decimal.TryParse(Console.ReadLine(), out balance))
            {
                Console.WriteLine("Неверный ввод, попробуйте еще раз");
            }
            bankAccount.FillIn(number, balance);
            bankAccount.Type();
            if (operation.Equals("положить на счет"))
            {
                TestPutMoney(bankAccount);
                bankAccount.PrintValues();
            }
            else if (operation.Equals("снять со счета"))
            {
                TestWithdrawMoney(bankAccount);
                bankAccount.PrintValues();
            }
            else
            {
                bankAccount.PrintValues();
            }
        }
        class Building
        {
            private static int uniqNumber = 1;
            private int number;
            private int height;
            private int countOfFloors;
            private int countOfApartments;
            private int countOfEntrances;
            public void FillIn(int number, int height, int countOfFloors, int countOfApartments, int countOfEntrances)
            {
                this.number = number;
                this.height = height;
                this.countOfFloors = countOfFloors;
                this.countOfApartments = countOfApartments;
                this.countOfEntrances = countOfEntrances;
            }
            public static int UniqNumber()
            {
                return uniqNumber++;
            }
            public double HeightFloor(double height, double countOfFloors)
            {
                double heightOfFloors;
                if (countOfFloors != 0)
                {
                    heightOfFloors = height / countOfFloors;
                }
                else
                {
                    heightOfFloors = height;
                }
                return heightOfFloors;
            }
            public int CountApartmentsInEntrance(int countOfApartments, int countOfEntrances)
            {
                int apartOfEntrances;
                if (countOfEntrances != 0)
                {
                    apartOfEntrances = countOfApartments / countOfEntrances;
                }
                else
                {
                    apartOfEntrances = countOfApartments;
                }
                return apartOfEntrances;
            }
            public int CountApartmentsOnFloor(int countOfApartments, int countOfFloors)
            {
                int apartOfFloors;
                if (countOfFloors != 0)
                {
                    apartOfFloors = countOfApartments / countOfFloors;
                }
                else
                {
                    apartOfFloors = countOfApartments;
                }
                return apartOfFloors;
            }
            public void PrintValues()
            {
                Console.WriteLine("Номер здания" + number);
                Console.WriteLine("Высота здания" + height);
                Console.WriteLine("Кол-во этажей в здании" + countOfFloors);
                Console.WriteLine("Кол-во квартир в здании" + countOfApartments);
                Console.WriteLine("Кол-во подъездов в здании" + countOfEntrances);
                Console.WriteLine("Высота одного этажа" + HeightFloor(height, countOfFloors));
                Console.WriteLine("Кол-во квартир в подъезде" + CountApartmentsInEntrance(countOfApartments, countOfEntrances));
                Console.WriteLine("Кол-во квартир на этаже" + CountApartmentsOnFloor(countOfApartments, countOfFloors));
            }
        }
        public static void dz71()
        {
            Building building = new Building();
            int number = Building.UniqNumber();
            Console.WriteLine("Введите высоту здания");
            int height;
            while (!int.TryParse(Console.ReadLine(), out height))
            {
                Console.WriteLine("Неверный ввод, попробуйте еще раз");
            }
            Console.WriteLine("Введите кол-во этажей в здании");
            int countOfFloors;
            while (!int.TryParse(Console.ReadLine(), out countOfFloors))
            {
                Console.WriteLine("Неверный ввод, попробуйте еще раз");
            }
            Console.WriteLine("Введите кол-во квартир в здании");
            int countApart;
            while (!int.TryParse(Console.ReadLine(), out countApart))
            {
                Console.WriteLine("Неверный ввод, попробуйте еще раз");
            }
            Console.WriteLine("Введите кол-во подъездов в здании");
            int countEnter;
            while (!int.TryParse(Console.ReadLine(), out countEnter))
            {
                Console.WriteLine("Неверный ввод, попробуйте еще раз");
            }
            building.FillIn(number, height, countOfFloors, countApart, countEnter);
            building.PrintValues();

        }

    }
}
