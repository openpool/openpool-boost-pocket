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

### Prepare proper RXTX library

Default RXTX library in OpenpoolBoostPocket/library directory is
for Windows with 64bit Java VM. If you're running Processing in
another configuration, e.g. Windows with 32bit Java VM or Mac OS
with 64bit Java VM, please download proper RXTX library from
[its official website](rxtx.qbang.org/wiki/index.php/Download)
and extract files into the directory.

### Write code

Select "OpenpoolBoostPocket" from Sketch > Import Library...
menu.

```java
import openpool.pocket.*;

OpenpoolBoostPocket obp;

void setup() {
  // Connect to the pocket detector.
  try {
    obp = new OpenpoolBoostPocket(this, "COM4");
    obp.start();
  } catch (Exception e) {
    println("Pocket detector not found.");
  }
}

void draw() {
  if (obp == null) return;

  // When you want to know the total pockets, call
  // getTotalPockets() method.
  int[] totalPockets = obp.getTotalPockets();

  // You might sometimes want to call resetTotal() to reset the
  // total numbers.
  //obp.resetTotal();

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
```

## License

This work is licensed under GNU GPL, Version 2.0.

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
http://github.com/openpool/openpool-boost-pocket