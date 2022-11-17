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

public static class Messenger 
{
    private static uint idCount = 0;
    private  static Dictionary<string, List<Message>> messages = new Dictionary<string, List<Message>>();
    public static uint RegisterEvent(string key, MessageDel del)
    {        
        idCount++;
        messages[key].Add(new Message(del, idCount));
        return idCount;
    }

    public static void DeregisterEvent(string key, uint id)
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

    public static void SendEvent(string key, MessageArgs args)
    {
        foreach(Message m in messages[key])
        {
            m.del(args);
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
