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
  
    /*fg.Sequence(Speech.SQUARE, SecondMod.RED, Output.SQUARE_RED);
    fg.Sequence(Speech.SQUARE, SecondMod.BLUE, Output.SQUARE_BLUE);
    fg.Sequence(Speech.SQUARE, SecondMod.YELLOW, Output.SQUARE_YELLOW);
    fg.Complementary(Speech.TRIANGLE, SecondMod.RED, Output.TRIANGLE_RED);
    fg.Complementary(Speech.TRIANGLE, SecondMod.BLUE, Output.TRIANGLE_BLUE);
    fg.Complementary(Speech.TRIANGLE, SecondMod.YELLOW, Output.TRIANGLE_YELLOW);
    fg.Complementary(Speech.CIRCLE, SecondMod.RED, Output.CIRCLE_RED);
    fg.Complementary(Speech.CIRCLE, SecondMod.BLUE, Output.CIRCLE_BLUE);
    fg.Complementary(Speech.CIRCLE, SecondMod.YELLOW, Output.CIRCLE_YELLOW);*/
    
    //fg.Single(Speech.CIRCLE, Output.CIRCLE);  //EXAMPLE
    //fg.Redundancy(Speech.CIRCLE, SecondMod.CIRCLE, Output.CIRCLE);  //EXAMPLE
    
    // Speech
    /*fg.Single(Speech.SHR, Output.SHR);
    fg.Single(Speech.SHP, Output.SHP);
    fg.Single(Speech.SFR, Output.SFR);
    fg.Single(Speech.SFP, Output.SFP);*/
    fg.Single(Speech.HELP, Output.HELP);
    fg.Single(Speech.CLOSE, Output.CLOSE);
    fg.Single(Speech.YES, Output.YES);
    fg.Single(Speech.NO, Output.NO);
    fg.Single(SecondMod.UP, Output.UP); // not implemented yet
    fg.Single(SecondMod.DOWN, Output.DOWN); // not implemented yet
    
    // Gesture
    fg.Single(SecondMod.FLIGHT_ROME, Output.FLIGHT_ROME);
    fg.Single(SecondMod.FLIGHT_PARIS, Output.FLIGHT_PARIS);
    fg.Single(SecondMod.FLIGHT_LONDON, Output.FLIGHT_LONDON);
    fg.Single(SecondMod.HOTEL_ROME, Output.HOTEL_ROME);
    fg.Single(SecondMod.HOTEL_PARIS, Output.HOTEL_PARIS);
    fg.Single(SecondMod.HOTEL_LONDON, Output.HOTEL_LONDON);
    fg.Single(SecondMod.M1, Output.M1);
    fg.Single(SecondMod.M2, Output.M2);
    
    // Redundancy
    fg.Redundancy(Speech.DOWN, SecondMod.DOWN, Output.DOWN);
    fg.Redundancy(Speech.UP, SecondMod.UP, Output.UP);
    
    // Complementarity
    fg.Complementary(SecondMod.M1, Speech.ROME, Output.M1_ROME);
    fg.Complementary(SecondMod.M1, Speech.PARIS, Output.M1_PARIS);
    fg.Complementary(SecondMod.M2, Speech.ROME, Output.M2_ROME);
    fg.Complementary(SecondMod.M2, Speech.PARIS, Output.M2_PARIS);
    
    fg.Build("fusion.scxml");
        
    }
    
}
