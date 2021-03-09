using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WpfApp29
{
    public class Inventory
    {
        public List<Item> Items { get; private set; }

        internal void IncrementItem(Item item)
        {
            var found = Items.FirstOrDefault(
                s => s.Name == item.Name);
            if (found == null)
            {
                item.Count = 1;
                Items.Add(item);
            }
            else
                found.Count++;
            Save();
        }

        internal void DecrementItem(Item item)
        {
            var found = Items.FirstOrDefault(
                s => s.Name == item.Name);
            if (found != null)
            {
                found.Count--;
                if (found.Count == 0)
                    Items.Remove(found);
                Save();
            }
        }

        const string db = "items.db";
        public void Save()
        {
            XmlSerializer xml = new XmlSerializer(typeof(List<Item>));
            using (var fs = File.Create(db))
                xml.Serialize(fs, Items);
        }

        public Inventory()
        {
            Load();
        }

        private void Load()
        {
            if (!File.Exists(db))
            {
                Items = new List<Item>();
                return;
            }
            XmlSerializer xml = new XmlSerializer(typeof(List<Item>));
            using (var fs = File.OpenRead(db))
                Items = (List<Item>)xml.Deserialize(fs);
        }
    }
}
