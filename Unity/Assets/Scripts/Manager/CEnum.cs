using System;
using System.Collections;

public class CEnum {

	public enum EAnimation : int {
		Idle = 0,
		Attack = 1,
		Angry = 2,
		Eat = 3
	}

	public enum EAttackType : int  {
		None = 0,
		Physic = 1,
		Magic = 2
	}

	public enum EPlaceType : int {
		None = 0,
		Street = 1,
		City = 2,
		Park = 3,
		Resort = 4
	}

	public enum EContructionLevel : int {
		None = 0,
		Level1 = 1,
		Level2 = 2,
		Level3 = 3
	}

	public enum EItemType: byte {
		None = 0,
		Common = 1,
		Uncommon = 2,
	}

	public enum EItem: int {
		None = 0,
		// Food
		Rice = 1,
		Fish = 2,

		// Nature
		Nature = 20,
		Water = 21,
		Fire = 22
	}

	public enum EBlockDirection: byte {
		Center = 0,
		Left = 1,
		Top = 2,
		Right = 3,
		Bottom = 4,
		TopLeft = 5,
		TopRight = 6,
		BottomRight = 7,
		BottomLeft = 8
	}

}
