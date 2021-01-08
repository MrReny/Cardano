using System;
using System.Linq;
using ArrayExtensions;
using Cardano.Commons;

namespace Cardano.Cipher.Generator
{
    public class CardanoCipherGenerator
    {
        /// <summary>
        /// Генерация квадратного шифра
        /// </summary>
        /// <param name="cipherLength"></param>
        /// <param name="rotationMethod"></param>
        /// <param name="gridType"></param>
        /// <returns></returns>
        public static Commons.Cipher GenerateCardanoGrid(int cipherLength, RotationMethod rotationMethod, GridType gridType)
        {
            int edge1=0;
            int edge2=0;

            int newMessageLength=0;

            switch (gridType)
            {
                case GridType.Square:
                    var edge = Math.Sqrt(cipherLength);
                    if (edge % 1 != 0)
                    {
                        foreach (var square in Enumerable.Range(2, 100).Where((x) => x % 2 == 0)
                            .Select(x => new long[] {x, x * x}))
                        {
                            if (square[1] <= cipherLength) continue;

                            edge = square[0];
                            break;
                        }
                    }

                    if (edge % 2 != 0)
                        edge++;

                    edge1 = Convert.ToInt32(edge);
                    edge2 = edge1;
                    break;
                case GridType.Rect:
                    var tail = cipherLength % 2;
                    if (tail != 0)
                        cipherLength += tail;

                    edge1 = 2;
                    edge2 = cipherLength / 2;
                    if (edge2 % 2 != 0)
                        edge2++;

                    while (edge2 / edge1 > 4)
                    {
                        edge2 /= 2;
                        edge1 *= 2;
                    }

                    if (edge2 == edge1)
                    {
                        edge1 /= 2;
                        edge2 *= 2;
                    }
                    if (edge2 % 2 != 0)
                        edge2++;

                    break;
            }
            newMessageLength = edge1 * edge2;

            var cardanoGrid = new int[edge1, edge2];
            var rnd = new Random();

            for (int a = 0; a < newMessageLength/4; a++)
            {
                var availableRowsNumbersArray =
                    Enumerable.Range(0, edge1)
                        .Where((n) => cardanoGrid.GetRow(n).Contains(0)).ToArray();

                var randomRow = availableRowsNumbersArray[rnd.Next(0, availableRowsNumbersArray.Length - 1)];

                try
                {
                    var currentRow = cardanoGrid.GetRow(randomRow);
                    var possibilityArray = Enumerable.Range(0, edge2 )
                        .Where((n) => currentRow[n] == 0).ToArray();

                    var rndInt = possibilityArray[rnd.Next(0, possibilityArray.Length - 1)];

                    var j = 1;
                    cardanoGrid[randomRow, rndInt] = 1;
                    do
                    {
                        switch (rotationMethod)
                        {
                            case RotationMethod.StraightAngle:
                                if (gridType is GridType.Rect) goto case RotationMethod.OpenAngle;
                                cardanoGrid.Rotate90Clockwise();
                                break;
                            case RotationMethod.OpenAngle:
                                if(j % 2 == 0)
                                    cardanoGrid.ReverseColumns();
                                else
                                    cardanoGrid.ReverseRows();
                                break;
                        }
                        cardanoGrid[randomRow, rndInt] = -1;
                        j++;
                    } while (j < 4);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            return new Commons.Cipher() {CipherGrid = cardanoGrid, RotationMethod = rotationMethod, GridType = gridType};
        }

        private static int[,] GenerateRectangleGrid(int cipherLength)
        {
            throw new Exception();
        }
    }
}