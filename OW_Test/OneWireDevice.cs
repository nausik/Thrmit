using System;
using System.Collections.Generic;
using System.Text;

namespace OW_Test
{
    class OneWireDevice
    {
        private string owd_id = "";
        private string owd_name = "";
        private com.dalsemi.onewire.adapter.DSPortAdapter adapter;

        protected OneWireDevice(string id, com.dalsemi.onewire.adapter.DSPortAdapter device_adapter)
        {
            owd_id = id;
            owd_name = id;
            adapter = device_adapter;
        }

        public string getId()
        {
            return owd_id;
        }

        public string getName()
        {
            return owd_name;
        }

        public void setName(string new_name)
        {
            owd_name = new_name;
        }

        protected com.dalsemi.onewire.adapter.DSPortAdapter getAdapter()
        {
            return adapter;
        }
    }
}
