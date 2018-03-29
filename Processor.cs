using System;
using System.Text;

namespace Modeling.Core {

    /// <summary>
    /// Processing device
    /// </summary>
    public class Processor : AbstractDevice {

        readonly double processingTimeMin;
        readonly double processingTimeMax;
        double ticksToEndRemaining;
        int transactsInWork;

        /// <summary>
        /// Number of processed transactions
        /// </summary>
        public int TransactsCount { get; private set; }

        /// <summary>
        /// Device busy time
        /// </summary>
        public double BusyTime { get; private set; }

        /// <summary>
        /// Device idle time
        /// </summary>
        public double IdleTime {
            get {
                return TicksCount - BusyTime;
            }
        }

        /// <summary>
        /// Returns true if device is busy
        /// </summary>
        public bool IsBusy {
            get {
                return transactsInWork > 0;
            }
        }

        /// <summary>
        /// Utilization rate
        /// </summary>
        public double LoadRate {
            get {
                return TicksCount != 0 ? BusyTime / TicksCount : 0;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name = "name">Name of the component</param>
        /// <param name = "processingTimeMin">Minimum duration of transaction processing</param>
        /// <param name = "processingTimeMax">Maximum duration of transaction processing</param>
        public Processor(string name, double processingTimeMin, double processingTimeMax) : base(name) {
            if (processingTimeMin > processingTimeMax) {
                throw new InvalidOperationException();
            }
            this.processingTimeMin = processingTimeMin;
            this.processingTimeMax = processingTimeMax;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name of the component</param>
        public Processor(string name) : this(name, 0, 0) { }

        /// <summary>
        /// Event called when the device is idle
        /// </summary>
        public event Action OnIdle;

        /// <summary>
        /// Event called when the transaction is completed
        /// </summary>
        public event Action<int> OnProcessingFinished;

        /// <summary>
        /// Start processing one transaction.
        /// Processing time is randomly selected in the processingTimeMin ... processingTimeMax range
        /// </summary>
        public void Seize() {            
            double processingTime = RandomGenerator.NextDouble(processingTimeMin, processingTimeMax);
            Seize(1, processingTime);
        }

        /// <summary>
        /// Start processing one transaction with a specified processing time
        /// </summary>
        public void Seize(double processingTime) {
            if (processingTime <= 0) {
                throw new InvalidOperationException("Некорректное значение времени обработки.");
            }
            Seize(1, processingTime);
        }

        /// <summary>
        /// Start processing the specified number of transactions
        /// </summary>
        /// <param name = "count"> Number of transactions </param>
        /// <param name = "processingTime"> The amount of time that will be spent on processing transactions </param>
        public void Seize(int count, double processingTime) {
            if (IsBusy) {
                throw new InvalidOperationException();
            }
            transactsInWork = count;
            ticksToEndRemaining += processingTime;
        }

        void Free() {
            if (!IsBusy) {
                throw new InvalidOperationException();
            }
            int n = transactsInWork;
            TransactsCount += n;
            transactsInWork = 0;
            if (OnProcessingFinished != null) {
                OnProcessingFinished(n);
            }
            if (OnIdle != null) {
                OnIdle();
            }
        }

        /// <summary>
        /// Processing of the model time unit
        /// </summary>
        public override void Tick() {
            base.Tick();
            if (IsBusy) {
                double remaining = ticksToEndRemaining;
                if (ticksToEndRemaining <= 1) {
                    BusyTime += ticksToEndRemaining;
                    ticksToEndRemaining = 0;
                    Free();
                    if (IsBusy) {
                        BusyTime += 1 - remaining;
                    }
                } else {
                    BusyTime++;
                    ticksToEndRemaining--;
                }
            } else {
                if (OnIdle != null) {
                    OnIdle();
                }
            }
        }

        /// <summary>
        /// Returns a string representation of the state of the component
        /// </summary>
        public override string ToString() {
            var sb = new StringBuilder();
            sb.AppendLine(Name + ":");
            sb.AppendLine("".PadLeft(Name.Length + 1, '-'));
            sb.AppendFormat("Transactions Processed: {0}\r\n", TransactsCount);
            sb.AppendFormat("Busy Time:              {0:F3}\r\n", BusyTime);
            sb.AppendFormat("Idle Time:              {0:F3}\r\n", IdleTime);
            sb.AppendFormat("Utilization Rate:       {0:F3}\r\n", LoadRate);
            return sb.ToString();
        }
    }
}
