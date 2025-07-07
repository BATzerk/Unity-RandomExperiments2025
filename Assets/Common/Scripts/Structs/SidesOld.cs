using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Side {
	T, R, B, L, TL, TR, BR, BL
}


public static class SidesOld {
	public const int NumSides = 4; // it's hip to be square!
	public const int Undefined = -1;
	public const int T = 0;
	public const int R = 1;
	public const int B = 2;
	public const int L = 3;
	public const int TL = 4;
	public const int TR = 5;
    public const int BR = 6;
    public const int BL = 7;

	public const int Min = 0;
	public const int Max = 8;


	static public int GetOpposite(int side) {
		switch (side) {
			case SidesOld.L: return SidesOld.R;
			case SidesOld.R: return SidesOld.L;
			case SidesOld.B: return SidesOld.T;
			case SidesOld.T: return SidesOld.B;
			case SidesOld.TL: return SidesOld.BR;
			case SidesOld.TR: return SidesOld.BL;
			case SidesOld.BL: return SidesOld.TR;
			case SidesOld.BR: return SidesOld.TL;
			default: throw new UnityException ("Whoa, " + side + " is not a valid side. Try 0 through 7.");
		}
	}
	static public int GetHorzFlipped(int side) {
		switch (side) {
			case L: return R;
			case R: return L;
			case TL: return TR;
			case TR: return TL;
			case BL: return BR;
			case BR: return BL;
			default: return side; // this side isn't affected by a flip.
		}
	}
	static public int GetVertFlipped(int side) {
		switch (side) {
			case T: return B;
			case B: return T;
			case TL: return BL;
			case TR: return BR;
			case BL: return TL;
			case BR: return TR;
			default: return side; // this side isn't affected by a flip.
		}
	}
}
