public interface MonitorAdapter {

	double lastMeasure = 0.0;

	double getLastMeasure();

	void setLastMeasure(double lastMeasure);

	String getDevicePath();

	void setDevicePath(String devicePath);

	/**
	 * 
	 * @return 
	 */
	double measure();

}