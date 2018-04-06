using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Engine;

namespace SuperAdenture
{
    public partial class SuperAdventure : Form
    {
        private Player _player;
        private Monster _currentMonster;

        public SuperAdventure()
        {
            InitializeComponent();


            _player = new Player(10, 10, 20, 0, 1);
            MoveTo(world.LocationByID(World.LOCATION_ID_HOME));
            _player.Inventory.Add(new InventoryItem(World.ItemByID(World.ITEM_ID_RUSTY_SWORD), 1));

            lblHitPoints.Text = _player.CurrentHitPoints.ToString();
            lblGold.Text = _player.Gold.ToString();
            lblExperience.Text = _player.ExperiencePoints.ToString();
            lblLevel.Text = _player.Level.ToString();
        }

        private void btnNorth_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToNorth);
        }

        private void btnWest_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToWest);
        }

        private void btnSouth_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToSouth);
        }

        private void btnEast_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToEast);
        }

        private void MoveTo(Location newLocation)
        {
            //Does the location have any required items
            if(newLocation.ItemRequiredToEnter != null)
            {
                //See if the player has the required items in their inventory
                bool playerHasRequiredItem = false;

                foreach(InventoryItem ii in _player.Inventory)
                {
                    if(ii.Details.ID == newLocation.ItemRequiredToEnter.ID)
                    {
                        //We found the required item
                        playerHasRequiredItem = true;
                        break; //Exit out of the foreach loop
                    }
                }

                if(!playerHasRequiredItem)
                {
                    // We didn't find the required item in the inventory
                    rtbMessages.Text += "You must have a " + newLocation.ItemRequiredToEnter.Name + " to enter this location." + Environment.NewLine;
                    return;
                }
            }

            // Update the player's current location
            _player.CurrentLocation = newLocation;

            // Show/hide available movement buttons
            btnNorth.Visible = (newLocation.LocationToNorth != null);
            btnEast.Visible = (newLocation.LocationToEast != null);
            btnSouth.Visible = (newLocation.LocationToSouth != null);
            btnWest.Visible = (newLocation.LocationToWest != null);

            // Display current location name and description
            rtbLocation.Text = newLocation.Name + Environment.NewLine;
            rtbLocation.Text += newLocation.Description + Environment.NewLine;

            // Completely heal the player
            _player.CurrentHitPoints = _player.MaximumHitPoints;

            // Update Hit Points in the UI
            lblHitPoints.Text = _player.CurrentHitPoints.ToString();

            // Does the location have a quest?
            if(newLocation.QuestAvailableHere != null)
            {
                // See if the player already has the quest, and if they've completed it
                bool playerAlreadyHasQuest = false;
                bool playerAlreadyCompletedQuest = false;

                foreach (PlayerQuest playerQuest in _player.Quests)
                {
                    if(playerQuest.Details.ID == newLocation.QuestAvailableHere.ID)
                    {
                        playerAlreadyHasQuest = true;
                        
                            if (playerQuest.IsCompleted)
                            {
                                playerAlreadyCompletedQuest = true;
                            }
                    }
                }

                // See if the player already has the quest
                if(playerAlreadyHasQuest)
                {
                    // If the player has not completed the quest yet
                    if(!playerAlreadyCompletedQuest)
                    {
                        // See if the player has all the items needed to complete the quest
                        bool playerHasAllItemsToCompleteQuest = true;

                        foreach(QuestCompletionItem qci in newLocation.QuestAvailableHere.QuestCompletionItems)
                        {
                            bool foundItemInPlayersInventory = false;

                            //Check each item in the inventory to see if the have it and enough of it
                            foreach(InventoryItem ii in _player.Inventory)
                            {
                                // The player has this item in their inventory
                                if(ii.Details.ID == qci.Details.ID)
                                {
                                    foundItemInPlayersInventory = true;

                                    if(ii.Quantity < qci.Quantity)
                                    {
                                        // The player does not have enough of the item to complete the quest
                                        playerHasAllItemsToCompleteQuest = false;

                                        // There is no reason to continue checking for the other items
                                        break;
                                    }

                                    // We found the item, so don't check the rest of the player's inventory
                                    break;
                                }
                            }

                            // If we didn't find the required item, set our variable and stop looking for the other items
                            if(!foundItemInPlayersInventory)
                            {
                                //The player does not have this item in their inventory
                                playerHasAllItemsToCompleteQuest = false;

                                // There is no reason to continue checking for other items
                                break;
                            }
                        }

                        //The player has all items required to complete the quest
                        if(playerHasAllItemsToCompleteQuest)
                        {
                            // Display message
                            rtbMessages.Text += Environment.NewLine;
                            rtbMessages.Text += "You complete the " + newLocation.QuestAvailableHere.Name + " quest." + Environment.NewLine;

                            // Remove quest items from inventory
                            foreach(QuestCompletionItem qci in newLocation.QuestAvailableHere.QuestCompletionItems)
                            {
                                if(ii.Details.ID == qci.Details.ID)
                                {
                                    // Subtract the quantity from the player's inventory needed to complete the quest
                                    ii.Quantity -= qci.Quantity;
                                    break;
                                }
                            }
                        }

                        //Give quest rewards
                        rtbMessages.Text += "You receive: " + Environment.NewLine;
                        rtbMessages.Text += newLocation.QuestAvailableHere.RewardExperiencePoints.ToString() + " experience points" + Environment.NewLine;

                    }
                }
            }
        }
        private void btnUseWeapon_Click(object sender, EventArgs e)
        {

        }

        private void btnUsePotion_Click(object sender, EventArgs e)
        {

        }
    }
}
