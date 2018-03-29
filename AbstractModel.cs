using System;
using System.Collections.Generic;
using System.Text;

namespace Modeling.Core {

    /// <summary>
    /// The base abstract class of the simulation model
    /// </summary>
    public abstract class AbstractModel {

        /// <summary>
        /// Name of the model
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// The number of time units elapsed since the beginning of the simulation
        /// </summary>
        public int TicksCount { get; protected set; }

        /// <summary>
        /// The list of model components
        /// </summary>
        public List<AbstractDevice> Devices { get; protected set; }

        /// <summary>
        /// Configure of the simulation model.
        /// Returns a list of configured components included in the model.
        /// </summary>
        /// <returns></returns>
        protected abstract List<AbstractDevice> Configure();

        /// <summary>
        /// Run simulation for a specified time
        /// </summary>
        /// <param name = "time"> Simulation time </param>
        public void RunByTime(int time) {
            Devices = Configure();
            TicksCount = 0;
            for (int i = 0; i < time; i++) {
                foreach (var device in Devices) {
                    device.Tick();
                }
                TicksCount++;
            }
        }

        /// <summary>
        /// Starts simulation of processing a specified number of transacts.
        /// </summary>
        /// <param name = "count"> The number of transacts after which the simulation stops</param>
        public void RunByTransacts(int count) {
            Devices = Configure();
            TicksCount = 0;
            int transactsRemaining = count;
            var device = Devices[Devices.Count - 1] as Processor;
            var generator = Devices[0] as Generator;
            if (device == null) {
                throw new InvalidOperationException("Обслуживающее устройство должно быть последним в списке Devices.");
            }
            if (device == null) {
                throw new InvalidOperationException("Генератор транзактов должен быть первым в списке Devices.");
            }
            generator.OnTransactGenerated += (n) => {
                if (generator.TransactsCount >= count) {
                    generator.Enabled = false;
                }
            };
            device.OnProcessingFinished += (n) => {
                transactsRemaining -= n;
            };
            while (true) {
                foreach (var dev in Devices) {
                    dev.Tick();
                }
                TicksCount++;
                if (transactsRemaining <= 0) {
                    break;
                }
            }
        }

        /// <summary>
        /// Returns a string representation of the state of a model
        /// </summary>
        public override string ToString() {
            var sb = new StringBuilder();
            sb.AppendLine(Name);
            sb.AppendFormat("Modeling Time Units: {0}", TicksCount);
            sb.AppendLine();
            sb.AppendLine();
            foreach (var device in Devices) {
                sb.AppendLine(device.ToString());
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
