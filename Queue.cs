using System;
using System.Collections.Generic;
using System.Text;

namespace Modeling.Core {

    /// <summary>
    /// Queue
    /// </summary>
    public class Queue : AbstractDevice {

        /// <summary>
        /// Current queue length
        /// </summary>
        public int Length {
            get { return transacts.Count; }
        }

        /// <summary>
        /// Maximum queue length during simulation
        /// </summary>
        public int MaxLength { get; private set; }

        /// <summary>
        /// Average queue length during simulation
        /// </summary>
        public double AvgLength {
            get {
                return TicksCount > 0 ? lengthCumulative / (double)TicksCount : 0;
            }
        }

        /// <summary>
        /// The maximum time a transaction is in the queue during the simulation
        /// </summary>
        public double MaxTime { get; private set; }

        /// <summary>
        /// The minimum time a transaction is in the queue during the simulation
        /// </summary>
        public double MinTime { get; private set; }

        /// <summary>
        /// Average time spent by a transaction in the queue during the simulation
        /// </summary>
        public double AvgTime {
            get {
                return totalTransacts != 0 ? totalWaitTime / (double)totalTransacts : 0;
            }
        }

        Queue<int> transacts = new Queue<int>();
        int totalTransacts;
        int totalWaitTime;
        int lengthCumulative;

        public Queue(string name) : base(name) { }

        /// <summary>
        /// Processing of the model time unit
        /// </summary>
        public override void Tick() {
            base.Tick();
            lengthCumulative += Length;
        }

        /// <summary>
        /// Enqueue the specified number of transactions
        /// </summary>
        /// <param name = "count">Number of transactions</param>
        public void Enqueue(int count) {
            if (count < 0) {
                throw new ArgumentException(nameof(count));
            }
            for (int i = 0; i < count; i++) {
                totalTransacts++;
                transacts.Enqueue(TicksCount);
                MaxLength = Math.Max(Length, MaxLength);
            }
        }

        /// <summary>
        /// Enqueuing one transaction
        /// </summary>
        public void Enqueue() {
            Enqueue(1);
        }

        /// <summary>
        /// Retrieve the specified number of transactions from the queue
        /// </summary>
        /// <param name = "count">Number of transactions</param>
        public void Dequeue(int count) {
            if (count < 0) {
                throw new ArgumentException(nameof(count));
            }
            for (int i = 0; i < count; i++) {
                if (Length == 0) {
                    throw new InvalidOperationException();
                }
                int time = TicksCount - transacts.Dequeue();
                totalWaitTime += time;
                MaxTime = Math.Max(time, MaxTime);
                if (MinTime == 0 || time < MinTime) {
                    MinTime = time;
                }
            }
        }

        /// <summary>
        /// Retrieving one transaction from the queue
        /// </summary>
        public void Dequeue() {
            Dequeue(1);
        }

        /// <summary>
        /// Returns a string representation of the state of the component
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            var sb = new StringBuilder();
            sb.AppendLine(Name + ":");
            sb.AppendLine("".PadLeft(Name.Length + 1, '-'));
            sb.AppendFormat("Current Length: {0}\r\n", Length);
            sb.AppendFormat("Max Length:     {0}\r\n", MaxLength);
            sb.AppendFormat("Average Length: {0:F3}\r\n", AvgLength);
            sb.AppendFormat("Min Time:       {0}\r\n", MinTime);
            sb.AppendFormat("Мax Time:       {0}\r\n", MaxTime);
            sb.AppendFormat("Average Time:   {0:F3}\r\n", AvgTime);
            return sb.ToString();
        }
    }
}
