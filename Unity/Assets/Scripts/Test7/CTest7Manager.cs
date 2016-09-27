using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class CTest7Manager : CMonoSingleton<CTest7Manager> {

	[SerializeField]	private LuckyWheel m_LuckyWheel;
	[SerializeField]	private int m_MaxPlayer = 4;
	[SerializeField]	private CPlace[] m_Places;

	private CPlayerQueue m_Players;
	private bool m_NextTurn = false;
	private CUITest7Manager m_UIManager;
	private float m_PlayingTime;
	private float m_PrevertPlayingTime;
	private CJumper m_MonsterJumper;
	private string m_MonsterJumperPath = "Prefabs/Test7/MonsterJumper";
	private string[] m_ApartmentPath = new string[] { "Prefabs/Test7/Apartment", "Prefabs/Test7/EnemyApartment" };

	public static bool IsEndGame = false;

	protected override void Awake ()
	{
		base.Awake ();
		UnityEngine.Random.seed = (int)DateTime.Now.Ticks;
		m_Players = new CPlayerQueue ();
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		IsEndGame = false;
	}

	private void Start() {
		m_UIManager = CUITest7Manager.GetInstance ();
		for (int i = 0; i < m_Places.Length; i++) {
			var place = m_Places [i];
			place.SetIndex (i);
		}
		m_UIManager.ShowMainMenuPanel (true);
	}

	protected override void UpdateBaseTime(float dt) {
		base.UpdateBaseTime (dt);
		if (IsEndGame == false) {
			m_PlayingTime += Time.deltaTime;
			if ((int)m_PlayingTime != m_PrevertPlayingTime) {
				m_PrevertPlayingTime = (int)m_PlayingTime;
				CTest7ManagerHook.Instance.OnGameTimeTick (m_PlayingTime);
			}
		}
	}	

	public void RpcOnGameTimeTick(float time) {
		m_UIManager.SetPlayingGameTimeText (time);
	}

	public virtual void OnStartGame() {
		m_MonsterJumper = Instantiate (Resources.Load<CJumper> (m_MonsterJumperPath));
		NetworkServer.Spawn (m_MonsterJumper.gameObject);
		m_MonsterJumper.JumpTo (GetPlace (38).GetJumpPosition());
		GetNextTurn ();
	}

	protected virtual void OnPlayingOneTurnGame() {
		
	}

	protected virtual void OnEndGame() {
		var player = m_Players.Dequeue ();
		m_UIManager.ShowEndGamePanel (true, player.GetPlayerName (), player.GetCurrentMoney ().ToString());
		IsEndGame = true;
		m_Players.Clear ();
		CTest7ManagerHook.Instance.RpcOnEndGame (player.GetPlayerName (), player.GetCurrentMoney ().ToString());
	}

	public void RpcOnEndGame(string playerName, string currentMoney) {
		m_UIManager.ShowEndGamePanel (true, playerName, currentMoney);
		IsEndGame = true;
	}

	/// <summary>
	/// Gets the next turn.
	/// </summary>
	public void GetNextTurn() {
		if (CTest7ManagerHook.Instance.isServer == false) {
			return;
		}
		if (m_Players.Count == 1) {
			OnEndGame ();
			return;
		}
		if (m_NextTurn == true) {
			return;
		}
		m_NextTurn = true;
		var player = m_Players.Dequeue ();
		if (player != null) {
			// Wait scroll dive
			OnActionPlayerScrollDive (player, () => {
				if (player != null) {
					player.RpcOnClientScroll ();
				} else {
					OnPlayerRemoveGame (player);
				}
			}, (random) => {
				if (player != null) {
					player.RpcOnClientEndScroll (random);
					OnPlayingOneTurnGame ();
					player.JumpToStep (random, (pos) => {
						OnClientCompleteOnceStep (player);
					}, () => {
						m_NextTurn = false;
						var place = GetPlace (player.GetJumpIndex ());
						if (OnPlayerEnterPlace (player, place)) {
							OnPlayerStayPlace (player);
						} else {
							OnPlayerRemoveGame (player);
						}
					});
				}else {
					OnPlayerRemoveGame (player);
				}
			});
		} else {
			OnPlayerRemoveGame (player);
		}
	}

	/// <summary>
	/// On player enter place once.
	/// </summary>
	private bool OnPlayerEnterPlace(CPlayer player, CPlace place) {
		if (player.GetCurrentMoney() <= 0)
			return false;
		var placeOwner = place.GetOwner ();
		player.SetCurrentPlace (place);
		if (place.GetPlaceType () == CEnum.EPlaceType.None) {
			return true;
		}
		if (placeOwner == null) {
			var totalMoney = player.GetCurrentMoney () - place.GetTax ();
			if (totalMoney > 0) {
				return true;
			} else {
				return false;
			}
		} else if (placeOwner != player) {
			if (placeOwner.GetIsPlaying ()) {
				var totalMoney = player.GetCurrentMoney () - place.GetTax ();
				if (totalMoney > 0) {
					player.SetCurrentMoney (totalMoney);
					player.OnClientShowTaxInfo ();
					placeOwner.SetCurrentMoney (placeOwner.GetCurrentMoney () + place.GetTax ());
					return true;
				} else {
					return false;
				}
			} else {
				return true;
			}
		} else {
			return true;
		}
	} 

	/// <summary>
	/// On the player stay place.
	/// </summary>
	private void OnPlayerStayPlace(CPlayer player) {
		if (player.GetCurrentPlace ().GetPlaceType () == CEnum.EPlaceType.Park) {
			if (m_Players.Count > 1) {
				OnPlayerShootRandomPlayer (player, (other) => {
					if (other != null) {
						var totalMoney = other.GetCurrentMoney () - player.GetCurrentPlace ().GetTax ();
						other.SetCurrentMoney(totalMoney);
						other.OnClientShowMoneyInfo (-player.GetCurrentPlace ().GetTax ());
						totalMoney = player.GetCurrentMoney () + player.GetCurrentPlace ().GetTax ();
						player.SetCurrentMoney(totalMoney);
					}
					GetNextTurn ();
				});
			}
		} else if (player.GetCurrentPlace().GetPlaceType() != CEnum.EPlaceType.None) {
			if (player.GetCurrentPlace().GetOwner () == null) {
				player.OnClientShowPlaceInfo ();
			} else {
				GetNextTurn ();
			} 
		} else {
			GetNextTurn();
		}
	}

	public void OnPlayerShootRandomPlayer(CPlayer player, Action<CPlayer> complete = null) {
		var random = UnityEngine.Random.Range (0, 999) % m_Players.Count;
		while (m_Players [random].GetIsPlaying () == false) {
			random = UnityEngine.Random.Range (0, 999) % m_Players.Count;
		}
		var randomPlayer = m_Players [random];
		if (m_MonsterJumper.GetJumping () == false) {
			var hook = CTest7ManagerHook.GetInstance ();
			m_MonsterJumper.JumpToPositions (null, () => {
				if (complete != null) {
					complete (randomPlayer);
				}
			}, new Vector3 [] { m_Places [0].GetJumpPosition (), 
				m_Places [38].GetJumpPosition (),
				m_Places [57].GetJumpPosition (), 
				m_Places [19].GetJumpPosition (), 
				randomPlayer.transform.position
			});
		}
	}
		
	public void OnClientCreateApartment(int placeIndex, int id) {
		CTest7ManagerHook.Instance.RpcOnClientCreateApartment (placeIndex, id);
	}

	public void RpcOnClientCreateApartment(int placeIndex, int id) {
		var path = m_ApartmentPath [id % m_ApartmentPath.Length];
		var place = GetPlace (placeIndex);
		var apartment = Instantiate (Resources.Load<CContruction> (path));
		apartment.ResetTransform (place.transform);
	}

	public void OnClientCompleteTurn() {
		GetNextTurn ();
	}

	public void OnClientCompleteOnceStep(CPlayer player) {
		if (player.GetJumpIndex() % GetMaxPlace() == 0) {
			var totalMoney = player.GetCurrentMoney () + 450;
			player.SetCurrentMoney (totalMoney);
			player.OnClientShowMoneyInfo (450);
		}
	}

	/// <summary>
	/// On the player scroll dive.
	/// </summary>
	private int OnPlayerScrollDive(CPlayer player) {
		var dice1 = (UnityEngine.Random.Range (0, 999) % 6) + 1;
		var dice2 = (UnityEngine.Random.Range (0, 999) % 6) + 1;
		return dice1 + dice2;
	}

	private void OnActionPlayerScrollDive(CPlayer player, Action onScroll = null, Action<int> completeScroll = null) {
		CHandleEvent.Instance.AddEvent (HandleOnActionPlayerScrollDive (player, onScroll, completeScroll));
	}

	private IEnumerator HandleOnActionPlayerScrollDive(CPlayer player, Action onScroll, Action<int> completeScroll) {
		if (onScroll != null) {
			onScroll ();
		}
		yield return WaitHelper.WaitForShortSeconds;
		if (completeScroll != null) {
			completeScroll (OnPlayerScrollDive(player));
		}
	}

	/// <summary>
	/// On the player remove from game.
	/// </summary>
	private bool OnPlayerRemoveGame(CPlayer player) {
		if (m_Players.RemoveQueue (player)) {
//			Debug.Log (player.GetPlayerName());
			player.SetIsPlaying (false);
			GetNextTurn ();
			return true;
		} 
		return false;
	}

	public void AddPlayerQueue(CPlayer player) {
		if (player != null) {	
			player.SetIsPlaying (true);
		}
		m_Players.Enqueue (player);
	}

	public void RemobePlayerQueue(CPlayer player) {	
		if (player != null) {	
			player.SetIsPlaying (false);
		}
		m_Players.RemoveQueue (player);
	}

	public int GetPlayerCount() {
		return m_Players.Count;
	}

	public int GetMaxPlace() {
		return m_Places.Length;
	}

	public CPlace GetPlace(int index) {
		return m_Places [index % m_Places.Length];
	}

	public static Sprite ResourceSprite(string name) {
		var resources = Resources.LoadAll<Sprite> ("Images");
		for (int i = 0; i < resources.Length; i++) {
			if (resources [i].name == name) {
				return resources [i];
			}
		}
		return null;
	}

}
