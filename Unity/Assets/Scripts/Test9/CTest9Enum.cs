using System;
using System.Collections;

public class CTest9Enum {


}

public enum ESkillType: byte {
	None = 0,
	Self = 1,
	Enemy = 2
}

public enum ESkillEffect: byte {
	None = 0,
	Health = 1,
	Mana = 2,
	Rage = 3
}

public enum ETeam: byte {
	None = 0,
	Neutrol = 1,
	Ally = 2,
	Enemy = 3
}

public enum EGameType: int {
	None = 0,
	ServerResult = 1,
	// Sodier
	Sodier = 100,
	// Skill
	Skill = 500,
}
