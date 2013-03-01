import processing.serial.*;
import openpool.pocket.*;

Serial myPort;
OpenpoolBoostPocket obp;

void setup() {
  // List all the available serial ports
  println(Serial.list());
  try {
    // Open the port you are using at the rate you want:
    myPort = new Serial(this, Serial.list()[0], 9600);

    // Connect to the pocket detector.
    obp = new OpenpoolBoostPocket(this, myPort);
    obp.start();
  }  
  catch(java.lang.RuntimeException e) {
    println("POCKET DETECTOR ERROR!!!!!!");
  }
}

void draw() {
  if (obp != null)
  {
    // When you want to know the total pockets, call
    // getTotalPockets() method.
    int[] totalPockets = obp.getTotalPockets();

    // You might sometimes want to call resetTotal() to reset the
    // total numbers.
    //opb.resetTotal();

    // When you want to know if there's any pocket after the last
    // frame, call getPockets() method.

    int[] pockets = obp.getPockets();
    for (int i = 0; i < pockets.length; i ++) {
      if (pockets[i] > 0) {
        print(pockets[i]);
        print(" balls fell in the pocket no.");
        println(i + 1);
      }
    }
  }
}

