using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace PandaCoin
{
    /// <summary>
    /// Klasa tworząca pojedynczy blok blockchainu. 
    /// </summary>
    public class Block
    {
        /// <summary>
        /// Czas utworzenia bloku.
        /// </summary>
        private readonly DateTime _timeStamp;

        /// <summary>
        /// Wartość jaką zmieniamy podczas każdej próby znalezienia prawidłowego bloku, tak by w każdym
        /// obrocie pętli hash byl wyliczany na postawie innego inputu.  
        /// </summary>
        private long _nonce;

        /// <summary>
        /// Hash poprzedniego bloku w chainie. 
        /// </summary>
        public string PreviousHash { get; set; }

        /// <summary>
        /// Dane, jakie zawierać będzie blok. Publiczna w celach testowych, ustawiony celowo set, by mozna bylo
        /// zaprezentowac validacje bloku.
        /// </summary>
        public List<Transaction> Transactions { get; set; }

        /// <summary>
        /// Hash wykopanego bloku. 
        /// </summary>
        public string Hash { get; private set; }

        
        /// <summary>
        /// W konstruktorze bloku tworzony jest hash tego bloku. W ten sposób możemy później łatwo znaleźć
        /// zmiany w bloku, jeśli takie by nastąpiły. (Próby oszukania)
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <param name="transactions"></param>
        /// <param name="previousHash"></param>
        public Block(DateTime timeStamp, List<Transaction> transactions, string previousHash = "")
        {
            _timeStamp = timeStamp;
            _nonce = 0;

            Transactions = transactions;
            PreviousHash = previousHash;
            Hash = CreateHash();
        }

        /// <summary>
        /// Funkcja tworząca nowy blok. W pętli while będzie działać do czasu, aż znajdzie taki hash, gdzie liczba
        /// zer na początku będzie równa proofOfWorkDifficulty. 
        /// </summary>
        /// <param name="proofOfWorkDifficulty">Trudność stworzenia nowego bloku. Zawiera informację
        /// jak dużo zer powinno znajdować się na początku bloku.</param>
        /// hashValidationTemplate - schemat, na podstawie jakiego sprawdzana jest poprawnosć hasha.
        /// składa się z samych zer o długości proofOfWorkDifficulty.
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

        /// <summary>
        /// Do wyliczenia bloku używamy SHA256.
        /// </summary>
        /// <returns></returns>
        public string CreateHash()
        {
            using (var sha256 = SHA256.Create())
            {
                var blockTransactions="";
                foreach (var transaction in Transactions)
                {
                    blockTransactions += transaction.From + transaction.To + transaction.Amount;
                }
                
                var rawData = PreviousHash + _timeStamp + blockTransactions + _nonce;
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                return Encoding.Default.GetString(bytes);
            } 
        }
    }
}