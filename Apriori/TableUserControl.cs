using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BAL;
using System.IO;

namespace Apriori
{
    public partial class TableUserControl : UserControl
    {
        public TableUserControl(ItemSet itemSet, List<AssociationRule> rules)
        {
            InitializeComponent();
            ItemSetLabel.Text = itemSet.Label;
            DirectoryInfo directoryInfo = new DirectoryInfo(@"C:\Users\apathak2\source\repos\ajaypathak\Apriori\Apriori\");
            string filename = itemSet.Label+".txt";
            string rulefile = itemSet.Label + "rule.txt";
            StringBuilder stringBuilder = new StringBuilder();
            Dictionary<string, int> myDict = new Dictionary<string, int>();
            Dictionary<string, double> rulesdict = new Dictionary<string, double>();

            foreach (var item in itemSet)
            {
                ItemSetsDataGridView.Rows.Add(item.Key.ToDisplay(), item.Value);
                
                myDict.Add(item.Key.ToDisplay(), item.Value);
                if (item.Value < itemSet.Support)                    
                    ItemSetsDataGridView.Rows[ItemSetsDataGridView.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            }
            
            var sortedDict = from entry in myDict orderby entry.Value descending select entry;
            foreach (var item in sortedDict)
            {
                stringBuilder.Append("ItemSet:{ " + item.Key).Append("} \t Values : ").Append(item.Value).AppendLine();
            }

            filename = Path.Combine(directoryInfo.FullName, filename);
            File.WriteAllText(filename, stringBuilder.ToString());
            myDict.Clear();

            if (rules.Count == 0)
            {
                ItemSetsDataGridView.Height = 342;
                RulesDataGridView.Hide();
            }
            else
            {
                foreach (var item in rules)
                {
                    if (rulesdict.ContainsKey(item.Label)==false)
                    { rulesdict.Add(item.Label, item.Confidance);

                    }
                   

                    RulesDataGridView.Rows.Add(item.Label, item.Confidance.ToPercentString(), 
                        item.Support.ToPercentString(),item.Description);
                }
            }
            var rulesortedDict = from entry in rulesdict orderby entry.Value descending select entry;
            stringBuilder= new StringBuilder();
            foreach (var item in rulesortedDict)
            {
                stringBuilder.Append("ItemSet:{ " + item.Key).Append("} \t Confidance : ").Append(item.Value).AppendLine();
            }
            filename = Path.Combine(directoryInfo.FullName, rulefile);
            File.WriteAllText(filename, stringBuilder.ToString());

        }

        public TableUserControl(List<string> Values)
        {
            InitializeComponent();
            ItemSetLabel.Text = "Transactions";
            ItemSetsDataGridView.Columns[0].Name = "TransactionID";
            ItemSetsDataGridView.Columns[1].Name = "Items";
            for (int i = 0; i < Values.Count; i++)
            {
                ItemSetsDataGridView.Rows.Add(i, Values[i]);
            }
            ItemSetsDataGridView.Height = 342;
            RulesDataGridView.Hide();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            ItemSetsDataGridView.ClearSelection();
        }

        private void ItemSetsDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
