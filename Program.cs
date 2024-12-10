using System;
using System.Collections.Generic;
using System.Linq;

namespace Amnistion
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string AmnestyCrimeKind = "власть не любил";

            List<string> lastNames = new List<string>
            {
                "Кукуев", "Печкин", "Вертляев", "Сидоренко", "Потапенко", "Заслонов", "Креольский", "Чубров", "Семочкин"
            };
            List<string> firstNames = new List<string>
            {
                "Александр", "Виктор", "Алексей", "Сергей", "Дмитрий", "Олег", "Андрей", "Ибрагим", "Варфаломей"
            };
            List<string> patronymics = new List<string>
            {
                "Батькович", "Сергеевич", "Александрович", "Исаакович", "Вессарионович", "Олегович", "Алексеевич"
            };
            List<string> crimesKinds = new List<string>
            {
                "стырил гвоздь", "голубям фигушки показывал", "плевал в потолок", "грыз ногти на ногах", "власть не любил"
            };
            CriminalFactory criminalFactory = new CriminalFactory(lastNames, firstNames, patronymics, crimesKinds);
            CrimeBook crimeBook = new CrimeBook(criminalFactory, 50, AmnestyCrimeKind);

            crimeBook.Execute();
        }
    }

    class CrimeBook
    {
        private CriminalFactory _criminalFactory;
        private List<Criminal> _criminals;
        private string _amnestyCrimeKind;

        public CrimeBook(CriminalFactory criminalFactory, int count, string amnestyCrimeKind)
        {
            _criminalFactory = criminalFactory;
            _criminals = _criminalFactory.Create(count);
            _amnestyCrimeKind = amnestyCrimeKind;
        }

        public void Execute()
        {
            Console.WriteLine($"До амнистии было {_criminals.Count} человек.\n");
            ShowCriminalsInfo(_criminals);

            Amnesty(_amnestyCrimeKind);

            Console.WriteLine($"После амнистии стало {_criminals.Count} человек.\n");
            ShowCriminalsInfo(_criminals);
        }

        private void Amnesty(string amnestyCrimeKind)
        {
            _criminals = _criminals.Except(_criminals.Where(criminal => criminal.CrimeKind.ToLower() == amnestyCrimeKind.ToLower())).ToList();
        }

        private void ShowCriminalsInfo(List<Criminal> criminals)
        {
            foreach (Criminal criminal in criminals)
            {
                criminal.ShowInfo();
            }

            Console.WriteLine();
        }
    }

    class CriminalFactory
    {
        private List<string> _lastNames;
        private List<string> _firstNames;
        private List<string> _patronymics;
        private List<string> _crimesKinds;

        public CriminalFactory(List<string> lastNames, List<string> firstNames, List<string> patronymics, List<string> crimesKinds)
        {
            _lastNames = lastNames;
            _firstNames = firstNames;
            _patronymics = patronymics;
            _crimesKinds = crimesKinds;
        }

        public List<Criminal> Create(int count)
        {
            List<Criminal> criminals = new List<Criminal>();

            for (int i = 0; i< count; i++)
            {
                criminals.Add(new Criminal(GenerateFullName(), _crimesKinds[UserUtills.GenerateLimitedPositiveNumber(_crimesKinds.Count)]));
            }

            return criminals;
        }

        private string GenerateFullName()
        {
            string separator = " ";

            string fullName = _lastNames[UserUtills.GenerateLimitedPositiveNumber(_lastNames.Count)]
                + separator + _firstNames[UserUtills.GenerateLimitedPositiveNumber(_firstNames.Count)]
                + separator + _patronymics[UserUtills.GenerateLimitedPositiveNumber(_patronymics.Count)];

            return fullName;
        }
    }

    class Criminal
    {
        private string _fullName;

        public Criminal(string fullName, string crimeKind)
        {
            _fullName = fullName;
            CrimeKind = crimeKind;
        }

        public string CrimeKind { get; }

        public void ShowInfo()
        {
            Console.WriteLine($"{_fullName} осуждён за то, что {CrimeKind}.");
        }
    }

    static class UserUtills
    {
        private static Random s_random = new Random();

        public static int GenerateLimitedPositiveNumber(int maxValueExclusive)
        {
            return s_random.Next(maxValueExclusive);
        }
    }
}
