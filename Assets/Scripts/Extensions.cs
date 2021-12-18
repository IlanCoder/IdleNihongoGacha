using System.Collections;
using System.Collections.Generic;

public static class Extensions {
	public static T GetLast<T>(this List<T> list) {
		return list[list.Count - 1];
	}

	public static void Shuffle<T>(this List<T> list, System.Random randomNum) {
		int listSize = list.Count;
		if (listSize <= 0) {
			return;
		}
		for (int i = listSize - 1; i >= 1; i--) {
			int cardToShuffle = randomNum.Next(listSize);
			T temp = list[i];
			list[i] = list[cardToShuffle];
			list[cardToShuffle] = temp;
		}
	}

	public static bool HasNull<T>(this T[] array) {
		for (int i = 0; i < array.Length; i++) {
			if(array[i] == null) {
				return true;
			}
		}
		return false;
	}
}
