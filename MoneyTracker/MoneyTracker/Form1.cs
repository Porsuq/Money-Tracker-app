using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace MoneyTracker
{
    public partial class Form1 : Form
    {

        List<Expenses> expensesList = new List<Expenses>();
        List<Earnings> earningsList = new List<Earnings>();
        List<Goals> goalsList = new List<Goals>();

        public int eIDCount = 0;
        public int index;
        public double totalCost;

        public int eaIDCount = 0;
        public int eaIndex;



        public class Expenses
        {
            public string ID;
            public string Expense;
            public string Cost;
            public string notes;
        }
        public void updateList()
        {
            try 
            {           
                totalCost = 0;
                lstExpenses.Items.Clear();
                foreach (var i in expensesList)
                {
     
                    double cost = Convert.ToDouble(i.Cost);
                    totalCost += cost;
                    string displayCost = "£" + Convert.ToString(totalCost);
                    string[] listAr = new string[5];

                    listAr[0] = i.ID;
                    listAr[1] = i.Expense;
                    listAr[2] = "£" + i.Cost;
                    listAr[3] = i.notes;
                    listAr[4] = displayCost;
                    ListViewItem myList = new ListViewItem(listAr);
                    lstExpenses.Items.Add(myList);


                }

            
            } 
            catch (Exception ex) 
            {

                MessageBox.Show($"There has been a critical error in the listing of the new expense, please make sure everything has been filled and/or done correctly. If the error consists ask for technical support {ex} ","Error", MessageBoxButtons.OK, MessageBoxIcon.Error );

            }


        }
        public void updateExpensesDataBase ()
        {
            try {  
            StreamWriter eWrite = new StreamWriter("Expenses.txt");
            foreach (var i in expensesList)
            {
                eWrite.WriteLine(i.ID + ";" + i.Expense + ";" + i.Cost + ";" + i.notes);
            }
            eWrite.Dispose(); }
            catch (Exception ex)
            {
                MessageBox.Show($"There has been a critical error when saving the new expense, please make sure everything has been filled and/or done correctly. If the error consists ask for technical support {ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        public Form1()
        {
            InitializeComponent();

            //tap on
            StreamReader eReader = new StreamReader("Expenses.txt");
            string eLine;

            while ((eLine = eReader.ReadLine()) != null)
            {

                string[] eAr = eLine.Split(';').Select(x => x.Trim()).ToArray();
                addingExpense(eAr[0], eAr[1], eAr[2], eAr[3]);
                eIDCount = Convert.ToInt32(eAr[0]);
            }
            //tap off
            eReader.Dispose();

            StreamReader eaReader = new StreamReader("Earnings.txt");
            string eaLine;
            while ((eaLine = eaReader.ReadLine()) != null)
            {
                string[] eaAr = eaLine.Split(';').Select(x => x.Trim()).ToArray();
                addingNewEarning(eaAr[0], eaAr[1], eaAr[2], eaAr[3]);
                eaIDCount = Convert.ToInt32(eaAr[0]);
            }
            eaReader.Dispose();

        }
        public void addingExpense(string eID, string eExpense, string eCost, string eNotes)
        {
            Expenses anExpense = new Expenses();
            anExpense.ID = eID;
            anExpense.Expense = eExpense;
            anExpense.Cost = eCost;
            anExpense.notes = eNotes;

            expensesList.Add(anExpense);

        }
        private void btnMExpenses_Click(object sender, EventArgs e)
        {
            pnlMenu.Enabled = false;
            pnlMenu.Visible = false;
            pnlExpenses.Visible = true;
            pnlExpenses.Enabled = true;


            updateList();

        }

        private void btnEBack_Click(object sender, EventArgs e)
        {
            pnlExpenses.Enabled = false;
            pnlExpenses.Visible = false;
            pnlMenu.Visible = true;
            pnlMenu.Enabled = true;
        }

        private void btnMExit_Click(object sender, EventArgs e)
        {
            DialogResult test;
            test = MessageBox.Show("Are you sure you want to quit the program?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (test == DialogResult.Yes)
            {

                Application.Exit();
            }
        }

        private void btnESave_Click(object sender, EventArgs e)
        {
            if (txtExpense.Text.Length < 16 && txtExpense.Text.Length > 2 )
            {
                double cost = Convert.ToDouble(txtCost.Text);
                if (cost > 0 && cost < 10000000)
                {
                    if (txtNotes.Text.Length >= 0 && txtNotes.Text.Length < 65 )
                    {
                        eIDCount++;
                        string sEID = Convert.ToString(eIDCount);
                        addingExpense(sEID, txtExpense.Text, txtCost.Text, txtNotes.Text);
                        updateList();
                        updateExpensesDataBase();
                        MessageBox.Show($"New expense has been successfully saved. The {eIDCount} ID has been assigned by the system.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("The notes cannot be longer than 65 characters.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("The Cost cannot be greater than 10000000 or less than 0", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            else
            {
                MessageBox.Show("The Expense cannot be longer than 16 letter or shorter than 2","Information",MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnELookUp_click(object sender, EventArgs e)
        {

            try
            {
                int ind = 0;
                while (expensesList[ind].ID != txtID.Text)
                {
                    ind++;
                }
     
                lstExpenses.Items.Clear();

                string[] myListAr = new string[4];
                myListAr[0] = expensesList[ind].ID;
                myListAr[1] = expensesList[ind].Expense;
                myListAr[2] = "£" + expensesList[ind].Cost;
                myListAr[3] = expensesList[ind].notes;

                ListViewItem listViewItem = new ListViewItem(myListAr);
                lstExpenses.Items.Add(listViewItem);
                index = ind;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Please make sure the ID has been entered correctly and you have entered only digits with no whispaces. If the erros continues ask for technical support.{ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnEListRefresh_Click(object sender, EventArgs e)
        {
            updateList();
        }


        private void btnERemove_Click(object sender, EventArgs e)
        {
            DialogResult confirm;
            confirm  = MessageBox.Show("Are you sure that you want to remove this expense?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            try 
            {
                if (confirm == DialogResult.Yes)
                {
                    expensesList.Remove(expensesList[index]);
                    updateList();
                    updateExpensesDataBase();
                }
                else if (confirm == DialogResult.No) 
                {
                    lstExpenses.Items.Clear();
                    index = -1;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occured when removing that expense, please ask for technical support if error continues{ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        /*
        --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
        THIS IS PART IS THE EARNINGS PANEL
         */





        public class Earnings
        {
            public string ID;
            public string Earning;
            public string Amount;
            public string Notes;
        }

        public void addingNewEarning (string ID, string Earning, string Amount, string Notes)
        {
            Earnings newEarning = new Earnings();
            newEarning.ID = ID;
            newEarning.Earning = Earning;
            newEarning.Amount = Amount;
            newEarning.Notes = Notes;

            earningsList.Add(newEarning);
        }

        public void earningsUpdateList()
        {
            try
            {
                totalCost = 0;
                lstEaEarnings.Items.Clear();
                foreach (var i in earningsList)
                {

                    double cost = Convert.ToDouble(i.Amount);
                    totalCost += cost;
                    string displayCost = "£" + Convert.ToString(totalCost);
                    string[] listAr = new string[5];

                    listAr[0] = i.ID;
                    listAr[1] = i.Earning;
                    listAr[2] = "£" + i.Amount;
                    listAr[3] = i.Notes;
                    listAr[4] = displayCost;
                    ListViewItem myList = new ListViewItem(listAr);
                    lstEaEarnings.Items.Add(myList);


                }


            }
            catch (Exception ex)
            {

                MessageBox.Show($"There has been a critical error in the listing of the new earnings, please make sure everything has been filled and/or done correctly. If the error consists ask for technical support {ex} ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        public void updateEarningDataBase ()
        {
            try
            {
                StreamWriter streamWriter = new StreamWriter("Earnings.txt");
                foreach (var i in earningsList)
                {
                    streamWriter.WriteLine(i.ID + ";" + i.Earning + ";" + i.Amount + ";" + i.Notes);
                }
                streamWriter.Dispose();
            }
            catch (Exception ex) 
            {
                MessageBox.Show($"There has been a critical error when saving the new earnings, please make sure everything has been filled and/or done correctly. If the error consists ask for technical support {ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }



        private void btnEaSave_Click(object sender, EventArgs e)
        {
            if (txtEaEarnings.Text.Length < 16 && txtEaEarnings.Text.Length > 2)
            {
                double cost = Convert.ToDouble(txtEaAmount.Text);
                if (cost > 0 && cost < 10000000)
                {
                    if (txtEaNotes.Text.Length >= 0 && txtEaNotes.Text.Length < 65)
                    {
                        eaIDCount++;
                        string sEaID = Convert.ToString(eaIDCount);
                        addingNewEarning(sEaID, txtEaEarnings.Text, txtEaAmount.Text, txtEaNotes.Text);
                        earningsUpdateList();
                        updateEarningDataBase();
                        MessageBox.Show($"New earning has been successfully saved. The system has assigned the {sEaID} to the earning.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("The notes cannot be longer than 65 characters.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("The amount cannot be greater than 10000000 or less than 0", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            else
            {
                MessageBox.Show("The earning cannot be longer than 16 letter or shorter than 2", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnEaRefresh_Click(object sender, EventArgs e)
        {
            earningsUpdateList();
        }

        private void btnEaRemove_Click(object sender, EventArgs e)
        {
            DialogResult confirm;
            confirm = MessageBox.Show("Are you sure that you want to remove this earning?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            try
            {
                if (confirm == DialogResult.Yes)
                {
                    earningsList.Remove(earningsList[eaIndex]);
                    earningsUpdateList();
                    updateEarningDataBase();
                }
                else if (confirm == DialogResult.No)
                {
                    lstEaEarnings.Items.Clear();
                    eaIndex = -1;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occured when removing that expense, please ask for technical support if error continues{ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEaLookUp_Click(object sender, EventArgs e)
        {
            try
            {
                int ind = 0;
                while (earningsList[ind].ID != txtEaID.Text)
                {
                    ind++;
                }

                lstEaEarnings.Items.Clear();

                string[] myListAr = new string[4];
                myListAr[0] = earningsList[ind].ID;
                myListAr[1] = earningsList[ind].Earning;
                myListAr[2] = "£" + earningsList[ind].Amount;
                myListAr[3] = earningsList[ind].Notes;

                ListViewItem listViewItem = new ListViewItem(myListAr);
                lstEaEarnings.Items.Add(listViewItem);
                eaIndex = ind;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Please make sure the ID has been entered correctly and you have entered only digits with no whispaces. If the erros continues ask for technical support.{ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnEaBack_Click(object sender, EventArgs e)
        {
            pnlEarnings.Enabled = false;
            pnlEarnings.Visible = false;
            pnlMenu.Enabled = true;
            pnlMenu.Visible = true;
        }

        private void btnMEarnings_Click(object sender, EventArgs e)
        {
            pnlMenu.Enabled = false;
            pnlMenu.Visible = false;
            pnlEarnings.Visible = true;
            pnlEarnings.Enabled = true;
            
            lstEaEarnings.Items.Clear();
            earningsUpdateList();
        }














        /*
        --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
        THIS IS PART IS THE GOALS PANEL
         */

        public class Goals
        {
            public string Earning;
            public string Amount;
            public string Expense;
            public string Cost;
        }

        public void addEarningToList(string earning, string amount)
        {
            Goals newGoals = new Goals();
            newGoals.Earning = earning;
            newGoals.Amount = amount;
            
            goalsList.Add(newGoals);
        }
        public void supplyEarningToList (string earning, string amount)
        {
            int listingIndex = 0;
            while (goalsList[listingIndex].Earning != null) 
            {
                listingIndex++;
            }

            goalsList[listingIndex].Earning = earning;
            goalsList[listingIndex].Amount = amount;
        }

        public void addExpenseToList (string expense, string cost)
        {
            Goals newExpense = new Goals();
            newExpense.Expense = expense;
            newExpense.Cost = cost;

            goalsList.Add(newExpense);
        }
        public void supplyExpenseToList(string Expense, string Cost)
        {
            int listingIndex = 0;
            while (goalsList[listingIndex].Expense != null)
            {
                listingIndex++;
            }

            goalsList[listingIndex].Expense = Expense;
            goalsList[listingIndex].Cost = Cost;

        }

        private void btnGRefreshList_Click(object sender, EventArgs e)
        {
            int earningIndex = 0;
            int expenseIndex = 0 ;

            foreach (var i in earningsList)
            {
                earningIndex++;
            }
            foreach (var i in expensesList)
            {
                expenseIndex++;
            }
            if (earningIndex >= expenseIndex)
            {
                foreach (var i in earningsList) 
                {
                    string[] earningsDisplay = new string[2];
                    earningsDisplay[0] = i.Earning;
                    earningsDisplay[1] = i.Amount;

                    addEarningToList(earningsDisplay[0], earningsDisplay[1]);
                } 
                foreach (var i in expensesList)
                {
                    string[] expensesDisplay = new string[2];
                    expensesDisplay[0] = i.Expense;
                    expensesDisplay[1] = i.Cost;

                    supplyExpenseToList(expensesDisplay[0], expensesDisplay[1]);
                }              
            }
            else
            {
                foreach (var i in expensesList)
                {
                    string[] expensesDisplay = new string[2];
                    expensesDisplay[0] = i.Expense;
                    expensesDisplay[1] = i.Cost;

                    addExpenseToList(expensesDisplay[0], expensesDisplay[1]);
                }
                foreach (var i in earningsList)
                {
                    string[] earningsDisplay = new string[2];
                    earningsDisplay[0] = i.Earning;
                    earningsDisplay[1] = i.Amount;

                    supplyEarningToList(earningsDisplay[0], earningsDisplay[1]);
                }
            }



            lstGoals.Items.Clear();
            double total = 0;
            ListViewItem list = new ListViewItem();
            foreach (var i in goalsList) 
            {
                double amount = 0;
                double cost = 0;
                if (i.Earning != null)
                {
                    amount = Convert.ToDouble(i.Amount);
                }
                if (i.Cost != null)
                {
                    cost = Convert.ToDouble(i.Cost);
                }
 
                total += amount - cost;
                string[] listing = new string[5];
                listing[0] = i.Earning;
                listing[1] = "£" + i.Amount;
                listing[2] = i.Expense;
                listing[3] = "£-" + i.Cost;
                listing[4] = "£" + Convert.ToString(total);

                list = new ListViewItem(listing);

                lstGoals.Items.Add(list);

            }
        }

        private void btnGBack_Click(object sender, EventArgs e)
        {
            pnlGoals.Enabled = false;
            pnlGoals.Visible = false;
            pnlMenu.Enabled = true;
            pnlMenu.Visible = true;
        }

        private void btnMGoals_Click(object sender, EventArgs e)
        {
            pnlMenu.Enabled = false;
            pnlMenu.Visible = false;
            pnlGoals.Visible = true;
            pnlGoals.Enabled = true;
        }
    }
}