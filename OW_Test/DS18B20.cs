using System;
using System.Collections.Generic;
using System.Text;

namespace OW_Test
{
    class DS18B20 : OneWireDevice
    {

        private com.dalsemi.onewire.utils.OWPath path;

        public DS18B20(string id, com.dalsemi.onewire.utils.OWPath current_path, com.dalsemi.onewire.adapter.DSPortAdapter device_adapter) : base(id, device_adapter)
        {
            path = current_path;
        }

        public double measure()
        {
            com.dalsemi.onewire.container.OneWireContainer28 monitor = (com.dalsemi.onewire.container.OneWireContainer28) getAdapter().getDeviceContainer(base.getId());
            sbyte[] owd_state = monitor.readDevice();
            double result = monitor.getTemperature(owd_state);

            bool d_state = getAdapter().select(base.getId());
            getAdapter().putByte(68);

            return result;
        }

        public void setPath(com.dalsemi.onewire.utils.OWPath new_path)
        {
            path = new_path;
        }

        public void openPath(){
            path.open();
        }

        public void closePath(){
            path.close();
        }
    }
}
