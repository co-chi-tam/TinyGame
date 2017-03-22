using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using ObjectPool;

public class CTest9Manager : CMonoSingleton<CTest9Manager> {

	#region Properties

	[SerializeField]	private Transform[] m_AllySodiers;
	[SerializeField]	private Transform[] m_EnemySodiers;
	[SerializeField]	private ETeam m_Winner;

	public static bool OnStartGame = false;
	public static bool OnPVP = false;

	private CSodierBehaviour[] m_ListAllySodier;
	private CSodierBehaviour[] m_ListEnemySodier;
	private CSimpleSodier[] m_ListSimpleAllySodier;
	private CSimpleSodier[] m_ListSimpleEnemySodier;

	private CTest9Queue<CSimpleSodier> m_FakeAllySodier;
	private CTest9Queue<CSimpleSodier> m_FakeEnemySodier;

	private Queue<CSodierBehaviour> m_QueueTurn;

	private static CTest9DataParser<string, CMapData> m_AllyMapParser;
	private CTest9DataParser<string, CMapData> m_EnemyMapParser;
	private CTest9DataParser<string, CSkillData> m_SkillDataParser;
	private CMapData m_AllyMapData;
	private CMapData m_EnemyMapData;

	private Dictionary<string, CSkillBehaviour> m_SkillPools;

	private string m_BattleLog;
	private bool m_AllyTurn;
	private int m_ObjectCount = 0;

	private CTest9UserManager m_UserManager;
	private CUserData m_CurrentUser;

	#endregion

	#region Monobehaviour

	protected override void Awake ()
	{
		base.Awake ();
		m_ListAllySodier = new CSodierBehaviour[m_AllySodiers.Length];
		m_ListEnemySodier = new CSodierBehaviour[m_EnemySodiers.Length];
		m_ListSimpleAllySodier 	= new CSimpleSodier[m_AllySodiers.Length];
		m_ListSimpleEnemySodier = new CSimpleSodier[m_EnemySodiers.Length];

		m_FakeAllySodier = new CTest9Queue<CSimpleSodier> ();
		m_FakeEnemySodier = new CTest9Queue<CSimpleSodier> ();

		m_QueueTurn = new Queue<CSodierBehaviour> ();

		m_SkillPools = new Dictionary<string, CSkillBehaviour> ();

		m_BattleLog = string.Empty;
		m_AllyTurn = true;
		OnStartGame = false;
	}

	protected override void Start ()
	{
		base.Start ();
		m_UserManager = CTest9UserManager.GetInstance ();
		m_CurrentUser = CTest9UserManager.currentUser;
		if (m_CurrentUser != null) {
			Loading.OnStart ();
			OnClientLoadData ();
		} else {
			m_UserManager.OnClientEndMatch ();
		}
	}
			
	private void OnApplicationQuit() {
		
	}

	#endregion

	#region Data

	public virtual void OnClientLoadData() {
		// TEST
		OnClientLoadDataComplete();
	}

	public virtual void OnClientLoadDataComplete() {
		OnClientSetUpAllyTurn ();
	}

	public virtual void OnClientSetUpAllyTurn() {
		if (m_CurrentUser != null) {
			if (m_CurrentUser.needUpdateAPI.IndexOf("{USERMAP}") != -1) {
				m_AllyMapParser = new CTest9DataParser<string, CMapData> ();
				var allySlotText = Resources.Load<TextAsset> ("Data/Test9/AllySlots");
				var currentMapURL = string.Format (CTest9ServerInfo.GET_USER_MAP_FORMATION, m_CurrentUser.userName, m_CurrentUser.token);
				m_AllyMapParser.ParseURL (currentMapURL, () => {
					m_CurrentUser.needUpdateAPI = m_CurrentUser.needUpdateAPI.Replace("{USERMAP}", string.Empty);
					m_AllyMapData = m_AllyMapParser.GetData("map-" + m_CurrentUser.userName);
					SetupMap(ETeam.Ally, m_AllyMapData);
					OnClientSetUpEnemyTurn ();
				}, (error) => {
					if (m_CurrentUser.needUpdateAPI.IndexOf("{USERMAP}") == -1) {
						m_CurrentUser.needUpdateAPI += "{USERMAP}";
					}
					Debug.Log(error);
					m_UserManager.OnClientEndMatch();
				});
			} else {
				m_AllyMapData = m_AllyMapParser.GetData("map-" + m_CurrentUser.userName);
				SetupMap(ETeam.Ally, m_AllyMapData);
				OnClientSetUpEnemyTurn ();
			}
		} else {
			m_UserManager.OnClientEndMatch();
			Loading.OnStop ();
		}
	}

