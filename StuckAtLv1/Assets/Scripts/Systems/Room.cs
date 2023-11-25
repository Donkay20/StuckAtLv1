using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    /*
    This script is added to each room. Each room needs to be instantiated and given a room type, which is given via the map manager.
    Map type can also be requested from here too. Managed by enum state machine.
    */
    private enum RoomType {
        Unassigned, //if something goes wrong, it should be this state, hopefully we never see it
        Combat, 
        Survival,
        Event,
        Shop,
        Miniboss,
        Boss,
    }

    private RoomType room = RoomType.Unassigned; //to check if a room was somehow never initialized

    public void AssignRoomType(string type) {
        switch (type) {
            case "combat":
                room = RoomType.Combat;
                Debug.Log("roomtype_combat");
                break;

            case "survival":
                room = RoomType.Survival;
                Debug.Log("roomtype_survival");
                break;

            case "event":
                room = RoomType.Event;
                Debug.Log("roomtype_event");
                break;

            case "shop":
                room = RoomType.Shop;
                Debug.Log("roomtype_shop");
                break;

            case "miniboss":
                room = RoomType.Miniboss;
                Debug.Log("roomtype_miniboss");
                break;

            case "boss":
                room = RoomType.Boss;
                Debug.Log("roomtype_boss");
                break;
        }
    }

    public string GetRoomType() {
        string answer = "null";
        switch (room) {
            case RoomType.Combat:
                answer = "combat";
                return answer;

            case RoomType.Survival:
                answer = "survival";
                return answer;

            case RoomType.Event:
                answer = "event";
                return answer;

            case RoomType.Shop:
                answer = "shop";
                return answer;

            case RoomType.Miniboss:
                answer = "miniboss";
                return answer;

            case RoomType.Boss:
                answer = "boss";
                return answer;

            case RoomType.Unassigned:
                answer = "unassigned";
                return answer;
        }
        return answer;
    }
}
