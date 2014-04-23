/**
 * Created by Nausik on 4/23/2014.
 */
public class NetworkNotConnectedException extends Exception {

    private static final long exceptionID = 1;

    private String errorCode = "Can't find tree root! No way to connect any device :(";

    public NetworkNotConnectedException(String message, String errorCode){
        super(message);
        this.errorCode = errorCode;
    }

    public String getErrorCode(){
        return this.errorCode;
    }
}
