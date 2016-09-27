using System;
using System.Collections;
using System.Collections.Generic;

namespace FSM
{
    public class FSMStateData
    {
        public string ConditionName;
		public bool ConditionValue;
        public string StateName;
        public List<FSMStateData> ListStates;

        public FSMStateData()
        {
            this.ConditionName = string.Empty;
			this.ConditionValue = true;
            this.StateName = string.Empty;
            this.ListStates = new List<FSMStateData>();
        }
    }
}
