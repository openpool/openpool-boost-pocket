openpool-boost-pocket
================================================================
Copyright (C) 2013 Kazunori Ogasawara and Jun Kato
- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

A boost module for openpool library.

## How to use

### Extract a zip archive

Extract contents of OpenPoolBoostPocket.zip into Processing
library directory. All files should reside in its subdirectory
e.g.

C:\Users\hoge\Documents\Processing\libraries\OpenpoolBoostPocket

### Write code

Select "Serial I/O" and "OpenpoolBoostPocket" from Sketch >
Import Library... menu.

```java
import openpool.pocket.*;

OpenpoolBoostPocket obp;

void setup() {
  // List all the available serial ports
  try {
  // Open the port you are using at the rate you want:

  // Connect to the pocket detector.
    obp = new OpenpoolBoostPocket(this, "COM4");
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
```

## License

This work is licensed under GNU GPL, Version 2.0.

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
http://github.com/openpool/openpool-boost-pocket