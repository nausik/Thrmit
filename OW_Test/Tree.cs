using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Diagnostics;

namespace OW_Test
{
    class Tree
    {
        //For thread-safe text append
        delegate void SetTextCallback(string id, double value);
        private bool should_terminate = false;

        private Dictionary<string, DS18B20> monitors = new Dictionary<string, DS18B20>();
        private com.dalsemi.onewire.adapter.DSPortAdapter adapter;
        private Hashtable last_measure;

        public Tree(com.dalsemi.onewire.adapter.DSPortAdapter tree_adapter)
        {
            adapter = tree_adapter;
        }

        public com.dalsemi.onewire.adapter.DSPortAdapter getAdapter(){
            return adapter;
        }

        public void setAdapter(com.dalsemi.onewire.adapter.DSPortAdapter new_adapter){
            adapter = new_adapter;
        }

        public void addMonitor(DS18B20 new_monitor){
            monitors.Add(new_monitor.getId(), new_monitor);
        }

        public Dictionary<string, DS18B20> getMonitors()
        {
            return monitors;
        }

        public Hashtable measure(Action<bool> callback)
        {
            Hashtable measure_result = new Hashtable();

                int counter = 0;

                foreach (KeyValuePair<string, DS18B20> monitor in monitors)
                {
                    counter++;

                    monitor.Value.openPath();
                    double result = monitor.Value.measure();
                    measure_result.Add(monitor.Value.getId(), result);

                    //Check if network is in use
                    if (should_terminate)
                    {
                        monitor.Value.closePath();
                        adapter.endExclusive();
                        should_terminate = false;
                        callback(true);
                    }

                    if (counter == monitors.Count)
                    {
                        monitor.Value.closePath();
                    }

                   
                }

                last_measure = measure_result;
         
            return measure_result;
        }

        public void stopMeasuring()
        {
            should_terminate = true;
        }

        public int monitorsCount()
        {
            return monitors.Count;
        }

        public void build(){
            java.util.Enumeration owd_enum;
            com.dalsemi.onewire.container.OneWireContainer owd;

            try
            {
                adapter.beginExclusive(true);
                adapter.setSearchAllDevices();
                adapter.targetAllFamilies();
                adapter.setSpeed(com.dalsemi.onewire.adapter.DSPortAdapter.SPEED_REGULAR);
                owd_enum = adapter.getAllDeviceContainers();

                com.dalsemi.onewire.utils.OWPath global_path = new com.dalsemi.onewire.utils.OWPath(adapter);

                while (owd_enum.hasMoreElements())
                {
                    java.util.Enumeration c_owd_enum;
                    owd = (com.dalsemi.onewire.container.OneWireContainer)owd_enum.nextElement();

                    if (owd.getName() == "DS2409")
                    {
                        DS2409 branch = new DS2409(owd.getAddressAsString(), adapter);
                        com.dalsemi.onewire.utils.OWPath current_path = new com.dalsemi.onewire.utils.OWPath(adapter);

                        current_path.copy(global_path);
                        current_path.add(owd, 0);

                        branch.openLatch(0);

                        c_owd_enum = adapter.getAllDeviceContainers();

                        while (c_owd_enum.hasMoreElements())
                        {
                            owd = (com.dalsemi.onewire.container.OneWireContainer)c_owd_enum.nextElement();

                            if (owd.getName() == "DS18B20")
                            {
                                this.addMonitor(new DS18B20(owd.getAddressAsString(), current_path, adapter));

                                //double result = checkMonitor(adapter, owd.getAddressAsString());
                                //richTextBox1.Text = richTextBox1.Text + owd.getAddressAsString() + ": " + result + "\r\n";
                            }
                        }

                        current_path = new com.dalsemi.onewire.utils.OWPath(adapter);
                        current_path.copy(global_path);
                        current_path.add(owd, 1);

                        branch.openLatch(1);


                        c_owd_enum = adapter.getAllDeviceContainers();

                        while (c_owd_enum.hasMoreElements())
                        {
                            owd = (com.dalsemi.onewire.container.OneWireContainer)c_owd_enum.nextElement();

                            if (owd.getName() == "DS18B20")
                            {
                                this.addMonitor(new DS18B20(owd.getAddressAsString(), current_path, adapter));
                            }
                        }

                        branch.closeLatch(1);
                    }
                }

                adapter.endExclusive();
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
            }
        }

        public string getMonitorsAsString(int number, int starting_from = 0){
            string return_val = "";
            int count = 0;

            if (number == 0 || number > this.monitorsCount() - starting_from)
            {
                number = this.monitorsCount() - starting_from;
            }

            foreach (KeyValuePair<string, DS18B20> monitor in monitors)
            {
                if (count >= starting_from)
                {
                    return_val += "('" + monitor.Key + "')";

                    if (count < starting_from + number - 1)
                    {
                        Debug.Write("Count: " + count + "; starting from: " + starting_from + "; number: " + number + "\r\n");
                        return_val += ",";
                    }
                }

                count++;

               if (count - starting_from >= number)
               {
                   break;
               }
            }

            return return_val;
        }

        public Hashtable getLastMeasurement()
        {
            return last_measure;
        }

        public float getValue(string owd_id)
        {
            return (float) last_measure[owd_id];
        }

        public string getLastMeasurementAsString(int number, int starting_from = 0)
        {
            string return_val = "";
            int count = 0;

            if (number == 0 || number > last_measure.Count - starting_from)
            {
                number = last_measure.Count - starting_from;
            }

            foreach (DictionaryEntry monitor in last_measure)
            {
                if (count >= starting_from)
                {
                    return_val += "('" + monitor.Key + "," + monitor.Value + "')";

                    if (count < starting_from + number - 1)
                    {
                        Debug.Write("Count: " + count + "; starting from: " + starting_from + "; number: " + number + "\r\n");
                        return_val += ",";
                    }
                }

                count++;

                if (count - starting_from >= number)
                {
                    break;
                }
            }

            return return_val;
        }
    }
}
