using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Scripts.Serialization;
using Assets.Scripts.UI;

public class ItemCollectionSerializer
{
    private List<ItemDataParams> Params; 
    private string SavePath;

    public ItemCollectionSerializer(List<ItemData> DataToSave)
    {
        Params = new List<ItemDataParams>();
        foreach (ItemData id in DataToSave )
        {
            Params.Add(new ItemDataParams(id));
        }
    }

    public ItemCollectionSerializer()
    {
        
    }

    public void Save(string Path)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (var stream = new MemoryStream())
        {
            formatter.Serialize(stream, Params);
            using (FileStream fs = new FileStream(Path + ".icf", FileMode.Create))
            {
                var vdata = stream.ToArray();
                fs.Write(vdata, 0, vdata.Length);
            }
        } 
    }

    public List<ItemDataParams> Load(string Path)
    {
        List<ItemDataParams> IDS = new List<ItemDataParams>();
        BinaryFormatter formatter = new BinaryFormatter();

        using (var stream = new FileStream(Path + ".icf",FileMode.Open))
        {
            IDS = (List<ItemDataParams>) formatter.Deserialize(stream);
        }

        return IDS;
    }
}
