using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace PandaCoin
{
    public class Block
    {
        /// <summary>
        /// Czas utworzenia bloku.
        /// </summary>
        private readonly DateTime _timeStamp;

        /// <summary>
        /// Zera, jakie znajdować się będą na początku bloku w celu utrudnienia kopania. (Proof of work). 
        /// </summary>
        private long _nonce;

        /// <summary>
        /// Hash poprzedniego bloku w chainie.
        /// </summary>
        public string PreviousHash { get; set; }

        /// <summary>
        /// Dane, jakie zawierać będzie blok. Zmienić na prywatną po demonstracji.
        /// </summary>
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

        /// <summary>
        /// Hash wykopanego bloku. 
        /// </summary>
        public string Hash { get; private set; }

        public Block(DateTime timeStamp, List<Transaction> transactions, string previousHash = "")
        {
            _timeStamp = timeStamp;
            _nonce = 0;

            Transactions = transactions;
            PreviousHash = previousHash;
            Hash = CreateHash();
        }

        public void MineBlock(int proofOfWorkDifficulty)
        {
            var hashValidationTemplate = new string('0', proofOfWorkDifficulty);
            while (Hash.Substring(0, proofOfWorkDifficulty) != hashValidationTemplate)
            {
                _nonce++;
                Hash = CreateHash();
            }

            Console.WriteLine("Block with HASH={0} successfully mined)", Hash);
        }

        public string CreateHash()
        {
            using var sha256 = SHA256.Create();

            var rawData = PreviousHash + _timeStamp + Transactions + _nonce;

            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            return Encoding.Default.GetString(bytes);
        }
    }
}