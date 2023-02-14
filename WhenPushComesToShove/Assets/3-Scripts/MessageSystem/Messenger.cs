using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void MessageDel(MessageArgs args);

struct Message 
{
    public MessageDel del;
    public uint uID;
    public Message(MessageDel del, uint uID)
    {
        this.del = del;
        this.uID = uID;
    }
}

//These are like events in that they contain lists of delegates. Instead of being defined and differentiated by their return types and arguments however,
//they are defined by a string key, and they all have the same very general arguments. When you register an event, you are telling the messenger system 
//"Hey, whenever someone tells you that this key has happened, call this delegate". When you send an event, you are telling the system "This key has just happened,
//call all the delegates associated with this key". Typically higher-level objects, like modifiers, register to events, and low-level objects, like props, send events.
// https://en.wikipedia.org/wiki/Messaging_pattern (This is a decent description of whats happening here, but my implementation is a lot less OOP, instead letting anyone send or register to events)
public static class Messenger 
{
    private static uint idCount = 0;
    private static Dictionary<string, List<Message>> messages = new Dictionary<string, List<Message>>();

    /// <summary>
    /// Registers a delegate to be called when send event is called with the same key
    /// </summary>
    /// <param name="key">Key to have the delegate associated with </param>
    /// <param name="del">Delegate to be called</param>
    /// <returns>Unique ID associated with delegate</returns>
    public static uint RegisterEvent(string key, MessageDel del)
    {        
        idCount++;
        if(!messages.ContainsKey(key))
        {
            messages.Add(key, new List<Message>());
        }
        messages[key].Add(new Message(del, idCount));
        return idCount;
    }

    /// <summary>
    /// Unregisters a delegate, unassociating it with its key. Quicker than calling with just ID
    /// </summary>
    /// <param name="key">Key delegate is listed under</param>
    /// <param name="id">Unique ID, returned by Register Event</param>
    public static void UnregisterEvent(string key, uint id)
    {
        for(int i = 0; i < messages[key].Count; i++)
        {
            if(messages[key][i].uID == id)
            {
                messages[key].RemoveAt(i);
                return;
            }
        }
    }

    /// <summary>
    /// Unregisters a delegate, unassociating it with its key. 
    /// </summary>
    /// <param name="id">Unique ID, returned by Register Event</param>
    public static void UnregisterEvent(uint id)
    {
        foreach(List<Message> list in messages.Values)
        {
            for(int i = 0; i < list.Count; i++)
            {
                if(list[i].uID == id)
                {
                    list.RemoveAt(i);
                }
            }
        }
    }

    /// <summary>
    /// Calls all delegates associated with key, with the given arguments
    /// </summary>
    /// <param name="key">Key to call delegates associated with it</param>
    /// <param name="args">Arguments to supply to the delegate</param>
    public static void SendEvent(string key, MessageArgs args)
    {
        if (messages.ContainsKey(key))
        {
            foreach (Message m in messages[key])
            {
                m.del(args);
            }
        }
    }

}

public struct MessageArgs
{
   public float floatArg;
   public int intArg;
   public bool boolArg;
   public string stringArg;
   public object objectArg;
   public PlayerConfiguration playerArg;
   public Vector2? vectorArg;
    public MessageArgs(float floatArg = 0, int intArg = 0, bool boolArg = false, string stringArg = "", object objectArg = null, Vector2? vectorArg = null, PlayerConfiguration playerArg = null)
    {
        this.floatArg = floatArg;
        this.boolArg = boolArg;
        this.intArg = intArg;
        this.vectorArg = vectorArg;
        this.stringArg = stringArg;
        this.objectArg = objectArg;
        this.playerArg = playerArg;
    }

}
