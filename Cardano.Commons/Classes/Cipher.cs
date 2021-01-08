using System;

namespace Cardano.Commons
{
    [Serializable]
    public class Cipher
    {
        public RotationMethod RotationMethod { get; set; }
        public int[,] CipherGrid { get; set; }
        public GridType GridType { get; set; }
    }
}