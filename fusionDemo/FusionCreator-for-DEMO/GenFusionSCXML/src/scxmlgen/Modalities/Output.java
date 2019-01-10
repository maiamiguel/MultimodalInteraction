package scxmlgen.Modalities;

import scxmlgen.interfaces.IOutput;

public enum Output implements IOutput {
    
    /*SQUARE_RED("[shape][SQUARE][color][RED]"),
    SQUARE_BLUE("[shape][SQUARE][color][BLUE]"),
    SQUARE_YELLOW("[shape][SQUARE][color][YELLOW]"),
    TRIANGLE_RED("[shape][TRIANGLE][color][RED]"),
    TRIANGLE_BLUE("[shape][TRIANGLE][color][BLUE]"),
    TRIANGLE_YELLOW("[shape][TRIANGLE][color][YELLOW]"),
    CIRCLE_RED("[shape][CIRCLE][color][RED]"),
    CIRCLE_BLUE("[shape][CIRCLE][color][BLUE]"),
    CIRCLE_YELLOW("[shape][CIRCLE][color][YELLOW]"),
    CIRCLE("[shape][CIRCLE]");*/
    
    //SPEECH

    SEARCH_FLIGHT_ROME("[SEARCH][FLIGHT][ROME]", 1500),
    SEARCH_FLIGHT_PARIS("[SEARCH][FLIGHT][PARIS]", 1500),
    SEARCH_HOTEL_ROME("[SEARCH][HOTEL][ROME]", 1500),
    SEARCH_HOTEL_PARIS("[SEARCH][HOTEL][PARIS]", 1500),
    HELP("[HELP]"),
    CLOSE("[CLOSE]"),
    UP("[UP]"),
    DOWN("[DOWN]"),
    YES("[YES]"),
    NO("[NO]"),

    //GESTURES
    
    M1_ROME("[M1][ROME]"),
    M1_PARIS("[M1][PARIS]"),
    M2_ROME("[M2][ROME]"),
    M2_PARIS("[M2][PARIS]"),
    SEARCH_FLIGHT_ROME("[FLIGHT][ROME]"),
    FLIGHT_PARIS("[FLIGHT][PARIS]"),
    FLIGHT_LONDON("[FLIGHT][LONDON]"),
    HOTEL_ROME("[HOTEL][ROME]"),
    HOTEL_PARIS("[HOTEL][PARIS]"),
    HOTEL_LONDON("[HOTEL][LONDON]"),
    M1("[M1]"),
    M2("[M2]");
    
    
    private String event;

    Output(String m) {
        event=m;
    }
    
    public String getEvent(){
        return this.toString();
    }

    public String getEventName(){
        return event;
    }
    
}
