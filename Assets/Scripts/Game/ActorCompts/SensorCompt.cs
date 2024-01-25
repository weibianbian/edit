using System.Collections.Generic;
using UEngine.Components;

namespace RailShootGame
{
    public class SensorCompt: ActorComponent
    {
        public Dictionary<Sensor.ESensorType, Sensor> sensors = new Dictionary<Sensor.ESensorType, Sensor>();
        public Sensor GetSensor(Sensor.ESensorType sensorType)
        {
            return sensors[sensorType];
        }
        public void AddSensor(Sensor sensor)
        {
            sensors[sensor.sensorType] = sensor;
        }

    }
}