	public virtual void OnClientSetUpEnemyTurn() {
		m_EnemyMapParser = new CTest9DataParser<string, CMapData> ();
		var enemySlotText = Resources.Load<TextAsset> ("Data/Test9/Map1Slots");
		var currentMapURL = string.Empty;
		if (OnPVP) {
			currentMapURL = string.Format (CTest9ServerInfo.GET_PVP_MAP_FORMATION, m_CurrentUser.currentMap, m_CurrentUser.currentTarget, m_CurrentUser.userName, m_CurrentUser.token);
		} else {
			currentMapURL = string.Format (CTest9ServerInfo.GET_MAP_FORMATION, m_CurrentUser.currentMap, m_CurrentUser.token);
		}
		m_EnemyMapParser.ParseURL (currentMapURL, () => {
			m_EnemyMapData = m_EnemyMapParser.GetValues()[0];
			SetupMap(ETeam.Enemy, m_EnemyMapData);
			m_UserManager.OnClientSetupMatch(m_EnemyMapData.backgroundImage);
		}, (error) => {
			Debug.Log(error);
			m_UserManager.OnClientEndMatch();
		});
	}

	#endregion

	#region Match result

	private void OnAllyWin() {
		Debug.Log ("OnAllyWin");
		m_UserManager.OnClientWinMatch ();
	}

	private void OnEnemyWin() {
		Debug.Log ("OnEnemyWin");
		m_UserManager.OnClientCloseMatch ();
	}

	#endregion

	#region Main methods

	private void SetUpTeamComplete() {
		m_ObjectCount++;
		if (m_ObjectCount == 2) {
			SetupSkill (m_AllyMapData.skillValueDatas, () => { 
				SetupSkill (m_EnemyMapData.skillValueDatas, () => { 
					FakeResult ();
					Loading.OnStop ();
				});
			});
		}
	}

	public void ShowLog() {
		
	}

	public void SetupSkill(CSkillData[] skills, Action complete = null) {
		CHandleEvent.Instance.AddEvent (HandleCreateSkillObject(skills, complete));
	}

	public void SetupMap(ETeam team, CMapData mapData, Action complete = null) {
		CHandleEvent.Instance.AddEvent (HandleSetupMap (team, mapData, SetUpTeamComplete));
	}

	private IEnumerator HandleSetupMap(ETeam team, CMapData mapData, Action complete = null) {
		for (int i = 0; i < mapData.mapSlots.Length; i++) {
			var mapSlotInfo = mapData.mapSlots [i];
			if (mapData.sodierDatas.ContainsKey (mapSlotInfo.gameType) == false)
				continue;
			var sodierData = CSodierData.Clone (mapData.sodierDatas[mapSlotInfo.gameType]);
			var sodierBehaviour = Instantiate (Resources.Load<CSodierBehaviour> (sodierData.modelPath));
			yield return sodierBehaviour != null;
			sodierData.IncreaseData (sodierData.levelData * (sodierData.level - 1));
			sodierBehaviour.SetTeam (team);
			sodierBehaviour.SetData (sodierData);
			var slotId = (int)(mapSlotInfo.slotIds.x + (mapSlotInfo.slotIds.y * 3));
			sodierData.slotIds = new Vector2 (mapSlotInfo.slotIds.x, mapSlotInfo.slotIds.y);
			var simpleSodier = CSimpleSodier.Clone (sodierData);
			switch (team) {
			case ETeam.Ally:
				sodierBehaviour.SetActive (true, m_AllySodiers [slotId]);
				sodierBehaviour.SetPosition (m_AllySodiers [slotId].position);
				simpleSodier.id = slotId;
				m_ListAllySodier[slotId] = sodierBehaviour;
				m_ListSimpleAllySodier[slotId] = simpleSodier;
				m_FakeAllySodier.Enqueue (simpleSodier);
				break;
			case ETeam.Enemy:
				sodierBehaviour.SetActive (true, m_EnemySodiers [slotId]);
				sodierBehaviour.SetPosition (m_EnemySodiers [slotId].position);
				simpleSodier.id = slotId;
				m_ListEnemySodier[slotId] = sodierBehaviour;
				m_ListSimpleEnemySodier[slotId] = simpleSodier;
				m_FakeEnemySodier.Enqueue (simpleSodier);
				break;
			}
			for (int s = 0; s < sodierData.skillSlots.Length; s++) {
				simpleSodier.skillSlots[s] = CSkillSimple.Clone (mapData.skillDatas[sodierData.skillSlots[s].ToString()]);
			}
			sodierBehaviour.name = sodierBehaviour.GetGameType() + "-" + team + "-" + i;
			sodierBehaviour.Init ();
			sodierBehaviour.SetActive (true);
		}
		if (complete != null) {
			complete ();
		}
	}

	private IEnumerator HandleCreateSkillObject(CSkillData[] skills, Action complete = null) {
		for (int i = 0; i < skills.Length; i++) {
			if (m_SkillPools.ContainsKey (skills [i].id) == false) {
				var skillData = CSkillData.Clone (skills [i]);
				var skillBehaviour = Instantiate (Resources.Load<CSkillBehaviour> (skillData.modelPath));
				yield return skillBehaviour != null;
				skillBehaviour.SetActive (false);
				skillBehaviour.SetData (skillData);
				skillBehaviour.Init ();
				m_SkillPools.Add (skills [i].id, skillBehaviour);
			} 
		}
		if (complete != null) {
			complete ();
		}
	}

