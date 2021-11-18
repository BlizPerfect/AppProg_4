using System;
using System.Collections.Generic;
using System.Linq;

namespace AppProg_4
{
    class AnalysisOfHierarchies
    {
        public int NumberOfCriterias { private set; get; }
        public double[,] Coefficients { private set; get; }
        public List<string> Criterias { private set; get; }
        private List<double> Sums = new List<double>();
        private List<double> Proportions = new List<double>();
        public double Sum { private set; get; }
        public double Prop { private set; get; }
        private int Rows;
        private int Columns;

        public AnalysisOfHierarchies()
        {
            Criterias = new List<string>();

            EnterNumberOfCriterias();

            Sums.Capacity = NumberOfCriterias;
            Proportions.Capacity = NumberOfCriterias;

            Coefficients = new double[NumberOfCriterias, NumberOfCriterias];
            Rows = Coefficients.GetUpperBound(0) + 1;
            Columns = Coefficients.GetUpperBound(1) + 1;
            InitializeCoefficients();

            EnterCriterias();
            FillInCoefficients();
        }

        /// <summary>
        /// Обновление поля Prop.
        /// </summary>
        /// <returns>void</returns>
        public void AddAllProportions()
        {
            foreach (var proportion in Proportions)
            {
                Prop += proportion;
            }
        }

        /// <summary>
        /// Приведение поля Prop к значению, равному 1.
        /// </summary>
        /// <returns>void</returns>
        public void FixProportions(double value)
        {
            var maxElementIndex = Proportions.LastIndexOf(Proportions.Max());
            var temp = Proportions[maxElementIndex] + value;
            Proportions[maxElementIndex] = temp;
            Prop = 0;
            AddAllProportions();
        }

        /// <summary>
        /// Высчитывание всех пропорций.
        /// </summary>
        /// <returns>void</returns>
        public void CalculateProportions()
        {
            foreach (var e in Sums)
            {
                double proportion = 0;
                proportion = Math.Round(e / Sum, 2);
                Proportions.Add(proportion);
            }
            AddAllProportions();
        }

        /// <summary>
        /// Высчитывание всех сумм.
        /// </summary>
        /// <returns>void</returns>
        public void CalculateSums()
        {
            Console.Clear();
            for (int i = 0; i < Rows; i++)
            {
                double sum = 0;
                for (int j = 0; j < Columns; j++)
                {
                    sum += Coefficients[i, j];
                }
                Sums.Add(sum);
                Sum += sum;
            }
            Sum = Math.Round(Sum, 2);
        }

        /// <summary>
        /// Заполнение коэффициентов.
        /// </summary>
        /// <returns>void</returns>
        public void FillInCoefficients()
        {
            Console.Clear();
            for (var i = 0; i < Criterias.Count - 1; i++)
            {
                for (var j = i + 1; j < Criterias.Count; j++)
                {
                    ShowHierarchy();
                    Console.SetCursorPosition(0, Rows + 2);
                    Console.WriteLine("Введите отношение критерия \"" + Criterias[i] + "\" к критерию \"" + Criterias[j] + "\":");
                    double coefficient = InputCount();
                    Coefficients[i, j] = coefficient;
                    Coefficients[j, i] = Math.Round(1 / coefficient, 2);
                }
            }
        }

        /// <summary>
        /// Начальная инициализация коэффициентов.
        /// </summary>
        /// <returns>void</returns>
        public void InitializeCoefficients()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    if (i == j)
                    {
                        Coefficients[i, j] = 1;
                    }
                    else
                    {
                        Coefficients[i, j] = -1;
                    }
                }
            }
        }

        /// <summary>
        /// Ввод числа.
        /// </summary>
        /// <returns>double</returns>
        public double InputCount()
        {
            double result = 0;
            while (!double.TryParse(Console.ReadLine(), out result))
            {
                Console.WriteLine("Вы ввели не число, попробуйте снова.");
            }
            return result;
        }

        /// <summary>
        /// Ввод числа критериев.
        /// </summary>
        /// <returns>void</returns>
        public void EnterNumberOfCriterias()
        {
            Console.WriteLine("Введите количество критериев: ");
            NumberOfCriterias = (int)InputCount();
            Console.Clear();
        }

        /// <summary>
        /// Ввод критериев.
        /// </summary>
        /// <returns>void</returns>
        public void EnterCriterias()
        {
            for (var i = 0; i < NumberOfCriterias; i++)
            {
                Console.Clear();
                Console.WriteLine("Введите название критерия номер: " + (i + 1));
                Criterias.Add(Console.ReadLine());
            }
            Console.Clear();
        }

        /// <summary>
        /// Отрисовка таблицы иерархии.
        /// </summary>
        /// <returns>void</returns>
        public void ShowHierarchy()
        {
            Console.Clear();
            Console.Write((String.Format("|{0, -10}|", "")));
            foreach (var criteria in Criterias)
            {
                Console.Write(String.Format("|{0, -10}|", criteria));
            }
            Console.Write("\n");
            foreach (var criteria in Criterias)
            {
                Console.WriteLine(String.Format("|{0, -10}|", criteria));
            }
            int row = 1;
            Console.SetCursorPosition(12, row);
            for (int i = 0; i < Rows; i++)
            {
                Console.SetCursorPosition(12, row);
                for (int j = 0; j < Columns; j++)
                {
                    if (Coefficients[i, j] != -1)
                    {
                        Console.Write(String.Format("|{0, -10}|", Coefficients[i, j]));
                    }
                    else
                    {
                        Console.Write(String.Format("|{0, -10}|", " "));
                    }
                }
                row += 1;
                Console.WriteLine();
            }
            if (Sums.Count > 0)
            {
                int rowSums = 0;
                int width = (NumberOfCriterias + 1) * 12;
                Console.SetCursorPosition(width, rowSums);
                Console.Write(String.Format("|{0, -10}|", "Сумма"));
                foreach (var sum in Sums)
                {
                    rowSums += 1;
                    Console.SetCursorPosition(width, rowSums);
                    Console.Write(String.Format("|{0, -10}|", sum));
                }
                rowSums += 1;
                Console.SetCursorPosition(width, rowSums);
                Console.Write(String.Format("|{0, -10}|", Sum));
            }
            if (Proportions.Count > 0)
            {
                int rowProps = 0;
                int width = (NumberOfCriterias + 2) * 12;
                Console.SetCursorPosition(width, rowProps);
                Console.Write(String.Format("|{0, -10}|", "Вес. Коэф."));
                foreach (var prop in Proportions)
                {
                    rowProps += 1;
                    Console.SetCursorPosition(width, rowProps);
                    Console.Write(String.Format("|{0, -10}|", prop));
                }
                rowProps += 1;
                Console.SetCursorPosition(width, rowProps);
                Console.Write(String.Format("|{0, -10}|", Prop));
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var hierarchy = new AnalysisOfHierarchies();
            hierarchy.ShowHierarchy();
            hierarchy.CalculateSums();
            hierarchy.CalculateProportions();
            if (hierarchy.Prop != 1.0)
            {
                var temp = Math.Round(1.0 - hierarchy.Prop, 2);
                hierarchy.FixProportions(temp);
            }
            hierarchy.ShowHierarchy();
            Console.ReadLine();
        }
    }
}
