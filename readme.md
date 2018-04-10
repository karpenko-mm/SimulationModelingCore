# SimulationModelingCore

# Overview
Core Library for Simulation Process Modeling.
This is a study project.

# Usage Sample

## Create and configure Simulation Model:

```csharp
class Model : AbstractModel {

        public override string Name {
            get { return "Sample Model"; }
        }

        protected override List<AbstractDevice> Configure() {

            var generator = new Generator("Generator", 10, 20, 1, 3);
            var queue = new Queue("Queue");
            var processor = new Processor("Processor", 3, 7);

            generator.OnTransactGenerated += (n) => {
                queue.Enqueue(n);
            };

            processor.OnIdle += () => {
                if (queue.Length > 0) {
                    queue.Dequeue();
                    processor.Seize();
                }
            };

            return new List<AbstractDevice>() {
                generator, queue, processor
            };
        }
    }
```

## Run the Simulation:

```csharp
  var model = new Modeling.Model();
  model.RunByTime(1000);
  Console.WriteLine(model.ToString());
```

## Sample Console Output:

```
Sample Model
Modeling Time Units: 1000

Generator:
----------
Transactions Generated: 129


Queue:
------
Current Length: 2
Max Length:     4
Average Length: 0,754
Min Time:       1
Мax Time:       19
Average Time:   5,829


Processor:
----------
Transactions Processed: 126
Busy Time:              686,123
Idle Time:              313,877
Utilization Rate:       0,686
```
