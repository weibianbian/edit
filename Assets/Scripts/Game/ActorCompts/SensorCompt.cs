using System.Collections.Generic;

namespace RailShootGame
{
    public class SensorCompt: ActorCompt
    {
        public Dictionary<Sensor.ESensorType, Sensor> sensors = new Dictionary<Sensor.ESensorType, Sensor>();

        public SensorCompt(Actor owner) : base(owner)
        {
        }

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

