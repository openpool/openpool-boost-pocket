import openpool.pocket.OpenpoolBoostPocket;
import processing.core.PApplet;

public class PocketDetectorExample extends PApplet {
	private static final long serialVersionUID = 7739447543564699435L;
	private OpenpoolBoostPocket obp;

	public static void main(String[] args) {
		new PocketDetectorExample();
	}

	public void setup() {
		super.setup();
		size(640, 480);
		obp = new OpenpoolBoostPocket(this, "COM5");
		obp.setDebug(true);
		obp.start();
	}

	public void draw() {
		int[] pockets = obp.getPockets();
		for (int i = 0; i < pockets.length; i++) {
			if (pockets[i] > 0) {
				print(pockets[i]);
				print(" balls fell in the pocket no.");
				println(i + 1);
			}
		}
	}

}