	private void FakeResult() {
		var allyStartTurn = m_AllyTurn;
		var step = 9999;
		CSimpleSodier attacker = null;
		CSimpleSodier target = null;
		CSimpleSodier[] attackerSodiers = null;
		CSimpleSodier[] targetSodiers = null;
		while (step > 0) {
			if (m_FakeAllySodier.Count == 0) {
				m_Winner = ETeam.Enemy;
				Debug.Log ("Enemy win");
				FakeResultComplete (false);
				ShowLog ();
				return;
			}
			if (m_FakeEnemySodier.Count == 0) {
				m_Winner = ETeam.Ally;
				Debug.Log ("Ally win");
				FakeResultComplete (true);
				ShowLog ();
				return;
			}

			attacker = allyStartTurn ? m_FakeAllySodier.Peek () : m_FakeEnemySodier.Peek ();
			target = allyStartTurn ? m_FakeEnemySodier.Peek () : m_FakeAllySodier.Peek ();
			attackerSodiers = allyStartTurn ? m_ListSimpleAllySodier : m_ListSimpleEnemySodier;
			targetSodiers = allyStartTurn ? m_ListSimpleEnemySodier : m_ListSimpleAllySodier;
			if (attacker.health <= 0) {
				if (allyStartTurn) {
					m_FakeAllySodier.RemoveQueue (attacker); 
				} else {
					m_FakeEnemySodier.RemoveQueue (attacker); 
				}
				continue;
			} 
			if (target.health <= 0) {
				if (allyStartTurn) {
					m_FakeEnemySodier.RemoveQueue (target); 
				} else {
					m_FakeAllySodier.RemoveQueue (target); 
				}
				continue;
			} 
			if (allyStartTurn) {
				if (attacker.health > 0) {
					m_QueueTurn.Enqueue (m_ListAllySodier [attacker.id]);
				}
				if (target.health > 0) {
					m_QueueTurn.Enqueue (m_ListEnemySodier [target.id]);
				}
			} else {
				if (attacker.health > 0) {
					m_QueueTurn.Enqueue (m_ListEnemySodier [attacker.id]);
				}
				if (target.health > 0) {
					m_QueueTurn.Enqueue (m_ListAllySodier [target.id]);
				}
			}
			var skillSlots = attacker.skillSlots [0];
//			target.ApplyDamage (attacker.damage + skillSlots.skillValue, 0, 0);
			for (int i = 0; i < skillSlots.skillRange.Count; i++) {
				var x = target.currentSlot.x + skillSlots.skillRange [i].x;
				var y = target.currentSlot.y + skillSlots.skillRange [i].y;
				var slotAttacked = (int)(x + (y * 3));
				if (slotAttacked < 0 || slotAttacked > 3 * 3) {
					continue;
				}
				var targetBySkill = targetSodiers [slotAttacked];
				var totalDamage = attacker.damage + skillSlots.skillValue;
				targetBySkill.ApplyDamage (skillSlots.skillEffect == ESkillEffect.Health ? totalDamage : 0,
											skillSlots.skillEffect == ESkillEffect.Mana ? skillSlots.skillValue : 0,
											skillSlots.skillEffect == ESkillEffect.Rage ? skillSlots.skillValue : 0);
				attacker.ApplyCost (skillSlots.costHealth, skillSlots.costMana, skillSlots.costRage);
			}
			m_BattleLog += string.Format ("{0} attack {1} DMG {2}\n", attacker.name, target.name, attacker.damage);
			// Switch turn
			if (attacker.currentAttackSpeed - 1 <= 0) {
				if (allyStartTurn) {
					m_FakeAllySodier.Dequeue(); 
				} else {
					m_FakeEnemySodier.Dequeue(); 
				}
				attacker.currentAttackSpeed = attacker.attackSpeed;
				allyStartTurn = !allyStartTurn;
			} else {
				attacker.currentAttackSpeed -= 1;
			}
			step--;
		}
	}

	private void FakeResultComplete(bool result) {
		m_UserManager.OnClientCreateBattleLog (m_CurrentUser.currentMap, result);
	}

	public void GetNextTurn() {
		CSodierBehaviour attacker = null;
		CSodierBehaviour target = null;
		if (m_QueueTurn.Count == 0) {
			// TODO
			if (m_Winner == ETeam.Ally) {
				OnAllyWin ();
			} else if (m_Winner == ETeam.Enemy) {
				OnEnemyWin ();
			}
			return;
		}
		if (m_QueueTurn.Count > 0) {
			attacker = m_QueueTurn.Dequeue ();
		} 
		if (m_QueueTurn.Count > 0) {
			target = m_QueueTurn.Dequeue ();
		}
		if (attacker != null) {
			attacker.SetEnemy (target);
		}
		if (attacker.OnEndTurn == null) {
			attacker.OnEndTurn += GetNextTurn;
		}
	}

	private bool GetSodierAlready(CSodierBehaviour sodier) {
		return sodier.IsDeath () == false;
	}

	public CSkillBehaviour GetSkillPool (string key) {
		if (m_SkillPools.ContainsKey (key)) {
			return m_SkillPools[key];
		}
		return null;
	}

	#endregion

}
