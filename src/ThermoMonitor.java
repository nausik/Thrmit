public class ThermoMonitor extends OneWireDevice implements MonitorAdapter {
    @Override
    public double getLastMeasure() {
        return 0;
    }

    @Override
    public void setLastMeasure(double lastMeasure) {

    }

    @Override
    public String getDevicePath() {
        return null;
    }

    @Override
    public void setDevicePath(String devicePath) {

    }

    @Override
    public double measure() {
        return 0;
    }
}