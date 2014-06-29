using System;
using System.Collections.Generic;
using System.Text;

namespace OW_Test
{
    class DS2409 : OneWireDevice
    {
        //Latches:
        //0 - main
        //1 - AUX

        private com.dalsemi.onewire.container.OneWireContainer1F branch;
        private sbyte[] owd_state;

        public DS2409(string owd_id, com.dalsemi.onewire.adapter.DSPortAdapter device_adapter)
            : base(owd_id, device_adapter)
        {
            branch = (com.dalsemi.onewire.container.OneWireContainer1F) getAdapter().getDeviceContainer(getId());
            owd_state = branch.readDevice();
        }

        public void openLatch(int latch)
        {
            sbyte[] owd_state = branch.readDevice();
            branch.setLatchState(latch, true, true, owd_state);
            branch.writeDevice(owd_state);
        }

        public void closeLatch(int latch)
        {
            sbyte[] owd_state = branch.readDevice();
            branch.setLatchState(latch, false, true, owd_state);
            branch.writeDevice(owd_state);
        }

        //-1 - closed
        public int openedLatch()
        {
            if (branch.getLatchState(0, owd_state))
            {
                return 0;
            } else if (branch.getLatchState(1, owd_state))
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        public com.dalsemi.onewire.container.OneWireContainer1F getContainer()
        {
            return branch;
        }


    }
}
