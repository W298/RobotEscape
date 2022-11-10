using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public enum ItemDB
{
    AidKit = 0
}

public class Item
{
    public int id;
    public string name;
    public int count;

    public Item(int id, int count)
    {
        this.id = id;
        this.name = ((ItemDB)id).ToString();
        this.count = count;
    }

    public Item(string name, int count)
    {
        ItemDB.TryParse(name, out ItemDB res);
        this.id = (int)res;
        this.name = name;
        this.count = count;
    }
}

public class PlayerInventory : MonoBehaviour
{
    private List<Item> inventory;

    private void Awake()
    {
        inventory = new List<Item>();

        AddItem(0, 2);
    }

    public bool AddItem(int id, int count)
    {
        if (inventory.Any(i => i.id == id))
        {
            inventory.Find(i => i.id == id).count += count;
            return false;
        }
        
        inventory.Add(new Item(id, count));
        return true;
    }

    public bool AddItem(string name, int count)
    {
        if (inventory.Any(i => i.name == name))
        {
            inventory.Find(i => i.name == name).count += count;
            return false;
        }

        inventory.Add(new Item(name, count));
        return true;
    }

    public bool UseItem(int id, int count)
    {
        if (inventory.All(i => i.id != id)) return false;

        var item = inventory.Find(i => i.id == id);
        item.count -= count;
        if (item.count < 0)  
        {
            item.count = 0;
            return false;
        }

        return true;

    }

    public bool UseItem(string name, int count)
    {
        if (inventory.All(i => i.name != name)) return false;

        var item = inventory.Find(i => i.name == name);
        item.count -= count;
        if (item.count < 0)  
        {
            item.count = 0;
            return false;
        }

        return true;
    }

    public Item GetItem(int id)
    {
        return inventory.All(i => i.id != id) ? null : inventory.Find(i => i.id == id);
    }

    public Item GetItem(string name)
    {
        return inventory.All(i => i.name != name) ? null : inventory.Find(i => i.name == name);
    }
}
