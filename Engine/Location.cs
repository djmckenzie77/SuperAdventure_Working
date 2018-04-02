using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public class Location
    {
        public int ID { get; set; }
        public int Name { get; set; }
        public int Description { get; set; }
        public Item ItemRequiredToEnter { get; set; }
        public Quest QuestAvailableHere { get; set; }
        public Monster MonsterLivingHere { get; set; }
        public Location LocationToNorth { get; set; }
        public Location LocationToSouth { get; set; }
        public Location LocationToEast { get; set; }
        public Location LocationToWest { get; set; }

        public Location(int ID, string name, string description, Item itemRequiredToEnter = null, Monster monsterLivingHere = null, Quest questAvailableHere = null)
        {
            ID = id;
            Name = name;
            Description = description;
            ItemRequiredToEnter = itemRequiredToEnter;
            QuestAvailableHere = questAvailableHere;
            MonsterLivingHere = monsterLivingHere;

        }

    }
}
