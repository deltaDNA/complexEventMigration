# DeltaDNA Complex Event Migration

## Overview

DeltaDNA used complex events to record player actions that resulted in the exchange of items, virtual currencies and real currencies. These events contained product **objects** containing nested arrays of the items and currencies received, or spent. 

Unity Analytics only supports product objects on the transaction event and it has a fixed structure that cannot be extended. The Unity Analytics event manager does not support the creation of product objects on custom events and the Unity Analytics SDK does not let you record and upload custom events containing product objects. 

Unity Analytics only supports product objects on the standard, imutable, transaction event. 

This limitation was introduced to simplify the SDK integration and event creation process. 

## Use case
There were several deltaDNA standard events, other than the transaction, that used complex objects to communicate player actions along with their associated product objects. Examples include the missionCompleted and levelUp events, both of which let you define any rewarded items and curerencies associated with the event. 

```
{
    "userID": "Complex-Event-User",
    "sessionID": "a602398c-54c4-4d60-9c7f-3fa6fbd3ea8c",
    "eventUUID": "ba0d5d99-1a46-471e-b53c-55148804a3e4",
    "eventTimestamp": "2024-05-31 15:05:44.856",
    "eventName": "levelUp",
    "eventParams": {
        "goldBalance": 5400,
        "livesBalance": 3,
        "levelUpName": "Level 5",
        "userLevel": 5,
        "reward": {
            "rewardName": "Level Up Reward",
            "rewardProducts": {
                "virtualCurrencies": [
                    {
                        "virtualCurrency": {
                            "virtualCurrencyName": "Diamonds",
                            "virtualCurrencyType": "GRIND",
                            "virtualCurrencyAmount": 20
                        }
                    }
                ],
                "items": [
                    {
                        "item": {
                            "itemName": "MultiStrike",
                            "itemType": "Power Up",
                            "itemAmount": 1
                        }
                    }
                ]
            }
        },
        "platform": "PC_CLIENT",
        "sdkVersion": "Unity SDK v6.0.3"
    }
}
```

## Problem

This inability to record events containing product objects causes a problem for games that originated on deltaDNA and had their events mapped accross to Unity Analytics. The event schema for these complex events will have been mapped across to Unity Analytics, but you can no longer send data to them from the Unity Analytics SDK. 

## Solution 

There are two options for working with this limitiation 

1. To split events containing product objects in to two separate events. One to record the player action and a seperate transaction event to record the transfer of items or currencies associated with the player action.
2. To record a single custom event to record the player action, but stringify the product object and send it in a custom string parameter. This can be parsed back into an object in SQL for later analysis and reporting. 

Option 1. Splitting the event is the preferred option as it will be future proof and is easier to work with later during Analysis and reporting. Both options will require client code and analysis & reporting changes. 

This project contains example code showing:
1. An original deltaDNA levelUp event
2. A Unity Analytics SPLIT event implementation
3. A unity Analytics STRING based product implementation. 

![Menu Screenshot](/Images/menu-screenshot.png)
