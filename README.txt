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
Serial serial;
OpenpoolBoostPocket obp;

setup() {
  // List all the available serial ports
  println(Serial.list());
  // Open the port you are using at the rate you want:
  myPort = new Serial(this, Serial.list()[0], 9600);
  // Connect to the pocket detector.
  opb = new OpenpoolBoostPocket(this, serial);
  opb.start();
}

loop() {
  // When you want to know the total pockets, call
  // getTotalPockets() method.
  int[] totalPockets = opb.getTotalPockets();
  
  // You might sometimes want to call resetTotal() to reset the
  // total numbers.
  //opb.resetTotal();

  // When you want to know if there's any pocket after the last
  // frame, call getPockets() method.
  int[] pockets = opb.getPockets();
  for (int i = 0; i < pockets.length; i ++) {
    if (pockets[i] > 0) {
      print(pockets[i]);
      print(" balls fell in the pocket no.");
      println(i + 1));
    }
  }
}
```

## License

This work is licensed under GNU GPL, Version 2.0.

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
http://github.com/openpool/openpool-boost-pocket