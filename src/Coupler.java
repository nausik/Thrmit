public class Coupler extends OneWireDevice implements CouplerAdapter {
    @Override
    public int getBranchesCount() {
        return 0;
    }

    @Override
    public void setBranchesCount(int branchesCount) {

    }

    @Override
    public boolean openChannel(int channelId) {
        return false;
    }

    @Override
    public boolean closeChannel(int channelId) {
        return false;
    }
}