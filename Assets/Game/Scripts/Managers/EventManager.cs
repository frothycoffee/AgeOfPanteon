using System;
using System.Collections.Generic;

public enum GameEvent
{
    LeftMouseButtonActionPerformed,
    RightMouseButtonActionPerformed,

    OpenProductionPanel,
    CloseProductionPanel,
    
    ShowEntityPreview,
    ClearEntityPreview,

    SpawnEntity,
    DestroyEntity,

    Attack,
    Move,
}

public static class EventManager
{
    private static Dictionary<GameEvent, Action<int>> eventTableInt = new Dictionary<GameEvent, Action<int>>();

    private static Dictionary<GameEvent, Action<float>> eventTableFloat = new Dictionary<GameEvent, Action<float>>();

    private static Dictionary<GameEvent, Action<string>> eventTableString = new Dictionary<GameEvent, Action<string>>();

    private static Dictionary<GameEvent, Action> eventTable = new Dictionary<GameEvent, Action>();

    public static void AddHandler(GameEvent gameEventInt, Action<int> actionInt)
    {
        if (!eventTableInt.ContainsKey(gameEventInt)) eventTableInt[gameEventInt] = actionInt;
        else eventTableInt[gameEventInt] += actionInt;
    }

    public static void AddHandler(GameEvent gameEventFloat, Action<float> actionFloat)
    {
        if (!eventTableFloat.ContainsKey(gameEventFloat)) eventTableFloat[gameEventFloat] = actionFloat;
        else eventTableFloat[gameEventFloat] += actionFloat;
    }

    public static void AddHandler(GameEvent gameEventString, Action<string> actionString)
    {
        if (!eventTableString.ContainsKey(gameEventString)) eventTableString[gameEventString] = actionString;
        else eventTableString[gameEventString] += actionString;
    }

    public static void AddHandler(GameEvent gameEvent, Action action)
    {
        if (!eventTable.ContainsKey(gameEvent)) eventTable[gameEvent] = action;
        else eventTable[gameEvent] += action;
    }


    public static void RemoveHandler(GameEvent gameEventInt, Action<int> actionInt)
    {
        if (eventTableInt[gameEventInt] != null)
            eventTableInt[gameEventInt] -= actionInt;
        if (eventTableInt[gameEventInt] == null)
            eventTableInt.Remove(gameEventInt);
    }

    public static void RemoveHandler(GameEvent gameEventFloat, Action<float> actionFloat)
    {
        if (eventTableFloat[gameEventFloat] != null)
            eventTableFloat[gameEventFloat] -= actionFloat;
        if (eventTableFloat[gameEventFloat] == null)
            eventTableFloat.Remove(gameEventFloat);
    }

    public static void RemoveHandler(GameEvent gameEventString, Action<string> actionString)
    {
        if (eventTableString[gameEventString] != null)
            eventTableString[gameEventString] -= actionString;
        if (eventTableString[gameEventString] == null)
            eventTableString.Remove(gameEventString);
    }

    public static void RemoveHandler(GameEvent gameEvent, Action action)
    {
        if (eventTable[gameEvent] != null)
            eventTable[gameEvent] -= action;
        if (eventTable[gameEvent] == null)
            eventTable.Remove(gameEvent);
    }

    public static void Brodcast(GameEvent gameEventInt, int objectId) => eventTableInt[gameEventInt]?.Invoke(objectId);

    public static void Brodcast(GameEvent gameEventFloat, float value) => eventTableFloat[gameEventFloat]?.Invoke(value);

    public static void Brodcast(GameEvent gameEventString, string name) => eventTableString[gameEventString]?.Invoke(name);

    public static void Brodcast(GameEvent gameEvent) => eventTable[gameEvent]?.Invoke();
}