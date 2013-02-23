package openpool.pocket;

import java.util.Arrays;
import java.util.concurrent.Executors;
import java.util.concurrent.Future;
import java.util.concurrent.ScheduledExecutorService;
import java.util.concurrent.TimeUnit;

import processing.core.PApplet;
import processing.serial.Serial;

public class OpenpoolBoostPocket {
	private PApplet pa;
	private ScheduledExecutorService ses;
	private Serial serial;
	private Future<?> future;
	private boolean isDebug;
	
	private int[] pockets;
	private int[] totalPockets;

	public OpenpoolBoostPocket(PApplet pa, Serial serial) {
		this.pa = pa;
		this.serial = serial;
		pockets = new int[6];
		totalPockets = new int[6];
		pa.registerDispose(this);
	}
	
	public void start() {
		ses = Executors.newSingleThreadScheduledExecutor();
		ses.execute(new Runnable() {
			public void run() {
				startMeasurement();
				waitForLEDs();
				startPolling();
			}
		});
	}
	
	public void stop() {
		ses.execute(new Runnable() {
			public void run() {
				stopPolling();
			}
		});
	}
	
	public void resetTotal() {
		Arrays.fill(totalPockets, 0);
	}
	
	public int[] getTotalPockets() {
		return this.totalPockets.clone();
	}
	
	public int[] getPockets() {
		int[] pockets = this.pockets.clone();
		Arrays.fill(this.pockets, 0);
		return pockets;
	}
	
	public void setDebug(boolean isDebug) {
		this.isDebug = isDebug;
	}
	
	public void dispose() {
		pa.unregisterDispose(this);
		stop();
	}
	
	private void startMeasurement() {
		serial.clear();
		serial.write("s");
		String message = serial.readStringUntil('\n');
		if (isDebug) {
			System.out.print("pocket detector: ");
			System.out.println(message);
		}
	}
	
	private void waitForLEDs() {
		try {
			Thread.sleep(1000);
		} catch (InterruptedException e) {
			// Do nothing.
		}
	}
	
	private void startPolling() {
		future = ses.scheduleAtFixedRate(new Runnable() {
			public void run() {
				poll();
			}
		}, 33, 33, TimeUnit.MILLISECONDS);
	}
	
	private void stopPolling() {
		future.cancel(false);
	}
	
	private void poll() {
		serial.clear();
		serial.write("r");
		for (int pocket = 0; pocket < 6; pocket ++) {
			String message = serial.readStringUntil('\n');
			if (message == null) {
				if (isDebug) {
					System.err.print("pocket detector: "
							+ "something wrong happened when "
							+ "retrieving info at pocket no.");
					System.err.println(pocket);
				}
				pockets[pocket] = -1;
			} else {
				try {
					int numPocket = Integer.parseInt(message.trim());
					pockets[pocket] += numPocket;
					totalPockets[pocket] += numPocket;
				} catch (NumberFormatException nfe) {
					if (isDebug) {
						System.err.print("pocket detector: "
								+ "something wrong happened when "
								+ "parsing info at pocket no.");
						System.err.println(pocket);
						System.err.println(message);
					}
					pockets[pocket] = -1;
				}
			}
		}
	}
}
