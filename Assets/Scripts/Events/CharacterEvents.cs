using UnityEngine;
using UnityEngine.Events;

public class CharacterEvents
{
    // персонаж получил урон и значение урона
    public static UnityAction<GameObject, int> characterDamaged;

    // персонаж вылечился и само значение хила
    public static UnityAction<GameObject, int> characterHealed;
}
