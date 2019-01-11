/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package genfusionscxml;

import java.io.IOException;
import scxmlgen.Fusion.FusionGenerator;
import scxmlgen.Modalities.Output;
import scxmlgen.Modalities.Speech;
import scxmlgen.Modalities.SecondMod;

/**
 *
 * @author nunof
 */
public class GenFusionSCXML {

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) throws IOException {

    FusionGenerator fg = new FusionGenerator();
    
    // Gesture
    //fg.Single(SecondMod.M1, Output.M1);
    //fg.Single(SecondMod.M2, Output.M2);

    // Redundancy
    fg.Redundancy(Speech.SEARCH_FLIGHT_ROME, SecondMod.FLIGHT_ROME, Output.SEARCH_FLIGHT_ROME);
    fg.Redundancy(Speech.SEARCH_FLIGHT_PARIS, SecondMod.FLIGHT_PARIS, Output.SEARCH_FLIGHT_PARIS);
    fg.Redundancy(Speech.SEARCH_HOTEL_PARIS, SecondMod.HOTEL_PARIS, Output.SEARCH_HOTEL_PARIS);
    fg.Redundancy(Speech.SEARCH_HOTEL_ROME, SecondMod.HOTEL_ROME, Output.SEARCH_HOTEL_ROME);
    fg.Redundancy(Speech.MAX, SecondMod.ZOOM, Output.MAX);
    fg.Redundancy(Speech.CLOSE, SecondMod.CLOSE, Output.CLOSE);

    // Complementarity
    fg.Complementary(SecondMod.M1, Speech.ROME, Output.M1_ROME);
    fg.Complementary(SecondMod.M1, Speech.PARIS, Output.M1_PARIS);
    fg.Complementary(SecondMod.M1, Speech.LONDON, Output.M1_LONDON);
    fg.Complementary(SecondMod.M2, Speech.ROME, Output.M2_ROME);
    fg.Complementary(SecondMod.M2, Speech.PARIS, Output.M2_PARIS);
    fg.Complementary(SecondMod.M2, Speech.LONDON, Output.M1_LONDON);
    
    fg.Build("fusion.scxml");
        
    }
}
