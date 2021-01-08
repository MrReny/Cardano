using Cardano.Cipherer;
using Cardano.Commons;

namespace EasyCipherer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Cipherer.CipherFileWithCardano("plaintext.txt", RotationMethod.OpenAngle, GridType.Rect);
            Cipherer.DecipherTextUsingKey("key.bin", "ciphertext.txt");
        }
    }
}