using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FishGame
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        public Point mouseLocation;
        bool autoBuyIngredients = false;
        bool userABIchoice = false;

        int StorageUnits = 1;
        int StorageSize = 50;
        int StoreIncreasePrice = 2000;
        int StoreSize = 4;
        int aFryers = 0;
        int Ingredients = 35;
        int UnitsCreated = 0;
        int IngredientPrice = 40;
        int Money = 1000000;
        int SellPrice = 1;
        int AvailableUnits = 0;
        int AutoRate = 0;
        double demand = 150;
        double sellrate = 10;
        int storagePrice = 800;
        int aFryerPrice = 1000;
        int productionPS = 0;
        int marketingPrice = 2100;
        int marketingLevel = 1;
        int seasoningPrice = 1800;
        int seasoningLevel = 0;
        int fryerEfficiency = 1;
        int fryerEPrice = 3500;
        int revenuePS = 0;

        List<PictureBox> upgradesPic = new List<PictureBox>();

        List<Label> upgradesLbl = new List<Label>();

        private bool CheckStorage(int number)
        {
            if (StorageSize >= (Ingredients + number + AvailableUnits)) //check if storage will not be full after adding 'number' to current storage
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        private void UpdateStorage() //updates storage labels to reflect storage values
        {
            lblStorage.Text = "Storage: " + (Ingredients + AvailableUnits).ToString() + "/" + StorageSize.ToString(); //updates storage label with current storage used
            lblStorageUnits.Text = "Storage Units: " + StorageUnits; //updates label to show how many storage units there are
        }

        private void ChangeIngredient(int number)
        {
            if (CheckStorage(number) == true) //checks if storage is available.
            {
                Ingredients += number; //adds (number) of ingredients to ingredients
                lblIngredientCount.Text = "Ingredients: " + Ingredients.ToString(); //updates ingredient label to reflect now value
                UpdateStorage();
            }
        }

        private void ChangeMoney(int number)
        {
            if (Money != 0 || (Money + number) >= 0) // checks if current money != 0 and if money after adding number is greater than or equal to 0 (so no bankruptcy)
            {
                Money += number; //adds number to money
                lblMoney.Text = "$" + Money.ToString(); //updates money label
                revenuePS += number; //updates revenue to reflect money change
            }
        }

        private void UpdateStoreSize() //updates the store size labels to reflect new store size
        {
            lblStoreSize.Text = "Store: " + (StorageUnits + aFryers + 1).ToString() + "/" + StoreSize.ToString() + " tiles";
        }


        private void CreateFishChips (int number)
        {
            if (number > 0) //checks if number is greater than 0... after all you can't make negative or zero fish and chips
            {
                if (Ingredients >= number ) //checks if ingredients are available to create number of fish and chips
                {
                    UnitsCreated += number; //adds number to total units created
                    lblFCcounter.Text = "Units created: " + UnitsCreated.ToString();
                    ChangeIngredient(-number); //takes away number of ingredients for 'production'
                    AvailableUnits += number; //updates available fish and chip value
                    UpdateStorage();
                    productionPS += number; // adds (number) to production per second count
                }
                else if (Ingredients < number) // if not enough ingredients are avialable for the full number, it will produce use up all the ingredients there are for fish and chips
                {
                    UnitsCreated += Ingredients;
                    lblFCcounter.Text = "Units created: " + UnitsCreated.ToString();
                    ChangeIngredient(-Ingredients);
                    AvailableUnits += Ingredients;
                    UpdateStorage();
                    productionPS += number;
                }
                lblAvailableUnits.Text = AvailableUnits.ToString() + " Fish and Chips"; //updates available fish and chips counter
            }
        }

        private void CalculateDemand()
        {
            demand = Math.Round(-Math.Pow(SellPrice, 1.15) + 70 + (marketingLevel-1)*28 + (seasoningLevel)*15 , MidpointRounding.AwayFromZero); //calculation for calculating demand
            lblDemand.Text = "Demand: " + demand.ToString() + "%"; //updates demand label
            if (demand > 5)
            {
                sellrate = Math.Round(Math.Pow(10, demand / 100)); //determines sell rate based on demand.
            }
            else
            {
                sellrate = 0; //any demand under 5% will not sell anything
            }
            lblSellRate.Text = "Predicted units sold per second:\n" + sellrate.ToString();
        }

        private void AutoBuyIngredient()
        {
            if (Ingredients <= 0) //checks if no ingredients are left
            {
                int amountBuy; //multiplier used later on.
                if ((AutoRate * fryerEfficiency) > 1) 
                {
                    amountBuy = Convert.ToInt32(Math.Round(Math.Pow((AutoRate * fryerEfficiency), 0.56) - 0.9, MidpointRounding.AwayFromZero)); //Calculates the 'perfect' amount to buy based on the automatic production rate
                }
                else
                {
                    amountBuy = 1;
                }
                int ingredientsToPurchase = amountBuy * 20; //calculates the number of ingredients to buy (goes up in 20 intervals)
                for (int i = 0; i < ingredientsToPurchase; i++) // does a loop to buy every single ingredeient individually so as many ingredients can be bought from the calculated amount.
                {
                    if ((IngredientPrice/20) <= Money)
                    {
                        ChangeIngredient(1);
                        ChangeMoney(IngredientPrice/20); //takes away 1x ingredient worth of money
                    }
                }
            }
        }

        private void picClickFish_Click(object sender, EventArgs e)
        {
            if (Ingredients > 0) //checks if ingredients are available
            {
                CreateFishChips(1);
            }
        }


        private void frmMainGame_Load(object sender, EventArgs e)
        {
            CalculateDemand();

            //pictureboxes (images) for upgrades:
            upgradesPic.Add(picBuyAFryer); //adds pictureboxes to a list for a future loop (reduces lines of code)
            upgradesPic.Add(picIncreaseStorage);
            upgradesPic.Add(picIncreaseMarketing);
            upgradesPic.Add(picAutoBuyIngredients);
            upgradesPic.Add(picBuyIngredient);
            upgradesPic.Add(picSeasoning);
            upgradesPic.Add(picStoreIncrease);
            upgradesPic.Add(picFryerE);

            //price tags for upgrades:
            upgradesLbl.Add(lblBuyAFryer);
            upgradesLbl.Add(lblIncreaseStorage);
            upgradesLbl.Add(lblIncreaseMarketing);
            upgradesLbl.Add(lblAutoBuyIngredients);
            upgradesLbl.Add(lblBuyIngredients);
            upgradesLbl.Add(lblSeasoning);
            upgradesLbl.Add(lblStoreIncrease);
            upgradesLbl.Add(lblFryerE);

            //Name tags for upgrades:
            upgradesLbl.Add(lblAFname);
            upgradesLbl.Add(lblStrgeName);
            upgradesLbl.Add(lblMarName);
            upgradesLbl.Add(lblABIname);
            upgradesLbl.Add(lblFishPotatoname);
            upgradesLbl.Add(lblSeaName);
            upgradesLbl.Add(lblSIncName);
            upgradesLbl.Add(lblFryerEname);

            for (int i = 0; i < upgradesPic.Count; i++) // loop to 'finalise' the display of every object
            {
                upgradesLbl[i].Parent = upgradesPic[i]; //adds labels of an upgrade to the picturebox
                upgradesLbl[i].Location = new Point(143, 50); //correctly positions the label within the picture box
                upgradesLbl[i].BackColor = Color.Transparent; //makes picturebox transparent

                upgradesLbl[i + upgradesPic.Count].Parent = upgradesPic[i];
                upgradesLbl[i + upgradesPic.Count].Location = new Point(66, 13);
                upgradesLbl[i + upgradesPic.Count].BackColor = Color.Transparent;
                
            }
            picFryerE.Location = picAutoBuyIngredients.Location; 
            picFryerE.Visible = false;
            picFryerE.Enabled = false; //due to lack of space, this upgrade will only be displayed after autobuy has been purchased

        }

        private void trackBarSellPrice_ValueChanged(object sender, EventArgs e)
        {
            lblSellPrice.Text = "Selling for: $" + trackBarSellPrice.Value;
            SellPrice = trackBarSellPrice.Value; //updates sellprice based on user set sell price
            CalculateDemand();

        }

        private void tmrSell_Tick(object sender, EventArgs e)
        {
           if (AvailableUnits >= sellrate) 
            {
                AvailableUnits -= Convert.ToInt32(sellrate); //"sells" the fish and chips at the sellrate
                ChangeMoney(Convert.ToInt32(sellrate * SellPrice)); //gives player money for the sold fish and chips
            }
            else if (AvailableUnits != 0)
            {
                ChangeMoney(Convert.ToInt32(AvailableUnits * SellPrice)); //sells as many fish and chips as possible because there wasn't enough to meet the sellrate
                AvailableUnits -= AvailableUnits;
            }

            lblAvailableUnits.Text = AvailableUnits.ToString() + " Fish and Chips"; //Updates some labels with current information
            lblUnsold.Text = "Unsold Stock: " + AvailableUnits;
            UpdateStorage();
            CreateFishChips(AutoRate * fryerEfficiency); //autofryer's fish and chips production
            lblProductionRate.Text = "Per Second: " + productionPS.ToString();
            productionPS = 0;
            lblIncome.Text = "Revenue Per Second:\n" + revenuePS;
            revenuePS = 0;

            IngredientPrice = Convert.ToInt32(Math.Pow((AutoRate + 1), 0.6) * 20);
            lblBuyIngredients.Text = "$" + IngredientPrice;

            for (int i = 0; i < upgradesPic.Count; i++)
            {
               int price = Convert.ToInt32(upgradesLbl[i].Text.Split('$')[1]); //gets price from the price tag label
               if (price <= Money) //checks if user can purchase upgrade
                {
                   upgradesPic[i].BackColor = Color.LightGreen; //if they can, the upgrade background turns green
                }
              else
                {
                    upgradesPic[i].BackColor = Color.FromArgb(255, 192, 192); //if they cannot purchase the upgrade, the upgrade background turns red
                }
            }

            if (autoBuyIngredients == true && userABIchoice == true) //detects of autobuy has been purchased and turned on
            {
                AutoBuyIngredient();
            }
        }


        private void pnlTop_MouseDown(object sender, MouseEventArgs e)
        {
            mouseLocation = new Point(-e.X, -e.Y);  //Find cursor's position on the screen
        }

        private void pnlTop_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) //detects if the panel is being 'dragged' with a left mouse button
            {
                Point mousePose = Control.MousePosition; //Gets the position of the mouse relative to the top-left corner of screen
                mousePose.Offset(mouseLocation.X, mouseLocation.Y); //offsets the position of mousePose with mouseLocation so the form is in the accurate location
                Location = mousePose; //sets form location to where the mouse is
            }
        }

        private void chbAutoBuyIngredient_CheckedChanged(object sender, EventArgs e)
        {
            userABIchoice = chbAutoBuyIngredient.Checked; //updates user's choice on whether or not they want to auto buy ingredients
        }

        private void picMinimise_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized; //minimise window
        }

        private void picExit_Click(object sender, EventArgs e)
        {
            Application.Exit(); //exit the program
        }


        private void picBuyIngredient_Click(object sender, EventArgs e)
        {
            if (Money >= IngredientPrice && CheckStorage(20) == true) //checks if money and storage are available to purchase ingredients
            {
                ChangeIngredient(20); //updates the ingredient values
                ChangeMoney(-IngredientPrice); //charges the player money for the purchase
                if (Money < IngredientPrice)
                {
                    picBuyIngredient.BackColor = Color.FromArgb(255, 192, 192); //if the user cannot purchase more ingredients, the upgrade background will turn red
                }
            }
        }

        private void picIncreaseStorage_Click(object sender, EventArgs e)
        {
            if (Money != 0 && (Money - storagePrice) >= 0 && StoreSize >= (StorageUnits + aFryers + 2)) //checks if user has money to buy the upgrade and if they have enough store space for more storage
            {
                StorageUnits++; //adds a storage unit
                StorageSize += 50; //adds 50 storage space
                ChangeMoney(-storagePrice); 
                storagePrice = Convert.ToInt32(Math.Round(Convert.ToDouble(storagePrice) * 1.45)); //calculates new storage unit price
                lblIncreaseStorage.Text = "$" + storagePrice.ToString();
                if (Money < storagePrice)
                {
                    picIncreaseStorage.BackColor = Color.FromArgb(255, 192, 192); 
                }
                UpdateStoreSize();
            }
        }

        private void picBuyAFryer_Click(object sender, EventArgs e)
        {
            if (Money != 0 && (Money - aFryerPrice) >= 0 && StoreSize >= (StorageUnits + aFryers + 2)) //checks if user has money to buy the upgrade and if they have enough store space for more auto fryers
            {
                AutoRate += 1; //adds one to how many fish and chips auto fryers produce per second
                ChangeMoney(-aFryerPrice);
                aFryers++; //adds 1 auto fryer to a tally
                aFryerPrice = Convert.ToInt32(Math.Round(Convert.ToDouble(aFryerPrice) * 1.42)); //calculates price for next time the user wishes to buy an auto fryer
                lblBuyAFryer.Text = "$" + aFryerPrice.ToString();
                lblAutoFryer.Text = "Auto Fryers: " + aFryers.ToString();
                if (Money < aFryerPrice)
                {
                    picBuyAFryer.BackColor = Color.FromArgb(255, 192, 192);
                }
                UpdateStoreSize();
            }
        }

        private void picIncreaseMarketing_Click(object sender, EventArgs e)
        {
            if (Money >= marketingPrice) //checks if user can buy the upgrade
            {
                ChangeMoney(-marketingPrice);
                marketingLevel++; //adds 1 level to the marketing level.
                marketingPrice = Convert.ToInt32(marketingPrice * 2.1); //calculates new price
                lblMarName.Text = "Lvl " + (marketingLevel + 1) + " marketing";
                lblIncreaseMarketing.Text = "$" + marketingPrice.ToString();
                if (Money < marketingPrice)
                {
                    picIncreaseMarketing.BackColor = Color.FromArgb(255, 192, 192);
                }
                CalculateDemand();
            }
        }

        private void picAutoBuyIngredients_Click(object sender, EventArgs e)
        {
            if (Money >= 2700) 
            {
                autoBuyIngredients = true; //updates the value to reflect that the user has purchased auto buy ingredients
                ChangeMoney(-2700);
                picAutoBuyIngredients.Visible = false; //makes the autobuy upgrade disappear
                picAutoBuyIngredients.Enabled = false;
                picFryerE.Visible = true; //makes the fryer efficiency upgrade appear
                picFryerE.Enabled = true;
                chbAutoBuyIngredient.Visible = true; // makes the check box visible so that user can turn on/off autobuy
                MessageBox.Show("To auto purchase ingredients, check the 'auto buy ingredients' box under storage", "Information"); //Informs the user on how to operate autobuy
            }
        }

        private void picStoreIncrease_Click(object sender, EventArgs e)
        {
            if (Money >= StoreIncreasePrice)
            {
                ChangeMoney(-StoreIncreasePrice);
                StoreSize = StoreSize * 2; //increases store size by double
                StoreIncreasePrice = Convert.ToInt32(StoreIncreasePrice * 2.1);
                lblStoreIncrease.Text = "$" + StoreIncreasePrice.ToString();
                if (Money < StoreIncreasePrice)
                {
                    picStoreIncrease.BackColor = Color.FromArgb(255, 192, 192);
                }
                UpdateStoreSize();
            }
        }

        private void picSeasoning_Click(object sender, EventArgs e)
        {
            if (Money >= seasoningPrice)
            {
                ChangeMoney(-seasoningPrice);
                seasoningLevel++; //adds 1 to seasoning level
                seasoningPrice = Convert.ToInt32(seasoningPrice * 1.99);
                lblSeaName.Text = "Lvl " + (seasoningLevel + 1) + " seasoning";
                lblSeasoning.Text = "$" + seasoningPrice.ToString();
                if (Money < seasoningPrice)
                {
                    picSeasoning.BackColor = Color.FromArgb(255, 192, 192);
                }
                CalculateDemand();
            }
        }


        private void picFryerE_Click(object sender, EventArgs e)
        {
            if (Money >= fryerEPrice)
            {
                ChangeMoney(-fryerEPrice);
                fryerEfficiency++; //adds 1 to the fryer efficiency level
                fryerEPrice += fryerEPrice; //doubles the price of the upgrade for next level
                lblFryerEname.Text = (fryerEfficiency * 2) + "x Faster Fryers";
                lblFryerE.Text = "$" + fryerEPrice.ToString();
                if (Money < fryerEPrice)
                {
                    picSeasoning.BackColor = Color.FromArgb(255, 192, 192);
                }
            }
        }


        private void picBuyIngredient_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("Buy 20 ingredients",picBuyIngredient); //Creates a hint on what the upgrade does when the user hovers over one.
        }

        private void picIncreaseStorage_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("Buy 1 storage unit (50 storage space)", picIncreaseStorage);
        }

        private void picBuyAFryer_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("Buy 1 auto fryer (1 fish and chips per second)", picBuyAFryer);
        }

        private void picIncreaseMarketing_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("Increase demand through marketing", picIncreaseMarketing);
        }

        private void picSeasoning_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("Increase demand through enhanced seasoning", picSeasoning);
        }

        private void picStoreIncrease_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("Increase store tiles by 50%", picStoreIncrease);
        }

        private void picAutoBuyIngredients_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("Automatically buy ingredients for you", picAutoBuyIngredients);
        }

        private void picFryerE_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show(("Increase auto fryers output by " + fryerEfficiency*2 + " times"), picFryerE);
        }

        private void pnlTop_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
