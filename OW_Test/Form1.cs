using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Threading;
using System.Diagnostics;

namespace OW_Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //For thread-safe text append
        delegate void SetTextCallback(Hashtable results);

        Tree current_tree;
        private Dictionary<string, Tree> trees = new Dictionary<string,Tree>();

        private com.dalsemi.onewire.adapter.DSPortAdapter adapter = com.dalsemi.onewire.OneWireAccessProvider.getAdapter("{DS9490}", "USB1");
        private System.Threading.Thread measure_thread;
        private DB db_connection = new DB("localhost", "test_db", "root", "");

        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            Tree new_tree = new Tree(adapter);
            new_tree.build();

            if (new_tree.monitorsCount() > 0)
            {
                displayMonitors(new_tree);
                trees.Add(adapter.getPortName(), new_tree);
                button2.Enabled = true;

                //Upload monitors from the new tree to the DB
                db_connection.OpenConnection();
                db_connection.uploadMonitors(new_tree);
                //db_connection.testQuery();
                db_connection.CloseConnection();
            }

        }

        private void displayMonitors(Tree current_tree)
        {
            foreach (KeyValuePair<string, DS18B20> monitor in current_tree.getMonitors())
            {
                ListViewItem listview_monitor = new ListViewItem(monitor.Value.getName());
                listview_monitor.SubItems.Add("");
                listView1.Items.Add(listview_monitor);
            }
        }

        private void measure()
        {
            while (true)
            {

                foreach (KeyValuePair<string, Tree> tree in trees)
                {
                    current_tree = tree.Value;
                    setResult(tree.Value.measure(onThreadAbort));
                }
            }
        }

        private void onThreadAbort(bool val)
        {
            measure_thread.Abort();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "Начать измерение")
            {
                button1.Enabled = false;
                button2.Text = "Закончить измерение";
                db_connection.OpenConnection();
                measure_thread = new Thread(new ThreadStart(measure));
                measure_thread.Start();
            }
            else
            {
                current_tree.stopMeasuring();
                db_connection.CloseConnection();
                button1.Enabled = true;
                button2.Text = "Начать измерение";
            }
        }

        private void uploadResult(){

        }

        //Thread safe text append
        private void setResult(Hashtable results)
        {
            if (listView1.InvokeRequired)
            {
               SetTextCallback d = new SetTextCallback(setResult);
                this.Invoke(d, new object[] { results });
            }
            else
            {
                foreach (DictionaryEntry pair in results)
                {
                    for (int i = 0; i < listView1.Items.Count; i++)
                    {

                        if (listView1.Items[i].SubItems[0].Text == (string) pair.Key)
                        {
                            listView1.Items[i].SubItems[1].Text = Convert.ToString(pair.Value);
                            break;
                        }
                    }
                }
            }
        }


    }
}
