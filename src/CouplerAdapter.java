public interface CouplerAdapter {

	int branchesCount = 0;

	int getBranchesCount();

	void setBranchesCount(int branchesCount);

	/**
	 * 
	 * @param channelId
	 * @return 
	 */
	boolean openChannel(int channelId);

	/**
	 * 
	 * @param channelId
	 * @return 
	 */
	boolean closeChannel(int channelId);

}