using UnityEngine;
using UnityEngine.Events;

// Evento que se dispara cuando algo importante ocurre en el juego
[CreateAssetMenu(fileName = "NewGameEvent", menuName = "Game/GameEvent")]
public class GameEvent : ScriptableObject
{
    public UnityEvent OnEventTriggered = new UnityEvent();

    public void TriggerEvent()
    {
        OnEventTriggered?.Invoke();
    }
}

// Listener para GameEvent
public class GameEventListener : MonoBehaviour
{
    public GameEvent gameEvent;
    public UnityEvent onEvent = new UnityEvent();

    void OnEnable()
    {
        if (gameEvent != null)
            gameEvent.OnEventTriggered.AddListener(Respond);
    }

    void OnDisable()
    {
        if (gameEvent != null)
            gameEvent.OnEventTriggered.RemoveListener(Respond);
    }

    void Respond()
    {
        onEvent.Invoke();
    }
}