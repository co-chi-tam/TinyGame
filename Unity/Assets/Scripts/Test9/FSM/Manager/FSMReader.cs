using System;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;

namespace FSM
{
    public class FSMReader
    {
        private Dictionary<string, FSMStateData> m_Maps;
        private FSMStateData m_CurrentState;
        private FSMStateData m_AnyState;
        public FSMStateData FSMCurrentState
        {
            get { return m_CurrentState; }
        }
        public FSMStateData FSMAnyState
        {
            get { return m_AnyState; }
        }
		public Dictionary<string, FSMStateData> FSMMaps
		{
			get { return m_Maps; }
		}

        public FSMReader()
        {
            m_Maps = new Dictionary<string, FSMStateData>();
        }

        public void LoadJSON(string path)
        {
            var jsonText = Resources.Load<TextAsset>(path);
            var jsonObject = Json.Deserialize(jsonText.text) as Dictionary<string, object>;
            var states = jsonObject["fsm"] as List<object>;
            LoadFSM(states[0] as Dictionary<string, object>, ref m_CurrentState);
            LoadFSM(states[1] as Dictionary<string, object>, ref m_AnyState);
        }

        private void LoadFSM(Dictionary<string, object> data, ref FSMStateData stateData)
        {
            var state = new FSMStateData();
            state.ConditionName = data["condition_name"].ToString();
			state.ConditionValue = bool.Parse (data ["condition_value"].ToString ());
            state.StateName = data["state_name"].ToString();
            m_Maps[state.StateName] = state;
            LoadChild(data["states"] as List<object>, state.ListStates);
            stateData = state;
        }

        private void LoadChild(List<object> value, List<FSMStateData> states)
        {
            for (int i = 0; i < value.Count; i++)
            {
                var data = value[i] as Dictionary<string, object>;
                var stateName 			= data["state_name"].ToString();
				var state 				= new FSMStateData();
				state.ConditionValue 	= bool.Parse (data ["condition_value"].ToString ());
                if (m_Maps.ContainsKey(stateName))
                {
                    state.ConditionName = data["condition_name"].ToString();
                    state.StateName 	= stateName;
                    state.ListStates 	= m_Maps[stateName].ListStates;
                }
                else
                {
                    state.ConditionName = data["condition_name"].ToString();
                    state.StateName 	= stateName;
                    m_Maps[stateName] 	= state;
                    LoadChild(data["states"] as List<object>, state.ListStates);
                }
                states.Add(state);
            }
        }
    }
}
