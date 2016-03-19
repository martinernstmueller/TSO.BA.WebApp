using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TSO.BA.WebApp.Models
{
    public class Floor
    {
        public Guid FloorId { get; set; }
        public string FloorName { get; set; }
        public List<BuildingState> BuildingStates { get; set; }

        public Floor(string argFloorName)
        {
            FloorName = argFloorName;
            Initialize();
        }

        public Floor()
        {
            Initialize();
        }

        private void Initialize()
        {
            FloorId = Guid.NewGuid();
            BuildingStates = new List<BuildingState>();
        }

    }
}
