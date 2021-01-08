using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Cardano.Cipher.Generator;
using Cardano.Commons;
using ArrayExtensions;


namespace Cardano.Cipherer
{
    public static class Cipherer
    {
        public static void CipherFileWithCardano(string filePath, RotationMethod rotationMethod, GridType gridType)
        {
            var fileDirectory = Path.GetDirectoryName(filePath);
            var plainText = ReadTextFromFile(filePath);

            Console.WriteLine(plainText);

            var cipherObj = CardanoCipherGenerator.GenerateCardanoGrid(plainText.Length, rotationMethod, gridType);
            var cipher = cipherObj.CipherGrid;

            cipher.PrintArray();

            var cipherLength = cipher.GetLength(0) * cipher.GetLength(1);

            var sub = cipherLength - plainText.Length;
            if (sub != 0)
               plainText = plainText
                   .PadLeft(plainText.Length + (sub / 2), ' ')
                   .PadRight(plainText.Length + sub, ' ');

            var plainTextQueue = new Queue<char>(plainText.ToArray());

            char [,] cipheredText = new char [cipher.GetLength(0), cipher.GetLength(1)];


            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < cipher.GetLength(0); j++)
                {
                    for (int n = 0; n < cipher.GetLength(1); n++)
                    {
                        if (cipher[j, n] == 1)
                            cipheredText[j, n] = plainTextQueue.Dequeue();
                    }
                }

                switch (rotationMethod)
                {
                    case RotationMethod.StraightAngle:
                        if (gridType is GridType.Rect) goto case RotationMethod.OpenAngle;
                        cipher.Rotate90Clockwise();
                        break;
                    case RotationMethod.OpenAngle:
                        if(i % 2 == 0)
                            cipher.ReverseColumns();
                        else
                            cipher.ReverseRows();
                        break;
                }
            }

            cipheredText.PrintArray();

            string cipheredString = "";

            for (int j = 0; j < cipher.GetLength(0); j++)
            {
                for (int n = 0; n < cipher.GetLength(1); n++)
                {
                    cipheredString += cipheredText[j, n];
                }
            }

            Console.WriteLine(cipheredString);

            WriteCipherKeyToFile(fileDirectory + "key.bin", cipherObj);

            WriteTextToFile(fileDirectory + "ciphertext.txt", cipheredString);

        }

        public static void DecipherTextUsingKey(string keyPath, string cipheredTextPath)
        {
            var cipherObj = ReadCipherKeyFromFile(keyPath);

            var cipher = cipherObj.CipherGrid;

            cipher.PrintArray();

            var cipheredText = ReadTextFromFile(cipheredTextPath);

            var cipheredTextArray = new char[cipher.GetLength(0),cipher.GetLength(1)];

            var cipheredTextQueue = new Queue<char>(cipheredText.ToArray());

            for (int j = 0; j < cipher.GetLength(0); j++)
            {
                for (int n = 0; n < cipher.GetLength(1); n++)
                {
                    cipheredTextArray[j, n] = cipheredTextQueue.Dequeue();
                }
            }

            cipheredTextArray.PrintArray();

            var decipheredString = "";
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < cipher.GetLength(0); j++)
                {
                    for (int n = 0; n < cipher.GetLength(1); n++)
                    {
                        if (cipher[j, n] == 1)
                            decipheredString += cipheredTextArray[j, n];
                    }
                }

                switch (cipherObj.RotationMethod)
                {
                    case RotationMethod.StraightAngle:
                        if (cipherObj.GridType is GridType.Rect) goto case RotationMethod.OpenAngle;
                        cipher.Rotate90Clockwise();
                        break;
                    case RotationMethod.OpenAngle:
                        if(i % 2 == 0)
                            cipher.ReverseColumns();
                        else
                            cipher.ReverseRows();
                        break;
                }
            }

            decipheredString = decipheredString.Trim(' ');
            Console.WriteLine(decipheredString);
            WriteTextToFile( "plainText.txt",decipheredString);
        }

        private static void WriteTextToFile(string filePath, string text)
        {
            var fileStream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            using var bw = new StreamWriter(fileStream);
            bw.Write(text);
        }

        private static void WriteCipherKeyToFile(string filePath, Commons.Cipher cipher)
        {
            var fileStream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            using var bw = new BinaryWriter(fileStream);
            var bf = new BinaryFormatter();
            bf.Serialize(fileStream, cipher);
        }

        private static string ReadTextFromFile(string filePath)
        {
            var fileStream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite);
            using var bw = new StreamReader(fileStream);
            return bw.ReadToEnd();
        }

        private static Commons.Cipher ReadCipherKeyFromFile(string filePath)
        {
            var fileStream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite);
            using var bw = new BinaryReader(fileStream);
            var bf = new BinaryFormatter();
            var cipher = bf.Deserialize(fileStream) as Commons.Cipher;
            return cipher;
        }
    }
}