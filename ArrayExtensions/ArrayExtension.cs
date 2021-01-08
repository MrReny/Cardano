using System;
using System.Linq;

namespace ArrayExtensions
{

    public static class ArrayExtension
    {
        public static T[] GetColumn<T>(this T[,] matrix, int columnNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(0))
                .Select(x => matrix[x, columnNumber])
                .ToArray();
        }

        public static T[] GetRow<T>(this T[,] matrix, int rowNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(1))
                .Select(x => matrix[rowNumber, x])
                .ToArray();
        }

        public static void Rotate90Clockwise<T>(this T[,] a)
        {
            var n = a.GetLength(0);

            for (var i = 0; i < n / 2; i++)
            {
                for (var j = i; j < n - i - 1; j++)
                {
                    T temp = a[i, j];

                    a[i, j] = a[n - 1 - j, i];

                    a[n - 1 - j, i] = a[n - 1 - i, n - 1 - j];

                    a[n - 1 - i, n - 1 - j] = a[j, n - 1 - i];

                    a[j, n - 1 - i] = temp;
                }
            }
        }

        public static void ReverseColumns<T>(this T[, ] arr)
        {
            for (int i = 0; i < arr.GetLength(1); i++)
            {
                for (int j = 0, k = arr.GetLength(0) - 1; j < k; j++, k--)
                {
                    var t = arr[j, i];
                    arr[j, i] = arr[k, i];
                    arr[k, i] = t;
                }
            }
        }

        public static void ReverseRows<T>(this T[, ] arr)
        {
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0, k = arr.GetLength(1) - 1; j < k; j++, k--)
                {
                    var t = arr[i, j];
                    arr[i, j] = arr[i, k];
                    arr[i, k] = t;
                }
            }
        }

        public static void Transpose<T>(this T[, ] arr)
        {
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = i; j < arr.GetLength(1); j++)
                {
                    var t = arr[i, j];
                    arr[i, j] = arr[j, i];
                    arr[j, i] = t;
                }
            }
        }


        public static void PrintArray<T>(this T[,] grid)
        {
            for (int i = 0; i < grid.GetLength(0) ; i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    Console.Write($"{grid[i,j]}\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

    }
}