package scxmlgen.Modalities;

import scxmlgen.interfaces.IModality;

/**
 *
 * @author nunof
 */
public enum SecondMod implements IModality{

    /*RED("[color][RED]",1500),
    BLUE("[color][BLUE]",1500),
    YELLOW("[color][YELLOW]",1500);*/
    
    SELECT("[SELECT]",1500),
    UP("[UP]",1500),
    DOWN("[DOWN]",1500),
    FLIGHT_ROME("[FLIGHT][ROME]",1500),
    FLIGHT_PARIS("[FLIGHT][PARIS]",1500),
    FLIGHT_LONDON("[FLIGHT][LONDON]",1500),
    HOTEL_ROME("[HOTEL][ROME]",1500),
    HOTEL_PARIS("[HOTEL][PARIS]",1500),
    HOTEL_LONDON("[HOTEL][LONDON]",1500),
    M1("[M1]",1500),
    M2("[M2]",1500),
    CLOSE("[CLOSE]",1500);
    
    private String event;
    private int timeout;

    SecondMod(String m, int time) {
        event=m;
        timeout=time;
    }

    @Override
    public int getTimeOut() {
        return timeout;
    }

    @Override
    public String getEventName() {
        //return getModalityName()+"."+event;
        return event;
    }

    @Override
    public String getEvName() {
        return getModalityName().toLowerCase()+event.toLowerCase();
    }
    
}
