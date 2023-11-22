using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private enum RoomType {
        Unassigned,
        Combat, 
        Survival,
        Event,
        Shop,
        Miniboss,
        Boss,
    }

    private RoomType room;

    private void Start() {
        room = RoomType.Unassigned;
    }

    public void AssignRoomType(string type) {
        switch (type) {
            case "combat":
                room = RoomType.Combat;
                //Debug.Log("assigned combat");
                break;
            case "survival":
                room = RoomType.Survival;
                break;
            case "event":
                room = RoomType.Event;
                break;
            case "shop":
                room = RoomType.Shop;
                break;
            case "miniboss":
                room = RoomType.Miniboss;
                //Debug.Log("assigned miniboss");
                break;
            case "boss":
                room = RoomType.Boss;
                //Debug.Log("assigned boss");
                break;
        }
    }

    public string GetRoomType() {
        string answer = "";
        switch (room) {
            case RoomType.Combat:
                answer = "combat";
                break;
            case RoomType.Survival:
                answer = "survival";
                break;
            case RoomType.Event:
                answer = "event";
                break;
            case RoomType.Shop:
                answer = "shop";
                break;
            case RoomType.Miniboss:
                answer = "miniboss";
                break;
            case RoomType.Boss:
                answer = "boss";
                break;
        }
        return answer;
    }
}
