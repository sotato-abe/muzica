using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SlotWindow : Panel
{
    [SerializeField] CommandReel commandReel1;
    [SerializeField] CommandReel commandReel2;
    [SerializeField] CommandReel commandReel3;

    public List<Command> StopReels()
    {
        List<Command> commands = new List<Command>();
        commands.Add(commandReel1.StopReel());
        commands.Add(commandReel2.StopReel());
        commands.Add(commandReel3.StopReel());
        return commands;
    }
}
