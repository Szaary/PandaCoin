namespace PandaCoin
{
    /// <summary>
    /// Klasa zawierająca dane pojedynczej transakcji w blockchainie.
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Dane osoby wysyłającej kryptowalutę.
        /// </summary>
        public string From { get; }

        /// <summary>
        /// Osoba, która dostaje kryptowalutę.
        /// </summary>
        public string To { get; }

        /// <summary>
        /// Ilość wysyłanych pieniędzy.
        /// </summary>
        public double Amount { get; }

        public Transaction(string from, string to, double amount)
        {
            From = from;
            To = to;
            Amount = amount;
        }
    }
}