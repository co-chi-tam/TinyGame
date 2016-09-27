using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class CPlace : CBaseMonobehaviour {

	[SerializeField]	protected int m_Index;
	[SerializeField]	protected string m_PlaceName;
	[SerializeField]	protected CPlayer m_Owner;
	[SerializeField]	protected CEnum.EPlaceType m_PlaceType;
	[SerializeField]	protected int m_Price;
	[SerializeField]	protected int m_Tax;
	[SerializeField]	protected Sprite m_ImageInfoSprite;
	[SerializeField]	protected string m_IntroduceInfo;

	protected override void Awake ()
	{
		base.Awake ();
		this.name = m_PlaceName + "  " + m_PlaceType.ToString();
	}

	public virtual Vector3 GetJumpPosition() {
		var position = m_Transform.position;
		position.y = m_Transform.localScale.y;
		return position;
	}

	public virtual string GetJumpPositionString() {
		var position = GetJumpPosition();
		var result = new StringBuilder ("(");
		result.Append (position.x + ",");
		result.Append (position.y + ",");
		result.Append (position.z);
		result.Append (")");
		return result.ToString();
	}

	public virtual void SetActive(bool value) {
		this.gameObject.SetActive (value);
	}

	public virtual void SetScale(int value) {
		var scale = m_Transform.localScale;
		scale.y = value;
		m_Transform.localScale = scale;
	}

	public virtual void SetPosition(Vector3 poition) {
		m_Transform.position = poition;
	}

	public virtual void SetPotision(float x, float y, float z) {
		var position = m_Transform.position;
		position.x = x;
		position.y = y;
		position.z = z;
		m_Transform.position = position;
	}

	public virtual void SetIndex(int index) {
		m_Index = index;
	}

	public int GetIndex() {
		return m_Index;
	}

	public virtual int GetPrice() {
		return m_Price;
	}

	public virtual void SetPrice(int value) {
		m_Price = value;
	}

	public virtual int GetTax() {
		return m_Tax;
	}

	public virtual void SetTax(int value) {
		m_Tax = value;
	}

	public virtual string GetIntroduceInfo() {
		return m_IntroduceInfo;
	}

	public virtual void SetIntroduceInfo(string value) {
		m_IntroduceInfo = value;
	}

	public CEnum.EPlaceType GetPlaceType() {
		return m_PlaceType;
	}

	public virtual CPlayer GetOwner() {
		return m_Owner;
	}

	public virtual void SetOwner(CPlayer value) {
		if (m_Owner == null) {
			m_Owner = value;
		}
	}

	public virtual void SetPlaceName(string value) {
		m_PlaceName = value;
	}

	public virtual string GetPlaceName() {
		return m_PlaceName;
	}

	public virtual void SetPlaceType(CEnum.EPlaceType type) {
		m_PlaceType = type;
	}

	public virtual void SetImageInfo(Sprite sprite) {
		m_ImageInfoSprite = sprite;
	}

	public virtual Sprite GetImageInfo() {
		return m_ImageInfoSprite;
	}

	public override string ToString ()
	{
		var introduce = string.Format ("***<color=#FF5454FF>{0}</color>***\n* {1}\n* Tax: {2}.", GetPlaceType(), GetIntroduceInfo(), GetTax());
		return introduce;
	}

	public string JsonSerialize() {
		var json = new StringBuilder ("{");
		json.Append (string.Format("\"id\": {0},", GetIndex()));
		json.Append (string.Format("\"name\": \"{0}\",", GetPlaceName()));
		json.Append (string.Format("\"type\": {0},", (int)GetPlaceType()));
		json.Append (string.Format("\"price\": {0},", GetPrice()));
		json.Append (string.Format("\"tax\": {0},", GetTax()));
		json.Append (string.Format("\"image\": \"{0}\",", GetImageInfo() != null ? GetImageInfo().name : string.Empty));
		json.Append (string.Format("\"introduce\": \"{0}\"", GetIntroduceInfo()));
		json.Append ("}");
		return json.ToString();
	}

	public void JsonDeserialize(string json) {
		var jsonDeser = MiniJSON.Json.Deserialize (json) as Dictionary<string, object>;
		var id = int.Parse (jsonDeser["id"].ToString());
		var name = jsonDeser["name"].ToString();
		var type = (CEnum.EPlaceType) int.Parse (jsonDeser["type"].ToString());
		var price = int.Parse (jsonDeser["price"].ToString());
		var tax = int.Parse (jsonDeser["tax"].ToString());
		var image = jsonDeser["image"].ToString();
		var introduce = jsonDeser["introduce"].ToString();

		SetIndex (id);
		SetPlaceName (name);
		SetPlaceType (type);
		SetPrice (price);
		SetTax (tax);
		SetImageInfo (CTest7Manager.ResourceSprite(image));
		SetIntroduceInfo (introduce);
	}

}
