using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class CPlayer : CJumper {

	[SerializeField]	[SyncVar] private int m_PlayerId = 0;
	[SerializeField]	[SyncVar] private string m_PlayerName;
	[SerializeField]	private int m_CurrentMoney = 9999;
	[SerializeField]	[SyncVar] private int m_Lap;
	[SerializeField]	private CPlace m_CurrentPlace;
	[SerializeField]	[SyncVar] private bool m_IsPlaying = false;

	private CTest7Manager m_Manager;
	private CUITest7Manager m_UIManager;

	protected override void Awake ()
	{
		base.Awake ();
	}

	protected override void Start ()
	{
		base.Start ();
	}

	public override void OnStartLocalPlayer ()
	{
		base.OnStartLocalPlayer ();
		CmdOnClientAlready ();
	}

	public override void OnStartClient ()
	{
		base.OnStartClient ();
		m_Manager = CTest7Manager.GetInstance();
		m_UIManager = CUITest7Manager.GetInstance ();
		m_UIManager.SetPlayerCurrentMoneyText (m_CurrentMoney.ToString ());
	}

	[ClientRpc]
	public virtual void RpcJumpToStep(int step) {
		if (m_OnClientJumping == false) {
			m_OnClientJumping = true;
			JumpToStep (step, CmdOnClientJumpComplete, () => {
				m_OnClientJumping = false;
				CmdOnClientAllStepComplete ();
			});
		}
	}

	public virtual void JumpToStep(int step, Action<Vector3> jumpComplete = null, Action allComplete = null) {
		var positions = new Vector3 [step];
		for (int i = 0; i < step; i++) {
			positions [i] = m_Manager.GetPlace (m_JumpIndex + i + 1).GetJumpPosition ();
		}
		JumpToPositions (jumpComplete, allComplete, positions);
		if (isServer) {
			RpcJumpToStep (step);
		} 
	}

	public void OnClientShowTaxInfo() {
		OnClientShowMoneyInfo (-m_CurrentPlace.GetTax ());
	}

	[Command]
	public void CmdOnClientAlready() {
		var playerName = CTest7LobbyManager.Instance.GetPlayerLobbyBaseConnection (this.connectionToClient).GetPlayerName ();
		CTest7ManagerHook.Instance.AddPlayer (this);
		this.SetPosition (Vector3.zero);
		this.SetPlayerName (playerName);
		this.SetIsPlaying (true);
		CTest7ManagerHook.Instance.OnStartGame ();
	}

	[ServerCallback]
	public void OnClientShowMoneyInfo(int value) {
		m_UIManager.ShowMoneyInfoPanel (true, m_Transform.position, value);
		RpcOnClientShowMoneyInfo (value);
	}

	[ClientRpc]
	public void RpcOnClientShowMoneyInfo(int value) {
		m_UIManager.ShowMoneyInfoPanel (true, m_Transform.position, value);
	}

	[ServerCallback]
	public void OnClientShowPlaceInfo() {
		RpcOnClientShowPlaceInfo ();
	}

	[ClientRpc]
	public void RpcOnClientShowPlaceInfo() {
		if (isLocalPlayer) {
			m_UIManager.ShowPlaceInfoPanel (true, m_CurrentPlace, () => {
				OnPlayerWantBuyPlace ();
			}, () => {
				OnClientCancelPlaceInfo ();
			});
		}
	}

	public void OnClientBuyPlace() {
		if (isLocalPlayer) {
			m_UIManager.ShowStatusPanel (true, true);
		}
		OnClientShowMoneyInfo (-m_CurrentPlace.GetPrice ());
		OnClientBuildPlace ();
//		m_Manager.OnClientCompleteTurn ();
		CmdOnClientCompleteTurn ();
	}

	[ServerCallback]
	public void OnClientBuildPlace() {
		m_Manager.OnClientCreateApartment (m_CurrentPlace.GetIndex(), GetPlayerId());
	}

	public void OnClientCancelPlaceInfo() {
		if (isLocalPlayer) {
			m_UIManager.ShowStatusPanel (true, false);
		}
//		m_Manager.OnClientCompleteTurn ();
		CmdOnClientCompleteTurn ();
	}

	[Command]
	protected virtual void CmdOnClientCompleteTurn() {
		m_Manager.OnClientCompleteTurn ();
	}

	public void OnPlayerWantBuyPlace() {
		CmdOnPlayerWantBuyPlace ();
	}

	[Command]
	public void CmdOnPlayerWantBuyPlace() {
		if (OnPlayerWantBuyPlace (m_CurrentPlace)) {
			OnClientBuyPlace ();
		} else {
			OnClientCancelPlaceInfo ();
		}
	}

	public bool OnPlayerWantBuyPlace(CPlace place) {
		if (m_CurrentPlace.GetOwner () == null) {
			var totalMoney = this.GetCurrentMoney () - place.GetPrice ();
			if (totalMoney >= 0) {
				this.SetCurrentMoney (totalMoney);
				m_CurrentPlace.SetOwner (this);
				return true;
			} else {
				return false;
			}
		}
		return false;
	}

	[ServerCallback]
	public void OnClientScroll() {
		RpcOnClientScroll ();
	}

	[ClientRpc]
	public void RpcOnClientScroll() {
		
	}

	[ServerCallback]
	public void OnClientEndScroll(int step) {
		m_UIManager.ShowScrollDiveInfoPanel (true, m_Transform.position, step);
		RpcOnClientEndScroll (step);
	}

	[ClientRpc]
	public void RpcOnClientEndScroll(int step) {
		m_UIManager.ShowScrollDiveInfoPanel (true, m_Transform.position, step);
	}

	public override void AddStep() {
		base.AddStep ();
		m_Lap = m_JumpIndex / m_Manager.GetMaxPlace();
	}

	public virtual void SetCurrentPlace(CPlace value) {
		m_CurrentPlace = value;
		RpcSetCurrentPlace (value.GetIndex());
	}

	[ServerCallback]
	public virtual void SetCurrentPlace(int value) {
		m_CurrentPlace = m_Manager.GetPlace (value);
		RpcSetCurrentPlace (value);
	}

	[ClientRpc]
	public virtual void RpcSetCurrentPlace(int value) {
		m_CurrentPlace = m_Manager.GetPlace (value);
	}

	public virtual CPlace GetCurrentPlace () {
		return m_CurrentPlace;
	}

	public virtual void SetJumpIndex(int value) {
		m_JumpIndex = value;
	}

	public virtual int GetJumpIndex() {
		return m_JumpIndex;
	}

	public virtual void SetPlayerName(string value) {
		m_PlayerName = value;
		this.gameObject.name = value;
		RpcSetPlayerName (value);
	}

	[ClientRpc]
	public virtual void RpcSetPlayerName(string value) {
		m_PlayerName = value;
		this.gameObject.name = value;
	}

	public virtual string GetPlayerName() {
		return m_PlayerName;
	}

	public virtual void SetPlayerId(int value) {
		m_PlayerId = value;
		RpcSetPlayerId (value);
	}

	[ClientRpc]
	public virtual void RpcSetPlayerId(int value) {
		m_PlayerId = value;
	}

	public virtual int GetPlayerId() {
		return m_PlayerId;
	}

	[ServerCallback]
	public virtual void SetCurrentMoney(int value) {
		m_CurrentMoney = value;
		RpcSetCurrentMoney (value);
	}

	[ClientRpc]
	protected virtual void RpcSetCurrentMoney(int value) {
		m_CurrentMoney = value;
		if (isLocalPlayer) {
			m_UIManager.SetPlayerCurrentMoneyText (value.ToString ());
		}
	}

	public virtual int GetCurrentMoney() {
		return m_CurrentMoney;
	}

	public virtual bool GetIsPlaying() {
		return m_IsPlaying;
	}

	public virtual void SetIsPlaying(bool value) {
		m_IsPlaying = value;
		RpcSetIsPlaying (value);
	}

	[ClientRpc]
	protected virtual void RpcSetIsPlaying(bool value) {
		m_IsPlaying = value;
	}

	public virtual int GetLap() {
		return m_Lap;
	}

	public string GetTransformPositionString() {
		var positionStr = new StringBuilder ("(");
		positionStr.Append (m_Transform.position.x + ",");
		positionStr.Append (m_Transform.position.y + ",");
		positionStr.Append (m_Transform.position.z);
		positionStr.Append (")");
		return positionStr.ToString ();
	}

}
