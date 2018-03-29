using System;

namespace Modeling.Core
{
    /// <summary>
    /// Base abstract class for simulation model components
    /// </summary>
    public abstract class AbstractDevice
    {
        /// <summary>
        /// Name of the component
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The number of model time units elapsed since the beginning of the simulation
        /// </summary>
        public int TicksCount { get; protected set; }

        /// <summary>
        /// Processing the counting of the unit of model time
        /// </summary>
        public virtual void Tick()
        {
            TicksCount++;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name of the component</param>
        public AbstractDevice(string name) {
            this.Name = name;
        }
    }
}
