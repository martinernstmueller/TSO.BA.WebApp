using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TSO.BA.WebApp.Models
{
    public class BuildingState
    {
        public Guid BuildingStateId { get; set; }
        public string BuildingStateKey { get; set; }
        public string BuildingStateValue { get; set; }

        public BuildingState(string argBuildingStateKey, string argBuildingStateValue)
        {
            Initialize();
            BuildingStateKey = argBuildingStateKey;
            BuildingStateValue = argBuildingStateValue;
        }

        public BuildingState()
        {
            Initialize();
        }

        private void Initialize()
        {
            BuildingStateId = Guid.NewGuid();
        }
    }
}
