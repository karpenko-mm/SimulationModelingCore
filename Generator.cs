using System;
using System.Text;

namespace Modeling.Core {

    /// <summary>
    /// Transact generator
    /// </summary>
    public class Generator : AbstractDevice {

        readonly double generationIntevalMin;
        readonly double generationIngervalMax;
        readonly int transactCountMin;
        readonly int transactCountMax;
        double ticksToNextRemaining;

        /// <summary>
        /// Количество сгенерированных транзактов за время моделирования
        /// </summary>
        public int TransactsCount { get; private set; }

        /// <summary>
        /// The number of generated transactions during the simulation
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name of the component</param>
        /// <param name="generationIntevalMin">Min transaction generation interval</param>
        /// <param name="generationIngervalMax">Max transaction generation interval</param>
        /// <param name="transactCountMin">The minimum number of transactions generated at a time (default 1)</param>
        /// <param name="transactCountMax">The maximum number of transactions generated at a time (default 1)</param>
        public Generator(string name, double generationIntevalMin, double generationIngervalMax,
            int transactCountMin = 1, int transactCountMax = 1) 
            : base(name) {
            if (generationIntevalMin > generationIngervalMax || transactCountMin > transactCountMax) {
                throw new InvalidOperationException();
            }
            this.generationIntevalMin = generationIntevalMin;
            this.generationIngervalMax = generationIngervalMax;
            this.transactCountMin = transactCountMin;
            this.transactCountMax = transactCountMax;
            ticksToNextRemaining = RandomGenerator.NextDouble(generationIntevalMin, generationIngervalMax);
        }

        /// <summary>
        /// Event called when generating a new transaction
        /// </summary>
        public event Action<int> OnTransactGenerated;

        /// <summary>
        /// Processing of the model time unit
        /// </summary>
        public override void Tick() {
            base.Tick();
            if (!Enabled) {
                return;
            }
            while (ticksToNextRemaining <= 1) {
                ticksToNextRemaining += RandomGenerator.NextDouble(generationIntevalMin, generationIngervalMax);
                Generate();
            }
            ticksToNextRemaining--;
        }

        void Generate() {
            int count = RandomGenerator.NextInt(transactCountMin, transactCountMax + 1);
            TransactsCount += count;
            if (OnTransactGenerated != null) {
                OnTransactGenerated(count);
            }
        }

        /// <summary>
        /// Returns a string representation of the state of the component
        /// </summary>
        public override string ToString() {
            var sb = new StringBuilder();
            sb.AppendLine(Name + ":");
            sb.AppendLine("".PadLeft(Name.Length + 1, '-'));
            sb.AppendFormat("Transactions Generated: {0}\r\n", TransactsCount);
            return sb.ToString();
        }
    }
}
